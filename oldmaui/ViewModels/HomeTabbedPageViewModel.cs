using System;
using System.Windows.Input;
using CloudCheckInMaui.Views;
using Microsoft.Maui.Controls;

namespace CloudCheckInMaui.ViewModels
{
    public class HomeTabbedPageViewModel : BaseViewModel
    {
        #region Private/Public Properties
        private bool _isMenuVisible;
        
        public bool IsMenuVisible
        {
            get => _isMenuVisible;
            set 
            { 
                _isMenuVisible = value;
                OnPropertyChanged(nameof(IsMenuVisible));
            }
        }
        
        private string _userName;
        
        public string UserName
        {
            get => _userName;
            set
            {
                _userName = value;
                OnPropertyChanged(nameof(UserName));
            }
        }
        #endregion
        
        #region Commands
        public ICommand MenuCommand => new Command(() =>
        {
            IsMenuVisible = !IsMenuVisible;
        });
        
        public ICommand NavigateToTabCommand => new Command<int>((tabIndex) =>
        {
            // Navigate to the selected tab
            CurrentTabIndex = tabIndex;
        });
        
        public ICommand LogoutCommand => new Command(async () =>
        {
            // Logout and navigate to login page
            await Shell.Current.GoToAsync("//Login");
        });
        
        public ICommand NavigateToSettingsCommand => new Command(async () =>
        {
            // Navigate to settings page
            await Shell.Current.GoToAsync("//MainApp/SettingsPage");
        });
        
        public ICommand NavigateToProfileCommand => new Command(async () =>
        {
            // Navigate to profile page
            await Shell.Current.GoToAsync("//MainApp/ProfilePage");
        });
        #endregion
        
        #region Properties
        private int _currentTabIndex;
        
        public int CurrentTabIndex
        {
            get => _currentTabIndex;
            set
            {
                _currentTabIndex = value;
                OnPropertyChanged(nameof(CurrentTabIndex));
            }
        }
        #endregion
        
        #region Constructor
        public HomeTabbedPageViewModel()
        {
            // Initialize properties
            UserName = "User";
            IsMenuVisible = false;
            CurrentTabIndex = 0;
            Title = "Home";
        }
        #endregion
    }
} 