using Microsoft.Maui.Controls;
using System;
using CloudCheckInMaui.ViewModels;
using CloudCheckInMaui.Services.ApiService;
using System.Diagnostics;

namespace CloudCheckInMaui.Views
{
    public partial class HolidayPage : ContentPage
    {
        private HolidayViewModel _viewModel;
        private bool _isInitialized;

        public HolidayPage()
        {
            try
            {
                // Ensure required resources exist before initialization
                if (!Application.Current.Resources.ContainsKey("NewThemeTextColor"))
                {
                    Application.Current.Resources.Add("NewThemeTextColor", Colors.Blue); // Fallback color
                }

                InitializeComponent();
                _viewModel = new HolidayViewModel();
                BindingContext = _viewModel;
                _isInitialized = false;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error in HolidayPage constructor: {ex.Message}");
                Debug.WriteLine($"Stack trace: {ex.StackTrace}");
            }
        }

        protected override void OnAppearing()
        {
            try
            {
                base.OnAppearing();

                if (!_isInitialized)
                {
                    InitializeViewModel();
                    _isInitialized = true;
                }

                // Ensure the holiday list is refreshed every time the page appears
                _viewModel.LoadHolidayRequests();

                Debug.WriteLine("Holiday page appeared in tab navigation");
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error in OnAppearing: {ex.Message}");
                Debug.WriteLine($"Stack trace: {ex.StackTrace}");
            }
        }

        private void InitializeViewModel()
        {
            try
            {
                if (Handler?.MauiContext != null)
                {
                    _viewModel = new HolidayViewModel();
                    BindingContext = _viewModel;
                    Debug.WriteLine("HolidayViewModel initialized successfully");
                }
                else
                {
                    Debug.WriteLine("Warning: Handler.MauiContext is null, deferring ViewModel initialization");
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error initializing ViewModel: {ex.Message}");
                Debug.WriteLine($"Stack trace: {ex.StackTrace}");
            }
        }

        private async void NewLeave_Tapped(object sender, EventArgs e)
        {
            try
            {
                await Navigation.PushAsync(new ApplyHolidayPage());
                // If using Shell navigation, use:
                // await Shell.Current.GoToAsync(nameof(ApplyHolidayPage));
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error in NewLeave_Tapped: {ex.Message}");
            }
        }
    }
} 