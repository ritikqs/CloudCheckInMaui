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
	public partial class CalenderPage : ContentPage
	{
		public CalenderViewModel ViewModel = new CalenderViewModel();
		public CalenderPage ()
		{
			InitializeComponent ();
			this.BindingContext = ViewModel;
		}
        protected override void OnAppearing()
        {
            base.OnAppearing();
			ViewModel.CheckGeofence();
        }
        private void TapGestureRecognizer_Tapped(object sender, EventArgs e)
		{
			var mainPage = (Application.Current.MainPage as NavigationPage).CurrentPage;
			(mainPage as FlyoutPage).IsPresented = true;
		}

		private void TapGestureRecognizer_Tapped_1(object sender, EventArgs e)
		{

		}

		private void Button_Clicked(object sender, EventArgs e)
		{
			
		}

		private void Button_Clicked_1(object sender, EventArgs e)
		{
			
		}

        private void PreviousMonthTap(object sender, EventArgs e)
        {
			ViewModel.LoadLastMonthEvents.Execute(null);
		}
		private void NextMonthTap(object sender, EventArgs e)
		{
			ViewModel.LoadNextMonthEvents.Execute(null);
		}
	}
}
