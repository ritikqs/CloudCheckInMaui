using Microsoft.Maui.Controls;
using CloudCheckInMaui.ViewModels;

namespace CloudCheckInMaui.Views.Headers
{
    public partial class CustomTitleBar : ContentView
    {
        // Default constructor - required for XAML initialization
        public CustomTitleBar()
        {
            InitializeComponent();
        }

        // Constructor for dependency injection
        public CustomTitleBar(HeaderViewModel viewModel)
        {
            InitializeComponent();
            // Set the BindingContext to the injected ViewModel
            BindingContext = viewModel;
        }
    }
} 