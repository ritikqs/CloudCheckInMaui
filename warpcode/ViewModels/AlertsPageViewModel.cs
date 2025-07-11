using CCIMIGRATION.ConstantHelper;
using CCIMIGRATION.Models;
using CCIMIGRATION.Models.SiteManagerModel;
using CCIMIGRATION.Service.ApiService;
using Newtonsoft.Json;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace CCIMIGRATION.ViewModels
{
    public class AlertsPageViewModel : BaseViewModel
    {
        #region Public/Private Variables
        private ObservableCollection<EvacuationEmployeeListModel> evacuationsList = new ObservableCollection<EvacuationEmployeeListModel>();
        private bool _isDetailPopupVisible;
        private EvacuationEmployeeListModel selectedAlert;
        private EvacuationEmployeeListModel _alertDetail;
        public EvacuationEmployeeListModel AlertDetail
        {
            get { return _alertDetail; }
            set
            {
                _alertDetail = value;
                OnPropertyChanged();
            }
        }
        public EvacuationEmployeeListModel SelectedAlert
        {
            get { return selectedAlert; }
            set 
            { 
                selectedAlert = value;
                OnPropertyChanged();
                if(SelectedAlert != null)
                {
                    AlertDetail = SelectedAlert;
                    IsDetailPopupVisible = true;
                    MarkRead(Convert.ToInt32(SelectedAlert.Id));
                }
            }
        }

        public bool IsDetailPopupVisible
        {
            get { return _isDetailPopupVisible; }
            set { _isDetailPopupVisible = value;OnPropertyChanged(); }
        }

        public ObservableCollection<EvacuationEmployeeListModel> EvacuationsList
        {
            get { return evacuationsList; }
            set { evacuationsList = value; OnPropertyChanged(); }
        }

        public ICommand HideDetailPopup
        {
            get
            {
                return new Command(() =>
                {
                    IsDetailPopupVisible = false;
                });
            }
        }
        #endregion
        
        #region Constructor

        public AlertsPageViewModel()
        {
            GetUser();
        }
        #endregion
        #region Methods
        private void MarkRead(int id)
        {
            Application.Current.Dispatcher.Dispatch(async() =>
            {
                try
                {
                    var _req = new UpdateEvacuation()
                    {
                        Id = id,
                        EmpId = LoginUser.EmployeeId.ToString()
                    };
                    var response = await ApiHelper.CallApi(HttpMethod.Post, Constants.MarkReadEvacuation, JsonConvert.SerializeObject(_req), true);
                    if (response.Status)
                    {
                       EvacuationsList.Where(x => x.Id == id).FirstOrDefault().IsRead = true;
                    }
                    else
                    {
                        ShowToast(response.Message);
                    }
                }
                catch (Exception ex)
                {
                    WriteLog(ex.Message);
                }
            });
            
        }
        public async void GetAlerts()
        {
            try
            {
                ShowLoader("");
                EvacuationsList.Clear();
                var _req = new StaffModel.GetStaffOnSiteModel()
                {
                    draw = 0,
                    empId = LoginUser.EmployeeId.ToString(),
                    length = 50,
                    columns = new StaffModel.Column[]
                    {
                       new StaffModel.Column()
                       {
                           data = "Id",
                           name = "Id",
                           searchable = true,
                           orderable = false,
                           search = new StaffModel.Search()
                           {
                               isRegex = false,
                               value = ""
                           }
                       }
                    }.ToList(),
                    order = new StaffModel.Order[]
                    {
                        new StaffModel.Order()
                        {
                            column = 0,
                            dir = "desc"
                        }

                    }.ToList(),
                    search = new StaffModel.Search()
                    {
                        value = "",
                        isRegex = false
                    }
                };
                var response = await ApiHelper.CallApi(HttpMethod.Post, Constants.GetEvacuationsListForEmployee, JsonConvert.SerializeObject(_req), true);
                if (response.Status)
                {
                    EvacuationsList = ApiHelper.ConvertResult<ObservableCollection<EvacuationEmployeeListModel>>(response.Result);
                   
                }
                else
                {
                    ShowToast(response.Message);
                }
            }
            catch (Exception ex)
            {
                WriteLog(ex.Message);
            }
            finally
            {
                HideLoader();
            }
        }
        #endregion
    }
}
