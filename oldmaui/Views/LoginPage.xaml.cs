using Microsoft.Maui.Controls;
using CloudCheckInMaui.ViewModels;

namespace CloudCheckInMaui.Views
{
    public partial class LoginPage : ContentPage
    {
        public LoginPage(LoginViewModel viewModel)
        {
            InitializeComponent();
            BindingContext = viewModel;
        }
    }
} 