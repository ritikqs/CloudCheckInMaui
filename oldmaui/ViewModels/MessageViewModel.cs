using CloudCheckInMaui.Models;
using CloudCheckInMaui.Services.ApiService;
using Newtonsoft.Json;
using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;
using Microsoft.Maui.Controls;
using System.Net.Http;

namespace CloudCheckInMaui.ViewModels
{
    public class MessageViewModel : BaseViewModel
    {
        private ObservableCollection<Message> _messages = new ObservableCollection<Message>();
        public ObservableCollection<Message> Messages
        {
            get => _messages;
            set => SetProperty(ref _messages, value);
        }

        public ICommand LoadMessagesCommand { get; }
        public ICommand RefreshCommand { get; }

        public MessageViewModel()
        {
            Title = "Messages";
            
            LoadMessagesCommand = new Command(async () => await LoadMessages());
            RefreshCommand = new Command(async () => await RefreshMessages());
            
            // Initialize by loading messages
            Task.Run(async () => await LoadMessages());
        }

        private async Task LoadMessages()
        {
            try
            {
                App.ShowLoader();
                Messages.Clear();
                
                var response = await ApiHelper.CallApi(HttpMethod.Get, "messages");
                
                if (response.Status)
                {
                    var messages = response.GetData<Message[]>();
                    if (messages != null)
                    {
                        foreach (var message in messages)
                        {
                            Messages.Add(message);
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

        private async Task RefreshMessages()
        {
            await LoadMessages();
        }
    }
} 