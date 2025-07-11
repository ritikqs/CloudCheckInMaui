// using Rg.Plugins.Popup.Pages; // REMOVED: Replace with CommunityToolkit.Maui.Popup
// using CommunityToolkit.Maui.Views; // TODO: Setup proper popup implementation
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Maui.Controls.Xaml;
using Microsoft.Maui.Controls.Compatibility;
using Microsoft.Maui.Controls;
using Microsoft.Maui;

namespace CCIMIGRATION.Views.LoaderView
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Loader : ContentPage // TODO: Change back to popup when CommunityToolkit.Maui popup is properly setup
    {
        public Loader()
        {
            InitializeComponent();
        }
    }
}
