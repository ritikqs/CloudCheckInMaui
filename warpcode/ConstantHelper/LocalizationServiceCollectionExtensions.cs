using CCIMIGRATION.MultilanguageHelper;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System.Globalization;

namespace CCIMIGRATION.ConstantHelper
{
    /// <summary>
    /// Extension methods for configuring localization services
    /// </summary>
    public static class LocalizationServiceCollectionExtensions
    {
        /// <summary>
        /// Adds localization services to the service collection
        /// </summary>
        /// <param name="services">The service collection</param>
        /// <param name="defaultCulture">The default culture to use (optional)</param>
        /// <returns>The service collection for chaining</returns>
        public static IServiceCollection AddLocalization(this IServiceCollection services, string defaultCulture = "en")
        {
            // Register the LocalizationResourceManager as a singleton
            services.AddSingleton<LocalizationResourceManager>(provider =>
            {
                var manager = LocalizationResourceManager.Instance;
                
                // Set default culture if provided
                if (!string.IsNullOrEmpty(defaultCulture))
                {
                    try
                    {
                        var culture = new CultureInfo(defaultCulture);
                        manager.SetCulture(culture);
                    }
                    catch (Exception ex)
                    {
                        var logger = provider.GetService<ILogger<LocalizationResourceManager>>();
                        logger?.LogWarning(ex, "Failed to set default culture '{DefaultCulture}', using system default", defaultCulture);
                    }
                }
                
                return manager;
            });

            return services;
        }

        /// <summary>
        /// Configures localization with specific settings
        /// </summary>
        /// <param name="services">The service collection</param>
        /// <param name="configure">Configuration action</param>
        /// <returns>The service collection for chaining</returns>
        public static IServiceCollection ConfigureLocalization(this IServiceCollection services, Action<LocalizationOptions> configure)
        {
            var options = new LocalizationOptions();
            configure(options);

            services.AddSingleton(options);
            services.AddLocalization(options.DefaultCulture);

            return services;
        }
    }

    /// <summary>
    /// Configuration options for localization
    /// </summary>
    public class LocalizationOptions
    {
        /// <summary>
        /// Default culture to use
        /// </summary>
        public string DefaultCulture { get; set; } = "en";

        /// <summary>
        /// Supported cultures
        /// </summary>
        public List<string> SupportedCultures { get; set; } = new() { "en", "pl", "ro", "sq" };

        /// <summary>
        /// Whether to fall back to default culture if resource is not found
        /// </summary>
        public bool UseFallback { get; set; } = true;

        /// <summary>
        /// Whether to cache translated strings
        /// </summary>
        public bool EnableCaching { get; set; } = true;
    }

    /// <summary>
    /// Helper class for managing application localization
    /// </summary>
    public static class LocalizationHelper
    {
        /// <summary>
        /// Sets the application culture
        /// </summary>
        /// <param name="cultureCode">The culture code (e.g., "en", "pl")</param>
        public static void SetCulture(string cultureCode)
        {
            try
            {
                var culture = new CultureInfo(cultureCode);
                LocalizationResourceManager.Instance.SetCulture(culture);
                
                // Save the selected culture to preferences for persistence
                Preferences.Set("AppLanguageCode", cultureCode);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error setting culture '{cultureCode}': {ex.Message}");
            }
        }

        /// <summary>
        /// Gets the current culture code
        /// </summary>
        /// <returns>The current culture code</returns>
        public static string GetCurrentCulture()
        {
            return LocalizationResourceManager.Instance.CurrentCulture?.Name ?? "en";
        }

        /// <summary>
        /// Gets the saved culture from preferences or system default
        /// </summary>
        /// <returns>The saved or default culture code</returns>
        public static string GetSavedCulture()
        {
            return Preferences.Get("AppLanguageCode", "en");
        }

        /// <summary>
        /// Initializes localization from saved preferences
        /// </summary>
        public static void InitializeFromPreferences()
        {
            var savedCulture = GetSavedCulture();
            SetCulture(savedCulture);
        }

        /// <summary>
        /// Gets available cultures based on resource files
        /// </summary>
        /// <returns>List of available culture codes</returns>
        public static IEnumerable<string> GetAvailableCultures()
        {
            return LocalizationResourceManager.Instance.GetAvailableCultures();
        }

        /// <summary>
        /// Gets a localized string by key
        /// </summary>
        /// <param name="key">The resource key</param>
        /// <param name="defaultValue">Default value if key not found</param>
        /// <returns>The localized string</returns>
        public static string GetString(string key, string defaultValue = null)
        {
            return ResourceConstants.GetString(key) ?? defaultValue ?? key;
        }

        /// <summary>
        /// Checks if a resource key exists
        /// </summary>
        /// <param name="key">The resource key</param>
        /// <returns>True if the key exists</returns>
        public static bool HasKey(string key)
        {
            return ResourceConstants.HasKey(key);
        }
    }
}
