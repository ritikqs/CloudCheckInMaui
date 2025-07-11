using Microsoft.Maui.Controls;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;

namespace CCIMIGRATION.Services
{
    public class NavigationService : INavigationService
    {
        public async Task PushAsync(Page page)
        {
            if (Shell.Current != null)
            {
                await Shell.Current.Navigation.PushAsync(page);
            }
            else if (App.Current.MainPage is NavigationPage navigationPage)
            {
                await navigationPage.PushAsync(page);
            }
        }

        public async Task PushModalAsync(Page page)
        {
            if (Shell.Current != null)
            {
                await Shell.Current.Navigation.PushModalAsync(page);
            }
            else if (App.Current.MainPage is NavigationPage navigationPage)
            {
                await navigationPage.Navigation.PushModalAsync(page);
            }
        }

        public async Task PopAsync()
        {
            if (Shell.Current != null)
            {
                await Shell.Current.Navigation.PopAsync();
            }
            else if (App.Current.MainPage is NavigationPage navigationPage)
            {
                await navigationPage.Navigation.PopAsync();
            }
        }

        public async Task PopModalAsync()
        {
            if (Shell.Current != null)
            {
                await Shell.Current.Navigation.PopModalAsync();
            }
            else if (App.Current.MainPage is NavigationPage navigationPage)
            {
                await navigationPage.Navigation.PopModalAsync();
            }
        }

        public async Task PopToRootAsync()
        {
            if (Shell.Current != null)
            {
                await Shell.Current.Navigation.PopToRootAsync();
            }
            else if (App.Current.MainPage is NavigationPage navigationPage)
            {
                await navigationPage.Navigation.PopToRootAsync();
            }
        }

        public async Task GoToAsync(string route)
        {
            if (Shell.Current != null)
            {
                await Shell.Current.GoToAsync(route);
            }
        }

        public async Task GoToAsync(string route, IDictionary<string, object> parameters)
        {
            if (Shell.Current != null)
            {
                await Shell.Current.GoToAsync(route, parameters);
            }
            else if (App.Current.MainPage is NavigationPage navigationPage)
            {
                var page = navigationPage.CurrentPage;
                if (page.BindingContext != null && parameters != null)
                {
                    foreach (var parameter in parameters)
                    {
                        var propertyInfo = page.BindingContext.GetType().GetProperty(parameter.Key);
                        if (propertyInfo != null)
                        {
                            propertyInfo.SetValue(page.BindingContext, parameter.Value);
                        }
                    }
                }
                await navigationPage.Navigation.PushAsync(page);
            }
        }

        public void SetMainPage(Page page)
        {
            if (App.Current != null)
            {
                App.Current.MainPage = page;
            }
        }

        public Page GetCurrentPage()
        {
            if (App.Current.MainPage is NavigationPage navigationPage)
            {
                return navigationPage.CurrentPage;
            }
            return App.Current.MainPage;
        }
    }
}
