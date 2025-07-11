using Microsoft.Maui.Controls;

namespace CloudCheckInMaui.Views.Headers
{
    public partial class PageHeaders : ContentView
    {
        public PageHeaders()
        {
            InitializeComponent();
        }
        
        public static readonly BindableProperty TitleProperty = BindableProperty.Create(
            propertyName: "Title", 
            returnType: typeof(string), 
            declaringType: typeof(PageHeaders), 
            defaultValue: "");
            
        public string Title
        {
            get { return (string)GetValue(TitleProperty); }
            set { SetValue(TitleProperty, value); }
        }
    }
} 