using CCIMIGRATION.Services;
using CCIMIGRATION.CustomControls;
using CCIMIGRATION.Interface;
using CCIMIGRATION.SiteManagerViews;
using CCIMIGRATION.ViewModels;
using CCIMIGRATION.Views.SiteManagerViews;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Maui.Controls.Xaml;
using Microsoft.Maui.Controls.Compatibility;
using Microsoft.Maui.Controls;
using Microsoft.Maui;

namespace CCIMIGRATION.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class HomeTabbedPage : TabbedPage
    {
        public event EventHandler UpdateIcons;
        Page currentPage;

        public HomeTabbedPage()
        {
            InitializeComponent();
            if (App.LoggedInUser != null && App.LoggedInUser.Role == "Site Manager")
            {
                this.Children[2] = new SiteManagerHolidayPage()
                {
                    Title = "Holiday",
                    IconImageSource = "holiday"
                };
            }
            else
            {
                this.Children[2] = new HolidayPage()
                {
                    Title = "Holiday",
                    IconImageSource = "holiday"
                };
            }
            //this.CurrentPageChanged += HomeTabbedPage_CurrentPageChanged;
            MessagingService.Subscribe(this, "SwitchTab", (sender) =>
            {
                this.CurrentPage = Children[Convert.ToInt32(sender)];
            });
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            MessagingService.SubscribeEnum<TransitionType>(this, AppSettings.TransitionMessage, (transitionType) =>
            {
                var transitionNavigationPage = this.Parent as TransitionNavigationPage;

                if (transitionNavigationPage != null)
                {
                    transitionNavigationPage.TransitionType = transitionType;
                    if (App.LoggedInUser != null && App.LoggedInUser.Role.ToLower() == "site manager")
                    {
                        Navigation.PushAsync(new SiteManagerMenuPage());
                    }
                    else
                    {
                        Navigation.PushAsync(new EmployeeMenuPage());
                    }
                }
            });
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            //MessagingService.Unsubscribe<string>(this, "SwitchTab");
            MessagingService.Unsubscribe(this);
        }

        private void HomeTabbedPage_CurrentPageChanged(object sender, EventArgs e)
        {
            var currentBinding = currentPage.BindingContext as IIconChange;
            if (currentBinding != null)
                currentBinding.IsSelected = false;

            currentPage = CurrentPage;
            currentBinding = currentPage.BindingContext as IIconChange;
            if (currentBinding != null)
                currentBinding.IsSelected = true;

            UpdateIcons?.Invoke(this, EventArgs.Empty);
        }
    }
}
