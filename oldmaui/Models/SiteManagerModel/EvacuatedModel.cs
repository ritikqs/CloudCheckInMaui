using CloudCheckInMaui.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace CloudCheckInMaui.Models.SiteManagerModel
{
    public class EvacuatedModel : BaseViewModel
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string TradeName { get; set; }
        public bool IsEvavcuated { get; set; }
        
        private bool isChecked;
        public bool IsChecked 
        { 
            get => isChecked;
            set
            {
                isChecked = value;
                OnPropertyChanged();
            }
        }
    }
} 