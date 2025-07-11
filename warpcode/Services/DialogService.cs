using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Core;
using Microsoft.Maui.Controls;

namespace CCIMIGRATION.Services;

public interface IDialogService
{
    Task ShowLoadingAsync(string message = "Loading...");
    void HideLoading();
    Task ShowToastAsync(string message, ToastDuration duration = ToastDuration.Short);
    Task<bool> ShowConfirmAsync(string title, string message, string accept = "Yes", string cancel = "No");
    Task ShowAlertAsync(string title, string message, string cancel = "OK");
    Task<string> ShowPromptAsync(string title, string message, string accept = "OK", string cancel = "Cancel", string placeholder = "", int maxLength = -1, Keyboard keyboard = null, string initialValue = "");
}

public class DialogService : IDialogService
{
    private bool _isLoadingVisible = false;
    private Page _currentPage;

    public DialogService()
    {
        // Get the current page
        _currentPage = Application.Current?.MainPage;
    }

    public async Task ShowLoadingAsync(string message = "Loading...")
    {
        if (_isLoadingVisible) return;
        
        _isLoadingVisible = true;
        
        // For now, we'll use a simple approach. In a more complex implementation,
        // you might want to show a custom popup or use a loading indicator
        await MainThread.InvokeOnMainThreadAsync(() =>
        {
            // You can implement a custom loading popup here
            // For now, we'll just set a flag
        });
    }

    public void HideLoading()
    {
        if (!_isLoadingVisible) return;
        
        _isLoadingVisible = false;
        
        MainThread.BeginInvokeOnMainThread(() =>
        {
            // Hide the loading indicator
        });
    }

    public async Task ShowToastAsync(string message, ToastDuration duration = ToastDuration.Short)
    {
        await MainThread.InvokeOnMainThreadAsync(async () =>
        {
            var toast = Toast.Make(message, duration);
            await toast.Show();
        });
    }

    public async Task<bool> ShowConfirmAsync(string title, string message, string accept = "Yes", string cancel = "No")
    {
        return await MainThread.InvokeOnMainThreadAsync(async () =>
        {
            var currentPage = GetCurrentPage();
            if (currentPage != null)
            {
                return await currentPage.DisplayAlert(title, message, accept, cancel);
            }
            return false;
        });
    }

    public async Task ShowAlertAsync(string title, string message, string cancel = "OK")
    {
        await MainThread.InvokeOnMainThreadAsync(async () =>
        {
            var currentPage = GetCurrentPage();
            if (currentPage != null)
            {
                await currentPage.DisplayAlert(title, message, cancel);
            }
        });
    }

    public async Task<string> ShowPromptAsync(string title, string message, string accept = "OK", string cancel = "Cancel", string placeholder = "", int maxLength = -1, Keyboard keyboard = null, string initialValue = "")
    {
        return await MainThread.InvokeOnMainThreadAsync(async () =>
        {
            var currentPage = GetCurrentPage();
            if (currentPage != null)
            {
                return await currentPage.DisplayPromptAsync(title, message, accept, cancel, placeholder, maxLength, keyboard, initialValue);
            }
            return null;
        });
    }

    private Page GetCurrentPage()
    {
        var mainPage = Application.Current?.MainPage;
        
        if (mainPage is NavigationPage navigationPage)
        {
            return navigationPage.CurrentPage;
        }
        
        if (mainPage is TabbedPage tabbedPage)
        {
            return tabbedPage.CurrentPage;
        }
        
        if (mainPage is FlyoutPage flyoutPage)
        {
            return flyoutPage.Detail;
        }
        
        return mainPage;
    }
}
