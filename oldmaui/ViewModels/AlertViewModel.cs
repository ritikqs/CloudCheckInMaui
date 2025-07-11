using CloudCheckInMaui.ConstantHelper;
using CloudCheckInMaui.Models;
using CloudCheckInMaui.Models.SiteManagerModel;
using CloudCheckInMaui.Services.ApiService;
using CloudCheckInMaui.Services.ImageUploadService;
using Newtonsoft.Json;
using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Input;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Storage;
using System.Net.Http;

namespace CloudCheckInMaui.ViewModels
{
    public class AlertViewModel : BaseViewModel
    {
        private ObservableCollection<EvacuationMessagesList> _evacuationMessages = new ObservableCollection<EvacuationMessagesList>();
        public ObservableCollection<EvacuationMessagesList> EvacuationMessages
        {
            get => _evacuationMessages;
            set => SetProperty(ref _evacuationMessages, value);
        }

        private ObservableCollection<EvacuationEmployeeListModel> _alertsList = new ObservableCollection<EvacuationEmployeeListModel>();
        public ObservableCollection<EvacuationEmployeeListModel> AlertsList
        {
            get => _alertsList;
            set => SetProperty(ref _alertsList, value);
        }

        private EvacuationMessagesList _selectedMessage;
        public EvacuationMessagesList SelectedMessage
        {
            get => _selectedMessage;
            set => SetProperty(ref _selectedMessage, value);
        }

        private string _alertMessage;
        public string AlertMessage
        {
            get => _alertMessage;
            set => SetProperty(ref _alertMessage, value);
        }

        private bool _isLoading;
        public bool IsLoading
        {
            get => _isLoading;
            set => SetProperty(ref _isLoading, value);
        }

        private bool _isAdmin;
        public bool IsAdmin
        {
            get => _isAdmin;
            set => SetProperty(ref _isAdmin, value);
        }

        private string _capturedPhotoPath;
        public string CapturedPhotoPath
        {
            get => _capturedPhotoPath;
            set => SetProperty(ref _capturedPhotoPath, value);
        }

        private byte[] _capturedPhotoBytes;

        public ICommand LoadMessagesCommand { get; }
        public ICommand LoadAlertsCommand { get; }
        public ICommand RefreshCommand { get; }
        public ICommand CapturePhotoCommand { get; }
        public ICommand SendAlertCommand { get; }
        public ICommand MarkAsReadCommand { get; }

        public AlertViewModel()
        {
            Title = "Alerts";
            LoadAlertsCommand = new Command(async () => await LoadAlerts());
            LoadMessagesCommand = new Command(async () => await LoadMessages());
            RefreshCommand = new Command(async () => await LoadAlerts());
            CapturePhotoCommand = new Command(async () => await CapturePhoto());
            SendAlertCommand = new Command(async () => await SendAlert());
            MarkAsReadCommand = new Command<EvacuationEmployeeListModel>(async (alert) => await MarkAsRead(alert));
            
            IsAdmin = App.LoggedInUser?.Role?.ToLower() == "admin" || App.LoggedInUser?.Role?.ToLower() == "manager";
            
            // Initialize
            Task.Run(async () =>
            {
                await LoadMessages();
                await LoadAlerts();
            });
        }

        private async Task LoadMessages()
        {
            try
            {
                App.ShowLoader();
                EvacuationMessages.Clear();
                
                var response = await ApiHelper.CallApi(new System.Net.Http.HttpMethod("GET"), "alert/evacuationmessages", null);
                
                if (response.Status)
                {
                    var messages = JsonConvert.DeserializeObject<EvacuationMessagesList[]>(response.Data.ToString());
                    foreach (var message in messages)
                    {
                        EvacuationMessages.Add(message);
                    }
                    
                    if (EvacuationMessages.Count > 0)
                    {
                        SelectedMessage = EvacuationMessages[0];
                        AlertMessage = SelectedMessage.Description;
                    }
                }
                else
                {
                    await DisplayAlert("Error", response.Message, "OK");
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", $"Failed to load messages: {ex.Message}", "OK");
            }
            finally
            {
                App.HideLoader();
            }
        }

        private async Task LoadAlerts()
        {
            try
            {
                App.ShowLoader();
                AlertsList.Clear();
                
                string endpoint = IsAdmin ? "alert/evacuationlist" : $"alert/evacuation/{App.LoggedInUser.EmployeeId}";
                var response = await ApiHelper.CallApi(HttpMethod.Get, endpoint);
                
                if (response.Status)
                {
                    var alerts = response.GetData<EvacuationEmployeeListModel[]>();
                    if (alerts != null)
                    {
                        foreach (var alert in alerts)
                        {
                            AlertsList.Add(alert);
                        }
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
                App.HideLoader();
            }
        }

        private async Task CapturePhoto()
        {
            try
            {
                var photo = await MediaPicker.CapturePhotoAsync(new MediaPickerOptions
                {
                    Title = "Take Photo"
                });

                if (photo != null)
                {
                    var stream = await photo.OpenReadAsync();
                    using var memoryStream = new MemoryStream();
                    await stream.CopyToAsync(memoryStream);
                    _capturedPhotoBytes = memoryStream.ToArray();
                    CapturedPhotoPath = photo.FullPath;
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", ex.Message, "OK");
            }
        }

        private async Task SendAlert()
        {
            if (string.IsNullOrEmpty(AlertMessage))
            {
                await DisplayAlert("Error", "Please enter a message or select a predefined message", "OK");
                return;
            }

            try
            {
                IsLoading = true;

                // Get all employees
                var employeesResponse = await ApiHelper.CallApi(HttpMethod.Get, "alert/employeeslist");
                if (!employeesResponse.Status)
                {
                    await DisplayAlert("Error", "Failed to get employees list", "OK");
                    return;
                }

                var employees = employeesResponse.GetData<Employee[]>();
                if (employees == null)
                {
                    await DisplayAlert("Error", "No employees found", "OK");
                    return;
                }

                string[] employeeIds = new string[employees.Length];
                for (int i = 0; i < employees.Length; i++)
                {
                    employeeIds[i] = employees[i].EmpId.ToString();
                }

                var request = new CreateEvacuationRequestModel
                {
                    EmpId = employeeIds,
                    UserId = App.LoggedInUser.EmployeeId.ToString(),
                    Message = AlertMessage,
                    Image = _capturedPhotoBytes != null ? Convert.ToBase64String(_capturedPhotoBytes) : null
                };

                var alertResponse = await ApiHelper.CallApi(HttpMethod.Post, "alert/evacuation", JsonConvert.SerializeObject(request));
                
                if (alertResponse.Status)
                {
                    await DisplayAlert("Success", "Alert sent successfully", "OK");
                    AlertMessage = string.Empty;
                    CapturedPhotoPath = null;
                    _capturedPhotoBytes = null;
                    
                    // Refresh alerts
                    await LoadAlerts();
                }
                else
                {
                    await DisplayAlert("Error", alertResponse.Message, "OK");
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", ex.Message, "OK");
            }
            finally
            {
                IsLoading = false;
            }
        }

        private async Task MarkAsRead(EvacuationEmployeeListModel alert)
        {
            if (alert == null || alert.IsRead)
                return;

            try
            {
                IsLoading = true;

                var request = new UpdateEvacuation
                {
                    Id = alert.Id,
                    EmpId = App.LoggedInUser.EmployeeId.ToString()
                };

                var response = await ApiHelper.CallApi(HttpMethod.Post, "alert/markasread", JsonConvert.SerializeObject(request));
                
                if (response.Status)
                {
                    alert.IsRead = true;
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
                IsLoading = false;
            }
        }
    }
} 