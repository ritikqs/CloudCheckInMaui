using CloudCheckInMaui.Models;
using CloudCheckInMaui.Services.ApiService;
using Newtonsoft.Json;
using System;
using System.Threading.Tasks;
using System.Windows.Input;
using Microsoft.Maui.Controls;
using System.Net.Http;

namespace CloudCheckInMaui.ViewModels
{
    public class ForgotPasswordViewModel : BaseViewModel
    {
        private string _email;
        public string Email
        {
            get => _email;
            set => SetProperty(ref _email, value);
        }

        private string _pinCode;
        public string PinCode
        {
            get => _pinCode;
            set => SetProperty(ref _pinCode, value);
        }

        private string _newPassword;
        public string NewPassword
        {
            get => _newPassword;
            set => SetProperty(ref _newPassword, value);
        }

        private string _confirmPassword;
        public string ConfirmPassword
        {
            get => _confirmPassword;
            set => SetProperty(ref _confirmPassword, value);
        }

        private bool _isStepOne = true;
        public bool IsStepOne
        {
            get => _isStepOne;
            set => SetProperty(ref _isStepOne, value);
        }

        private bool _isStepTwo;
        public bool IsStepTwo
        {
            get => _isStepTwo;
            set => SetProperty(ref _isStepTwo, value);
        }

        private bool _isLoading;
        public bool IsLoading
        {
            get => _isLoading;
            set => SetProperty(ref _isLoading, value);
        }

        public ICommand SendPinCommand { get; }
        public ICommand ResetPasswordCommand { get; }
        public ICommand CancelCommand { get; }

        public ForgotPasswordViewModel()
        {
            Title = "Forgot Password";
            ResetPasswordCommand = new Command(async () => await ResetPassword());
        }

        private async Task SendPin()
        {
            if (string.IsNullOrEmpty(Email))
            {
                await DisplayAlert("Error", "Email is required", "OK");
                return;
            }

            try
            {
                App.ShowLoader();

                // Create forgot password request
                var forgotPasswordRequest = new ForgotPasswordRequest
                {
                    Email = Email
                };

                // Call forgot password API
                var response = await ApiHelper.CallApi(HttpMethod.Post, "account/forgotpassword", JsonConvert.SerializeObject(forgotPasswordRequest));
                
                if (response.Status)
                {
                    await DisplayAlert("Success", "A PIN code has been sent to your email", "OK");
                    IsStepOne = false;
                    IsStepTwo = true;
                }
                else
                {
                    await DisplayAlert("Error", response.Message, "OK");
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", ex.Message, "OK");
            }
            finally
            {
                App.HideLoader();
            }
        }

        private async Task ResetPassword()
        {
            if (string.IsNullOrEmpty(PinCode) || string.IsNullOrEmpty(NewPassword) || string.IsNullOrEmpty(ConfirmPassword))
            {
                await DisplayAlert("Error", "All fields are required", "OK");
                return;
            }

            if (NewPassword != ConfirmPassword)
            {
                await DisplayAlert("Error", "Passwords do not match", "OK");
                return;
            }

            try
            {
                App.ShowLoader();

                // Create reset password request
                var resetPasswordRequest = new ResetPasswordRequest
                {
                    Email = Email,
                    Password = NewPassword,
                    PinCode = PinCode
                };

                // Call reset password API
                var response = await ApiHelper.CallApi(HttpMethod.Post, "account/resetpassword", JsonConvert.SerializeObject(resetPasswordRequest));
                
                if (response.Status)
                {
                    await DisplayAlert("Success", "Your password has been reset successfully", "OK");
                    await NavigateBack();
                }
                else
                {
                    await DisplayAlert("Error", response.Message, "OK");
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", ex.Message, "OK");
            }
            finally
            {
                App.HideLoader();
            }
        }

        private async Task NavigateBack()
        {
            await Shell.Current.GoToAsync("..");
        }
    }
} 