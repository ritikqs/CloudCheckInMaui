using CloudCheckInMaui.ConstantHelper;
using CloudCheckInMaui.Models;
using Microsoft.Maui.Networking;
using System;
using System.Diagnostics;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
//using static Android.Preferences.PreferenceActivity;

namespace CloudCheckInMaui.Services.ApiService
{
    public static class ApiHelper
    {
        private static readonly string API_URL = ConstantHelper.Constants.BaseUrl;
        private static readonly HttpClient _httpClient = new HttpClient { Timeout = TimeSpan.FromSeconds(30) };
        private static readonly JsonSerializerOptions _jsonOptions = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true,
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        };

        static ApiHelper()
        {
            // Enable response compression
            if (!_httpClient.DefaultRequestHeaders.AcceptEncoding.Contains(new System.Net.Http.Headers.StringWithQualityHeaderValue("gzip")))
                _httpClient.DefaultRequestHeaders.AcceptEncoding.Add(new System.Net.Http.Headers.StringWithQualityHeaderValue("gzip"));
            if (!_httpClient.DefaultRequestHeaders.AcceptEncoding.Contains(new System.Net.Http.Headers.StringWithQualityHeaderValue("deflate")))
                _httpClient.DefaultRequestHeaders.AcceptEncoding.Add(new System.Net.Http.Headers.StringWithQualityHeaderValue("deflate"));
        }

        public static async Task<ApiResponse> CallApi(HttpMethod method, string url, string postData = null, bool addAuthHeader = false)
        {
            if (Connectivity.Current.NetworkAccess != NetworkAccess.Internet)
            {
                return new ApiResponse { Status = false, Message = "No Internet Connection" };
            }

            try
            {
                var request = new HttpRequestMessage(method, new Uri(API_URL + url));

                // Add headers
                if (addAuthHeader && !string.IsNullOrWhiteSpace(App.LoggedInUser.Token))
                {
                    request.Headers.Add("Authorization", $"Bearer {App.LoggedInUser.Token}");
                }

                request.Headers.Add("XApiKey", App.XApikey ?? string.Empty);
                request.Headers.Add("X-User-Token", App.DBKey ?? string.Empty);

                // Add content
                if (!string.IsNullOrEmpty(postData))
                { 
                    request.Content = new StringContent(postData, Encoding.UTF8, "application/json");
                }

                Debug.WriteLine($"Request: {method} {API_URL + url}");
                if (postData != null) Debug.WriteLine($"Payload: {postData}");

                string fullRequestLog = addAuthHeader ? $"  Authorization: Bearer {App.Token}\n" : "";

                using var response = await _httpClient.SendAsync(request);
                var responseContent = await response.Content.ReadAsStringAsync();

                Debug.WriteLine($"Response: {responseContent}");

                if (response.IsSuccessStatusCode)
                {
                    try 
                    {
                        var apiResponse = JsonSerializer.Deserialize<ApiResponse>(responseContent, _jsonOptions);
                        return apiResponse ?? new ApiResponse { Status = true, Message = "Success but empty response" };
                    }
                    catch (JsonException ex)
                    {
                        Debug.WriteLine($"JSON Deserialization Error: {ex.Message}");
                        return new ApiResponse { Status = false, Message = "Invalid response format" };
                    }
                }

                Debug.WriteLine($"HTTP Error: {response.StatusCode}");
                return new ApiResponse 
                { 
                    Status = false, 
                    Message = $"HTTP Error: {response.StatusCode}",
                    Result = responseContent 
                };
            }
            catch (TaskCanceledException)
            {
                Debug.WriteLine("Request timed out");
                return new ApiResponse { Status = false, Message = "Request timed out" };
            }
            catch (HttpRequestException ex)
            {
                Debug.WriteLine($"Network Error: {ex.Message}");
                return new ApiResponse { Status = false, Message = "Network connection error" };
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Exception: {ex.Message}");
                return new ApiResponse { Status = false, Message = $"Error: {ex.Message}" };
            }
        }

        public static T ConvertResult<T>(object result)
        {
            try
            {
                var json = JsonSerializer.Serialize(result, _jsonOptions);
                return JsonSerializer.Deserialize<T>(json, _jsonOptions);
            }
            catch (JsonException ex)
            {
                Debug.WriteLine($"JSON Conversion Error: {ex.Message}");
                throw new InvalidOperationException($"Failed to convert result to type {typeof(T).Name}", ex);
            }
        }

        public static async Task<List<TimesheetModel>> GetTimeSheetData()
        {
            try
            {
                //var response = await CallApi(HttpMethod.Get, "timesheet/5", null, true);
                var response = await ApiHelper.CallApi(HttpMethod.Get, "TimeSheet", null, true);
                if (response.Status && response.Result != null)
                {
                    return ConvertResult<List<TimesheetModel>>(response.Result);
                }
                return new List<TimesheetModel>();
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error getting timesheet data: {ex.Message}");
                throw;
            }
        }

        // Helper for parallel fetching
        public static async Task<ApiResponse[]> CallApisInParallel(params (HttpMethod method, string url, string postData, bool addAuthHeader)[] requests)
        {
            var tasks = new List<Task<ApiResponse>>();
            foreach (var req in requests)
            {
                tasks.Add(CallApi(req.method, req.url, req.postData, req.addAuthHeader));
            }
            return await Task.WhenAll(tasks);
        }
    }

    public class ApiResponse
    {
        public bool Status { get; set; }
        public string Message { get; set; }
        public object Result { get; set; }

        public object Data => Result;

        public T GetData<T>()
        {
            try
            {
                if (Result == null) return default;
                var json = JsonSerializer.Serialize(Result, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true,
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase
                });
                return JsonSerializer.Deserialize<T>(json, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true,
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase
                });
            }
            catch (JsonException ex)
            {
                Debug.WriteLine($"Data conversion error: {ex.Message}");
                return default;
            }
        }
    }
}
