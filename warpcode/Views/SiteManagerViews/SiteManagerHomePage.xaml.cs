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
    public partial class SiteManagerHomePage : ContentPage
    {
        public SiteManagerHomePageViewModel HomePageViewModel = new SiteManagerHomePageViewModel();
        public SiteManagerHomePage()
        {
            InitializeComponent();
            this.BindingContext = HomePageViewModel;
            sitestafflist.SelectionChanged += Sitestafflist_SelectionChanged;
        }

        private void Sitestafflist_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            sitestafflist.SelectedItem = null;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            
        }

        private void Menu_icon_Tapped(object sender, EventArgs e)
        {
            var mainPage = (Application.Current.MainPage as NavigationPage).CurrentPage;
            (mainPage as FlyoutPage).IsPresented = true;
        }
    }
}
