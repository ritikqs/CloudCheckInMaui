using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Maui.Controls.Xaml;
using Microsoft.Maui.Controls.Compatibility;
using Microsoft.Maui.Controls;
using Microsoft.Maui;

namespace CCIMIGRATION.CustomControls.SiteManagerHeader
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class HeaderView : ContentView
    {
        public HeaderView()
        {
            InitializeComponent();
        }
        public static readonly BindableProperty BackButtonVisibleProperty = BindableProperty.Create("BackButtonVisible", typeof(bool), typeof(HeaderView), false);
        public bool BackButtonVisible
        {
            get { return (bool)GetValue(BackButtonVisibleProperty); }
            set { SetValue(BackButtonVisibleProperty, value); }
        }
    }
}
