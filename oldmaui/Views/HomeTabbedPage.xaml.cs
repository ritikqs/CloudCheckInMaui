using Microsoft.Maui.Controls;
using CloudCheckInMaui.ViewModels;
using CloudCheckInMaui.ConstantHelper;
using System;
using System.Diagnostics;
using Microsoft.Maui.Controls.PlatformConfiguration;
using Microsoft.Maui.Controls.PlatformConfiguration.AndroidSpecific;
using Microsoft.Extensions.DependencyInjection;

namespace CloudCheckInMaui.Views
{
    public partial class HomeTabbedPage : Microsoft.Maui.Controls.TabbedPage
    {
        public event EventHandler UpdateIcons;
        private Page currentPage;
        private HomeTabbedPageViewModel _viewModel;
        private IServiceProvider _serviceProvider;
        
        public HomeTabbedPage()
        {
            try
        {
            InitializeComponent();

                // Set tab bar to bottom using correct MAUI API
                On<Microsoft.Maui.Controls.PlatformConfiguration.Android>()
                    .SetToolbarPlacement(ToolbarPlacement.Bottom);

                // Set colors for the tab bar
                this.BarBackgroundColor = Colors.White;
                this.BarTextColor = Colors.Gray;
                this.SelectedTabColor = Color.FromArgb("#007AFF");
                this.UnselectedTabColor = Colors.Gray;

                // Wire up the current page changed event
                this.CurrentPageChanged += HomeTabbedPage_CurrentPageChanged;

                // Handle initialization after the page is created
                Dispatcher.Dispatch(InitializeAsync);
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error in HomeTabbedPage constructor: {ex.Message}");
                Debug.WriteLine($"Stack trace: {ex.StackTrace}");
            
                // Add at least one tab as fallback
                Children.Add(new NavigationPage(CreateFallbackPage("Home")) { Title = "Home" });
            }
        }

        private async void InitializeAsync()
        {
            try
            {
                // Wait for handler to be ready
                while (Microsoft.Maui.Controls.Application.Current?.Handler?.MauiContext == null)
                {
                    await Task.Delay(50);
                }

                _serviceProvider = Microsoft.Maui.Controls.Application.Current.Handler.MauiContext.Services;

                // Create the view model using DI if available, otherwise create directly
                _viewModel = _serviceProvider?.GetService<HomeTabbedPageViewModel>() 
                    ?? new HomeTabbedPageViewModel();
                BindingContext = _viewModel;

                SetupTabs();
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error in InitializeAsync: {ex.Message}");
                Debug.WriteLine($"Stack trace: {ex.StackTrace}");
                
                // Add fallback tab if initialization fails
                MainThread.BeginInvokeOnMainThread(() =>
                {
                    Children.Clear();
                    Children.Add(new NavigationPage(CreateFallbackPage("Home")) { Title = "Home" });
                });
            }
        }

        private void SetupTabs()
        {
            try
            {
                // Add tabs in specific order with proper styling
                AddTabWithFallback(() => 
                {
                    var viewModel = _serviceProvider?.GetService<HomeViewModel>();
                    return new Home(viewModel);
                }, "Home", "home", 0);
                AddTabWithFallback(() => 
                {
                    var viewModel = _serviceProvider?.GetService<TimesheetViewModel>();
                    return new TimesheetPage(viewModel);
                }, "Timesheet", "timesheet", 1);
                AddTabWithFallback(() => new HolidayPage(), "Holiday", "holiday", 2);
                AddTabWithFallback(() => new MessagePage(), "Messages", "message", 3);
                AddTabWithFallback(() => new Alerts(), "Alerts", "alert", 4);
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error in SetupTabs: {ex.Message}");
                Debug.WriteLine($"Stack trace: {ex.StackTrace}");
                
                // Ensure at least one tab exists
                if (Children.Count == 0)
                {
                    var viewModel = _serviceProvider?.GetService<HomeViewModel>();
                    Children.Add(new NavigationPage(new Home(viewModel)) { Title = "Home" });
                }
            }
        }

