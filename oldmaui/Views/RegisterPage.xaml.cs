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

            datepicker.DateSelected += DatePicker_DateSelected;
            datepicker.Focused += Datepicker_Focused;
        }

        public RegisterPage() : this(new RegistrationViewModel(null, null, null)) { }

        private void ConfigureDatePicker()
        {
            datepicker.MaximumDate = DateTime.Now.Date;
            datepicker.Date = DateTime.Now.Date;
        }

        private void FirstName_TextChanged(object sender, TextChangedEventArgs e)
        {
            ViewModel.IsFirstNameNotValid = string.IsNullOrWhiteSpace(e.NewTextValue) ||
                !CloudCheckInMaui.ConstantHelper.ValidationHelpers.NameRegex.IsMatch(e.NewTextValue);
        }

        private void LastName_TextChanged(object sender, TextChangedEventArgs e)
        {
            ViewModel.IsLastNameNotValid = string.IsNullOrWhiteSpace(e.NewTextValue) ||
                !CloudCheckInMaui.ConstantHelper.ValidationHelpers.NameRegex.IsMatch(e.NewTextValue);
        }

        private void Email_TextChanged(object sender, TextChangedEventArgs e)
        {
            ViewModel?.ValidateEmail();
        }

        private async void Email_Unfocused(object sender, FocusEventArgs e)
        {
            await ViewModel?.CheckEmailUniquenessAsync();
        }

        private void PhoneNumber_TextChanged(object sender, TextChangedEventArgs e)
        {
            ViewModel.IsPhoneNumberNotValid = string.IsNullOrWhiteSpace(e.NewTextValue) ||
                !System.Text.RegularExpressions.Regex.IsMatch(e.NewTextValue, @"^[0-9]+$") ||
                e.NewTextValue.Length != 11;
        }

        private void Password_TextChanged(object sender, TextChangedEventArgs e)
        {
            ViewModel?.ValidatePassword();
        }

        private void ConfirmPassword_TextChanged(object sender, TextChangedEventArgs e)
        {
            ViewModel?.ValidateConfirmPassword();
        }

        private void NINumber_TextChanged(object sender, TextChangedEventArgs e)
        {
            ViewModel.IsNINumberNotValid = string.IsNullOrWhiteSpace(e.NewTextValue) ||
                !CloudCheckInMaui.ConstantHelper.ValidationHelpers.AlphaNumber.IsMatch(e.NewTextValue);
        }

        private async void Ninumber_Unfocused(object sender, FocusEventArgs e)
        {
            await ViewModel?.CheckNINumberUniquenessAsync();
        }

        private void MotherName_TextChanged(object sender, TextChangedEventArgs e)
        {
            ViewModel.IsMotherNameNotValid = string.IsNullOrWhiteSpace(e.NewTextValue) ||
                !CloudCheckInMaui.ConstantHelper.ValidationHelpers.NameRegex.IsMatch(e.NewTextValue);
        }

        private void OContractorTextChanged(object sender, TextChangedEventArgs e)
        {
            ViewModel.IsOtherContractorNotValid = string.IsNullOrWhiteSpace(e.NewTextValue) ||
                !CloudCheckInMaui.ConstantHelper.ValidationHelpers.NameRegex.IsMatch(e.NewTextValue);
        }

        private void OtherTradeTextChanged(object sender, TextChangedEventArgs e)
        {
            ViewModel.IsOtherTradeNotValid = string.IsNullOrWhiteSpace(e.NewTextValue) ||
                !CloudCheckInMaui.ConstantHelper.ValidationHelpers.NameRegex.IsMatch(e.NewTextValue);
        }

        private void TapGestureRecognizer_Tapped(object sender, EventArgs e)
        {
            ViewModel.IsDobPickerVisible = true;
            datepicker.Focus();
        }

        private void DatePicker_DateSelected(object sender, DateChangedEventArgs e)
        {
            dobTextField.Text = e.NewDate.ToString("dd MMM yyyy");
            ViewModel.DOB = e.NewDate.ToString("yyyy-MM-dd");
            ViewModel.IsDobNotValid = false;
            ViewModel.IsDobPickerVisible = false;
        }

        private void Datepicker_Focused(object sender, FocusEventArgs e)
        {
            dobTextField.Text = datepicker.Date.ToString("dd MMM yyyy");
            ViewModel.DOB = datepicker.Date.ToString("yyyy-MM-dd");
            ViewModel.IsDobNotValid = false;
            ViewModel.IsDobPickerVisible = false;
        }
    }
}
