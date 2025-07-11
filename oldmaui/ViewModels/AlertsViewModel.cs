using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using CloudCheckInMaui.Models;
using System.Net.Http;
using CloudCheckInMaui.Services.ApiService;

namespace CloudCheckInMaui.ViewModels
{
    public class AlertsViewModel : BaseViewModel
    {
        private ObservableCollection<AlertModel> _alerts;
        public ObservableCollection<AlertModel> Alerts
        {
            get => _alerts;
            set => SetProperty(ref _alerts, value);
        }

        public AlertsViewModel()
        {
            Title = "Alerts";
            Alerts = new ObservableCollection<AlertModel>();
        }

        public async Task LoadAlertsAsync()
        {
            try
            {
                IsBusy = true;

                var response = await ApiHelper.CallApi(HttpMethod.Get, $"alert/list/{App.LoggedInUser.EmployeeId}");
                
                if (response.Status)
                {
                    var alerts = response.GetData<List<AlertModel>>();
                    Alerts.Clear();
                    foreach (var alert in alerts)
                    {
                        Alerts.Add(alert);
                    }
                }
                else
                {
                    await DisplayAlert("Error", response.Message, "OK");
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", ex.Message, "OK");
            }
            finally
            {
                IsBusy = false;
            }
        }
    }
} 