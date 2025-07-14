using Microsoft.Maui.Controls;
using System;
using CloudCheckInMaui.ViewModels;
using System.Diagnostics;
using Newtonsoft.Json;
using System.Net.Http;
using CloudCheckInMaui.Services.ApiService;
using CloudCheckInMaui.Models;

namespace CloudCheckInMaui.Views
{
    public partial class RegisterPage : ContentPage
    {
        private RegistrationViewModel ViewModel => BindingContext as RegistrationViewModel;

        public RegisterPage(RegistrationViewModel viewModel)
        {
            InitializeComponent();
            BindingContext = viewModel;
            ConfigureDatePicker();
        }

        public RegisterPage() : this(new RegistrationViewModel(null, null, null)) { }

        private void ConfigureDatePicker()
        {
            datepicker.MaximumDate = DateTime.Now.Date;
            datepicker.Date = DateTime.Now.Date;
        }

        private void FirstName_TextChanged(object sender, TextChangedEventArgs e)
        {
            ViewModel.FirstName = e.NewTextValue;
            ViewModel.ValidateFirstName();
        }

        private void LastName_TextChanged(object sender, TextChangedEventArgs e)
        {
            ViewModel.LastName = e.NewTextValue;
            ViewModel.ValidateLastName();
        }

        private void Email_TextChanged(object sender, TextChangedEventArgs e)
        {
            ViewModel.Email = e.NewTextValue;
            ViewModel.ValidateEmail();
        }

        private async void Email_Unfocused(object sender, FocusEventArgs e)
        {
            await ViewModel?.CheckEmailUniquenessAsync();
        }

        private void PhoneNumber_TextChanged(object sender, TextChangedEventArgs e)
        {
            ViewModel.PhoneNumber = e.NewTextValue;
            ViewModel.ValidatePhoneNumber();
        }

        private void Password_TextChanged(object sender, TextChangedEventArgs e)
        {
            ViewModel.Password = e.NewTextValue;
            ViewModel.ValidatePassword();
        }

        private void ConfirmPassword_TextChanged(object sender, TextChangedEventArgs e)
        {
            ViewModel.ConfirmPassword = e.NewTextValue;
            ViewModel.ValidateConfirmPassword();
        }

        private void NINumber_TextChanged(object sender, TextChangedEventArgs e)
        {
            ViewModel.NINumber = e.NewTextValue;
            ViewModel.ValidateNINumber();
        }

        private async void Ninumber_Unfocused(object sender, FocusEventArgs e)
        {
            await ViewModel?.CheckNINumberUniquenessAsync();
        }

        private void MotherName_TextChanged(object sender, TextChangedEventArgs e)
        {
            ViewModel.MotherMaidenName = e.NewTextValue;
            ViewModel.ValidateMotherName();
        }

        private void OContractorTextChanged(object sender, TextChangedEventArgs e)
        {
            ViewModel.OtherContractor = e.NewTextValue;
            ViewModel.IsOtherContractorNotValid = string.IsNullOrWhiteSpace(e.NewTextValue) ||
                !CloudCheckInMaui.ConstantHelper.ValidationHelpers.NameRegex.IsMatch(e.NewTextValue);
        }

        private void OtherTradeTextChanged(object sender, TextChangedEventArgs e)
        {
            ViewModel.OtherTrade = e.NewTextValue;
            ViewModel.IsOtherTradeNotValid = string.IsNullOrWhiteSpace(e.NewTextValue) ||
                !CloudCheckInMaui.ConstantHelper.ValidationHelpers.NameRegex.IsMatch(e.NewTextValue);
        }

        private void DatePicker_DateSelected(object sender, DateChangedEventArgs e)
        {
            ViewModel.DOB = e.NewDate;
            ViewModel.IsDobPickerVisible = false;
            ViewModel.ValidateDateOfBirth();
        }
    }
}