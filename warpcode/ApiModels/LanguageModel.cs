using CCIMIGRATION.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace CCIMIGRATION.ApiModels
{
    public class LanguageModel : BaseViewModel
    {
        public string Name { get; set; }
        public string Code { get; set; }
        private bool _isSelected;
        public bool IsSelected 
        { 
            get
            {
                return _isSelected;
            }
            set
            {
                _isSelected = value;
                OnPropertyChanged();
            }
        }
    
    }
}
