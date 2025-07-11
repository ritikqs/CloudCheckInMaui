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

namespace CCIMIGRATION.Views.Headers
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CustomTitleBar : ContentView
    {
        public CustomTitleBar()
        {
            InitializeComponent();
        }

        private void Menu_Click(object sender, EventArgs e)
        {
            if (App.LoggedInUser!=null && App.LoggedInUser.Role.ToLower() == "site manager")
            {
                App.Current.MainPage.Navigation.PushModalAsync(new SiteManagerMenuPage());
            }
            else
            {
                App.Current.MainPage.Navigation.PushModalAsync(new EmployeeMenuPage());
            }
           
        }
    }
}
