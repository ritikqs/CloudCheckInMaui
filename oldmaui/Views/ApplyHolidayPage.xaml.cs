using Microsoft.Maui.Controls;
using CloudCheckInMaui.ViewModels;
using System;

namespace CloudCheckInMaui.Views
{
    public partial class ApplyHolidayPage : ContentPage
    {
        private ApplyHolidayPageViewModel _viewModel;

        public ApplyHolidayPage()
        {

            InitializeComponent();
            startDate.MinimumDate = DateTime.Now.Date;
            endDate.MinimumDate = DateTime.Now.Date;
            _viewModel = new ApplyHolidayPageViewModel();
             this.BindingContext = _viewModel;
            SetInitialValues();

            startDate.DateSelected += StartDate_DateSelected;
            endDate.DateSelected += EndDate_DateSelected;
            starttimepicker.PropertyChanged += Starttimepicker_PropertyChanged;
            endTimePicker.PropertyChanged += EndTimePicker_PropertyChanged;

            startDateLabel.GestureRecognizers.Clear();
            var startDateTap = new TapGestureRecognizer();
            startDateTap.Tapped += StartDatePickerTapped;
            startDateLabel.GestureRecognizers.Add(startDateTap);

            endDateLabel.GestureRecognizers.Clear();
            var endDateTap = new TapGestureRecognizer();
            endDateTap.Tapped += EndDatePickerTapped;
            endDateLabel.GestureRecognizers.Add(endDateTap);

            startTimeLabel.GestureRecognizers.Clear();
            var startTimeTap = new TapGestureRecognizer();
            startTimeTap.Tapped += StartTimePickerTapped;
            startTimeLabel.GestureRecognizers.Add(startTimeTap);

            endTimeLabel.GestureRecognizers.Clear();
            var endTimeTap = new TapGestureRecognizer();
            endTimeTap.Tapped += EndTimePickerTapped;
            endTimeLabel.GestureRecognizers.Add(endTimeTap);
        }

        private void SetInitialValues()
        {
            startDateLabel.Text = DateTime.Now.Date.ToString("dd MMMM, yyyy");
            endDateLabel.Text = DateTime.Now.Date.ToString("dd MMMM, yyyy");
            startTimeLabel.Text = DateTime.Now.ToString("hh:mm tt");
            endTimeLabel.Text = DateTime.Now.ToString("hh:mm tt");
        }

        private void StartDate_DateSelected(object sender, DateChangedEventArgs e)
        {
            startDateLabel.Text = startDate.Date.ToString("dd MMMM, yyyy");
        }

        private void EndDate_DateSelected(object sender, DateChangedEventArgs e)
        {
            endDateLabel.Text = endDate.Date.ToString("dd MMMM, yyyy");
        }

        private void Starttimepicker_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "Time")
            {
                var newdate = new DateTime();
                var date = newdate + starttimepicker.Time;
                startTimeLabel.Text = date.ToString("hh:mm tt");
            }
        }

        private void EndTimePicker_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "Time")
            {
                var newdate = new DateTime();
                var date = newdate + endTimePicker.Time;
                endTimeLabel.Text = date.ToString("hh:mm tt");
            }
        }

        private void StartDatePickerTapped(object sender, EventArgs e)
        {
            startDate.IsVisible = true;
            startDate.Focus();
        }

        private void EndDatePickerTapped(object sender, EventArgs e)
        {
            endDate.IsVisible = true;
            endDate.Focus();
        }

        private void StartTimePickerTapped(object sender, EventArgs e)
        {
            starttimepicker.IsVisible = true;
            starttimepicker.Focus();
        }

        private void EndTimePickerTapped(object sender, EventArgs e)
        {
            endTimePicker.IsVisible = true;
            endTimePicker.Focus();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            _viewModel.IsHolidaySuccessAppliedViewVisible = false;
            _viewModel.SetDefaultDates();
        }
    }
} 