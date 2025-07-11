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
    public partial class PageHeaders : ContentView
    {
        public PageHeaders()
        {
            InitializeComponent();
        }
        public static readonly BindableProperty TitleProperty = BindableProperty.Create("Title", typeof(string), typeof(Label), "");
        public string Title
        {
            get { return (string)GetValue(TitleProperty); }
            set { SetValue(TitleProperty, value); }
        }

        private void TapGestureRecognizer_Tapped(object sender, EventArgs e)
        {
            var mainpage = App.Current.MainPage.Navigation.NavigationStack.LastOrDefault();
            (mainpage as FlyoutPage).IsPresented = true;
        }
    }
}
