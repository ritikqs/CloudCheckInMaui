using CCIMIGRATION.Services;
using CCIMIGRATION.ConstantHelper;
using CCIMIGRATION.Models;
using CCIMIGRATION.Service.ApiService;
using CCIMIGRATION.ViewModels;
using Newtonsoft.Json;
// using Plugin.Media; // REMOVED: Replace with Microsoft.Maui.Essentials.MediaPicker
// using Plugin.Media.Abstractions; // REMOVED: Replace with Microsoft.Maui.Essentials.MediaPicker
using System.Diagnostics;

namespace CCIMIGRATION.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class RegisterPage : ContentPage
    {
        public RegisterViewModel RegisterViewModel = new RegisterViewModel();

        public RegisterPage()
        {
            InitializeComponent();

            App.PhotoCaptureStatus = String.Empty;
            this.BindingContext = RegisterViewModel;
            datepicker.MaximumDate = DateTime.Now.Date;
            datepicker.Date = DateTime.Now.Date;
            datepicker.DateSelected += _date_DateSelected;
            datepicker.Focused += Datepicker_Focused;
        }

        private void Datepicker_Focused(object sender, FocusEventArgs e)
        {
            dobTextField.Text = datepicker.Date.ToString("dd MMM yyyy");
            RegisterViewModel.IsDobNotValid = false;
        }

        private void _date_DateSelected(object sender, DateChangedEventArgs e)
        {
            dobTextField.Text = e.NewDate.ToString("dd MMM yyyy");
            RegisterViewModel.IsDobNotValid = false;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            RegisterViewModel.GetData();
        }

        private void Login_Tapped(object sender, EventArgs e)
        {
            Navigation.PopAsync();
        }

        private void captureclicked(object sender, EventArgs e)
        {
            MessagingService.Send("Capture", "Capture");
        }

        public byte[] ImageSourceToBytes(ImageSource imageSource)
        {
            StreamImageSource streamImageSource = (StreamImageSource)imageSource;
            System.Threading.CancellationToken cancellationToken =
            System.Threading.CancellationToken.None;
            Task<Stream> task = streamImageSource.Stream(cancellationToken);
            Stream stream = task.Result;
            byte[] bytesAvailable = new byte[stream.Length];
            stream.Read(bytesAvailable, 0, bytesAvailable.Length);
            return bytesAvailable;
        }

        private void TapGestureRecognizer_Tapped(object sender, EventArgs e)
        {
            datepicker.Focus();
        }

        private void HomeButton_Clicked(object sender, EventArgs e)
        {
            //App.Current.MainPage = new NavigationPage(new StartupPage());
        }

        private void FirstNameTextChanged(object sender, TextChangedEventArgs e)
        {
            if (String.IsNullOrEmpty(firstname.Text) && String.IsNullOrWhiteSpace(firstname.Text))
            {
                RegisterViewModel.IsFirstNameNotValid = true;
            }
            else
            {
                if (!ValidationHelpers.NameRegex.IsMatch(firstname.Text))
                {
                    RegisterViewModel.IsFirstNameNotValid = true;
                }
                else
                {
                    RegisterViewModel.IsFirstNameNotValid = false;
                }
            }
        }

        private void NINumberTextChanged(object sender, TextChangedEventArgs e)
        {
            if (String.IsNullOrEmpty(ninumber.Text) && String.IsNullOrWhiteSpace(ninumber.Text))
            {
                RegisterViewModel.IsNINumberNotValid = true;
                ninumbervalidation.Text = "NI Validation"; // TODO: Fix Resource.NIValidation
            }
            else
            {
                if (!ValidationHelpers.AlphaNumber.IsMatch(ninumber.Text))
                {
                    RegisterViewModel.IsNINumberNotValid = true;
                    ninumbervalidation.Text = "NI Validation"; // TODO: Fix Resource.NIValidation
                }
                else
                {
                    RegisterViewModel.IsNINumberNotValid = false;
                }
            }
        }

        private void LastNameTextChanged(object sender, TextChangedEventArgs e)
        {
            if (String.IsNullOrEmpty(lastname.Text) && String.IsNullOrWhiteSpace(lastname.Text))
            {
                RegisterViewModel.IsLastNameNotValid = true;
            }
            else
            {
                if (!ValidationHelpers.NameRegex.IsMatch(lastname.Text))
                {
                    RegisterViewModel.IsLastNameNotValid = true;
                }
                else
                {
                    RegisterViewModel.IsLastNameNotValid = false;
                }
            }
        }

        private void MotherNameTextChanged(object sender, TextChangedEventArgs e)
        {
            if (String.IsNullOrEmpty(mothername.Text) && String.IsNullOrWhiteSpace(mothername.Text))
            {
                RegisterViewModel.IsMotherNameNotValid = true;
            }
            else
            {
                if (!ValidationHelpers.NameRegex.IsMatch(mothername.Text))
                {
                    RegisterViewModel.IsMotherNameNotValid = true;
                }
                else
                {
                    RegisterViewModel.IsMotherNameNotValid = false;
                }
            }
        }

        private void PasswordTextChanged(object sender, TextChangedEventArgs e)
        {
            if (String.IsNullOrEmpty(password.Text) && String.IsNullOrWhiteSpace(password.Text))
            {
                RegisterViewModel.IsPasswordNotValid = true;
            }
            else
            {
                if (password.Text.Length > 3)
                {
                    RegisterViewModel.IsPasswordNotValid = false;
                }
                else
                {
                    RegisterViewModel.IsPasswordNotValid = true;
                }
            }
        }

        private void EmailTextChanged(object sender, TextChangedEventArgs e)
        {
            if (String.IsNullOrEmpty(email.Text) && String.IsNullOrWhiteSpace(email.Text))
            {
                RegisterViewModel.IsEmailNotValid = true;
            }
            else
            {
                if (!ValidationHelpers.EmailRegex.IsMatch(email.Text))
                {
                    RegisterViewModel.IsEmailNotValid = true;
                }
                else
                {
                    RegisterViewModel.IsEmailNotValid = false;
                }
            }
        }

        private void OContractorTextChanged(object sender, TextChangedEventArgs e)
        {
            if (String.IsNullOrEmpty(otherContractor.Text) && String.IsNullOrWhiteSpace(otherContractor.Text))
            {
                RegisterViewModel.IsOtherContractorNotValid = true;
            }
            else
            {
                if (!ValidationHelpers.NameRegex.IsMatch(otherContractor.Text))
                {
                    RegisterViewModel.IsOtherContractorNotValid = true;
                }
                else
                {
                    RegisterViewModel.IsOtherContractorNotValid = false;
                }
            }
        }

        private void OtherTradeTextChanged(object sender, TextChangedEventArgs e)
        {
            if (String.IsNullOrEmpty(othertrade.Text) && String.IsNullOrWhiteSpace(othertrade.Text))
            {
                RegisterViewModel.IsOtherTradeNotValid = true;
            }
            else
            {
                if (!ValidationHelpers.NameRegex.IsMatch(othertrade.Text))
                {
                    RegisterViewModel.IsOtherTradeNotValid = true;
                }
                else
                {
                    RegisterViewModel.IsOtherTradeNotValid = false;
                }
            }
        }

        private void PhoneTextChanged(object sender, TextChangedEventArgs e)
        {
            if (String.IsNullOrEmpty(phone.Text) && String.IsNullOrWhiteSpace(phone.Text))
            {
                RegisterViewModel.IsPhoneNumberNotValid = true;
            }
            else
            {
                if (phone.Text.Length == 11 && ConstantHelper.ValidationHelpers.Number.IsMatch(phone.Text))
                {
                    RegisterViewModel.IsPhoneNumberNotValid = false;
                }
                else
                {
                    RegisterViewModel.IsPhoneNumberNotValid = true;
                }
            }
        }

        void EmailUnfocused(System.Object sender, FocusEventArgs e) // Updated to MAUI FocusEventArgs
        {
            if (!String.IsNullOrEmpty(email.Text) && !string.IsNullOrWhiteSpace(email.Text) && ValidationHelpers.EmailRegex.IsMatch(email.Text))
            {
                Application.Current.Dispatcher.Dispatch(async () =>
                {
                    try
                    {
                        emailcheckloader.IsVisible = true;
                        var req = new EmailExistCheck()
                        {
                            EmailOrNiNumber = email.Text,
                            Type = 1
                        };

                        var response = await ApiHelper.CallApi(HttpMethod.Post, Constants.CheckEmailOrNiNumberExist, JsonConvert.SerializeObject(req), false);
                        if (response.Status)
                        {
                            RegisterViewModel.IsEmailAvailable = true;
                        }
                        else
                        {
                            RegisterViewModel.IsEmailAvailable = false;
                            RegisterViewModel.IsEmailNotValid = true;
                            emailvalidation.Text = "Email Already Exist";
                        }
                    }
                    catch (Exception ex)
                    {
                        Debug.WriteLine(ex.Message);
                    }
                    finally
                    {
                        emailcheckloader.IsVisible = false;
                    }
                });
            }
        }

        void Ninumber_Unfocused(System.Object sender, FocusEventArgs e) // Updated to MAUI FocusEventArgs
        {
            RegisterViewModel.IsNiNumberAvailable = true;
        }
    }
}
