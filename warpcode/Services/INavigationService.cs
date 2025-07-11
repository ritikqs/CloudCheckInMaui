using Microsoft.Maui.Controls;

namespace CCIMIGRATION.Services
{
    public interface INavigationService
    {
        Task PushAsync(Page page);
        Task PushModalAsync(Page page);
        Task PopAsync();
        Task PopModalAsync();
        Task PopToRootAsync();
        Task GoToAsync(string route);
        Task GoToAsync(string route, IDictionary<string, object> parameters);
        void SetMainPage(Page page);
        Page GetCurrentPage();
    }
}
