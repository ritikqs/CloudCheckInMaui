using CloudCheckInMaui.ConstantHelper;
using CloudCheckInMaui.Models;
using CloudCheckInMaui.Services.ApiService;
using CloudCheckInMaui.Services.FaceService;
using CloudCheckInMaui.Views;
using System;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Input;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Storage;
using System.Net.Http;
using System.Text.Json;
using CloudCheckInMaui.Commands;
using CloudCheckInMaui.Services.DeviceService;
using System.Linq;
using System.Text.RegularExpressions;
using Newtonsoft.Json;

namespace CloudCheckInMaui.ViewModels
{
    public class LoginViewModel : BaseViewModel
    {
        private readonly IFaceRecognitionService _faceRecognitionService;
        private readonly IDeviceInfoService _deviceInfoService;

        private string _email;
        public string Email
        {
            get => _email;
            set => SetProperty(ref _email, value);
        }

        private string _pin;
        public string Pin
        {
            get => _pin;
            set => SetProperty(ref _pin, value);
        }

        private bool _isRememberMe;
        public bool IsRememberMe
        {
            get => _isRememberMe;
            set => SetProperty(ref _isRememberMe, value);
        }

        private bool _isEmailNotValid;
        public bool IsEmailNotValid
        {
            get => _isEmailNotValid;
            set => SetProperty(ref _isEmailNotValid, value);
        }

        private bool _isPinNotValid;
        public bool IsPinNotValid
        {
            get => _isPinNotValid;
            set => SetProperty(ref _isPinNotValid, value);
        }

        // Email validation regex (copied from Xamarin ValidationHelpers)
        private static readonly Regex EmailRegex = new Regex(@"^([\w-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([\w-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$");

        public ICommand LoginCommand { get; }
        public ICommand ForgotPasswordCommand { get; }
        public ICommand RegisterCommand { get; }

        public LoginViewModel(
            IFaceRecognitionService faceRecognitionService,
            IDeviceInfoService deviceInfoService)
        {
            _faceRecognitionService = faceRecognitionService;
            _deviceInfoService = deviceInfoService;

            Title = "Login";
            
            LoginCommand = new Command(async () => await Login());
            ForgotPasswordCommand = new AsyncCommand(NavigateToForgotPassword);
            RegisterCommand = new AsyncCommand(NavigateToRegister);
            
            LoadSavedCredentials();
        }

        private bool ValidateInputs()
        {
            bool valid = true;
            if (string.IsNullOrEmpty(Email) || !EmailRegex.IsMatch(Email))
            {
                IsEmailNotValid = true;
                valid = false;
            }
            else
            {
                IsEmailNotValid = false;
            }
            if (string.IsNullOrEmpty(Pin))
            {
                IsPinNotValid = true;
                valid = false;
            }
            else
            {
                IsPinNotValid = false;
            }
            return valid;
        }

