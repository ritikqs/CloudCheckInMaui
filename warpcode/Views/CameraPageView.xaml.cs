// using Matcha.BackgroundService; // REMOVED: Replace with MAUI background services
using CCIMIGRATION.Models;
using CCIMIGRATION.ViewModels;
using CCIMIGRATION.Services;
using System.Diagnostics;

namespace CCIMIGRATION.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CameraPageView : ContentPage
    {
        public CameraPageViewModel CameraPageViewModel = new CameraPageViewModel();
        public RegistrationRequestModel Data;

        public CameraPageView()
        {
            InitializeComponent();
            demoview.IsVisible = true;
            cameragrid.IsVisible = false;
            this.BindingContext = CameraPageViewModel;
        }

        public CameraPageView(RegistrationRequestModel data)
        {
            InitializeComponent();
            demoview.IsVisible = true;
            cameragrid.IsVisible = false;
            Data = data;
            this.BindingContext = CameraPageViewModel;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            Subscribe();
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            MessagingService.Unsubscribe(this, "Image");
        }

        private void Subscribe()
        {
            try
            {
                MessagingService.Unsubscribe(this, "Image");
                MessagingService.Subscribe(this, "Image", (sender) =>
                {
                    captureBtn.Text = "Captured"; // TODO: Fix Resource.Captured
                    try
                    {
                        if (App.Data.Count > 0)
                        {
                            switch (App.CameraFrom)
                            {
                                case "Retake":
                                    CameraPageViewModel.SubmitPhotos();
                                    break;
                                case "Login":
                                    CameraPageViewModel.SubmitPhotosForLogin();
                                    break;
                                case "Register":
                                    CameraPageViewModel.RegisterUser(Data);
                                    break;
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Debug.WriteLine(ex.Message);
                    }
                });
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
        }

        private async void captureclicked(object sender, EventArgs e)
        {
            try
            {
                var status = await Permissions.CheckStatusAsync<Permissions.Camera>();
                if (status == PermissionStatus.Granted)
                {
                    captureBtn.Text = "Capturing"; // TODO: Fix Resource.Capturing
                    App.iOSCameraCaptureCounter = 0;
                    MessagingService.Send("Capture", "Capture");
                }
                else if (status != PermissionStatus.Granted)
                {
                    status = await Permissions.RequestAsync<Permissions.Camera>();
                    if (status == PermissionStatus.Granted)
                    {
                        captureBtn.Text = "Capturing"; // TODO: Fix Resource.Capturing
                        App.iOSCameraCaptureCounter = 0;
                        MessagingService.Send("Capture", "Capture");
                    }
                    else
                    {
                        await App.Current.MainPage.DisplayAlert("Alert", "Grant camera permission", "Ok"); // TODO: Fix Resource.GrantCameraPermission
                        //UserDialogs.Instance.Toast(Resource.GrantCameraPermission);
                    }
                }
                else
                {
                    await App.Current.MainPage.DisplayAlert("Alert", "Grant camera permission", "Ok"); // TODO: Fix Resource.GrantCameraPermission
                    //UserDialogs.Instance.Toast(Resource.GrantCameraPermission);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
        }

        private void Registerclicked(object sender, EventArgs e)
        {
        }

        private async void GetStartedButton_Clicked(object sender, EventArgs e)
        {
            try
            {
                // BackgroundAggregatorService.StopBackgroundService(); // TODO: Implement with MAUI background services
                var status = await Permissions.CheckStatusAsync<Permissions.Camera>();
                if (status == PermissionStatus.Granted)
                {
                    demoview.IsVisible = false;
                    cameragrid.IsVisible = true;
                    captureBtn.IsVisible = true;
                    submitBtn.IsVisible = false;
                }
                else if (status != PermissionStatus.Granted)
                {
                    status = await Permissions.RequestAsync<Permissions.Camera>();
                    if (status == PermissionStatus.Granted)
                    {
                        demoview.IsVisible = false;
                        cameragrid.IsVisible = true;
                        captureBtn.IsVisible = true;
                        submitBtn.IsVisible = false;
                    }
                    else
                    {
                        await App.Current.MainPage.DisplayAlert("Alert", "Grant camera permission", "Ok"); // TODO: Fix Resource.GrantCameraPermission
                        //UserDialogs.Instance.Toast(Resource.GrantCameraPermission);
                    }
                }
                else
                {
                    await App.Current.MainPage.DisplayAlert("Alert", "Grant camera permission", "Ok"); // TODO: Fix Resource.GrantCameraPermission
                    //UserDialogs.Instance.Toast(Resource.GrantCameraPermission);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
        }

        private void CameraBack_Clicked(object sender, EventArgs e)
        {
            Navigation.PopModalAsync();
        }

        private void HomeButton_Clicked(object sender, EventArgs e)
        {
            App.Current.MainPage = new NavigationPage(new LoginPage());
        }
    }
}
