using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CloudCheckInMaui.ViewModels;
using Microsoft.Maui.Controls;
using Microsoft.Extensions.DependencyInjection;

// File: Views/TimesheetPage.xaml.cs
namespace CloudCheckInMaui.Views
{
    public partial class TimesheetPage : ContentPage
    {
        private TimesheetViewModel _viewModel;

        public TimesheetPage()
        {
            InitializeComponent();
            _viewModel = IPlatformApplication.Current.Services.GetService<TimesheetViewModel>();
            BindingContext = _viewModel;
        }

        public TimesheetPage(TimesheetViewModel viewModel)
        {
            InitializeComponent();
            _viewModel = viewModel;
            BindingContext = _viewModel;
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            await _viewModel.GetTimeSheet();
        }
    }
}
