using Microsoft.Maui.Controls;
using System;
using CloudCheckInMaui.ViewModels;

namespace CloudCheckInMaui.Views
{
    public partial class ResetPassword : ContentPage
    {
        public ResetPassword(ForgotPasswordViewModel viewModel)
        {
            InitializeComponent();
            // Set the BindingContext to the injected ViewModel
            BindingContext = viewModel;
        }
    }
} 