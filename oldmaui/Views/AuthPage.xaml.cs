using CloudCheckInMaui.ViewModels;
using Microsoft.Maui.Controls;

namespace CloudCheckInMaui.Views
{
    public partial class AuthPage : ContentPage
    {
        public AuthPage(AuthViewModel viewModel)
        {
            InitializeComponent();
            BindingContext = viewModel;
        }
    }
} 