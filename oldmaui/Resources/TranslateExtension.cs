using System;
using Microsoft.Maui.Controls.Xaml;
using Microsoft.Maui.Controls;

namespace CloudCheckInMaui.Resources
{
    [ContentProperty("Text")]
    public class TranslateExtension : IMarkupExtension
    {
        public string Text { get; set; }

        public object ProvideValue(IServiceProvider serviceProvider)
        {
            if (Text == null)
                return "";

            return AppTranslater.Translate(Text);
        }
    }
} 