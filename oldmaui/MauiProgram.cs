using Microsoft.Extensions.Logging;
using Microsoft.Maui;
using Microsoft.Maui.Controls.Hosting;
using Microsoft.Maui.Hosting;
using CloudCheckInMaui.Views;
using CloudCheckInMaui.Views.Headers;
using CloudCheckInMaui.ViewModels;
using CloudCheckInMaui.Services.FaceService;
using CloudCheckInMaui.Services.ApiService;
using CloudCheckInMaui.Services.ImageUploadService;
using CloudCheckInMaui.Services.DeviceService;
using CloudCheckInMaui.Services;
using CommunityToolkit.Maui;
using Microsoft.Maui.Networking;
using Plugin.Fingerprint;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Maui.Devices;
using Microsoft.Maui.Storage;

namespace CloudCheckInMaui;

public static class MauiProgram
{
	public static MauiApp CreateMauiApp()
	{
		var builder = MauiApp.CreateBuilder();
		builder
			.UseMauiApp<App>()
			.UseMauiCommunityToolkit()
			.ConfigureFonts(fonts =>
			{
				fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
				fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
				// Add custom fonts for the app
				fonts.AddFont("fa-light-300.ttf", "LightIcons");
				fonts.AddFont("fa-solid-900.ttf", "Icons");
				fonts.AddFont("Hoftype - Carnas-Light.otf", "LightFont");
				fonts.AddFont("Hoftype - Carnas-Medium.otf", "MediumFont");
				fonts.AddFont("Hoftype - Carnas-Regular.otf", "RegularFont");
			});

		// Register services
		builder.Services.AddSingleton<IConnectivity>(Connectivity.Current);
		builder.Services.AddSingleton<IGeolocation>(Geolocation.Default);
		builder.Services.AddSingleton<IPreferences>(Preferences.Default);
		builder.Services.AddSingleton<IFaceRecognitionService, FaceRecognitionService>();
		builder.Services.AddSingleton<IDeviceInfoService, DeviceInfoService>();
		builder.Services.AddSingleton<IImageSyncService, ImagesSyncService>();
		builder.Services.AddSingleton(CrossFingerprint.Current);
		builder.Services.AddSingleton<IPermissionService, PermissionService>();

		// Platform-specific service registration
#if IOS
		builder.Services.AddSingleton<ILocalAuth, CloudCheckInMaui.Platforms.iOS.LocalAuthenticateService>();
#else
		builder.Services.AddSingleton<ILocalAuth, DefaultLocalAuthService>();
#endif

		// Register ViewModels with their dependencies
		builder.Services.AddTransient<LoginViewModel>();
		builder.Services.AddSingleton<HomeViewModel>();
		builder.Services.AddTransient<ForgotPasswordViewModel>();
		builder.Services.AddTransient<RegistrationViewModel>();
		builder.Services.AddTransient<HolidayViewModel>();
		builder.Services.AddTransient<TimesheetViewModel>();
		builder.Services.AddTransient<AlertsViewModel>();
		builder.Services.AddTransient<MessageViewModel>();
		builder.Services.AddTransient<HeaderViewModel>();
		builder.Services.AddTransient<ApplyHolidayPageViewModel>();
		builder.Services.AddTransient<AuthViewModel>();

		// Register pages and views
		builder.Services.AddTransient<LoginPage>();
		builder.Services.AddTransient<RegisterPage>();
		builder.Services.AddTransient<ForgotPasswordPage>();
		builder.Services.AddTransient<ResetPassword>();
		builder.Services.AddTransient<AlertsPage>();
		builder.Services.AddTransient<HolidayPage>();
		builder.Services.AddTransient<TimesheetPage>();
		builder.Services.AddTransient<MessagePage>();
		builder.Services.AddTransient<CustomTitleBar>();
		builder.Services.AddTransient<ApplyHolidayPage>();
		builder.Services.AddTransient<AuthPage>();
		builder.Services.AddTransient<HomeTabbedPage>();

#if DEBUG
		builder.Logging.AddDebug();
#endif

		return builder.Build();
	}
}
