using CCIMIGRATION.SiteManagerViewModel;
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
	public partial class Evacuation : ContentPage
	{
        public Evacuation()
        {
            InitializeComponent();
            templatemessage.ItemSelected += Templatemessage_ItemSelected;
        }

        private void Templatemessage_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            templatemessage.SelectedItem = null;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            //evacuationPageViewModel.CheckGeofence();
        }
        //private void evacuatedList_ItemTapped(object sender, ItemTappedEventArgs e)
        //{
        //    if (e.Item == null) return;
        //    if (sender is ListView lv) lv.SelectedItem = null;
        //}

        private void TapGestureRecognizer_Tapped(object sender, System.EventArgs e)
        {
            var mainPage = (Application.Current.MainPage as NavigationPage).CurrentPage;
            (mainPage as FlyoutPage).IsPresented = true;
        }

    }
}
