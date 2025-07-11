using Microsoft.Maui.Controls;
using CCIMIGRATION.ConstantHelper;
using CCIMIGRATION.ViewModels;
using CCIMIGRATION.Views;
using CCIMIGRATION.SiteManagerViews;
using CCIMIGRATION.Views.SiteManagerViews;
using System.Diagnostics;

namespace CCIMIGRATION;

public partial class AppShell : Shell
{
    public AppShell()
    {
        InitializeComponent();
        
        RegisterRoutes();
        
        // Subscribe to navigation events
        Navigating += Current_Navigating;
        Navigated += Current_Navigated;  
    }

    private void RegisterRoutes()
    {
        Debug.WriteLine("Registering routes...");
        try
        {
            // Register routes with absolute paths
            Routing.RegisterRoute("Login", typeof(LoginPage));
            Routing.RegisterRoute("MainApp", typeof(HomeTabbedPage));
            Routing.RegisterRoute("Camera", typeof(CameraPageView));
            Routing.RegisterRoute("Register", typeof(RegisterPage));
            Routing.RegisterRoute("ForgotPassword", typeof(ForgotPasswordPage));
            Routing.RegisterRoute("ResetPassword", typeof(ResetPassword));
            Routing.RegisterRoute("ApplyHoliday", typeof(ApplyHolidayPage));
            Routing.RegisterRoute("Language", typeof(LanguageSelectionPage));
            Routing.RegisterRoute("Auth", typeof(AuthPage));
            Routing.RegisterRoute("EmployeeMenu", typeof(EmployeeMenuPage));
            
            // Site Manager Routes
            Routing.RegisterRoute("SiteManagerMenu", typeof(SiteManagerMenuPage));
            Routing.RegisterRoute("SiteManagerHome", typeof(SiteManagerHomePage));
            Routing.RegisterRoute("SiteManagerHoliday", typeof(SiteManagerHolidayPage));
            Routing.RegisterRoute("SiteManagerVisitor", typeof(SiteManagerVisitorPage));
            Routing.RegisterRoute("Evacuation", typeof(Evacuation));
            Routing.RegisterRoute("EvacuationList", typeof(EvacuationList));
            Routing.RegisterRoute("CalenderPage", typeof(CalenderPage));
            
            Debug.WriteLine("Routes registered successfully");
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Error registering routes: {ex.Message}");
            Debug.WriteLine($"Stack trace: {ex.StackTrace}");
        }
    }

    private void Current_Navigating(object sender, ShellNavigatingEventArgs e)
    {
        if (e.Target.Location.OriginalString.Contains("MainApp"))
        {
            Debug.WriteLine($"Navigating to MainApp...");
            // Don't cancel the navigation
            return;
        }
        
        Debug.WriteLine($"Navigating to: {e.Target.Location}");
        Debug.WriteLine($"Current: {e.Current?.Location}");
    }

    private void Current_Navigated(object sender, ShellNavigatedEventArgs e)
    {
        Debug.WriteLine($"Successfully navigated to: {e.Current.Location}");
    }

    protected override bool OnBackButtonPressed()
    {
        // Prevent back button from working when on main pages
        var location = Shell.Current.CurrentState.Location.OriginalString;
        if (location.Contains("MainApp") || location.Contains("Login"))
        {
            return true; // Handled - prevents back navigation
        }
        return base.OnBackButtonPressed();
    }
}
