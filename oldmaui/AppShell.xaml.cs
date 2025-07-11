using Microsoft.Maui.Controls;
using CloudCheckInMaui.ConstantHelper;
using CloudCheckInMaui.ViewModels;
using CloudCheckInMaui.Views;
using System.Diagnostics;

namespace CloudCheckInMaui;

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
			//Routing.RegisterRoute("Camera", typeof(CameraPageView));
			Routing.RegisterRoute("Register", typeof(RegisterPage));
			Routing.RegisterRoute("ForgotPassword", typeof(ForgotPasswordPage));
			Routing.RegisterRoute("ResetPassword", typeof(ResetPassword));
			Routing.RegisterRoute("Profile", typeof(ProfilePage));
			Routing.RegisterRoute("Settings", typeof(SettingsPage));
			Routing.RegisterRoute("ApplyHoliday", typeof(ApplyHolidayPage));
			Routing.RegisterRoute("Language", typeof(LanguageSelectionPage));
			
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
