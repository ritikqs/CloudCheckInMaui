using CCIMIGRATION.Services;
using CCIMIGRATION.ConstantHelper;
using CCIMIGRATION.Models;
using CCIMIGRATION.MultilanguageHelper;
using CCIMIGRATION.Service.ApiService;
using CCIMIGRATION.SiteManagerViews;
using CCIMIGRATION.ViewModels;
using Newtonsoft.Json;
using System.Diagnostics;
using ServiceHelper = CCIMIGRATION.Services.ServiceHelper;

namespace CCIMIGRATION.Views.SiteManagerViews
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SiteManagerMenuPage : ContentPage
    {
        public BaseViewModel BaseViewModel;
        private readonly IDialogService _dialogService;

        public SiteManagerMenuPage()
        {
            InitializeComponent();
            _dialogService = ServiceHelper.GetService<IDialogService>();
            BaseViewModel = new BaseViewModel(_dialogService);
            this.BindingContext = BaseViewModel;
        }

        private async void Logout_Tapped(object sender, EventArgs e)
        {
            try
            {
                if (!App.CheckedIn)
                {
                    var _loginUser = JsonConvert.DeserializeObject<User>(Preferences.Get(Constants.UserData, ""));
                    await _dialogService.ShowLoadingAsync(ResourceConstants.LoggingOut);
                    var _req = new LogoutRequest()
                    {
                        UserId = Convert.ToInt32(_loginUser.Id)
                    };
                    var response = await ApiHelper.CallApi(HttpMethod.Post, Constants.Logout, JsonConvert.SerializeObject(_req), true);
                    if (response.Status)
                    {
                        var languagecode = Preferences.Get(Constants.AppLanguageCode, "");
                        if (Preferences.ContainsKey(Constants.OnceRegistered))
                        {
                            var isonceregistered = Preferences.Get(Constants.OnceRegistered, "");
                            Preferences.Clear();
                            Preferences.Set(Constants.OnceRegistered, isonceregistered);
                        }
                        else
                        {
                            Preferences.Clear();
                        }
                        Preferences.Set(Constants.AppLanguageCode, languagecode);
                        App.Current.MainPage = new NavigationPage(new LoginPage());
                    }
                    else
                    {
                        await App.Current.MainPage.DisplayAlert("Alert", ResourceConstants.SomethingWentWrong, "Ok");
                        //UserDialogs.Instance.Toast(Resource.SomethingwentWrong);
                    }
                }
                else
                {
                    await App.Current.MainPage.DisplayAlert("Alert", ResourceConstants.YouMustCheckOutFirst, "Ok");
                    //UserDialogs.Instance.Toast(Resource.YouMustCheckOutFirst);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
            finally
            {
                _dialogService.HideLoading();
            }
        }

        private void Visitor_Tapped(object sender, EventArgs e)
        {
            Navigation.PushAsync(new SiteManagerVisitorPage());
        }

        private void EvacuationTapped(object sender, EventArgs e)
        {
            Navigation.PushAsync(new EvacuationList());
        }

        private void Holiday_Tapped(object sender, EventArgs e)
        {
            MessagingService.Send("2", "SwitchTab");
            Navigation.PopAsync();
        }

        private async void Calender_Tapped(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new CalenderPage());
        }

        private void StaffOnSite_Tapped(object sender, EventArgs e)
        {
            Navigation.PushAsync(new SiteManagerHomePage());
        }

        private void TimeSheet_Tapped(object sender, EventArgs e)
        {
            MessagingService.Send("1", "SwitchTab");
            Navigation.PopAsync();
        }

        private void Home_Tapped(object sender, EventArgs e)
        {
            MessagingService.Send("0", "SwitchTab");
            Navigation.PopAsync();
        }

        private void MenuCloseTapped(object sender, EventArgs e)
        {
            Navigation.PopAsync();
        }
    }
}
