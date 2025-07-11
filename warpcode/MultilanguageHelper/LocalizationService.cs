using System.Globalization;
using System.Text.Json;
using Microsoft.Maui.Storage;

namespace CCIMIGRATION.MultilanguageHelper
{
    public class LocalizationService
    {
        private Dictionary<string, string> _translations = new();
        private string _currentLanguage;
        
        public event EventHandler LanguageChanged;
        
        public string CurrentLanguage => _currentLanguage;

        public async Task LoadAsync(string languageCode)
        {
            try
            {
                // Fallback to English if requested language file doesn't exist
                var availableLanguages = new[] { "en", "pl", "ro", "sq" };
                if (!availableLanguages.Contains(languageCode))
                {
                    languageCode = "en";
                }
                
                string filePath = $"Resources/Lang/{languageCode}.json";
                using var stream = await FileSystem.OpenAppPackageFileAsync(filePath);
                using var reader = new StreamReader(stream);
                var json = await reader.ReadToEndAsync();
                _translations = JsonSerializer.Deserialize<Dictionary<string, string>>(json) ?? new Dictionary<string, string>();
                
                _currentLanguage = languageCode;
                
                // Save the selected language preference
                Preferences.Set("AppLanguage", languageCode);
                
                // Raise language changed event
                LanguageChanged?.Invoke(this, EventArgs.Empty);
            }
            catch (Exception)
            {
                // If loading fails, try to load English as fallback
                if (languageCode != "en")
                {
                    await LoadAsync("en");
                }
                else
                {
                    // If even English fails, use empty dictionary
                    _translations = new Dictionary<string, string>();
                }
            }
        }

        public string this[string key] =>
            _translations.TryGetValue(key, out var value) ? value : $"#{key}";
            
        public async Task ChangeLanguageAsync(string languageCode)
        {
            await LoadAsync(languageCode);
        }
        
        public string GetSavedLanguage()
        {
            return Preferences.Get("AppLanguage", CultureInfo.CurrentUICulture.TwoLetterISOLanguageName);
        }
    }
}