        private async Task Login()
        {
            Debug.WriteLine("\n=== Starting Login Process ===");
            Debug.WriteLine($"Attempting login with email: {Email}");

            if (!ValidateInputs())
            {
                await DisplayAlert("Error", "Please enter a valid email and PIN", "OK");
                return;
            }

            LoginResponse loginResponse = null;
            try
            {
                Debug.WriteLine("1. Showing loader...");
                await MainThread.InvokeOnMainThreadAsync(() => App.ShowLoader());

                // Step 2: Get device token
                Debug.WriteLine("4. Getting device token...");
                string deviceToken;
                try 
                {
                    deviceToken = await _deviceInfoService.GetDeviceToken();
                    Debug.WriteLine($"Device token generated successfully: {deviceToken.Substring(0, 20)}...");
                }
                catch (Exception ex)
                {
                    Debug.WriteLine($"Error getting device token: {ex.Message}");
                    deviceToken = Guid.NewGuid().ToString(); // Fallback token
                }

                // Step 3: Create login request
                Debug.WriteLine("5. Creating login request...");
                var loginRequest = new LoginRequest
                {
                    Email = Email?.Trim(),
                    Password = Pin?.Trim(),
                    DeviceToken = deviceToken
                };

                Debug.WriteLine("6. Calling login API...");
                var response = await ApiHelper.CallApi(
                    HttpMethod.Post, 
                    ConstantHelper.Constants.Login, 
                    System.Text.Json.JsonSerializer.Serialize(loginRequest));
                
                Debug.WriteLine($"7. API Response received. Status: {response.Status}");
                Debug.WriteLine($"Response Message: {response.Message}");
                
                if (!response.Status)
                {
                    Debug.WriteLine($"Login failed: {response.Message}");
                    await DisplayAlert("Login Failed", response.Message, "OK");
                    return;
                }

                // Step 2: Process Response
                Debug.WriteLine("8. Login successful, processing response...");
                loginResponse = response.GetData<LoginResponse>();
                Debug.WriteLine($"User Role: {loginResponse.Role}");
                Debug.WriteLine($"IsCapture: {loginResponse.IsCapture}");
                
                // Step 3: Handle Remember Me
                if (IsRememberMe)
                {
                    Debug.WriteLine("9. Saving credentials...");
                    await SaveCredentials();
                }
                else
                {
                    Debug.WriteLine("9. Clearing saved credentials...");
                    await ClearSavedCredentials();
                }

                // Step 4: Set App Data - All UI operations in one block
                Debug.WriteLine("10. Setting app data...");
                await MainThread.InvokeOnMainThreadAsync(async () =>
                {
                    try
                    {
                        Debug.WriteLine("Setting app data - Start");
                        // Defensive null checks
                        if (loginResponse.User == null)
                        {
                            Debug.WriteLine("Login response user is null");
                            await DisplayAlert("Error", "Login failed: user data missing.", "OK");
                            return;
                        }
                        App.LoggedInUser = loginResponse.User;
                        App.LoggedInUser.Token = loginResponse.AccessToken;
                        App.LoggedInUser.Role = loginResponse.Role;
                        App.LoggedInUser.RoleType = loginResponse.RoleType;
                        var data = JsonConvert.SerializeObject(loginResponse.User);
                        Preferences.Set("UserData", data);
                        Debug.WriteLine($"App data set - Role: {App.Role}, Token exists: {!string.IsNullOrEmpty(App.Token)}");

                        if (loginResponse.IsCapture)
                        {
                            await MainThread.InvokeOnMainThreadAsync(() => App.HideLoader());
                            Application.Current.MainPage = new AppShell();
                            try
                            {
                            await Shell.Current.GoToAsync("///MainApp");
                            }
                            catch (Exception navEx)
                            {
                                Debug.WriteLine($"Navigation to MainApp failed: {navEx.Message}");
                                await DisplayAlert("Error", "Navigation failed after login.", "OK");
                            }
                        }
                        else
                        {
                            await MainThread.InvokeOnMainThreadAsync(() => App.HideLoader());
                            App.CameraFrom = "Login";
                            if (!string.IsNullOrEmpty(loginResponse.EmployeeId) && Guid.TryParse(loginResponse.EmployeeId, out var empGuid))
                            {
                                App.LoggedInUser.EmployeeId = empGuid;
                            }
                            else
                            {
                                Debug.WriteLine($"Invalid EmployeeId: {loginResponse.EmployeeId}");
                                await DisplayAlert("Error", "Invalid EmployeeId received from server.", "OK");
                                return;
                            }
                            try
                            {
                            await Shell.Current.GoToAsync("///FaceCapture");
                            }
                            catch (Exception navEx)
                            {
                                Debug.WriteLine($"Navigation to FaceCapture failed: {navEx.Message}");
                                await DisplayAlert("Error", "Navigation failed after login.", "OK");
                            }
                        }
                    }
                    catch (Exception navEx)
                    {
                        Debug.WriteLine($"Navigation failed: {navEx.Message}");
                        await DisplayAlert("Error", navEx.Message, "OK");
                    }
                });
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Exception during login: {ex.Message}");
                await DisplayAlert("Error", ex.Message, "OK");
            }
        }

        private async Task SaveCredentials()
        {
            await SecureStorage.SetAsync("email", Email);
            await SecureStorage.SetAsync("pin", Pin);
            await SecureStorage.SetAsync("remember_me", "true");
        }

        private async Task ClearSavedCredentials()
        {
            SecureStorage.Remove("email");
            SecureStorage.Remove("pin");
            SecureStorage.Remove("remember_me");
        }

        private async Task LoadSavedCredentials()
        {
            var savedEmail = await SecureStorage.GetAsync("email");
            var savedPin = await SecureStorage.GetAsync("pin");
            var rememberMe = await SecureStorage.GetAsync("remember_me");

            if (!string.IsNullOrEmpty(savedEmail) && !string.IsNullOrEmpty(savedPin))
            {
                Email = savedEmail;
                Pin = savedPin;
                IsRememberMe = rememberMe == "true";
            }
        }

        private async Task NavigateToForgotPassword()
        {
            await Shell.Current.GoToAsync("///ForgotPassword");
        }

        private async Task NavigateToRegister()
        {
            await Shell.Current.GoToAsync("///Register");
        }
    }
} 