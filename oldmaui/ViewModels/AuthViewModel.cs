using System;
using System.IO;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows.Input;
using CloudCheckInMaui.ConstantHelper;
using CloudCheckInMaui.Models;
using CloudCheckInMaui.Services;
using CloudCheckInMaui.Services.ApiService;
using CloudCheckInMaui.Services.FaceService;
using CloudCheckInMaui.Views;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Storage;
using Plugin.Fingerprint;
using Plugin.Fingerprint.Abstractions;
using System.Diagnostics; 

namespace CloudCheckInMaui.ViewModels
{
    public class AuthViewModel : BaseViewModel
    {
        private readonly IFaceRecognitionService _faceRecognitionService;
        private readonly IFingerprint _fingerprint;
        private readonly IPermissionService _permissionService;

        private bool _isAuthenticated;
        public bool IsAuthenticated
        {
            get => _isAuthenticated;
            set
            {
                _isAuthenticated = value;
                OnPropertyChanged(nameof(IsAuthenticated));
            }
        }
        
        private string _authMessage;
        public string AuthMessage
        {
            get => _authMessage;
            set
            {
                _authMessage = value;
                OnPropertyChanged(nameof(AuthMessage));
            }
        }

        public ICommand AuthenticateCommand => new Command(async () => await Authenticate());
        public ICommand BackCommand => new Command(async () => await Shell.Current.GoToAsync(".."));

        public AuthViewModel(
            IFaceRecognitionService faceRecognitionService,
            IPermissionService permissionService)
        {
            _faceRecognitionService = faceRecognitionService;
            _permissionService = permissionService;
            _fingerprint = CrossFingerprint.Current;
            
            AuthMessage = "Scan your fingerprint or enter PIN to continue";
            LoadUser();
            RefreshToken();

            // Subscribe to iOS PIN result message
            MessagingCenter.Subscribe<string>(this, "iOSPinResult", async (result) =>
            {
                if (result == "Success")
                {
                    if (await UpdateLoginType(2))
                    {
                        await CheckEmployeeImageStatus();
                    }
                }
                else
                {
                    await Shell.Current.DisplayAlert("Alert", "Authentication Failed", "OK");
                }
            });
        }

        private void LoadUser()
        {
            if (Preferences.Default.ContainsKey(ConstantHelper.Constants.UserData))
            {
                var userData = Preferences.Default.Get(ConstantHelper.Constants.UserData, "");
                App.LoggedInUser = JsonSerializer.Deserialize<User>(userData);
            }
        }

        private async Task Authenticate()
        {
            try
            {
                IsBusy = true;

                if (DeviceInfo.Platform == DevicePlatform.iOS)
                {
                    var isFingerprintAvailable = await _fingerprint.IsAvailableAsync(false);
                    if (isFingerprintAvailable)
                    {
                        var conf = new AuthenticationRequestConfiguration(
                            "Authentication",
                            "Use your fingerprint or PIN to authenticate");
                        
                        var authResult = await _fingerprint.AuthenticateAsync(conf);
                        if (authResult.Authenticated)
                        {
                            if (await UpdateLoginType(1))
                            {
                                Preferences.Default.Set("IsFaceLock", true);
                                await CheckEmployeeImageStatus();
                            }
                        }
                        else
                        {
                            DependencyService.Get<ILocalAuth>()?.AuthenticatPin();
                        }
                    }
                    else
                    {
                        DependencyService.Get<ILocalAuth>()?.AuthenticatPin();
                    }
                }
                else if (DeviceInfo.Platform == DevicePlatform.Android)
                {
                    var availability = await _fingerprint.GetAvailabilityAsync(false);
                    if (availability == FingerprintAvailability.Available)
                    {
                        var conf = new AuthenticationRequestConfiguration(
                            "Authentication",
                            "Use your fingerprint or PIN to authenticate");

                        var authResult = await _fingerprint.AuthenticateAsync(conf);
                        if (authResult.Authenticated)
                        {
                            if (await UpdateLoginType(1))
                            {
                                Preferences.Default.Set("IsFaceLock", true);
                                await CheckEmployeeImageStatus();
                            }
                        }
                    }
                    else
                    {
                        MessagingCenter.Send("CheckPin", "CheckPin");
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Authentication error: {ex.Message}");
                AuthMessage = "Authentication failed. Please try again.";
                IsAuthenticated = false;
            }
            finally
            {
                IsBusy = false;
            }
        }

        private async Task<bool> UpdateLoginType(int loginType)
        {
            try
            {
                var request = new UserLoginType
                {
                    UserId = App.LoggedInUser?.Id ?? 0,
                    LoginType = loginType
                };

                var response = await ApiHelper.CallApi(
                    HttpMethod.Post,
                    ConstantHelper.Constants.LoggedBy, 
                    JsonSerializer.Serialize(request));

                return response.Status;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Update login type error: {ex.Message}");
                await Shell.Current.DisplayAlert("Error", "Failed to update login type.", "OK");
                return false;
            }
        }

        public async Task CheckEmployeeImageStatus()
        {
            try
            {
                IsBusy = true;
                var response = await ApiHelper.CallApi(
                    HttpMethod.Get,
                    $"{ConstantHelper.Constants.EmployeeImageStatus}?employeId={App.LoggedInUser?.EmployeeId}",
                    null,
                    true);

                if (response.Status)
                {
                    await MainThread.InvokeOnMainThreadAsync(async () =>
                    {
                        await Shell.Current.GoToAsync("//MainApp");
                    });
                }
                else
                {
                    App.CameraFrom = "Retake";
                    if (DeviceInfo.Platform == DevicePlatform.iOS)
                    {
                        await MainThread.InvokeOnMainThreadAsync(async () =>
                        {
                            await Shell.Current.GoToAsync("Camera");
                        });
                    }
                    else
                    {
                        await Shell.Current.GoToAsync("Camera");
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Check employee status error: {ex.Message}");
                await Shell.Current.DisplayAlert("Error", "Failed to check employee status", "OK");
            }
            finally
            {
                IsBusy = false;
            }
        }

        private async Task RefreshToken()
        {
            try
            {
                if (string.IsNullOrEmpty(App.Token))
                {
                    await Shell.Current.GoToAsync("//Login");
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Token refresh error: {ex.Message}");
            }
        }

        public async Task<bool> ValidateRefreshTokenAsync()
        {
            if (string.IsNullOrEmpty(App.Token))
                return false;

            try
            {
                var request = new { Token = App.Token }; // Adjust to your API contract if needed
                var response = await ApiHelper.CallApi(
                    HttpMethod.Post,
                    ConstantHelper.Constants.ValidateRefreshToken,
                    System.Text.Json.JsonSerializer.Serialize(request),
                    true
                );

                if (response.Status)
                {
                    // Token is valid
                    return true;
                }
                else
                {
                    // Only redirect to login if we're not in the middle of a face verification process
                    // or if the user is not actively using the app
                    if (!App.IsCameraNavigated && !App.IsOnHomePage)
                    {
                        // Token is invalid, log out user
                        await Application.Current.MainPage.DisplayAlert("Session Expired", "Please log in again.", "OK");
                        App.Token = null;
                        App.LoggedInUser = null;
                        Preferences.Default.Remove(ConstantHelper.Constants.UserData);
                        await Shell.Current.GoToAsync("//Login");
                    }
                    return false;
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Token validation error: {ex.Message}");
                // Don't redirect to login on network errors or other exceptions
                return false;
            }
        }
    }
} 