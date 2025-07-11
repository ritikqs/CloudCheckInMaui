using Microsoft.Maui.Controls;
using CloudCheckInMaui.Models;
using System.Collections.Generic;
using System.Text.Json;
using Microsoft.Maui.Storage;
using CloudCheckInMaui.ConstantHelper;
using CloudCheckInMaui.Views;
using CommunityToolkit.Maui;
using CommunityToolkit.Maui.Views;
using System.Diagnostics;
using System.Threading;
using CloudCheckInMaui.Services;
using CloudCheckInMaui.ViewModels;
using Microsoft.Extensions.DependencyInjection;
//using Newtonsoft.Json;

namespace CloudCheckInMaui;

public partial class App : Application
{
	// Properties needed for API service
	public static string Token = "";
	public static string Role = "";
	public static string SiteName = "";
	public static string CameraFrom = "";
	public static bool CheckedIn;
	public static string CheckedInSite;
	public static bool IsOnHomePage;
	public static bool IsCameraNavigated;
    public static bool CanApplyHoliday;
    public static string DBKey = "cd7ede46-d841-4647-9af2-6a79ad64e30c";
	public static string XApikey = "pgH7QzFHJx4w46fI~5Uzi4RvtTwlEXp";
	public static User LoggedInUser;
	
	// Properties needed for image services
	public static List<byte[]> ImageList = new List<byte[]>();
	public static byte[] FaceRecogImage;
	public static string FaceMathError;

	private static readonly SemaphoreSlim _loaderSemaphore = new SemaphoreSlim(1, 1);
	public static LoaderPopup LoaderInstance;
	private static bool _isLoaderActive;

	public static int GeofenceTimeInterval = 60000; // Default value

	public App()
	{
		InitializeComponent();
		Current.UserAppTheme = AppTheme.Light;
		CameraFrom = string.Empty;
		
		// Initialize the main page with AppShell
		MainPage = new AppShell();

		// Add handler to force hide loader on MainPage change
		this.PropertyChanged += (s, e) =>
		{
			if (e.PropertyName == nameof(MainPage))
			{
				MainThread.BeginInvokeOnMainThread(async () =>
				{
					if (LoaderInstance != null)
					{
						try { await LoaderInstance.CloseAsync(); } catch { }
						LoaderInstance = null;
						_isLoaderActive = false;
					}
				});
			}
		};
	}
	
	private void CheckUser()
	{
		// Check if user data exists in preferences
		if (Preferences.Default.ContainsKey(ConstantHelper.Constants.UserData))
		{
			var userData = Preferences.Default.Get(ConstantHelper.Constants.UserData, "");
            //LoggedInUser = JsonSerializer.Deserialize<User>(userData);
            LoggedInUser = System.Text.Json.JsonSerializer.Deserialize<User>(userData);
            Token = LoggedInUser.Token;
			
			// Navigate to authentication page
			MainPage = new AppShell();
			Shell.Current.GoToAsync("//Auth");
		}
		else
		{
			// Navigate to login page
			MainPage = new AppShell();
			Shell.Current.GoToAsync("//Login");
		}
	}

	public static async void ShowLoader()
	{
		try
		{
			await _loaderSemaphore.WaitAsync();

			if (_isLoaderActive)
			{
				Debug.WriteLine("Loader is already active, skipping show operation");
				return;
			}

			await MainThread.InvokeOnMainThreadAsync(async () =>
			{
				try
				{
					if (LoaderInstance != null)
					{
						try
						{
							await LoaderInstance.CloseAsync();
						}
						catch (Exception)
						{
							// Ignore any errors when trying to close existing instance
						}
						LoaderInstance = null;
					}

					LoaderInstance = new LoaderPopup();
					if (Current?.Windows.FirstOrDefault()?.Page != null)
					{
						await Current.Windows.FirstOrDefault().Page.ShowPopupAsync(LoaderInstance);
						_isLoaderActive = true;
						Debug.WriteLine("Loader shown successfully");
					}
					else
					{
						Debug.WriteLine("Cannot show loader: no valid page found");
					}
				}
				catch (Exception ex)
				{
					Debug.WriteLine($"Error in ShowLoader UI thread: {ex.Message}");
					_isLoaderActive = false;
					LoaderInstance = null;
				}
			});
		}
		catch (Exception ex)
		{
			Debug.WriteLine($"Error in ShowLoader: {ex.Message}");
			_isLoaderActive = false;
			LoaderInstance = null;
		}
		finally
		{
			_loaderSemaphore.Release();
		}
	}

