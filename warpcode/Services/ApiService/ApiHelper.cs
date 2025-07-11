using CCIMIGRATION.ConstantHelper;
using Newtonsoft.Json;
using System.Diagnostics;
using System.Net;
using System.Text;

namespace CCIMIGRATION.Service.ApiService
{
    public static class ApiHelper
    {

        private static string API_URL = Constants.BaseUrl;

        //public async static Task<ApiResponse> CallApi(HttpMethod method, string url, string postData = null,bool header = false)
        //{
        //    if (Connectivity.NetworkAccess != NetworkAccess.Internet)
        //    {
        //        return new ApiResponse() { Status = false, Message = Resources.Resource,NoInternet };
        //    }

        //    HttpWebRequest request = (HttpWebRequest)WebRequest.Create(API_URL + url);
        //    //timeout set to 200 seconds
        //    request.Timeout = 200000;
        //    request.ServerCertificateValidationCallback = delegate { return true; };
        //    if (header)
        //    {
        //        request.Headers.Add(HttpRequestHeader.Authorization, "Bearer " + App.Token);
        //    }

        //    request.Headers.Add("XApiKey", App.XApikey);
        //    request.Headers.Add("X-User-Token", App.DBKey);
        //    try
        //    {
        //        request.Method = method.ToString();

        //        if (postData != null)
        //        {
        //            var data = Encoding.UTF8.GetBytes(postData);
        //            request.ContentType = "application/json";

        //            using (Stream stream = await request.GetRequestStreamAsync())
        //            {
        //                stream.Write(data, 0, data.Length);
        //            }
        //        }
        //        Debug.WriteLine("request " + API_URL + url +" " +postData);
        //        using (WebResponse response = await request.GetResponseAsync())
        //        {
        //            using (Stream objStream = response.GetResponseStream())
        //            {
        //                var responseString = new StreamReader(objStream).ReadToEnd();
        //                Debug.WriteLine("response " + responseString);
        //                return JsonConvert.DeserializeObject<ApiResponse>(responseString);
        //            }
        //        }
        //    }

        //    catch (WebException ex)
        //    {
        //        var resp = ex.Response as HttpWebResponse;
        //        if (resp != null)
        //        {
        //            var responseString = new StreamReader(ex.Response.GetResponseStream()).ReadToEnd();
        //            Debug.WriteLine("exception " + responseString);
        //        }
        //        return new ApiResponse() { Status = false, Message = ex.Message };
        //    }
        //    catch(TimeoutException ex)
        //    {
        //        return new ApiResponse() { Status = false, Message = ex.Message };
        //    }
        //    catch (Exception)
        //    {
        //        Debug.WriteLine("Exception ");
        //        return null;

        //    }
        //    finally
        //    {
        //        request = null;
        //    }
        //}


        public async static Task<ApiResponse> CallApi(HttpMethod method, string url, string postData = null, bool header = false)
        {
            if (Connectivity.NetworkAccess != NetworkAccess.Internet)
            {
                return new ApiResponse() { Status = false, Message = ResourceConstants.NoInternet };
            }

            try
            {
                using (var client = new HttpClient())
                {
                    client.Timeout = TimeSpan.FromSeconds(60); // 60 second timeout
                    client.BaseAddress = new Uri(API_URL);
                    
                    // Set headers
                    client.DefaultRequestHeaders.Add("XApiKey", App.XApikey);
                    client.DefaultRequestHeaders.Add("X-User-Token", App.DBKey);
                    
                    if (header && !string.IsNullOrEmpty(App.Token))
                    {
                        client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", App.Token);
                    }

                    HttpResponseMessage response = null;
                    Debug.WriteLine($"[API REQUEST] {method} {API_URL + url}");
                    Debug.WriteLine($"[API REQUEST DATA] {postData}");

                    if (method == HttpMethod.Get)
                    {
                        response = await client.GetAsync(url);
                    }
                    else if (method == HttpMethod.Post)
                    {
                        var content = new StringContent(postData ?? "", Encoding.UTF8, "application/json");
                        response = await client.PostAsync(url, content);
                    }
                    else if (method == HttpMethod.Put)
                    {
                        var content = new StringContent(postData ?? "", Encoding.UTF8, "application/json");
                        response = await client.PutAsync(url, content);
                    }
                    else if (method == HttpMethod.Delete)
                    {
                        response = await client.DeleteAsync(url);
                    }

                    var responseString = await response.Content.ReadAsStringAsync();
                    Debug.WriteLine($"[API RESPONSE] Status: {response.StatusCode}, Body: {responseString}");

                    if (response.IsSuccessStatusCode)
                    {
                        try
                        {
                            return JsonConvert.DeserializeObject<ApiResponse>(responseString);
                        }
                        catch (JsonException)
                        {
                            // If JSON parsing fails, return a generic success response
                            return new ApiResponse() { Status = true, Message = "Success", Result = responseString };
                        }
                    }
                    else
                    {
                        return new ApiResponse() { Status = false, Message = $"HTTP {response.StatusCode}: {responseString}" };
                    }
                }
            }
            catch (HttpRequestException ex)
            {
                Debug.WriteLine($"[HTTP REQUEST EXCEPTION] {ex.Message}");
                return new ApiResponse() { Status = false, Message = $"Network error: {ex.Message}" };
            }
            catch (TaskCanceledException ex)
            {
                Debug.WriteLine($"[TIMEOUT EXCEPTION] {ex.Message}");
                return new ApiResponse() { Status = false, Message = "Request timeout. Please check your internet connection." };
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"[GENERAL EXCEPTION] {ex.ToString()}");
                return new ApiResponse() { Status = false, Message = $"An error occurred: {ex.Message}" };
            }
        }

        public static T ConvertResult<T>(object result)
        {
            return JsonConvert.DeserializeObject<T>(JsonConvert.SerializeObject(result));
        }
       
    }
    
    public class ApiResponse
    {
        public bool Status { get; set; }

        public string Message { get; set; }

        public object Result { get; set; }
    }
}
