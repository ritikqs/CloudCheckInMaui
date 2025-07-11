using CCIMIGRATION.ConstantHelper;

namespace CCIMIGRATION.MultilanguageHelper
{
    /// <summary>
    /// XAML markup extension for localizing text in XAML with binding support
    /// Usage: Text="{local:Translate AppName}" or Text="{local:Translate Text=AppName}"
    /// </summary>
    [ContentProperty("Text")]
    public class TranslateExtension : IMarkupExtension<BindingBase>
    {
        /// <summary>
        /// The resource key to translate
        /// </summary>
        public string Text { get; set; }
        
        /// <summary>
        /// String format for the localized text
        /// </summary>
        public string StringFormat { get; set; }
        
        /// <summary>
        /// Default value to return if the key is not found
        /// </summary>
        public string Default { get; set; }
        
        /// <summary>
        /// Alternative property name for the resource key (for clarity)
        /// </summary>
        public string Key 
        { 
            get => Text; 
            set => Text = value; 
        }

        object IMarkupExtension.ProvideValue(IServiceProvider serviceProvider)
        {
            return ProvideValue(serviceProvider);
        }

        public BindingBase ProvideValue(IServiceProvider serviceProvider)
        {
            if (string.IsNullOrEmpty(Text))
            {
                return new Binding { Source = Default ?? Text };
            }

            try
            {
                var binding = new Binding
                {
                    Mode = BindingMode.OneWay,
                    Path = $"[{Text}]",
                    Source = LocalizationResourceManager.Instance,
                    StringFormat = StringFormat,
                    FallbackValue = Default ?? Text,
                    TargetNullValue = Default ?? Text
                };
                return binding;
            }
            catch (Exception)
            {
                // If binding creation fails, return a static binding with the translated value
                try
                {
                    var value = ResourceConstants.GetString(Text) ?? Default ?? Text;
                    return new Binding { Source = value };
                }
                catch
                {
                    return new Binding { Source = Default ?? Text };
                }
            }
        }
    }

    /// <summary>
    /// Simple markup extension that returns the localized string directly (no binding)
    /// Usage: Text="{local:Localize AppName}"
    /// Use this for static text that doesn't need to update when language changes
    /// </summary>
    [ContentProperty("Key")]
    public class LocalizeExtension : IMarkupExtension
    {
        /// <summary>
        /// The resource key to translate
        /// </summary>
        public string Key { get; set; }

        /// <summary>
        /// Default value to return if the key is not found
        /// </summary>
        public string Default { get; set; }

        public object ProvideValue(IServiceProvider serviceProvider)
        {
            if (string.IsNullOrEmpty(Key))
                return Default ?? Key;

            try
            {
                return ResourceConstants.GetString(Key) ?? Default ?? Key;
            }
            catch
            {
                return Default ?? Key;
            }
        }
    }
}
