using CCIMIGRATION.ConstantHelper;
using CCIMIGRATION.CustomControls;
using CCIMIGRATION.Models;
using CCIMIGRATION.Service.ApiService;
using CCIMIGRATION.Views;
using Newtonsoft.Json;
using System.Windows.Input;
using System.ComponentModel;

namespace CCIMIGRATION.ViewModels
{
    public class LoginPageViewModel : BaseViewModel
    {
        #region Private/Public Variables
        
        private string _email = string.Empty;
        private string _pin = string.Empty;
        private bool _isEmailNotValid;
        private bool _isPinNotValid;
        private bool _isLoading;
        private bool _isLoginEnabled;
        
        public LoginResponse UserData = new LoginResponse();
        
        public bool IsEmailNotValid
        {
            get { return _isEmailNotValid; }
            set { _isEmailNotValid = value; OnPropertyChanged(); }
        }
        
        public bool IsPinNotValid
        {
            get { return _isPinNotValid; }
            set { _isPinNotValid = value; OnPropertyChanged(); }
        }
        
        public bool IsLoading
        {
            get => _isLoading;
            set
            {
                if (_isLoading != value)
                {
                    _isLoading = value;
                    OnPropertyChanged();
                }
            }
        }
        
        public bool IsLoginEnabled
        {
            get => _isLoginEnabled;
            set
            {
                if (_isLoginEnabled != value)
                {
                    _isLoginEnabled = value;
                    OnPropertyChanged();
                }
            }
        }
        
        public string Pin
        {
            get { return _pin; }
            set { _pin = value; OnPropertyChanged("Pin"); }
        }
        
        public string Email
        {
            get { return _email; }
            set { _email = value; OnPropertyChanged("Email"); }
        }
        #endregion
        #region Commands
        
        private ICommand _registerButtonCommand;
        public ICommand RegisterButtonCommand
        {
            get
            {
                return new Command(() =>
                {
                    try
                    {
                        App.Current.MainPage.Navigation.PushAsync(new RegisterPage());
                    }
                    catch(Exception ex)
                    {
                        App.Current.MainPage.DisplayAlert("Exception", ex.Message, "Ok");
                    }
                });
            }
        }
        
        private ICommand _forgotPasswordCommand;
        public ICommand ForgotPasswordCommand
        {
            get
            {
                return new Command(() =>
                {
                    try
                    {
                        App.Current.MainPage.Navigation.PushAsync(new ForgotPasswordPage());
                    }
                    catch (Exception ex)
                    {
                        WriteLog(ex.Message);
                    }
                });
            }
        }
        
        private ICommand _loginButtonCommand;
        public ICommand LoginButtonCommand
        {
            get
            {
                return new Command(async () =>
                {
                    try
                    {
                        if (ValidateInputs())
                        {
                            ShowLoader("");
                            var response = await ApiHelper.CallApi(HttpMethod.Post, Constants.Login, JsonConvert.SerializeObject(CreateLoginRequest()));
                            if (response.Status)
                            {
                                UserData = ApiHelper.ConvertResult<LoginResponse>(response.Result);
                               
                                if (UserData.IsCapture)
                                {
                                    UserData.User.EmployeeId = UserData.EmployeeId;
                                    UserData.User.Token = UserData.AccessToken;
                                    UserData.User.Role = UserData.Role.ToString();
                                    UserData.User.RoleType = UserData.RoleType;

                                    App.Token = UserData.AccessToken;
                                    App.LoggedInUser = UserData.User;
                                    var data = JsonConvert.SerializeObject(UserData.User);
                                    Preferences.Set(Constants.UserData, data);
                                    App.Current.MainPage = new TransitionNavigationPage(new HomeTabbedPage());
                                }
                                else
                                {
                                    App.CameraFrom = "Login";
                                    App.LoggedInUser = UserData.User;
                                    App.LoggedInUser.EmployeeId = UserData.EmployeeId;
                                    await App.Current.MainPage.Navigation.PushModalAsync(new CameraPageView());
                                }
                            }
                            else
                            {
                                ShowToast(response.Message);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        WriteLog(ex.Message);
                    }
                    finally
                    {
                        HideLoader();
                    }
                });
            }
        }


        #endregion
        #region Constructor
        public LoginPageViewModel()
        {
            
        }

        #endregion
        #region Methods
        
        public async void CheckEmployeeImageStatus()
        {
            try
            {
                var response = await ApiHelper.CallApi(HttpMethod.Get, Constants.EmployeeImageStatus + "?employeId=" + App.LoggedInUser.EmployeeId, null, true);
                if (response.Status)
                {
                    Application.Current.Dispatcher.Dispatch(() =>
                    {
                        App.Current.MainPage = new TransitionNavigationPage(new HomeTabbedPage());
                    });
                }
                else
                {
                    App.CameraFrom = "Retake";
                    if (DeviceInfo.Platform == DevicePlatform.iOS)
                    {
                        Application.Current.Dispatcher.Dispatch(async () =>
                        {
                            await App.Current.MainPage.Navigation.PushModalAsync(new CameraPageView());
                        });
                    }
                    else
                    {
                        await App.Current.MainPage.Navigation.PushModalAsync(new CameraPageView());
                    }
                }
            }
            catch (Exception ex)
            {
                WriteLog(ex.Message);
                await ShowToastAsync(ex.Message);
            }
        }
        
        private LoginRequest CreateLoginRequest()
        {
            if(DeviceInfo.Platform == DevicePlatform.iOS)
            {
                return new LoginRequest() { Email = Email, Password = Pin, DeviceToken = ""};
            }
            else
            {
                return new LoginRequest() { Email = Email, Password = Pin, DeviceToken = App.DeviceToken ?? "" };
            }
        }
        
        private bool ValidateInputs()
        {
            if (String.IsNullOrEmpty(Email) && String.IsNullOrEmpty(Pin))
            {
                IsEmailNotValid = true;
                IsPinNotValid = true;
                return false;
            }
            else if (!string.IsNullOrEmpty(Email) && ValidationHelpers.EmailRegex.IsMatch(Email) && !string.IsNullOrEmpty(Pin))
            {
                IsEmailNotValid = false;
                IsPinNotValid = false;
                return true;
            }
            else
            {
                if (string.IsNullOrEmpty(Email))
                {
                    IsEmailNotValid = true;
                    return false;
                }
                else if (String.IsNullOrEmpty(Pin))
                {
                    IsPinNotValid = true;
                    return false;
                }
                else
                {
                    return false;
                }
            }
        }

        #endregion
    }
}
