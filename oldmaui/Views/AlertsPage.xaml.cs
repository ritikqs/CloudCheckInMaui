using Microsoft.Maui.Controls;
using CloudCheckInMaui.ViewModels;
using Microsoft.Extensions.DependencyInjection;

namespace CloudCheckInMaui.Views
{
    public partial class AlertsPage : ContentPage
    {
        private readonly AlertsViewModel _viewModel;

        public AlertsPage()
        {
            InitializeComponent();
            _viewModel = IPlatformApplication.Current.Services.GetService<AlertsViewModel>();
            BindingContext = _viewModel;
        }

        public AlertsPage(AlertsViewModel viewModel)
        {
            InitializeComponent();
            _viewModel = viewModel;
            BindingContext = _viewModel;
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            await _viewModel.LoadAlertsAsync();
        }
    }
} 