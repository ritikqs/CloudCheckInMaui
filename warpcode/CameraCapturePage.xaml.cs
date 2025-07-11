// Camera functionality temporarily disabled
using CCIMIGRATION.Models;
using CCIMIGRATION.ViewModels;
using CCIMIGRATION.Services;
using System.Collections.ObjectModel;
using System.Diagnostics;
using Microsoft.Maui.Controls;

namespace CCIMIGRATION
{
    public partial class CameraCapturePage : ContentPage
    {
        private readonly CameraPageViewModel _viewModel;
        private readonly RegistrationRequestModel _registrationData;
        private readonly ObservableCollection<ImageSource> _capturedImages;
        private readonly List<byte[]> _imageBytes;
        private Timer _captureTimer;
        private int _captureCount = 0;
        private readonly int _targetImageCount = 5; // Capture 5 images
        private readonly int _captureIntervalMs = 2400; // 12 seconds / 5 images = 2.4 seconds
        private bool _isCapturing = false;
        private bool _isProcessing = false;

        public CameraCapturePage(RegistrationRequestModel registrationData)
        {
            InitializeComponent();
            _registrationData = registrationData;
            _viewModel = new CameraPageViewModel();
            _capturedImages = new ObservableCollection<ImageSource>();
            _imageBytes = new List<byte[]>();
            
            BindingContext = new CameraCaptureViewModel(_capturedImages);
            
            // Start automatic capture after page loads
            Loaded += OnPageLoaded;
        }

        private async void OnPageLoaded(object sender, EventArgs e)
        {
            try
            {
                // Request camera permissions
                var cameraStatus = await Permissions.CheckStatusAsync<Permissions.Camera>();
                if (cameraStatus != PermissionStatus.Granted)
                {
                    cameraStatus = await Permissions.RequestAsync<Permissions.Camera>();
                }

                if (cameraStatus != PermissionStatus.Granted)
                {
                    await DisplayAlert("Permission Required", "Camera permission is required to register", "OK");
                    await Navigation.PopModalAsync();
                    return;
                }

                // Show instruction and start capture
                await ShowCaptureInstructions();
                StartAutomaticCapture();
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error in OnPageLoaded: {ex.Message}");
                await DisplayAlert("Error", "Failed to initialize camera", "OK");
                await Navigation.PopModalAsync();
            }
        }

        private async Task ShowCaptureInstructions()
        {
            await DisplayAlert("Registration Photos", 
                "Please look at the camera. We will automatically capture 5 photos over 12 seconds for registration.", 
                "Start");
        }

        private void StartAutomaticCapture()
        {
            if (_isCapturing) return;
            
            _isCapturing = true;
            _captureCount = 0;
            
            // Disable manual controls during auto capture
            CaptureButton.IsEnabled = false;
            BackButton.IsEnabled = false;
            CompleteButton.IsEnabled = false;
            
            // Start the timer for automatic capture
            _captureTimer = new Timer(async _ => await CaptureImageAutomatically(), 
                null, 
                1000, // Start after 1 second
                _captureIntervalMs); // Capture every 2.4 seconds
        }

        private async Task CaptureImageAutomatically()
        {
            try
            {
                if (_captureCount >= _targetImageCount || _isProcessing)
                {
                    _captureTimer?.Dispose();
                    _isCapturing = false;
                    
                    if (!_isProcessing)
                    {
                        await MainThread.InvokeOnMainThreadAsync(async () => await ProcessCapturedImages());
                    }
                    return;
                }

                await MainThread.InvokeOnMainThreadAsync(async () =>
                {
                    try
                    {
                        // Simulated capture for development (camera disabled)
                        _captureCount++;
                        
                        // Create a placeholder image
                        var placeholderBytes = new byte[] { 0xFF, 0xD8, 0xFF, 0xE0, 0x00, 0x10 }; // Basic JPEG header
                        _imageBytes.Add(placeholderBytes);
                        
                        // Create ImageSource for preview
                        var imageSource = ImageSource.FromStream(() => new MemoryStream(placeholderBytes));
                        _capturedImages.Add(imageSource);
                        
                        // Update UI
                        ImageCountLabel.Text = $"Images: {_captureCount}/{_targetImageCount} (simulated)";
                        
                        Debug.WriteLine($"Simulated capture {_captureCount}/{_targetImageCount}");
                    }
                    catch (Exception ex)
                    {
                        Debug.WriteLine($"Error capturing image: {ex.Message}");
                    }
                });
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error in CaptureImageAutomatically: {ex.Message}");
            }
        }

        private async Task<byte[]> ConvertStreamToBytes(Stream stream)
        {
            using var memoryStream = new MemoryStream();
            await stream.CopyToAsync(memoryStream);
            return memoryStream.ToArray();
        }

