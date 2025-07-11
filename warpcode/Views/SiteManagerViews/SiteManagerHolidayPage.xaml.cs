using CCIMIGRATION.SiteManagerViewModel;
using CCIMIGRATION.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Maui.Controls.Xaml;
using Microsoft.Maui.Controls.Compatibility;
using Microsoft.Maui.Controls;
using Microsoft.Maui;

namespace CCIMIGRATION.SiteManagerViews
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SiteManagerHolidayPage : ContentPage
    {
        SiteManagerHolidayPageViewModel HolidayViewModel = new SiteManagerHolidayPageViewModel();
        public SiteManagerHolidayPage()
        {
            InitializeComponent();
            this.BindingContext = HolidayViewModel;
        }
        protected override void OnAppearing()
        {
            base.OnAppearing();
            HolidayViewModel.CheckLocationStatus();
           
        }
        private void HolidayList_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            if (e.Item == null) return;
            if (sender is ListView lv) lv.SelectedItem = null;
        }

        private void TapGestureRecognizer_Tapped(object sender, System.EventArgs e)
        {
            var mainPage = (Application.Current.MainPage as NavigationPage).CurrentPage;
            (mainPage as FlyoutPage).IsPresented = true;
        }

        private void NewLeave_Tapped_1(object sender, EventArgs e)
        {
            Navigation.PushAsync(new ApplyHolidayPage());
        }
    }
}
