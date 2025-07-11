using CCIMIGRATION.Services;
using CCIMIGRATION.ConstantHelper;
using CCIMIGRATION.Interface;
using CCIMIGRATION.Models;
using CCIMIGRATION.MultilanguageHelper;
using CCIMIGRATION.Service.ApiService;
using CCIMIGRATION.ViewModels;
using CCIMIGRATION.Views.SiteManagerViews;
using Newtonsoft.Json;
using Plugin.Fingerprint;
using Plugin.Fingerprint.Abstractions;
using System.Diagnostics;
using ServiceHelper = CCIMIGRATION.Services.ServiceHelper;

namespace CCIMIGRATION.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AuthPage : ContentPage
    {
        private readonly IDialogService _dialogService;

        public AuthPage()
        {
            try
            {
                InitializeComponent();
                
                // Safely get the dialog service with fallback
                try
                {
                    _dialogService = ServiceHelper.GetService<IDialogService>() ?? new DialogService();
                }
                catch (Exception ex)
                {
                    Debug.WriteLine($"Error getting DialogService: {ex.Message}");
                    _dialogService = new DialogService();
                }
                
                try
                {
                    App._authVm = new AuthViewModel(_dialogService);
                    this.BindingContext = App._authVm;
                }
                catch (Exception ex)
                {
                    Debug.WriteLine($"Error creating AuthViewModel: {ex.Message}");
                    // Emergency fallback - navigate directly to login or main app
                    EmergencyFallback();
                    return;
                }
                
                Subscribe();
                App.RetakeStatus = String.Empty;
                App.RetakeError = String.Empty;
                
                // Delay biometric login to ensure everything is initialized
                Loaded += OnPageLoaded;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Critical error in AuthPage constructor: {ex.Message}");
                // Emergency fallback - navigate directly to login or main app
                EmergencyFallback();
            }
        }

        private async void OnPageLoaded(object sender, EventArgs e)
        {
            // Remove the event handler to avoid multiple calls
            Loaded -= OnPageLoaded;
            
            // Add a small delay to ensure everything is fully loaded
            await Task.Delay(100);
            
            // Now start the authentication process
            AskBiometricLogin();
        }
        
        protected override async void OnAppearing()
        {
            base.OnAppearing();
            if (!String.IsNullOrEmpty(App.RetakeStatus))
            {
                if (App.RetakeStatus == "true")
                {
                    var result = await App._authVm.SubmitPhotos();
                    if (result)
                    {
                        if (App.LoggedInUser.Role == "SiteManager")
                        {
                            App.Current.MainPage = new NavigationPage(new SiteManagerMenuPage());
                        }
                        else
                        {
                            App.Current.MainPage = new NavigationPage(new EmployeeMenuPage());
                        }
                    }
                    else
                    {
                        await App._authVm.ShowToastAsync("Something went wrong in Uploading Photos");
                    }
                }
                else
                {
                    await App._authVm.ShowToastAsync("Something went wrong in Retaking Photos");
                }
            }
        }

        private void Subscribe()
        {
            //MessagingService.Subscribe<string>(this, "PinSuccesful", async (sender) =>
            //{
            //    if (await UpdateLoginDetail(2))
            //    {
            //        App._authVm.CheckEmployeeImageStatus();
            //    }
            //    //await App.Current.MainPage.Navigation.PushAsync(new EmployeeMenuPage());
            //});
            MessagingService.Subscribe(this, "iOSPinResult", async (sender) =>
            {
                if (sender.ToString() == "Success")
                {
                    if (await UpdateLoginDetail(2))
                    {
                        App._authVm.CheckEmployeeImageStatus();
                    }
                }
                else
                    await App.Current.MainPage.DisplayAlert("Alert", "AuthenticationFailed", "Ok");
                //UserDialogs.Instance.Toast("Authentication Failed");
            });
        }

        private async void AskBiometricLogin()
        {
            try
            {
                Debug.WriteLine("Starting biometric authentication...");
                
                // Add a shorter timeout for the entire authentication process
                using (var cts = new CancellationTokenSource(TimeSpan.FromSeconds(10)))
                {
                    try
                    {
                        // Check if fingerprint is available first
                        bool isFingerprintAvailable = await CrossFingerprint.Current.IsAvailableAsync(false);
                        Debug.WriteLine($"Fingerprint available: {isFingerprintAvailable}");

                        if (DeviceInfo.Platform == DevicePlatform.iOS)
                        {
                            if (isFingerprintAvailable)
                            {
                                AuthenticationRequestConfiguration conf =
                                new AuthenticationRequestConfiguration(ResourceConstants.Authentication,
                                ResourceConstants.AuthenticateAccessToLoginToApp);
                                var authResult = await CrossFingerprint.Current.AuthenticateAsync(conf);
                                if (authResult.Authenticated)
                                {
                                    if (await UpdateLoginDetail(1))
                                    {
                                        Preferences.Set("IsFaceLock", true);
                                        App._authVm.CheckEmployeeImageStatus();
                                    }
                                    else
                                    {
                                        // Failed to update login, fall back to PIN
                                        FallbackToPin();
                                    }
                                }
                                else
                                {
                                    // Biometric authentication failed, use PIN
                                    FallbackToPin();
                                }
                            }
                            else
                            {
                                // Biometric not available, use PIN
                                FallbackToPin();
                            }
                        }
                        else if (DeviceInfo.Platform == DevicePlatform.Android)
                        {
                            var data = await CrossFingerprint.Current.GetAvailabilityAsync(false);
                            Debug.WriteLine($"Fingerprint availability: {data}");
                            
                            if (data == FingerprintAvailability.Available)
                            {
                                AuthenticationRequestConfiguration conf =
                              new AuthenticationRequestConfiguration(ResourceConstants.Authentication,
                                ResourceConstants.AuthenticateAccessToLoginToApp);

                                var authResult = await CrossFingerprint.Current.AuthenticateAsync(conf);
                                if (authResult.Authenticated)
                                {
                                    if (await UpdateLoginDetail(1))
                                    {
                                        Preferences.Set("IsFaceLock", true);
                                        App._authVm.CheckEmployeeImageStatus();
                                    }
                                    else
                                    {
                                        // Failed to update login, fall back to PIN
                                        FallbackToPin();
                                    }
                                }
                                else
                                {
                                    // Biometric authentication failed, use PIN
                                    FallbackToPin();
                                }
                            }
                            else
                            {
                                // Biometric not available, use PIN
                                FallbackToPin();
                            }
                        }
                        else
                        {
                            // Platform not supported or unknown, fall back to PIN
                            FallbackToPin();
                        }
                    }
                    catch (OperationCanceledException)
                    {
                        Debug.WriteLine("Biometric authentication timed out");
                        FallbackToPin();
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Biometric authentication error: {ex.Message}");
                // Critical: If biometric auth throws an exception, we must have a fallback
                FallbackToPin();
            }
        }

        private void FallbackToPin()
        {
            try
            {
                if (DeviceInfo.Platform == DevicePlatform.iOS)
                {
                    DependencyService.Get<ILocalAuth>()?.AuthenticatPin();
                }
                else if (DeviceInfo.Platform == DevicePlatform.Android)
                {
                    MessagingService.Send("CheckPin", "CheckPin");
                }
                else
                {
                    // If PIN authentication is also not available, go to login
                    FallbackToLogin();
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"PIN authentication fallback error: {ex.Message}");
                // Last resort: go to login
                FallbackToLogin();
            }
        }
        
        private void FallbackToLogin()
        {
            try
            {
                Debug.WriteLine("Authentication failed, falling back to login page");
                
                Application.Current.Dispatcher.Dispatch(() =>
                {
                    App.Current.MainPage = new NavigationPage(new LoginPage());
                });
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Fallback to login error: {ex.Message}");
            }
        }

        private async void SkipAuthentication()
        {
            try
            {
                // For development purposes - skip authentication and go to login
                Debug.WriteLine("Skipping authentication - going to login page");
                
                // Clear any existing user data since authentication failed
                Preferences.Remove(Constants.UserData);
                
                // Navigate to login page
                Application.Current.Dispatcher.Dispatch(() =>
                {
                    App.Current.MainPage = new NavigationPage(new LoginPage());
                });
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Skip authentication error: {ex.Message}");
                
                // Even if everything fails, go to login
                try
                {
                    Application.Current.Dispatcher.Dispatch(() =>
                    {
                        App.Current.MainPage = new NavigationPage(new LoginPage());
                    });
                }
                catch (Exception finalEx)
                {
                    Debug.WriteLine($"Final fallback error: {finalEx.Message}");
                }
            }
        }

        private void FingerprintImageClick(object sender, EventArgs e)
        {
            AskBiometricLogin();
        }

        private void EmergencyFallback()
        {
            try
            {
                Debug.WriteLine("Emergency fallback triggered - navigating to login page");
                
                Application.Current.Dispatcher.Dispatch(() =>
                {
                    try
                    {
                        // Always go to login page when emergency fallback is triggered
                        // This ensures user can authenticate properly
                        Debug.WriteLine("Navigating to login page");
                        App.Current.MainPage = new NavigationPage(new LoginPage());
                    }
                    catch (Exception ex)
                    {
                        Debug.WriteLine($"Emergency fallback navigation error: {ex.Message}");
                        // Last resort - create a simple page with login button
                        App.Current.MainPage = new ContentPage
                        {
                            Content = new Microsoft.Maui.Controls.StackLayout
                            {
                                Children = {
                                    new Label { Text = "Authentication Error", HorizontalOptions = LayoutOptions.Center },
                                    new Button { Text = "Go to Login", Command = new Command(() => 
                                        {
                                            App.Current.MainPage = new NavigationPage(new LoginPage());
                                        })
                                    }
                                },
                                VerticalOptions = LayoutOptions.Center
                            }
                        };
                    }
                });
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Emergency fallback error: {ex.Message}");
            }
        }

        private async Task<bool> UpdateLoginDetail(int value)
        {
            try
            {
                await _dialogService.ShowLoadingAsync("");
                var _request = new UserLoginType()
                {
                    UserId = Convert.ToInt32(App.LoggedInUser.Id),
                    LoginType = value
                };
                var result = await ApiHelper.CallApi(HttpMethod.Post, Constants.LoggedBy, JsonConvert.SerializeObject(_request), false);
                if (result.Status)
                {
                    return true;
                }
                else
                {
                    await App.Current.MainPage.DisplayAlert("Alert", result.Message, "Ok");
                    return false;
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                await App.Current.MainPage.DisplayAlert("Alert", ex.Message, "Ok");
                //UserDialogs.Instance.Toast(ex.Message);
                return false;
            }
            finally
            {
                _dialogService.HideLoading();
            }
        }
    }
}
