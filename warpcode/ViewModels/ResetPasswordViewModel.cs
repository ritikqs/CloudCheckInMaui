using CCIMIGRATION.ConstantHelper;
using CCIMIGRATION.Models;
using CCIMIGRATION.Service.ApiService;
using CCIMIGRATION.Views;
using Newtonsoft.Json;
using System.Windows.Input;

namespace CCIMIGRATION.ViewModels
{
    public class ResetPasswordViewModel : BaseViewModel
    {
        #region Private/Public Variables
        private string _code;
        private string _email;
        private string _password;
        private string _confirmPassword;
        private bool _isCodeNotValid;
        private bool _isPasswordNotValid;
        private bool _isConfirmPasswordNotValid;
        
        public bool IsCodeNotValid
        {
            get { return _isCodeNotValid; }
            set { _isCodeNotValid = value;OnPropertyChanged(); }
        }
        public bool IsPasswordNotValid
        {
            get { return _isPasswordNotValid; }
            set { _isPasswordNotValid = value; OnPropertyChanged(); }
        }
        public bool IsConfirmPasswordNotValid
        {
            get { return _isConfirmPasswordNotValid; }
            set { _isConfirmPasswordNotValid = value; OnPropertyChanged(); }
        }
        public string Email
        {
            get { return _email; }
            set { _email = value; OnPropertyChanged("Email");}
        }
        public string Code
        {
            get { return _code; }
            set { _code = value; OnPropertyChanged("Code"); }
        }
        public string Password
        {
            get { return _password; }
            set { _password = value; OnPropertyChanged("Password");}
        }
        public string ConfirmPassword
        {
            get { return _confirmPassword; }
            set { _confirmPassword = value; OnPropertyChanged("ConfirmPassword"); }
        }
        #endregion
        #region Commands
        public ICommand SubmitButtonCommand
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
                            var response = await ApiHelper.CallApi(HttpMethod.Post, Constants.ResetPassword, JsonConvert.SerializeObject(CreateResetPasswordRequest()), true);
                            if (response.Status)
                            {
                                await App.Current.MainPage.DisplayAlert("Success", ResourceConstants.PasswordResetSuccess, ResourceConstants.Ok);
                                
                                App.Current.MainPage = new NavigationPage(new LoginPage());
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
                        ShowToast(ex.Message);
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
        public ResetPasswordViewModel(String email)
        {
            Email = email;
        }
        #endregion
        #region Methods
        private ResetPasswordRequest CreateResetPasswordRequest()
        {
            return new ResetPasswordRequest()
            {
                Email = Email,
                PinCode = Code,
                Password = Password,
            };
        }
        private bool ValidateInputs()
        {
            if(!String.IsNullOrEmpty(Code) && !String.IsNullOrEmpty(Password) &&  Password == ConfirmPassword && !IsCodeNotValid && !IsPasswordNotValid && !IsConfirmPasswordNotValid)
            {
                return true;
            }
            else
            {
                CheckNullStrings();
                return false;
            }
        }
        private void CheckNullStrings()
        {

            if (!String.IsNullOrEmpty(Code))
                IsCodeNotValid = false;
            else
                IsCodeNotValid = true;
            if (!String.IsNullOrEmpty(Password) && Password.Length > 3)
                IsPasswordNotValid = false;
            else
                IsPasswordNotValid = true;
            if (!String.IsNullOrEmpty(ConfirmPassword) && Password == ConfirmPassword)
                IsConfirmPasswordNotValid = false;
            else
                IsConfirmPasswordNotValid = true;
            
        }
        #endregion
    }
}