	public static async void HideLoader()
	{
		try
		{
			await _loaderSemaphore.WaitAsync();

			if (!_isLoaderActive || LoaderInstance == null)
			{
				Debug.WriteLine("No active loader to hide");
				return;
			}

			await MainThread.InvokeOnMainThreadAsync(async () =>
			{
				try
				{
					await LoaderInstance.CloseAsync();
					Debug.WriteLine("Loader hidden successfully");
				}
				catch (ObjectDisposedException)
				{
					Debug.WriteLine("Loader was already disposed");
				}
				catch (Exception ex)
				{
					Debug.WriteLine($"Error hiding loader: {ex.Message}");
				}
				finally
				{
					LoaderInstance = null;
					_isLoaderActive = false;
				}
			});
		}
		catch (Exception ex)
		{
			Debug.WriteLine($"Error in HideLoader: {ex.Message}");
		}
		finally
		{
			_loaderSemaphore.Release();
		}
	}

	protected override void OnStart()
	{
		base.OnStart();
		RestoreTimerIfNeeded();
	}

	protected override void OnResume()
	{
		base.OnResume();
		RestoreTimerIfNeeded();
		// Automatically sync offline timesheets on resume
		var timesheetViewModel = this.Handler?.MauiContext?.Services.GetService<TimesheetViewModel>();
		if (timesheetViewModel != null)
		{
			MainThread.BeginInvokeOnMainThread(async () => await timesheetViewModel.SyncOfflineTimeSheetAsync());
		}
		// Only validate refresh token if user is not already logged in and we're not in the middle of a face verification process
		if (LoggedInUser != null && !string.IsNullOrEmpty(Token) && !IsCameraNavigated)
		{
			var authViewModel = this.Handler?.MauiContext?.Services.GetService<AuthViewModel>();
			if (authViewModel != null)
			{
				MainThread.BeginInvokeOnMainThread(async () => await authViewModel.ValidateRefreshTokenAsync());
			}
		}
		// Fetch monitoring time on resume
		MainThread.BeginInvokeOnMainThread(async () =>
		{
			try
			{
				var response = await CloudCheckInMaui.Services.ApiService.ApiHelper.CallApi(System.Net.Http.HttpMethod.Get, CloudCheckInMaui.ConstantHelper.Constants.GetMonitoringTime, null, false);
				if (response != null && response.Status)
				{
					// Try to parse the result as an int (seconds or minutes)
					if (int.TryParse(response.Result?.ToString(), out var interval))
					{
						GeofenceTimeInterval = 60000 * interval; // Assuming interval is in minutes
						Preferences.Default.Set(CloudCheckInMaui.ConstantHelper.Constants.MonitoringTime, interval.ToString());
					}
					else
					{
						GeofenceTimeInterval = 60000; // Default fallback
					}
				}
				else
				{
					GeofenceTimeInterval = 60000; // Default fallback
				}
			}
			catch
			{
				GeofenceTimeInterval = 60000; // Default fallback
			}
		});
	}

	private void RestoreTimerIfNeeded()
	{
		bool isTimerRunning = Preferences.Get("IsTimerRunning", false);
		if (isTimerRunning)
		{
			string startTimeStr = Preferences.Get("TimerStartTime", null);
			if (!string.IsNullOrEmpty(startTimeStr) && DateTime.TryParse(startTimeStr, out var startTime))
			{
				TimerService.Instance.StartTimer(startTime);
			}
		}
	}
}