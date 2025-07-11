using CCIMIGRATION.Services;
using CCIMIGRATION.Interface;
using CCIMIGRATION.Models;
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
using Microsoft.Maui.Storage;

namespace CCIMIGRATION.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Home : ContentPage
    {
        public HomePageViewModel HomePageVM = new HomePageViewModel();

        public Home()
        {
            InitializeComponent();
            this.BindingContext = HomePageVM;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            App.IsOnHomePage = true;
            HomePageVM.IsFaceCaptureViewVisible = false;
            //UserDialogs.Instance.ShowLoading();
            CheckOfflineData();
            Subscribe();
        }

        private async void CheckOfflineData()
        {
            var data = await App.Repository.GetAsync<LocationModel>();
            if (data != null && data.Count > 0)
            {
                HomePageVM.SyncLocations(data);
            }
            else
            {
                LoadHomePageData();
            }
        }

        private void LoadHomePageData()
        {
            if (!App.IsCameraNavigated)
            {
                HomePageVM.LoadRecetTimeSheet();
                HomePageVM.CheckLocationStatus();
                HomePageVM.GetUserCheckInStatus();
            }
        }

        private void Subscribe()
        {
            MessagingService.Subscribe(this, "UpdateHomePage", (sender) =>
            {
                LoadHomePageData();
            });
            MessagingService.Subscribe(this, "AnimateText", async (sender) =>
            {
                await locationstatus.TranslateTo(10, 0, 50, Easing.Linear);
                await locationstatus.TranslateTo(-10, 0, 50, Easing.Linear);
                await locationstatus.TranslateTo(10, 0, 50, Easing.Linear);
                await locationstatus.TranslateTo(-10, 0, 50, Easing.Linear);
            });
        }

        protected override void OnDisappearing()
        {
            App.IsOnHomePage = false;
            MessagingService.Unsubscribe<string>(this, "AnimateText");
            MessagingService.Unsubscribe<string>(this, "UpdateHomePage");
        }

        private void ListView_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            if (e.SelectedItem == null) return;
            if (sender is ListView lv) lv.SelectedItem = null;
        }

        void TapGestureRecognizer_Tapped(System.Object sender, System.EventArgs e)
        {
            DependencyService.Get<ILocationService>().OpenSettings();
        }

        private void StartService(object sender, EventArgs e)
        {
            Preferences.Set("OutLocation", "0");
            Preferences.Set("CheckedIn", "true");
            MessagingService.Send("StartService", "StartService");
        }

        private void StopService(object sender, EventArgs e)
        {
            MessagingService.Send("StopService", "StopService");
        }
    }
}
