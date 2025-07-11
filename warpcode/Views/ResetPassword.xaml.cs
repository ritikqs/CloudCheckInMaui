using CCIMIGRATION.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Maui.Controls.Xaml;
using Microsoft.Maui.Controls.Compatibility;
using Microsoft.Maui.Controls;
using Microsoft.Maui;

namespace CCIMIGRATION.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class ResetPassword : ContentPage
	{
        public ResetPasswordViewModel ResetPasswordViewModel;
        public ResetPassword(string email)
        {
            InitializeComponent();

            ResetPasswordViewModel = new ResetPasswordViewModel(email);
            this.BindingContext = ResetPasswordViewModel;
        }

        private void ConfirmPasswordTextChanged(object sender, TextChangedEventArgs e)
        {
            if (String.IsNullOrEmpty(confirmPassword.Text) && String.IsNullOrWhiteSpace(confirmPassword.Text))
            {
                ResetPasswordViewModel.IsConfirmPasswordNotValid = true;
            }
            else
            {
                if(password.Text == confirmPassword.Text)
                {
                    ResetPasswordViewModel.IsConfirmPasswordNotValid = false;
                }
                else
                {
                    ResetPasswordViewModel.IsConfirmPasswordNotValid = true;

                }
            }
        }

        private void PasswordTextChanged(object sender, TextChangedEventArgs e)
        {
            if (String.IsNullOrEmpty(password.Text) && String.IsNullOrWhiteSpace(password.Text))
            {
                ResetPasswordViewModel.IsPasswordNotValid = true;
            }
            else
            {
                if(password.Text.Length > 3)
                {
                    ResetPasswordViewModel.IsPasswordNotValid = false;
                }
                else
                {
                    ResetPasswordViewModel.IsPasswordNotValid = true;
                }
               
            }
        }

        private void CodeTextChanged(object sender, TextChangedEventArgs e)
        {
            if (String.IsNullOrEmpty(code.Text) && String.IsNullOrWhiteSpace(code.Text))
            {
                ResetPasswordViewModel.IsCodeNotValid = true;
            }
            else
            {
                ResetPasswordViewModel.IsCodeNotValid = false;
            }
        }
    }
}
