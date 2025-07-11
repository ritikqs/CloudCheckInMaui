using System;
using System.Timers;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Devices.Sensors;
using Microsoft.Maui.ApplicationModel;
using System.Threading.Tasks;
using CloudCheckInMaui.Models;
using CloudCheckInMaui.Services.ApiService;
using Newtonsoft.Json;
using System.Net.Http;

namespace CloudCheckInMaui.Services
{
    public class TimerService
    {
        private static TimerService _instance;
        private System.Timers.Timer _timer;
        private DateTime _startTime;
        private bool _isRunning;
        private IGeolocation _geolocation;
        private int _outLocationCounter;
        private const int MAX_OUT_OF_SITE_COUNT = 2;

        public event EventHandler<string> TimerTick;
        public event EventHandler<string> LocationWarning;
        public event EventHandler<DateTime> AutoCheckOut;

        public static TimerService Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new TimerService();
                }
                return _instance;
            }
        }

        private TimerService()
        {
            _timer = new System.Timers.Timer();
            _timer.Interval = 1000; // Timer for UI update
            _timer.Elapsed += Timer_Elapsed;
            _timer.AutoReset = true;
            
            _geolocation = Geolocation.Default;
            _outLocationCounter = 0;
        }

        public void StartTimer(DateTime startTime)
        {
            _startTime = startTime;
            _isRunning = true;
            _timer.Start();
            StartLocationMonitoring();

            // Persist timer state
            Preferences.Set("TimerStartTime", _startTime.ToString("o")); // ISO 8601
            Preferences.Set("IsTimerRunning", true);
        }

        public void StopTimer()
        {
            _isRunning = false;
            _timer.Stop();
            StopLocationMonitoring();
            _outLocationCounter = 0;

            // Remove persisted timer state
            Preferences.Set("IsTimerRunning", false);
            Preferences.Remove("TimerStartTime");
        }

        public void Cleanup()
        {
            try
            {
                if (_timer != null)
                {
                    _timer.Stop();
                    _timer.Elapsed -= Timer_Elapsed;
                    _timer.Dispose();
                    _timer = null;
                }
                _isRunning = false;
                StopLocationMonitoring();
                _outLocationCounter = 0;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error cleaning up timer: {ex.Message}");
            }
        }

        private void Timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            try
            {
                if (!_isRunning) return;

                var elapsedTime = DateTime.UtcNow - _startTime;
                var timeString = elapsedTime.ToString(@"hh\:mm\:ss");
                
                TimerTick?.Invoke(this, timeString);

                // Check location every minute
                if (elapsedTime.Seconds == 0)
                {
                    CheckLocation();
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error in timer elapsed: {ex.Message}");
            }
        }

        private async void CheckLocation()
        {
            try
            {
                if (!_isRunning) return;

                var location = await _geolocation.GetLocationAsync(new GeolocationRequest
                {
                    DesiredAccuracy = GeolocationAccuracy.Best,
                    Timeout = TimeSpan.FromSeconds(5)
                });

                if (location != null)
                {
                    var currentSiteData = GetCurrentSiteData();
                    if (currentSiteData != null)
                    {
                        var request = new GeofenceCheckRequest
                        {
                            SiteMapRefLat = location.Latitude.ToString(),
                            SiteMapRefLong = location.Longitude.ToString()
                        };

                        var response = await ApiHelper.CallApi(
                            HttpMethod.Post,
                            ConstantHelper.Constants.GeofenceSite,
                            JsonConvert.SerializeObject(request)
                        );

                        if (!response.Status)
                        {
                            HandleOutOfSite();
                        }
                        else
                        {
                            // Reset counter if back in site
                            _outLocationCounter = 0;
                        }
                    }
                }
                else
                {
                    HandleOutOfSite();
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error checking location: {ex.Message}");
            }
        }

        private void HandleOutOfSite()
        {
            _outLocationCounter++;

            if (_outLocationCounter == 1)
            {
                LocationWarning?.Invoke(this, "You are outside the site boundary. Please return to the site to avoid automatic checkout.");
            }
            else if (_outLocationCounter >= MAX_OUT_OF_SITE_COUNT)
            {
                var checkoutTime = DateTime.UtcNow;
                AutoCheckOut?.Invoke(this, checkoutTime);
                StopTimer();
            }
        }

        private SiteModel GetCurrentSiteData()
        {
            try
            {
                var siteJson = Preferences.Get("CheckedInSite", "");
                if (!string.IsNullOrEmpty(siteJson))
                {
                    return JsonConvert.DeserializeObject<SiteModel>(siteJson);
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error getting site data: {ex.Message}");
            }
            return null;
        }

        private void StartLocationMonitoring()
        {
            _outLocationCounter = 0;
        }

        private void StopLocationMonitoring()
        {
            _outLocationCounter = 0;
        }

        public bool IsRunning => _isRunning;
    }
} 