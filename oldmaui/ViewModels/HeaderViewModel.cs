using System;
using System.Windows.Input;
using Microsoft.Maui.Controls;

namespace CloudCheckInMaui.ViewModels
{
    public class HeaderViewModel : BaseViewModel
    {
        private string _pageTitle;
        public string PageTitle
        {
            get => _pageTitle;
            set => SetProperty(ref _pageTitle, value);
        }

        private bool _isBackButtonVisible;
        public bool IsBackButtonVisible
        {
            get => _isBackButtonVisible;
            set => SetProperty(ref _isBackButtonVisible, value);
        }

        public ICommand BackCommand { get; }

        public HeaderViewModel()
        {
            PageTitle = "Cloud Check-In";
            IsBackButtonVisible = false;
            
            BackCommand = new Command(async () => {
                await Shell.Current.GoToAsync("..");
            });
        }
    }
} 