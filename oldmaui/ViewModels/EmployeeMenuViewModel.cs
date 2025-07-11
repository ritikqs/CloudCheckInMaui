using System;
using System.Threading.Tasks;
using System.Windows.Input;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Storage;

namespace CloudCheckInMaui.ViewModels
{
    public class EmployeeMenuViewModel : BaseViewModel
    {
        private string _userName;
        public string UserName
        {
            get => _userName;
            set => SetProperty(ref _userName, value);
        }
        
        public ICommand CloseCommand { get; }
        public ICommand HomeCommand { get; }
        public ICommand TimesheetCommand { get; }
        public ICommand HolidayCommand { get; }
        public ICommand LogoutCommand { get; }
        
        public EmployeeMenuViewModel()
        {
            // Get user name from App
            if (App.LoggedInUser != null)
            {
                UserName = $"{App.LoggedInUser.FirstName} {App.LoggedInUser.LastName}";
            }
            else
            {
                UserName = "User";
            }
            
            // Initialize commands
            CloseCommand = new Command(async () => await GoBack());
            HomeCommand = new Command(async () => await GoToHome(0)); // Home tab index is 0
            TimesheetCommand = new Command(async () => await GoToHome(1)); // Timesheet tab index is 1
            HolidayCommand = new Command(async () => await GoToHome(2)); // Holiday tab index is 2
            LogoutCommand = new Command(async () => await Logout());
        }
        
        private async Task GoBack()
        {
            // Go back to previous page
            await Shell.Current.GoToAsync("..");
        }
        
        private async Task GoToHome(int tabIndex)
        {
            // Navigate to home and then switch to the specified tab
            await Shell.Current.GoToAsync("//Home");
            
            // Give it a moment to load the page
            await Task.Delay(100);
            
            // Send message to switch tab
            MessagingCenter.Send(tabIndex.ToString(), "SwitchTab");
        }
        
        private async Task Logout()
        {
            // Clear user data
            App.LoggedInUser = null;
            App.Token = string.Empty;
            
            // Clear preferences
            Preferences.Default.Remove(ConstantHelper.Constants.UserData);
            
            // Navigate to login page
            await Shell.Current.GoToAsync("//Login");
        }
    }
} 