using Microsoft.Maui.Controls;
using CloudCheckInMaui.ViewModels;

namespace CloudCheckInMaui.Views
{
    public partial class EmployeeMenuPage : ContentPage
    {
        public EmployeeMenuPage()
        {
            InitializeComponent();
            BindingContext = new EmployeeMenuViewModel();
            // This is just the UI migration, functionality will be implemented later
        }
    }
} 