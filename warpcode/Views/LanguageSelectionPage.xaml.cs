using CCIMIGRATION.ViewModels;

namespace CCIMIGRATION.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LanguageSelectionPage : ContentPage
    {
        LanguageSelectionViewModel LanguagePageViewModel = new LanguageSelectionViewModel();
        public LanguageSelectionPage()
        {
            InitializeComponent();
            this.BindingContext = LanguagePageViewModel;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            LanguagePageViewModel.GetLanguages();
            LanguagePageViewModel.UpdateSelectedLanguage();
        }

        private void LanguageList_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            LanguageList.SelectedItem = null;
        }
    }
}
