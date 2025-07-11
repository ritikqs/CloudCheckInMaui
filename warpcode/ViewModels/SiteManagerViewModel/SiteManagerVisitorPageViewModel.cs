using CCIMIGRATION.Models.SiteManagerModel;
using CCIMIGRATION.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Windows.Input;
using Microsoft.Maui.Controls.Compatibility;
using Microsoft.Maui.Controls;
using Microsoft.Maui;

namespace CCIMIGRATION.SiteManagerViewModel
{
    public class SiteManagerVisitorPageViewModel:BaseViewModel
    {
        #region Private Properties
        private ObservableCollection<VisitorModel> visitorsList;
        private bool _isListRefreshing;
        #endregion

        #region Public Properties
        public ObservableCollection<VisitorModel> VisitorsList
        {
            get { return visitorsList; }
            set { visitorsList = value; }
        }
        public bool IsListRefreshing
        {
            get { return _isListRefreshing; }
            set { _isListRefreshing = value; OnPropertyChanged(); }
        }
        #endregion

        #region ICommands
        public ICommand VisitorListRefreshCommand
        {
            get
            {
                return new Command(() =>
                {
                    IsListRefreshing = false;
                    LoadVisitorList();
                });
            }
        }


        #endregion

        #region Ctor
        public SiteManagerVisitorPageViewModel()
        {
            LoadVisitorList();
        }
        #endregion

        #region Mock Data
        private void LoadVisitorList()
        {
            var list = new ObservableCollection<VisitorModel>();
            list.Add(new VisitorModel { Id = 1, Name = "Willian Bahl", Contractor = "Labour Only", Employer = "Eastro", Trade = "Groundworker" }); ;
            list.Add(new VisitorModel { Id = 2, Name = "Willian Bahl", Contractor = "Labour Only", Employer = "Eastro", Trade = "Groundworker" }); ;
            list.Add(new VisitorModel { Id = 3, Name = "Willian Bahl", Contractor = "Labour Only", Employer = "Eastro", Trade = "Groundworker" }); ;
            list.Add(new VisitorModel { Id = 4, Name = "Willian Bahl", Contractor = "Labour Only", Employer = "Eastro", Trade = "Groundworker" }); ;
            list.Add(new VisitorModel { Id = 4, Name = "Willian Bahl", Contractor = "Labour Only", Employer = "Eastro", Trade = "Groundworker" }); ;
            VisitorsList = list;
        }
        #endregion
    }
}
