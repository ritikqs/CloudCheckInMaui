using CCIMIGRATION.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Maui.Controls.Xaml;
using Microsoft.Maui.Controls.Compatibility;
using Microsoft.Maui.Controls;
using Microsoft.Maui;

namespace CCIMIGRATION.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ApplyHolidayPage : ContentPage
    {
        public ApplyHolidayPageViewModel ViewModel;
        public ApplyHolidayPage()
        {
            InitializeComponent();
            startDate.MinimumDate = DateTime.Now.Date;
            endDate.MinimumDate = DateTime.Now.Date;
            ViewModel = new ApplyHolidayPageViewModel();
            this.BindingContext = ViewModel;
            SetInitialValues();
            startDate.DateSelected += StartDate_DateSelected;
            endDate.DateSelected += EndDate_DateSelected;
            starttimepicker.PropertyChanged += Starttimepicker_PropertyChanged;
            endTimePicker.PropertyChanged += EndTimePicker_PropertyChanged;
        }

        private void EndTimePicker_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            var newdate = new DateTime();
            var date = newdate + endTimePicker.Time;
            endTimeLabel.Text = date.ToString("hh:mm tt");
        }

        private void Starttimepicker_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            var newdate = new DateTime();
            var date = newdate + starttimepicker.Time;
            startTimeLabel.Text = date.ToString("hh:mm tt");
        }

        private void EndDate_DateSelected(object sender, DateChangedEventArgs e)
        {
            endDateLabel.Text = endDate.Date.ToString("dd MMMM, yyyy");
        }

        private void StartDate_DateSelected(object sender, DateChangedEventArgs e)
        {
            startDateLabel.Text = startDate.Date.ToString("dd MMMM, yyyy");
        }

        private void SetInitialValues()
        {
            startDateLabel.Text = DateTime.Now.Date.ToString("dd MMMM, yyyy");
            endDateLabel.Text = DateTime.Now.Date.ToString("dd MMMM, yyyy");
            startTimeLabel.Text = DateTime.Now.ToString("hh:mm tt");
            endTimeLabel.Text = DateTime.Now.ToString("hh:mm tt");
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            ViewModel.SetDefaultDates();
            
        }
        private void TapGestureRecognizer_Tapped(object sender, EventArgs e)
        {
            Navigation.PopAsync();
        }

        private void EndTimePickerTapped(object sender, EventArgs e)
        {
            endTimePicker.Focus();
        }

        private void StartDatePickerTapped(object sender, EventArgs e)
        {
            startDate.Focus();
        }

        private void StartTimePickerTapped(object sender, EventArgs e)
        {
            starttimepicker.Focus();
        }

        private void EndDatePickerTapped(object sender, EventArgs e)
        {
            endDate.Focus();
        }
    }
}
