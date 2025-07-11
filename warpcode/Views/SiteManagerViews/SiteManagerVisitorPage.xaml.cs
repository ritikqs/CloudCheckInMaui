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
    public partial class SiteManagerVisitorPage : ContentPage
    {
        SiteManagerVisitorPageViewModel VisitorViewModel = new SiteManagerVisitorPageViewModel();
        public SiteManagerVisitorPage()
        {
            InitializeComponent();
            this.BindingContext = VisitorViewModel;
        }
        private void TapGestureRecognizer_Tapped(object sender, EventArgs e)
        {
            var mainPage = (Application.Current.MainPage as NavigationPage).CurrentPage;
            (mainPage as FlyoutPage).IsPresented = true;
        }

        private void visitorList_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            if (e.Item == null) return;
            if (sender is ListView lv) lv.SelectedItem = null;
        }

    }
}
