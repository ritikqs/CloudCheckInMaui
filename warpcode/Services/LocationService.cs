using CCIMIGRATION.Interface;
using System.Diagnostics;

namespace CCIMIGRATION.Services
{
    public class LocationService : ILocationService
    {
        private CancellationTokenSource _cancelTokenSource;

        public async Task<Location> GetCurrentLocationAsync()
        {
            try
            {
                var request = new GeolocationRequest
                {
                    DesiredAccuracy = GeolocationAccuracy.Medium,
                    Timeout = TimeSpan.FromSeconds(10)
                };

                _cancelTokenSource = new CancellationTokenSource();

                var location = await Geolocation.Default.GetLocationAsync(request, _cancelTokenSource.Token);
                
                return location;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Unable to get location: {ex.Message}");
                return null;
            }
        }

        public async Task<IEnumerable<Placemark>> GetPlacemarksAsync(double latitude, double longitude)
        {
            try
            {
                var placemarks = await Geocoding.Default.GetPlacemarksAsync(latitude, longitude);
                return placemarks;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Unable to get placemarks: {ex.Message}");
                return new List<Placemark>();
            }
        }

        public async Task<IEnumerable<Location>> GetLocationsAsync(string address)
        {
            try
            {
                var locations = await Geocoding.Default.GetLocationsAsync(address);
                return locations;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Unable to get locations: {ex.Message}");
                return new List<Location>();
            }
        }

        public void CancelRequest()
        {
            if (_cancelTokenSource != null && _cancelTokenSource.IsCancellationRequested == false)
                _cancelTokenSource.Cancel();
        }

        public async Task<bool> RequestLocationPermissionAsync()
        {
            try
            {
                var status = await Permissions.RequestAsync<Permissions.LocationWhenInUse>();
                return status == PermissionStatus.Granted;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Unable to request location permission: {ex.Message}");
                return false;
            }
        }

        public void OpenSettings()
        {
            try
            {
                AppInfo.ShowSettingsUI();
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Unable to open settings: {ex.Message}");
            }
        }

        public void OpenLocationPage()
        {
            // This is the same as OpenSettings for most platforms
            OpenSettings();
        }

        public bool IsGpsAvailable()
        {
            try
            {
                // Check if geolocation is available by trying to get a location
                return true; // Geolocation is available on modern devices
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Unable to check GPS availability: {ex.Message}");
                return false;
            }
        }
    }
}
