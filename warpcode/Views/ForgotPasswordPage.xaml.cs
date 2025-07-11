using CCIMIGRATION.ConstantHelper;
using CCIMIGRATION.Services.FaceService;
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
using System.Diagnostics;

namespace CCIMIGRATION.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ForgotPasswordPage : ContentPage
    {
        public ForgotPasswordViewModel ForgotViewModel { get; private set; }
        
        public ForgotPasswordPage()
        {
            try
            {
                Debug.WriteLine("ForgotPasswordPage: Initializing component");
                InitializeComponent();
                
                Debug.WriteLine("ForgotPasswordPage: Creating ForgotPasswordViewModel");
                ForgotViewModel = new ForgotPasswordViewModel();
                
                Debug.WriteLine("ForgotPasswordPage: Setting BindingContext");
                BindingContext = ForgotViewModel;
                
                Debug.WriteLine("ForgotPasswordPage: Initialization complete");
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"ForgotPasswordPage constructor error: {ex.Message}");
                Debug.WriteLine($"Stack trace: {ex.StackTrace}");
                
                // Emergency fallback
                try
                {
                    ForgotViewModel = new ForgotPasswordViewModel();
                    BindingContext = ForgotViewModel;
                }
                catch (Exception fallbackEx)
                {
                    Debug.WriteLine($"ForgotPasswordPage fallback error: {fallbackEx.Message}");
                }
            }
        }
        
        // TextChanged method removed - using proper data binding instead
    }
}
