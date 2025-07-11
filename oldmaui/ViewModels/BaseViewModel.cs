using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Microsoft.Maui.Controls;
using CloudCheckInMaui.ConstantHelper;
using Newtonsoft.Json;
using CloudCheckInMaui.Models;
using CloudCheckInMaui.Transitioning;

namespace CloudCheckInMaui.ViewModels
{
    public class BaseViewModel : INotifyPropertyChanged
    {
        private bool _isBusy;
        public bool IsBusy
        {
            get => _isBusy;
            set => SetProperty(ref _isBusy, value);
        }

        private string _title = string.Empty;
        public string Title
        {
            get => _title;
            set => SetProperty(ref _title, value);
        }

        public User LoginUser { get; set; }

        protected bool SetProperty<T>(ref T backingStore, T value,
            [CallerMemberName] string propertyName = "",
            Action onChanged = null)
        {
            if (EqualityComparer<T>.Default.Equals(backingStore, value))
                return false;

            backingStore = value;
            onChanged?.Invoke();
            OnPropertyChanged(propertyName);
            return true;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public async Task DisplayAlert(string title, string message, string cancel)
        {
            await Application.Current.MainPage.DisplayAlert(title, message, cancel);
        }

        public async Task<bool> DisplayAlert(string title, string message, string accept, string cancel)
        {
            return await Application.Current.MainPage.DisplayAlert(title, message, accept, cancel);
        }

        public async Task<string> DisplayActionSheet(string title, string cancel, string destruction, params string[] buttons)
        {
            return await Application.Current.MainPage.DisplayActionSheet(title, cancel, destruction, buttons);
        }
        
        // Navigation methods using the proper Xamarin-style messaging
        public void OpenMenu()
        {
            // Send the OpenMenu transition type using the proper format
            MessagingCenter.Send(this, TransitionConstants.TransitionMessage, TransitionType.OpenMenu);
        }
        
        public void SwitchTab(int tabIndex)
        {
            MessagingCenter.Send(tabIndex.ToString(), "SwitchTab");
        }
        
        // Method to navigate to a specific page
        public async Task NavigateToPageAsync(string route, bool isModal = false)
        {
            if (isModal)
            {
                // Modal navigation in MAUI
                await Shell.Current.GoToAsync(route, true);
            }
            else
            {
                // Standard navigation in MAUI
                await Shell.Current.GoToAsync(route);
            }
        }
        
        // Method to navigate back
        public async Task NavigateBackAsync()
        {
            await Shell.Current.GoToAsync("..");
        }
        
        public void GetUser()
        {
            if (Preferences.ContainsKey(ConstantHelper.Constants.UserData))
            {
                var user = Preferences.Get(ConstantHelper.Constants.UserData, "");
                LoginUser = JsonConvert.DeserializeObject<User>(user);
                if (LoginUser != null)
                {
                    App.LoggedInUser = LoginUser;
                    App.Token = LoginUser.Token;
                    App.Role = LoginUser.Role;
                    // If you have a RoleType property in App, set it here as well
                    // App.RoleType = LoginUser.RoleType;
                }
            }
        }
    }
} 