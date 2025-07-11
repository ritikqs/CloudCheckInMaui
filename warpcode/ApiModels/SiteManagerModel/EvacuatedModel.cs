using CCIMIGRATION.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace CCIMIGRATION.Models.SiteManagerModel
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
            get
            {
                return isChecked;
            }
            set
            {
                isChecked = value;
                OnPropertyChanged();
            }
        }
    }
}
