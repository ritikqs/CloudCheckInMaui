using CCIMIGRATION.ConstantHelper;
using CCIMIGRATION.CustomControls;
using CCIMIGRATION.Interface;
using CCIMIGRATION.Models;
using CCIMIGRATION.MultilanguageHelper;
using CCIMIGRATION.Services;
using Newtonsoft.Json;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using static Microsoft.Maui.ApplicationModel.Permissions;
using ServiceHelper = CCIMIGRATION.Services.ServiceHelper;

namespace CCIMIGRATION.ViewModels
{
    public class BaseViewModel : INotifyPropertyChanged
    {
        public bool InternetAvailable;
        public User LoginUser = new User();
        public string UserName { get; set; }
        protected IDialogService DialogService { get; }
        
        public BaseViewModel(IDialogService dialogService = null)
        {
            try
            {
                DialogService = dialogService ?? ServiceHelper.GetService<IDialogService>() ?? new DialogService();
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error getting DialogService in BaseViewModel: {ex.Message}");
                DialogService = new DialogService();
            }
            //Connectivity.ConnectivityChanged += Connectivity_ConnectivityChanged;
            InternetAvailable = Connectivity.NetworkAccess == NetworkAccess.Internet;
            if (App.LoggedInUser != null)
            {
                UserName = App.LoggedInUser.FirstName + " " + App.LoggedInUser.LastName;
            }
           
        }

        private void Connectivity_ConnectivityChanged(object sender, ConnectivityChangedEventArgs e)
        {
            if(e.NetworkAccess == NetworkAccess.Internet)
            {
                InternetAvailable = true;
            }
            else
            {
                InternetAvailable = false;
            }
        }



        #region Methods
        public ICommand OpenMenu
        {
            get
            {
                return new Command(() =>
                {
                    MessagingService.SendEnum<TransitionType>(TransitionType.SlideFromLeft, AppSettings.TransitionMessage);
                });
            }
        }
        public async Task<PermissionStatus> CheckAndRequestPermissionAsync<T>(T permission)
                  where T : BasePermission
        {
            var status = await permission.CheckStatusAsync();
            if (status != PermissionStatus.Granted)
            {

                var statusCheck = await Permissions.RequestAsync<Permissions.LocationWhenInUse>();
                if (statusCheck == PermissionStatus.Granted)
                {
                    return statusCheck;
                }
                else 
                {
                    return statusCheck;
                }
            }
            else
            {
                return status;
            }
        }
        public void GetUser()
        {
            if (Preferences.ContainsKey(Constants.UserData))
            {
                var user = Preferences.Get(Constants.UserData, "");
                LoginUser = JsonConvert.DeserializeObject<User>(user);
                
            }
        }
        public async Task ShowLoaderAsync(string title = "Loading...")
        {
            await DialogService.ShowLoadingAsync(title);
        }
        
        public void ShowLoader(string title = "Loading...")
        {
            Task.Run(async () => await ShowLoaderAsync(title));
        }
        
        public void HideLoader()
        {
            DialogService.HideLoading();
        }
        
        public async Task ShowToastAsync(string message)
        {
            try
            {
                await DialogService.ShowToastAsync(message);
            }
            catch (Exception ex)
            {
                WriteLog(ex.Message);
                // Fallback to display alert if toast fails
                await App.Current.MainPage.DisplayAlert("Alert", message, "Ok");
            }
        }
        
        public void ShowToast(string message)
        {
            Task.Run(async () => await ShowToastAsync(message));
        }
        public void WriteLog(string message)
        {
            Debug.WriteLine(message);
        }
        public void InternetError()
        {
            App.Current.MainPage.DisplayAlert("Alert", ResourceConstants.NoInternet, "Ok");
            //UserDialogs.Instance.Toast(Resource.NoInternet);
        }
        public async Task<bool> IsLocationPermissionGranted()
        {
            var status = await Permissions.CheckStatusAsync<Permissions.LocationWhenInUse>();
            if (status == PermissionStatus.Granted)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public bool IsGpsOn()
        {
            return DependencyService.Get<ILocationService>().IsGpsAvailable();
        }
        public async void ShowLocationSettingAlert()
        {
            bool x = await App.Current.MainPage.DisplayAlert(ResourceConstants.EnableLocation, ResourceConstants.OpenSettingsForLocation, ResourceConstants.Ok, ResourceConstants.Cancel);
            if (x)
            {
                Application.Current.Dispatcher.Dispatch(() =>
                {
                    DependencyService.Get<ILocationService>().OpenLocationPage();
                });
            }
        }
        #endregion
        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs((propertyName)));
        }
        #region Commands
        public ICommand BackCommand
        {
            get
            {
                return new Command(() =>
                {
                    try
                    {
                        App.Current.MainPage.Navigation.PopAsync();
                    }
                    catch (Exception ex)
                    {
                        Debug.WriteLine(ex.Message);
                    }
                });
            }
        }
        #endregion
    }
}