        private void AddTabWithFallback(Func<Page> pageCreator, string title, string icon, int index)
        {
            try
            {
                var page = pageCreator();
                var navPage = new NavigationPage(page)
            {
                    Title = title
                };

                // Set icon and ensure it's visible
                try
                {
                    navPage.IconImageSource = icon;
                }
                catch (Exception iconEx)
                {
                    Debug.WriteLine($"Warning: Could not set icon for {title}: {iconEx.Message}");
                }

                // Add the tab at the specified index
                if (index < Children.Count)
                {
                    Children.Insert(index, navPage);
                }
                else
                {
                    Children.Add(navPage);
                }
                
                Debug.WriteLine($"Added tab page: {title} at index {index}");
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error adding tab {title}: {ex.Message}");
                Debug.WriteLine($"Stack trace: {ex.StackTrace}");
                
                // Add fallback page for this tab
                var fallbackPage = CreateFallbackPage(title);
                var fallbackNavPage = new NavigationPage(fallbackPage) 
                {
                    Title = title
                };

                try
                {
                    fallbackNavPage.IconImageSource = icon;
                }
                catch { }
                
                if (index < Children.Count)
                {
                    Children.Insert(index, fallbackNavPage);
                }
                else
                {
                    Children.Add(fallbackNavPage);
                }
            }
        }

        private ContentPage CreateFallbackPage(string title)
        {
            if (title == "Home")
            {
                var viewModel = _serviceProvider?.GetService<HomeViewModel>();
                return new Home(viewModel);
            }

            return new ContentPage
            {
                Title = title,
                Content = new StackLayout
                {
                    Children =
                    {
                        new Label
                        {
                            Text = $"Unable to load {title} tab. Please try again.",
                            HorizontalOptions = LayoutOptions.Center,
                            VerticalOptions = LayoutOptions.Center
                        }
                    }
                }
            };
        }
        
        protected override void OnAppearing()
        {
            try
        {
            base.OnAppearing();
            
            // Set this flag to help with location monitoring
            App.IsOnHomePage = true;
            
                // Check user role and update holiday page if needed
                if (App.LoggedInUser?.Role == "Site Manager")
                {
                    MainThread.BeginInvokeOnMainThread(() =>
                    {
                        try
                        {
                            var holidayPage = new NavigationPage(new HolidayPage())
                            {
                                Title = "Holiday",
                                IconImageSource = "holiday"
                            };
                            
                            if (Children.Count > 2)
                            {
                                Children[2] = holidayPage;
                            }
                        }
                        catch (Exception ex)
                        {
                            Debug.WriteLine($"Error updating holiday page: {ex.Message}");
                        }
                    });
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error in OnAppearing: {ex.Message}");
                Debug.WriteLine($"Stack trace: {ex.StackTrace}");
                    }
        }
        
        protected override void OnDisappearing()
        {
            try
        {
            base.OnDisappearing();
            App.IsOnHomePage = false;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error in OnDisappearing: {ex.Message}");
            }
        }
        
        private void HomeTabbedPage_CurrentPageChanged(object sender, EventArgs e)
        {
            try
            {
            var currentBinding = currentPage?.BindingContext as IIconChange;
            if (currentBinding != null)
                currentBinding.IsSelected = false;

            currentPage = CurrentPage;
            currentBinding = currentPage?.BindingContext as IIconChange;
            if (currentBinding != null)
                currentBinding.IsSelected = true;

            UpdateIcons?.Invoke(this, EventArgs.Empty);
            
            int tabIndex = Children.IndexOf(CurrentPage);
                Debug.WriteLine($"Tab changed to index: {tabIndex}");
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error in CurrentPageChanged: {ex.Message}");
            }
        }
    }
    
    // Interface for tab icon changes
    public interface IIconChange
    {
        bool IsSelected { get; set; }
    }
} 