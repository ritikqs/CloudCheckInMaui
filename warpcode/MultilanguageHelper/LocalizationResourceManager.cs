using Microsoft.Extensions.Logging;
using System;
using System.Collections.Concurrent;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Threading;
using Microsoft.Maui.Storage;

namespace CCIMIGRATION.MultilanguageHelper
{
    /// <summary>
    /// Localization resource manager that wraps LocalizationService for JSON-based localization
    /// This maintains backward compatibility while using the new JSON-based system
    /// </summary>
    public class LocalizationResourceManager : INotifyPropertyChanged
    {
        private static readonly Lazy<LocalizationResourceManager> _instance = 
            new Lazy<LocalizationResourceManager>(() => new LocalizationResourceManager());
        
        private readonly ConcurrentDictionary<string, string> _cachedStrings = new();
        private CultureInfo _currentCulture;
        private readonly ILogger<LocalizationResourceManager> _logger;
        private static LocalizationService _localizationService;

        public static LocalizationResourceManager Instance => _instance.Value;

        public static void Initialize(LocalizationService localizationService)
        {
            _localizationService = localizationService;
        }

        private LocalizationResourceManager()
        {
            _currentCulture = CultureInfo.CurrentUICulture;
        }

        /// <summary>
        /// Gets the localized string for the specified key
        /// </summary>
        /// <param name="key">The resource key</param>
        /// <returns>The localized string or the key if not found</returns>
        public string this[string key]
        {
            get
            {
                if (string.IsNullOrEmpty(key))
                    return key;

                try
                {
                    // If LocalizationService is available, use it
                    if (_localizationService != null)
                    {
                        return _localizationService[key];
                    }

                    // Fallback: Create cache key with culture
                    var cacheKey = $"{_currentCulture.Name}_{key}";
                    
                    // Try to get from cache first
                    if (_cachedStrings.TryGetValue(cacheKey, out var cachedValue))
                    {
                        return cachedValue;
                    }

                    // Fallback to key if no service is available
                    var result = key;
                    
                    // Cache the result
                    _cachedStrings.TryAdd(cacheKey, result);
                    
                    return result;
                }
                catch (Exception ex)
                {
                    // Log the error if logger is available
                    _logger?.LogError(ex, "Error getting localized string for key: {Key}", key);
                    
                    // Return the key as fallback
                    return key;
                }
            }
        }

        /// <summary>
        /// Gets the current culture
        /// </summary>
        public CultureInfo CurrentCulture => _currentCulture;

        /// <summary>
        /// Sets the application culture and clears the cache
        /// </summary>
        /// <param name="culture">The culture to set</param>
        public void SetCulture(CultureInfo culture)
        {
            if (culture == null)
                throw new ArgumentNullException(nameof(culture));

            try
            {
                _currentCulture = culture;
                Thread.CurrentThread.CurrentUICulture = culture;

                // If LocalizationService is available, use it
                if (_localizationService != null)
                {
                    // Change language using LocalizationService (async operation, but we'll call it sync for compatibility)
                    _localizationService.ChangeLanguageAsync(culture.TwoLetterISOLanguageName).Wait();
                }

                // Clear cache when culture changes
                _cachedStrings.Clear();

                Invalidate();
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "Error setting culture to: {CultureName}", culture.Name);
                throw;
            }
        }

        /// <summary>
        /// Checks if a resource key exists
        /// </summary>
        /// <param name="key">The resource key to check</param>
        /// <returns>True if the key exists, false otherwise</returns>
        public bool HasKey(string key)
        {
            if (string.IsNullOrEmpty(key))
                return false;

            try
            {
                if (_localizationService != null)
                {
                    var value = _localizationService[key];
                    // If LocalizationService returns #{key}, it means it wasn't found
                    return !string.IsNullOrEmpty(value) && value != $"#{key}";
                }
                return false;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Gets all available culture names
        /// </summary>
        /// <returns>List of available culture names</returns>
        public IEnumerable<string> GetAvailableCultures()
        {
            try
            {
                // Return the cultures supported by the JSON localization system
                return new[] { "en", "pl", "ro", "sq" };
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "Error getting available cultures");
                return new[] { "en" }; // Return default culture as fallback
            }
        }

        /// <summary>
        /// Clears the string cache
        /// </summary>
        public void ClearCache()
        {
            _cachedStrings.Clear();
        }

        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Invalidates all bindings to trigger UI updates
        /// </summary>
        public void Invalidate()
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(null));
        }
    }
}
