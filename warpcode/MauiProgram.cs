using CCIMIGRATION.Interface;
using CCIMIGRATION.Services;
using CCIMIGRATION.ViewModels;
using CCIMIGRATION.Views;
using CCIMIGRATION.SiteManagerViews;
using CCIMIGRATION.Data;
using CCIMIGRATION.Service.ApiService;
using CCIMIGRATION.SiteManagerViewModel;
using CCIMIGRATION.Views.SiteManagerViews;
using CommunityToolkit.Maui;
using Microsoft.Extensions.Logging;
using Plugin.Fingerprint;
using Plugin.Fingerprint.Abstractions;
using SkiaSharp.Views.Maui.Controls.Hosting;
using CCIMIGRATION.CustomControls;
using CCIMIGRATION.ConstantHelper;
using CCIMIGRATION.MultilanguageHelper;
using ServiceHelper = CCIMIGRATION.Services.ServiceHelper;

namespace CCIMIGRATION;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();
        builder
            .UseMauiApp<App>()
            .UseSkiaSharp()
            .UseMauiCommunityToolkit()
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                // Add custom fonts from the Xamarin project
                fonts.AddFont("fa-regular-400.ttf", "FontAwesome-Regular");
                fonts.AddFont("fa-solid-900.ttf", "FontAwesome-Solid");
                fonts.AddFont("fa-light-300.ttf", "FontAwesome-Light");
                fonts.AddFont("Hoftype - Carnas-Light.otf", "Carnas-Light");
                fonts.AddFont("Hoftype - Carnas-Medium.otf", "Carnas-Medium");
                fonts.AddFont("Hoftype - Carnas-Regular.otf", "Carnas-Regular");
            })
            .ConfigureEssentials(essentials =>
            {
                essentials.UseVersionTracking();
            });

#if DEBUG
		builder.Logging.AddDebug();
#endif

        // Register Localization Services
        builder.Services.AddSingleton<LocalizationService>();
        
        // Register Services
        builder.Services.AddSingleton<Repository>();
        builder.Services.AddSingleton<IDialogService, DialogService>();
        builder.Services.AddSingleton<INavigationService, NavigationService>();
        builder.Services.AddSingleton<IFingerprint>(CrossFingerprint.Current);
        builder.Services.AddSingleton<IPushNotificationService, PushNotificationService>();
        builder.Services.AddSingleton<ILocationService, LocationService>();
        builder.Services.AddSingleton<IAppStateService, AppStateService>();
        
        // Register ViewModels
        builder.Services.AddTransient<LoginPageViewModel>();
        builder.Services.AddTransient<RegisterViewModel>();
        builder.Services.AddTransient<AuthViewModel>();
        builder.Services.AddTransient<HomePageViewModel>();
        builder.Services.AddTransient<TimesheetPageViewModel>();
        builder.Services.AddTransient<HolidayViewModel>();
        builder.Services.AddTransient<ApplyHolidayPageViewModel>();
        builder.Services.AddTransient<AlertsPageViewModel>();
        builder.Services.AddTransient<CameraPageViewModel>();
        builder.Services.AddTransient<ForgotPasswordViewModel>();
        builder.Services.AddTransient<ResetPasswordViewModel>();
        builder.Services.AddTransient<LanguageSelectionViewModel>();
        
        // Register Site Manager ViewModels
        builder.Services.AddTransient<SiteManagerHomePageViewModel>();
        builder.Services.AddTransient<SiteManagerHolidayPageViewModel>();
        builder.Services.AddTransient<SiteManagerVisitorPageViewModel>();
        builder.Services.AddTransient<CalenderViewModel>();
        builder.Services.AddTransient<EvacuationPageViewModel>();
        
        // Register Views
        builder.Services.AddTransient<LoginPage>();
        builder.Services.AddTransient<RegisterPage>();
        builder.Services.AddTransient<AuthPage>();
        builder.Services.AddTransient<Home>();
        builder.Services.AddTransient<HomeTabbedPage>();
        builder.Services.AddTransient<TimesheetPage>();
        builder.Services.AddTransient<HolidayPage>();
        builder.Services.AddTransient<ApplyHolidayPage>();
        builder.Services.AddTransient<Alerts>();
        builder.Services.AddTransient<CameraPageView>();
        builder.Services.AddTransient<ForgotPasswordPage>();
        builder.Services.AddTransient<ResetPassword>();
        builder.Services.AddTransient<LanguageSelectionPage>();
        builder.Services.AddTransient<EmployeeMenuPage>();
        
        // Register Site Manager Views
        builder.Services.AddTransient<SiteManagerHomePage>();
        builder.Services.AddTransient<SiteManagerMenuPage>();
        builder.Services.AddTransient<SiteManagerHolidayPage>();
        builder.Services.AddTransient<SiteManagerVisitorPage>();
        builder.Services.AddTransient<CalenderPage>();
        builder.Services.AddTransient<Evacuation>();
        builder.Services.AddTransient<EvacuationList>();
        builder.Services.AddTransient<ComingSoonPage>();
        
        // Platform-specific service registrations
#if ANDROID
        // Add Android-specific services here
#elif IOS
        // Add iOS-specific services here
#elif WINDOWS
        // Add Windows-specific services here
#endif

        var app = builder.Build();
        ServiceHelper.Initialize(app.Services);
        return app;
    }
}
