using Microsoft.Maui.Controls;
using System;
using CloudCheckInMaui.ViewModels;
using Microsoft.Maui.ApplicationModel;
using System.Threading.Tasks;

namespace CloudCheckInMaui.Views
{
    public partial class Home : ContentPage
    {
        private readonly HomeViewModel _viewModel;
        
        public Home()
        {
            InitializeComponent();
            _viewModel = IPlatformApplication.Current.Services.GetService<HomeViewModel>();
            BindingContext = _viewModel;
        }

        public Home(HomeViewModel viewModel)
        {
            InitializeComponent();
            _viewModel = viewModel;
            BindingContext = _viewModel;
        }

        // Use the cleaner approach with MAUI Shell for tab navigation
        public Command GoToAlertsCommand => new Command(() => {
            _viewModel.SwitchTab(4); // 4 is the index for Alerts tab
        });

        // Use the cleaner approach with MAUI Shell for tab navigation
        public Command GoToHolidayCommand => new Command(() => {
            _viewModel.SwitchTab(2); // 2 is the index for Holiday tab
        });

        protected override void OnAppearing()
        {
            base.OnAppearing();
            App.IsOnHomePage = true;
            InitializeAsync().ConfigureAwait(false);
        }

        private async Task InitializeAsync()
        {
            try
            {
                await _viewModel.CheckLocationStatus();
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", "Failed to check location status: " + ex.Message, "OK");
            }
        }
        
        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            App.IsOnHomePage = false;
            // _viewModel.Cleanup(); // Removed to preserve timer and event hooks across tab switches
        }

        // Event handlers can be async void as they are event handlers
        private async void TapGestureRecognizer_Tapped(object sender, EventArgs e)
        {
            if (IsBusy)
                return;

            try
            {
                IsBusy = true;
                await OpenLocationSettings();
            }
            finally
            {
                IsBusy = false;
            }
        }

        private async Task OpenLocationSettings()
        {
            try
            {
                await Task.Run(() => AppInfo.Current.ShowSettingsUI());
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", "Unable to open location settings: " + ex.Message, "OK");
            }
        }
    }
} 