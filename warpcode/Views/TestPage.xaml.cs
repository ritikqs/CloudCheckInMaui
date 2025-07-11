using System;
using Microsoft.Maui.Controls;

namespace CCIMIGRATION.Views
{
    public partial class TestPage : ContentPage
    {
        public TestPage()
        {
            InitializeComponent();
            System.Diagnostics.Debug.WriteLine("TestPage: Constructor called");
        }

        private async void OnLoginButtonClicked(object sender, EventArgs e)
        {
            try
            {
                System.Diagnostics.Debug.WriteLine("TestPage: Button clicked, navigating to LoginPage");
                await Navigation.PushAsync(new LoginPage());
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"TestPage: Error navigating to LoginPage: {ex.Message}");
                await DisplayAlert("Error", ex.Message, "OK");
            }
        }
    }
}
