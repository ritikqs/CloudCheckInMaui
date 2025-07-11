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
    public partial class Alerts : ContentPage
    {
        AlertsPageViewModel ViewModel;
        public Alerts()
        {
            InitializeComponent();
            alertsList.SelectionChanged += AlertsList_SelectionChanged;
        }

        private void AlertsList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            alertsList.SelectedItem = null;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            ViewModel = new AlertsPageViewModel();
            this.BindingContext = ViewModel;
            ViewModel.GetAlerts();
        }
    }
}
