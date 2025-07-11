using CCIMIGRATION.Services;
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
    public partial class HolidayPage : ContentPage
    {
        public HolidayViewModel HolidayPageViewModel = new HolidayViewModel();

        public HolidayPage()
        {
            InitializeComponent();
            this.BindingContext = HolidayPageViewModel;
            holidayList.ItemSelected += HolidayList_ItemSelected;
        }

        private void HolidayList_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            holidayList.SelectedItem = null;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            HolidayPageViewModel.LoadHolidayRequests();
        }

        private void TapGestureRecognizer_Tapped(object sender, EventArgs e)
        {
            MessagingService.Send("Open", "OpenMenu");
        }

        private void NewLeave_Tapped_1(object sender, EventArgs e)
        {
            Navigation.PushAsync(new ApplyHolidayPage());
        }
    }
}