        private async Task ProcessCapturedImages()
        {
            if (_isProcessing) return;
            _isProcessing = true;
            
            try
            {
                await DisplayAlert("Processing", "Processing captured images and registering...", "OK");
                
                // Update App.ImageList for compatibility with existing code
                App.ImageList.Clear();
                App.ImageList.AddRange(_imageBytes);
                
                // Update App.Data for compatibility
                App.Data.Clear();
                for (int i = 0; i < _imageBytes.Count; i++)
                {
                    App.Data.Add(new ImageList
                    {
                        ImageByte = _imageBytes[i]
                    });
                }
                
                // Call the registration method
                await RegisterUserWithImages();
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error processing images: {ex.Message}");
                await DisplayAlert("Error", $"Failed to process images: {ex.Message}", "OK");
                
                // Re-enable controls for retry
                CaptureButton.IsEnabled = true;
                BackButton.IsEnabled = true;
                CompleteButton.IsEnabled = _capturedImages.Count >= 4;
            }
            finally
            {
                _isProcessing = false;
            }
        }

        private async Task RegisterUserWithImages()
        {
            try
            {
                // Show loading
                var loadingPage = new ContentPage
                {
                    Content = new Microsoft.Maui.Controls.StackLayout
                    {
                        Children =
                        {
                            new ActivityIndicator { IsRunning = true, Color = Colors.Blue },
                            new Label { Text = "Registering user...", HorizontalOptions = LayoutOptions.Center }
                        },
                        VerticalOptions = LayoutOptions.Center,
                        HorizontalOptions = LayoutOptions.Center
                    },
                    BackgroundColor = Colors.White
                };
                
                await Navigation.PushModalAsync(loadingPage);
                
                // Call the existing registration logic
                _viewModel.RegisterUser(_registrationData);
                
                // Wait a moment for the registration to complete
                await Task.Delay(3000);
                
                // Remove loading page
                await Navigation.PopModalAsync();
                
                // Check if registration was successful
                if (_viewModel.IsSuccessVisible)
                {
                    await DisplayAlert("Success", "Registration completed successfully!", "OK");
                    
                    // Navigate back to login
                    App.Current.MainPage = new NavigationPage(new Views.LoginPage());
                }
                else
                {
                    throw new Exception("Registration failed");
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error in RegisterUserWithImages: {ex.Message}");
                
                // Remove loading page if still showing
                if (Navigation.ModalStack.Count > 0)
                {
                    await Navigation.PopModalAsync();
                }
                
                await DisplayAlert("Registration Failed", 
                    $"Failed to register user: {ex.Message}\n\nPlease try again.", 
                    "OK");
                
                // Re-enable controls for retry
                CaptureButton.IsEnabled = true;
                BackButton.IsEnabled = true;
                CompleteButton.IsEnabled = true;
            }
        }

        // Manual capture button (backup)
        private async void OnCaptureClicked(object sender, EventArgs e)
        {
            if (_isCapturing || _isProcessing) return;
            
            try
            {
                // Simulated manual capture (camera disabled)
                var placeholderBytes = new byte[] { 0xFF, 0xD8, 0xFF, 0xE0, 0x00, 0x10 };
                _imageBytes.Add(placeholderBytes);
                
                var imageSource = ImageSource.FromStream(() => new MemoryStream(placeholderBytes));
                _capturedImages.Add(imageSource);
                
                ImageCountLabel.Text = $"Images: {_capturedImages.Count}/6 (simulated)";
                CompleteButton.IsEnabled = _capturedImages.Count >= 4;
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", $"Failed to capture image: {ex.Message}", "OK");
            }
        }

        private async void OnCompleteClicked(object sender, EventArgs e)
        {
            if (_capturedImages.Count < 4)
            {
                await DisplayAlert("Not Enough Images", "Please capture at least 4 images", "OK");
                return;
            }
            
            await ProcessCapturedImages();
        }

        private async void OnBackClicked(object sender, EventArgs e)
        {
            _captureTimer?.Dispose();
            await Navigation.PopModalAsync();
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            _captureTimer?.Dispose();
        }
    }

    // ViewModel for binding
    public class CameraCaptureViewModel : BaseViewModel
    {
        private readonly ObservableCollection<ImageSource> _capturedImages;

        public CameraCaptureViewModel(ObservableCollection<ImageSource> capturedImages)
        {
            _capturedImages = capturedImages;
        }

        public ObservableCollection<ImageSource> CapturedImages => _capturedImages;

        public bool CanComplete => _capturedImages.Count >= 4;
    }
}
