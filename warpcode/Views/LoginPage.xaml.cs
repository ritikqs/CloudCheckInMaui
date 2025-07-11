using CCIMIGRATION.ConstantHelper;
using CCIMIGRATION.CustomControls;
using CCIMIGRATION.ViewModels;
using CCIMIGRATION.Views.SiteManagerViews;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.Maui.Controls.Xaml;
using Microsoft.Maui.Controls.Compatibility;
using Microsoft.Maui.Controls;
using Microsoft.Maui;
using System.Diagnostics;

namespace CCIMIGRATION.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LoginPage : ContentPage
    {
        public LoginPageViewModel LoginViewModel = new LoginPageViewModel();
        
        public LoginPage()
        {
            InitializeComponent();
            this.BindingContext = LoginViewModel;
        }

        private void LoginTextChanged(object sender, TextChangedEventArgs e)
        {
            if (String.IsNullOrEmpty(email.Text) && String.IsNullOrWhiteSpace(email.Text))
            {
                LoginViewModel.IsEmailNotValid = true;
            }
            else if(!ValidationHelpers.EmailRegex.IsMatch(email.Text))
            {
                LoginViewModel.IsEmailNotValid = true;
            }
            else
            {
                LoginViewModel.IsEmailNotValid = false;
            }
        }

        private void PasswordTextChanged(object sender, TextChangedEventArgs e)
        {
            if (String.IsNullOrEmpty(password.Text) && String.IsNullOrWhiteSpace(password.Text))
            {
                LoginViewModel.IsPinNotValid = true;
            }
            else
            {
                LoginViewModel.IsPinNotValid = false;
            }
        }
    }
}
