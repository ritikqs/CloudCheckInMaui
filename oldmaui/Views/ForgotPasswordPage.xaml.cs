using Microsoft.Maui.Controls;
using System;
using CloudCheckInMaui.ViewModels;

namespace CloudCheckInMaui.Views
{
    public partial class ForgotPasswordPage : ContentPage
    {
        public ForgotPasswordPage(ForgotPasswordViewModel viewModel)
        {
            InitializeComponent();
            // Set the BindingContext to the injected ViewModel
            BindingContext = viewModel;
        }
    }
} 