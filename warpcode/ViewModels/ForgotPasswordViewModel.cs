using CCIMIGRATION.ConstantHelper;
using CCIMIGRATION.Models;
using CCIMIGRATION.Service.ApiService;
using CCIMIGRATION.Views;
using Newtonsoft.Json;
using System.Windows.Input;

namespace CCIMIGRATION.ViewModels
{
    public class ForgotPasswordViewModel : BaseViewModel
    {
        #region Private/Public Variables
        private string _email = string.Empty;
        private bool _isEmailNotValid;
        private bool _isLoading;
        private bool _isSubmitEnabled;

        public bool IsEmailNotValid
        {
            get => _isEmailNotValid;
            set
            {
                if (_isEmailNotValid != value)
                {
                    _isEmailNotValid = value;
                    OnPropertyChanged();
                    UpdateSubmitButtonState();
                }
            }
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
                    UpdateSubmitButtonState();
                }
            }
        }
        
        public bool IsSubmitEnabled
        {
            get => _isSubmitEnabled;
            set
            {
                if (_isSubmitEnabled != value)
                {
                    _isSubmitEnabled = value;
                    OnPropertyChanged();
                }
            }
        }
        
        public string Email
        {
            get => _email;
            set
            {
                if (_email != value)
                {
                    _email = value;
                    OnPropertyChanged();
                    ValidateEmail();
                    UpdateSubmitButtonState();
                }
            }
        }

        #endregion
        #region Constructor
        public ForgotPasswordViewModel()
        {
            // Initialize with default values
            IsEmailNotValid = false;
            IsLoading = false;
            IsSubmitEnabled = false;
        }
        #endregion
        #region Commands
        private ICommand _submitButtonCommand;
        public ICommand SubmitButtonCommand
        {
            get
            {
                return _submitButtonCommand ??= new Command(async () =>
                {
                    await ExecuteSubmitAsync();
                }, () => IsSubmitEnabled && !IsLoading);
            }
        }

        #endregion
        #region Methods
        
        private void ValidateEmail()
        {
            if (string.IsNullOrWhiteSpace(Email))
            {
                IsEmailNotValid = true;
            }
            else if (!ValidationHelpers.EmailRegex.IsMatch(Email))
            {
                IsEmailNotValid = true;
            }
            else
            {
                IsEmailNotValid = false;
            }
        }
        
        private void UpdateSubmitButtonState()
        {
            IsSubmitEnabled = !IsEmailNotValid && 
                            !string.IsNullOrWhiteSpace(Email) && 
                            !IsLoading;
            
            // Refresh the CanExecute for the submit command
            ((Command)SubmitButtonCommand).ChangeCanExecute();
        }
        
        private async Task ExecuteSubmitAsync()
        {
            if (IsLoading || !ValidateInputs())
                return;
                
            try
            {
                IsLoading = true;
                
                var forgotPasswordRequest = CreateForgotPasswordRequest();
                var response = await ApiHelper.CallApi(HttpMethod.Post, Constants.ForgotPassword, JsonConvert.SerializeObject(forgotPasswordRequest));
                
                if (response.Status)
                {
                    await App.Current.MainPage.DisplayAlert(ResourceConstants.Success, ResourceConstants.CheckEmailForResetPassword, ResourceConstants.Ok);
                    await App.Current.MainPage.Navigation.PushAsync(new ResetPassword(Email));
                }
                else
                {
                    await ShowToastAsync(response.Message ?? "Failed to send reset email. Please try again.");
                }
            }
            catch (Exception ex)
            {
                WriteLog(ex.Message);
                await ShowToastAsync("An error occurred. Please try again.");
            }
            finally
            {
                IsLoading = false;
            }
        }
        
        private bool ValidateInputs()
        {
            ValidateEmail();
            return !IsEmailNotValid && !string.IsNullOrWhiteSpace(Email);
        }
        
        private ForgotPasswordRequest CreateForgotPasswordRequest()
        {
            return new ForgotPasswordRequest(){Email = Email};
        }
        #endregion
    }
}
