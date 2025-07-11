using Microsoft.Maui.Controls;
using System;
using CloudCheckInMaui.ViewModels;

namespace CloudCheckInMaui.Views
{
    public partial class Alerts : ContentPage
    {
        public Alerts()
        {
            InitializeComponent();
        }

        public Alerts(AlertViewModel viewModel)
        {
            InitializeComponent();
            BindingContext = viewModel;
        }

        // This will run when page appears - verify we're in tab navigation
        protected override void OnAppearing()
        {
            base.OnAppearing();
            Console.WriteLine("Alerts page appeared in tab navigation");
        }
    }
}
