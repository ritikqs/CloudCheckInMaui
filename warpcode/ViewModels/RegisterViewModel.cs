// using Matcha.BackgroundService; // REMOVED: Replace with MAUI background services
using CCIMIGRATION.ConstantHelper;
using CCIMIGRATION.Models;
using CCIMIGRATION.Service.ApiService;
using CCIMIGRATION.Views;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace CCIMIGRATION.ViewModels
{
    public class RegisterViewModel : BaseViewModel
    {
        #region Private/Public Variables
        private bool _isFirstNameNotValid;
        private bool _isLastNameNotValid;
        private bool _isNINumberNotValid ;
        private bool _isEmailNotValid;
        private bool _isPasswordNotValid;
        private bool _isMotherNameNotValid;
        private bool _isOtherTradeNotValid;
        private bool _isOtherContractorNotValid;
  
        private string _email;
        private string _otherTrade;
        private string _otherContractor;
        private string _phoneNumber;
        private string _firstName;
        private string _lastName;
        private string _password;
        private string _confirmPassword;
        private string _nINumber;
        private string _dOB;
        private string _motherMaidenName;
        private bool _isSuccessVisible;
        private bool _isOtherTradeFieldVisible;
        private bool _isOtherContractorFieldVisible;
        private ObservableCollection<ContractorModel> contractorList;
        private List<EmployeeTypeModel> employeeTypeList;
        private ObservableCollection<TradeModel> tradeList;
        private ContractorModel selectedContractor;
        private EmployeeTypeModel selectedEmployeeType;
        private TradeModel selectedTrade;
        private string _pickerPopupInput;
        private string _pickerPopupPlaceholder;
        private ObservableCollection<PopupPicker> _popupListItemSource;
        private PopupPicker _selectedPopupItem;
        private bool _isPickerPopUpVisible;
        private bool _isEmployeeTypeListVisible;
        private bool _isContractorListVisible;
        private bool _isTradeListVisible;
        private string _selectedOption;
        private bool _isPhoneNumberNotValid;
        private List<ContractorModel> tempcontractorList;
        private List<EmployeeTypeModel> tempemployeeTypeList;
        private List<TradeModel> temptradeList;

        private bool _firstViewVisible;
        private bool _secondViewVisible;
        private bool _isDobnotValid;

        private bool _isSelectedContractorNotValid;
        private bool _isSelectedEmployeeTypeNotValid;
        private bool _isSelectedTradeNotValid;
        private bool _isNINumberAvailable;
        private bool _isEmailAvailable;
        private bool _isContractorNotValid;
        private bool _isTradeNotValid;
        private bool _isEmployeeTypeNotValid;

        public bool IsTradeNotValid
        {
            get { return _isTradeNotValid; }
            set { _isTradeNotValid = value; OnPropertyChanged(); }
        }
        public bool IsEmployeeTypeNotValid
        {
            get { return _isEmployeeTypeNotValid; }
            set { _isEmployeeTypeNotValid = value; OnPropertyChanged(); }
        }
        public bool IsContractorNotValid
        {
            get { return _isContractorNotValid; }
            set { _isContractorNotValid = value;OnPropertyChanged(); }
        }


        public bool IsEmailAvailable
        {
            get { return _isEmailAvailable; }
            set { _isEmailAvailable = value;OnPropertyChanged(); }
        }
        public bool IsNiNumberAvailable
        {
            get { return _isNINumberAvailable; }
            set { _isNINumberAvailable = value;OnPropertyChanged(); }
        }
        public bool IsSelectedEmployeeTypeNotValid
        {
            get { return _isSelectedEmployeeTypeNotValid; }
            set { _isSelectedEmployeeTypeNotValid = value; OnPropertyChanged(); }
        }
        public bool IsSelectedTradeNotValid
        {
            get { return _isSelectedTradeNotValid; }
            set { _isSelectedTradeNotValid = value; OnPropertyChanged(); }
        }
        public bool IsSelectedContractorNotValid
        {
            get { return _isSelectedContractorNotValid; }
            set { _isSelectedContractorNotValid = value;OnPropertyChanged(); }
        }


        public bool IsDobNotValid
        {
            get { return _isDobnotValid; }
            set { _isDobnotValid = value;OnPropertyChanged(); }
        }

        public bool SecondViewVisible
        {
            get { return _secondViewVisible; }
            set { _secondViewVisible = value; OnPropertyChanged(); }
        }

        public bool FirstViewVisible
        {
            get { return _firstViewVisible; }
            set { _firstViewVisible = value; OnPropertyChanged(); }
        }



        public bool IsPhoneNumberNotValid
        {
            get { return _isPhoneNumberNotValid; }
            set { _isPhoneNumberNotValid = value;OnPropertyChanged(); }
        }

        public string SelectedOption
        {
            get { return _selectedOption; }
            set { _selectedOption = value;OnPropertyChanged(); }
        }
        public bool IsContractorListVisible
        {
            get { return _isContractorListVisible; }
            set { _isContractorListVisible = value; OnPropertyChanged(); }
        }
        public bool IsEmployeeTypeListVisible
        {
            get { return _isEmployeeTypeListVisible; }
            set { _isEmployeeTypeListVisible = value; OnPropertyChanged(); }
        }
        public bool IsTradeListVisible
        {
            get { return _isTradeListVisible; }
            set { _isTradeListVisible = value; OnPropertyChanged(); }
        }


        public bool IsPickerPopUpVisible
        {
            get { return _isPickerPopUpVisible; }
            set { _isPickerPopUpVisible = value; OnPropertyChanged(); }
        }
        public PopupPicker SelectedPopupItem
        {
            get { return _selectedPopupItem; }
            set 
            {
                _selectedPopupItem = value; 
                OnPropertyChanged();
                if (SelectedPopupItem != null)
                {
                    switch (SelectedOption)
                    {
                        case "Contractor":
                            var data = ContractorList.Where(x => x.Id.ToString() == SelectedPopupItem.Id).ToList();
                            var item = ContractorList.IndexOf(data[0]);
                            SelectedContractor = ContractorList[item];
                            break;
                        case "Trade":
                            var data1 = TradeList.Where(x => x.Id.ToString() == SelectedPopupItem.Id).ToList();
                            var item1 = TradeList.IndexOf(data1[0]);
                            SelectedTrade = TradeList[item1];
                            break;
                        case "Employee":
                            var data2 = EmployeeTypeList.Where(x => x.Id.ToString() == SelectedPopupItem.Id).ToList();
                            var item2 = EmployeeTypeList.IndexOf(data2[0]);
                            SelectedEmployeeType = EmployeeTypeList[item2];
                            break;
                    }
                    IsPickerPopUpVisible = false;
                }
            }
            
        }

        public ObservableCollection<PopupPicker> PopupListItemSource
        {
            get { return _popupListItemSource; }
            set { _popupListItemSource = value;OnPropertyChanged("PopupListItemSource"); }
        }

        public string PickerPopupPlaceholder
        {
            get { return _pickerPopupPlaceholder; }
            set { _pickerPopupPlaceholder = value;OnPropertyChanged(); }
        }

        public string PickerPopupInput
        {
            get { return _pickerPopupInput; }
            set 
            { 
                _pickerPopupInput = value;
                OnPropertyChanged();
                SearchPopupData();
            }

        }

        public bool IsFirstNameNotValid
        {
            get { return _isFirstNameNotValid; }
            set { _isFirstNameNotValid = value; OnPropertyChanged();}
        }
        public bool IsLastNameNotValid
        {
            get { return _isLastNameNotValid; }
            set { _isLastNameNotValid = value; OnPropertyChanged(); }
        }
        public bool IsNINumberNotValid
        {
            get { return _isNINumberNotValid; }
            set { _isNINumberNotValid = value; OnPropertyChanged(); }
        }
        public bool IsMotherNameNotValid
        {
            get { return _isMotherNameNotValid; }
            set { _isMotherNameNotValid = value; OnPropertyChanged(); }
        }
        public bool IsEmailNotValid
        {
            get { return _isEmailNotValid; }
            set { _isEmailNotValid = value; OnPropertyChanged(); }
        }
        public bool IsPasswordNotValid
        {
            get { return _isPasswordNotValid; }
            set { _isPasswordNotValid = value; OnPropertyChanged(); }
        }
        public bool IsOtherTradeNotValid
        {
            get { return _isOtherTradeNotValid; }
            set { _isOtherTradeNotValid = value; OnPropertyChanged(); }
        }
        public bool IsOtherContractorNotValid
        {
            get { return _isOtherContractorNotValid; }
            set { _isOtherContractorNotValid = value; OnPropertyChanged(); }
        }
        public bool IsSuccessVisible
        {
            get { return _isSuccessVisible; }
            set { _isSuccessVisible = value;OnPropertyChanged("IsSuccessVisible");
            }
        }
        public string MotherMaidenName
        {
            get { return _motherMaidenName; }
            set { _motherMaidenName = value; OnPropertyChanged("MotherMaidenName"); }
        }

        public string Email
        {
            get { return _email; }
            set { _email = value; OnPropertyChanged("Email"); ClearFormErrors(); }
        }
        public string PhoneNumber
        {
            get { return _phoneNumber; }
            set { _phoneNumber = value; OnPropertyChanged("PhoneNumber"); ClearFormErrors(); }
        }
        public string FirstName
        {
            get { return _firstName; }
            set { _firstName = value; OnPropertyChanged("FirstName"); ClearFormErrors(); }
        }
        public string LastName
        {
            get { return _lastName; }
            set { _lastName = value; OnPropertyChanged("LastName"); ClearFormErrors(); }
        }
        public string Password
        {
            get { return _password; }
            set { _password = value; OnPropertyChanged("Password"); ClearFormErrors(); }
        }
        public string NINumber
        {
            get { return _nINumber; }
            set { _nINumber = value; OnPropertyChanged("NINumber"); ClearFormErrors(); }
        }
        public string DOB
        {
            get { return _dOB; }
            set { _dOB = value; OnPropertyChanged("DOB"); ClearFormErrors(); }
        }
        public string ConfirmPassword
        {
            get { return _confirmPassword; }
            set { _confirmPassword = value; OnPropertyChanged("ConfirmPassword"); }
        }
        public ObservableCollection<ContractorModel> ContractorList
        {
            get { return contractorList; }
            set { contractorList = value; OnPropertyChanged("ContractorList"); }
        }
        public List<ContractorModel> TempContractorList
        {
            get { return tempcontractorList; }
            set { tempcontractorList = value; OnPropertyChanged("TempContractorList"); }
        }
        public ContractorModel SelectedContractor
        {
            get { return selectedContractor; }
            set
            {
                selectedContractor = value;
                OnPropertyChanged("SelectedContractor");
                if (SelectedContractor != null)
                {
                    IsSelectedContractorNotValid = false;
                    IsContractorNotValid = false;
                    IsOtherContractorFieldVisible = false;
                    if (SelectedContractor.Title == "Other")
                    {
                        IsOtherContractorFieldVisible = true;
                    }
                    IsPickerPopUpVisible = false;
                }
                
            }
        }
        public List<EmployeeTypeModel> TempEmployeeTypeList
        {
            get { return tempemployeeTypeList; }
            set { tempemployeeTypeList = value; OnPropertyChanged("TempEmployeeTypeList"); }
        }

        public List<TradeModel> TempTradeList
        {
            get { return temptradeList; }
            set { temptradeList = value; OnPropertyChanged("TempTradeList"); }
        }
        public List<EmployeeTypeModel> EmployeeTypeList
        {
            get { return employeeTypeList; }
            set { employeeTypeList = value; OnPropertyChanged("EmployeeTypeList"); }
        }
        public EmployeeTypeModel SelectedEmployeeType
        {
            get { return selectedEmployeeType; }
            set 
            { 
                selectedEmployeeType = value;
                OnPropertyChanged("SelectedEmployeeType");
                if (SelectedEmployeeType != null)
                {
                    IsSelectedEmployeeTypeNotValid = false;
                    IsEmployeeTypeNotValid = false;
                    ClearFormErrors();
                    IsPickerPopUpVisible = false;
                }
               
            }
        }
        public ObservableCollection<TradeModel> TradeList
        {
            get { return tradeList; }
            set { tradeList = value; OnPropertyChanged("TradeList"); }
        }
        public string OtherTrade
        {
            get { return _otherTrade; }
            set { _otherTrade = value; OnPropertyChanged("OtherTrade"); ClearFormErrors(); }
        }
        public string OtherContractor
        {
            get { return _otherContractor; }
            set { _otherContractor = value; OnPropertyChanged("OtherContractor"); ClearFormErrors(); }
        }
        public TradeModel SelectedTrade
        {
            get { return selectedTrade; }
            set
            {
                selectedTrade = value;
                OnPropertyChanged("SelectedTrade");
                if (SelectedTrade != null)
                {
                    IsTradeNotValid = false;
                    IsOtherTradeFieldVisible = false;
                    if (SelectedTrade.Title == "Other")
                    {
                        IsOtherTradeFieldVisible = true;
                    }
                    IsPickerPopUpVisible = false;
                }
                
            }
        }
        public bool IsOtherTradeFieldVisible
        {
            get { return _isOtherTradeFieldVisible; }
            set { _isOtherTradeFieldVisible = value;OnPropertyChanged("IsOtherTradeFieldVisible"); }
        }
        public bool IsOtherContractorFieldVisible
        {
            get { return _isOtherContractorFieldVisible; }
            set { _isOtherContractorFieldVisible = value; OnPropertyChanged("IsOtherContractorFieldVisible"); }
        }

        #endregion

        #region Commands
        public ICommand OpenPopUp
        {
            get
            {
                return new Command((obj) =>
                {
                    IsContractorListVisible = false;
                    IsEmployeeTypeListVisible = false;
                    IsTradeListVisible = false;
                    SelectedOption = obj.ToString();
                    LoadPopupData(SelectedOption);
                });
            }
        }

        public ICommand HidePickerPopup
        {
            get
            {
                return new Command(() =>
                {
                    IsPickerPopUpVisible = false;
                });
            }
        }
        public ICommand BackStepCommand
        {
            get
            {
                return new Command(() =>
                {
                    FirstViewVisible = true;
                    SecondViewVisible = false;
                });
            }
        }
        public ICommand NextStepCommand
        {
            get
            {
                return new Command(() =>
                {
                    try
                    {
                        if (ValidateFirstStep())
                        {
                            FirstViewVisible = false;
                            SecondViewVisible = true;
                        }
                    }
                    catch(Exception ex)
                    {
                        WriteLog(ex.Message);
                    }
                    finally
                    {

                    }
                });
            }
        }


        public ICommand ProceedButtonCommand
        {
            get
            {
                return new Command(() =>
                {
                    try
                    {
                        if (ValidateSecondStep())
                        {
                            var _request = new RegistrationRequestModel()
                            {
                                Dob = Convert.ToDateTime(DOB),
                                Email = Email,
                                Password = Password,
                                PhoneNumber = PhoneNumber,
                                ConfirmPassword = Password,
                                FirstName = FirstName,
                                LastName = LastName,
                                Role = "Employee",
                                NiNumber = NINumber,
                                MotherMaidenName = MotherMaidenName,
                                TradeId = SelectedTrade.Id,
                                EmployeeTypeId = SelectedEmployeeType.Id,
                                ContractorId = SelectedContractor.Id,
                            };
                            if (IsOtherContractorFieldVisible && !String.IsNullOrEmpty(OtherTrade))
                            {
                                _request.OtherTrade = OtherTrade;
                            }
                            if (IsOtherContractorFieldVisible && !String.IsNullOrEmpty(OtherContractor))
                            {
                                _request.OtherContractor = OtherContractor;
                            }
                            App.CameraFrom = "Register";
                            App.Current.MainPage.Navigation.PushModalAsync(new CameraCapturePage(_request));
                            
                        }
                    }
                    catch(Exception ex)
                    {
                        WriteLog(ex.Message);
                    }
                    finally
                    {

                    }
                    
                });
            }
        }
      
        #endregion

        #region Constructor
        public RegisterViewModel()
        {
            EmployeeTypeList = new List<EmployeeTypeModel>();
            ContractorList = new ObservableCollection<ContractorModel>();
            TradeList = new ObservableCollection<TradeModel>();
            TempEmployeeTypeList = new List<EmployeeTypeModel>();
            TempContractorList = new List<ContractorModel>();
            TempTradeList = new List<TradeModel>();
            PopupListItemSource = new ObservableCollection<PopupPicker>();
            FirstViewVisible = true;
        }
        #endregion

        #region Methods
        private void SearchPopupData()
        {
            try
            {
                switch (SelectedOption)
                {
                    case "Contractor":
                        if (!string.IsNullOrEmpty(PickerPopupInput))
                        {
                            TempContractorList = ContractorList.Where(x => x.Title.ToLower().StartsWith(PickerPopupInput.ToLower())).ToList();
                        }
                        else
                        {
                            TempContractorList = ContractorList.ToList();
                            PickerPopupPlaceholder = ResourceConstants.SearchContractor;
                        }

                        break;
                    case "Trade":
                        if (!string.IsNullOrEmpty(PickerPopupInput))
                        {
                            TempTradeList = TradeList.Where(x => x.Title.ToLower().StartsWith(PickerPopupInput.ToLower())).ToList();
                        }
                        else
                        {
                            TempTradeList = TradeList.ToList();
                            PickerPopupPlaceholder = ResourceConstants.SearchTrade;
                        }
                        break;
                    case "Employee":
                        if (!string.IsNullOrEmpty(PickerPopupInput))
                        {
                            TempEmployeeTypeList = EmployeeTypeList.Where(x => x.EmployeeType.ToLower().StartsWith(PickerPopupInput.ToLower())).ToList();
                        }
                        else
                        {
                            TempEmployeeTypeList = EmployeeTypeList.ToList();
                            PickerPopupPlaceholder = ResourceConstants.SearchEmployer;
                        }
                        break;
                }
            }
            catch (Exception ex)
            {
                WriteLog(ex.Message);
            }

        }
        private void LoadPopupData(string selectedOption)
        {
            PickerPopupInput = String.Empty;
            switch (selectedOption)
            {
                case "Contractor":
                    PickerPopupPlaceholder = ResourceConstants.SearchContractor;
                    TempContractorList = ContractorList.ToList();
                    IsContractorListVisible = true;
                    break;
                case "Trade":
                    PickerPopupPlaceholder = ResourceConstants.SearchTrade;
                    TempTradeList = TradeList.ToList();
                    IsTradeListVisible = true;
                    break;
                case "Employee":
                    PickerPopupPlaceholder = ResourceConstants.SearchEmployer;
                    TempEmployeeTypeList = EmployeeTypeList;
                    IsEmployeeTypeListVisible = true;
                    break;
            }
            IsPickerPopUpVisible = true;
        }
        private void ClearFormErrors()
        {
           
        }
        public void GetData()
        {
            try
            {
                GetEmployer();
                GetContractor();
                GetTrade();
            }
            catch (Exception ex)
            {
                WriteLog(ex.Message);
            }
        }
        private async void GetContractor()
        {
            try
            {
                ContractorList.Add(new ContractorModel()
                {
                    Title = "Other"
                });
                var response = await ApiHelper.CallApi(HttpMethod.Get, Constants.Contractor, null, true);
                if (response.Status)
                {
                    ContractorList.Clear();
                    ContractorList = ApiHelper.ConvertResult<ObservableCollection<ContractorModel>>(response.Result);
                    ContractorList.Add(new ContractorModel()
                    {
                        Title = "Other"
                    });
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
        }
        private async void GetEmployer()
        {
            try
            {
                var response = await ApiHelper.CallApi(HttpMethod.Get, Constants.EmployeeType, null, true);
                if (response.Status)
                {
                    EmployeeTypeList = ApiHelper.ConvertResult<List<EmployeeTypeModel>>(response.Result);
                   
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
        }
        private async void GetTrade()
        {
            try
            {
                TradeList.Add(new TradeModel()
                {
                    Title = "Other",
                });
                var response = await ApiHelper.CallApi(HttpMethod.Get, Constants.Trade, null, true);
                if (response.Status)
                {
                    TradeList.Clear();
                    TradeList = ApiHelper.ConvertResult<ObservableCollection<TradeModel>>(response.Result);
                    TradeList.Add(new TradeModel()
                    {
                        Title = "Other",
                    });
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
        }
        public bool ValidateFirstStep()
        {
            if (!string.IsNullOrEmpty(FirstName) && !string.IsNullOrEmpty(LastName) && !string.IsNullOrEmpty(DOB) && !string.IsNullOrEmpty(NINumber) && !string.IsNullOrEmpty(MotherMaidenName))
            {
                if (!IsFirstNameNotValid && !IsLastNameNotValid && !IsNINumberNotValid && !IsMotherNameNotValid)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                //first Name
                if (!String.IsNullOrEmpty(FirstName))
                {
                    if (ValidationHelpers.NameRegex.IsMatch(FirstName))
                    {
                        IsFirstNameNotValid = false;
                    }
                    else
                    {
                        IsFirstNameNotValid = true;
                    }
                }
                else
                {
                    IsFirstNameNotValid = true;
                }
                //lastname
                if (!String.IsNullOrEmpty(LastName))
                {
                    if (ValidationHelpers.NameRegex.IsMatch(LastName))
                    {
                        IsLastNameNotValid = false;
                    }
                    else
                    {
                        IsLastNameNotValid = true;
                    }
                }
                else
                {
                    IsLastNameNotValid = true;
                }
                //Dob
                if (String.IsNullOrEmpty(DOB))
                {
                    IsDobNotValid = true;
                }
                else
                {
                    IsDobNotValid = false;
                }
                // NiNumber
                if (!String.IsNullOrEmpty(NINumber))
                {
                    if (ValidationHelpers.AlphaNumber.IsMatch(NINumber))
                    {
                        if (IsNiNumberAvailable)
                        {
                            IsNINumberNotValid = false;
                        }
                        else
                        {
                            IsNINumberNotValid = true;
                        }
                    }
                    else
                    {
                        IsNINumberNotValid = true;
                    }
                }
                else
                {
                    IsNINumberNotValid = true;
                }
                //MotherMaidenName
                if (!String.IsNullOrEmpty(MotherMaidenName))
                {
                    if (ValidationHelpers.NameRegex.IsMatch(MotherMaidenName))
                    {
                        IsMotherNameNotValid = false;
                    }
                    else
                    {
                        IsMotherNameNotValid = true;
                    }

                }
                else
                {
                    IsMotherNameNotValid = true;
                }
                return false;
            }

        }
        private bool CheckOtherFields()
        {
            if (IsOtherContractorFieldVisible && IsOtherTradeFieldVisible)
            {
                if (!String.IsNullOrEmpty(OtherContractor) && !String.IsNullOrEmpty(OtherTrade))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else if (IsOtherContractorFieldVisible && !IsOtherTradeFieldVisible)
            {
                if (!String.IsNullOrEmpty(OtherContractor))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else if(!IsOtherContractorFieldVisible && IsOtherTradeFieldVisible)
            {
                if (!String.IsNullOrEmpty(OtherTrade))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return true;
            }
        }
        public bool ValidateSecondStep()
        {
            if (!string.IsNullOrEmpty(Email) && !string.IsNullOrEmpty(Password) && !string.IsNullOrEmpty(PhoneNumber) && SelectedContractor!=null && SelectedEmployeeType!=null && SelectedTrade!=null && !IsEmailNotValid&& !IsPasswordNotValid && !IsPhoneNumberNotValid && CheckOtherFields())
            {
                return true;
            }
            else
            {
                //Email
                if (!String.IsNullOrEmpty(Email))
                {
                    if (ValidationHelpers.EmailRegex.IsMatch(Email))
                    {
                        if (IsEmailAvailable)
                        {
                            IsEmailNotValid = false;
                        }
                    }
                    else
                    {
                        IsEmailNotValid = true;
                    }
                }
                else
                {
                    IsEmailNotValid = true;
                }
                //Password
                if (!String.IsNullOrEmpty(Password))
                {
                    if (Password.Length > 3)
                    {
                        IsPasswordNotValid = false;
                    }
                    else
                    {
                        IsPasswordNotValid = true;
                    }
                }
                else
                {
                    IsPasswordNotValid = true;
                }
                //PhoneNumber
                if (!String.IsNullOrEmpty(PhoneNumber))
                {
                    if (PhoneNumber.Length == 11 && ValidationHelpers.Number.IsMatch(PhoneNumber))
                    {
                        IsPhoneNumberNotValid = false;
                    }
                    else
                    {
                        IsPhoneNumberNotValid = true;
                    }
                }
                else
                {
                    IsPhoneNumberNotValid = true;
                }
                //Contractor
                if (SelectedContractor!=null)
                {
                    if(SelectedContractor.Title == "Other")
                    {
                        if (!String.IsNullOrEmpty(OtherContractor))
                        {
                            IsOtherContractorNotValid = false;
                        }
                        else
                        {
                            IsOtherContractorNotValid = true;
                        }
                    }
                }
                else
                {
                    IsContractorNotValid = true;
                }
                //Trade
                if (SelectedTrade != null)
                {
                    if (SelectedTrade.Title == "Other")
                    {
                        if (!String.IsNullOrEmpty(OtherTrade))
                        {
                            IsOtherTradeNotValid = false;
                        }
                        else
                        {
                            IsOtherTradeNotValid = true;
                        }
                    }
                }
                else
                {
                    IsTradeNotValid = true;
                }
                //Employeetype
                if (SelectedEmployeeType != null)
                {
                    IsEmployeeTypeNotValid = false;
                }
                else
                {
                    IsEmployeeTypeNotValid = true;
                }
                return false;

            }

        }
        #endregion
    }
}

