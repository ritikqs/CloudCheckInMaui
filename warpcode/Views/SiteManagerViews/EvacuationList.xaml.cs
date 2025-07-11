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

namespace CCIMIGRATION.Views.SiteManagerViews
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class EvacuationList : ContentPage
    {
        EvacuationPageViewModel EvacuationPageViewModel = new EvacuationPageViewModel();
        public EvacuationList()
        {
            InitializeComponent();
            this.BindingContext = EvacuationPageViewModel;
        }
        protected override void OnAppearing()
        {
            base.OnAppearing();
            _ = EvacuationPageViewModel.LoadEvacuations();
        }
    }
}
