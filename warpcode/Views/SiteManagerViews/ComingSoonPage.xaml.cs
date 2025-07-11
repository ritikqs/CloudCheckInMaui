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
	public partial class ComingSoonPage : ContentPage
	{
		public ComingSoonPage ()
		{
            InitializeComponent();
		}

        private void TapGestureRecognizer_Tapped(object sender, EventArgs e)
        {
			var mainPage = (Application.Current.MainPage as NavigationPage).CurrentPage;
			(mainPage as FlyoutPage).IsPresented = true;
		}

    }
}
