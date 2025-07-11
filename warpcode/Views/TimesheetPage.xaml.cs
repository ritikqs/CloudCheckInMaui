using CCIMIGRATION.CustomControls;
using CCIMIGRATION.Services;
using CCIMIGRATION.ViewModels;
using CCIMIGRATION.Views.SiteManagerViews;
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
    public partial class TimesheetPage : ContentPage
    {
        public TimesheetPageViewModel TimeSheetViewModel = new TimesheetPageViewModel();
        public TimesheetPage()
        {
            InitializeComponent();
            this.BindingContext = TimeSheetViewModel;
        }
        protected override void OnAppearing()
        {
            base.OnAppearing();
            //TimeSheetViewModel.IsListRefreshing = true;
            TimeSheetViewModel.GetTimeSheet();
        }

        private void ListView_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            //timesheetlist.SelectedItem = null;
        }
        private void TapGestureRecognizer_Tapped(object sender, EventArgs e)
        {
            MessagingService.Send("Open", "OpenMenu");
        }
    }
}
