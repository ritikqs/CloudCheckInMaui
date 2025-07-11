using CloudCheckInMaui.ConstantHelper;
using CloudCheckInMaui.Models;
using CloudCheckInMaui.Services.ApiService;
using CloudCheckInMaui.Services.FaceService;
using CloudCheckInMaui.Services.ImageUploadService;
using CloudCheckInMaui.Services.DeviceService;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Storage;
using System.Net.Http;
using System.Text.RegularExpressions;
using CloudCheckInMaui.ConstantHelper;


namespace CloudCheckInMaui.ViewModels
{
    public class RegistrationViewModel : BaseViewModel
    {
        private readonly IFaceRecognitionService _faceRecognitionService;
        private readonly IDeviceInfoService _deviceInfoService;
        private readonly IImageSyncService _imageSyncService;

        // Step indicators
        private bool _isStepOne = true;
        public bool IsStepOne
        {
            get => _isStepOne;
            set => SetProperty(ref _isStepOne, value);
        }

        private bool _isStepTwo;
        public bool IsStepTwo
        {
            get => _isStepTwo;
            set => SetProperty(ref _isStepTwo, value);
        }

        private bool _isStepThree;
        public bool IsStepThree
        {
            get => _isStepThree;
            set => SetProperty(ref _isStepThree, value);
        }

        private bool _isStepFour;
        public bool IsStepFour
        {
            get => _isStepFour;
            set => SetProperty(ref _isStepFour, value);
        }

        // Step visibility properties for navigation
        private bool _firstViewVisible = true;
        public bool FirstViewVisible { get => _firstViewVisible; set => SetProperty(ref _firstViewVisible, value); }
        private bool _secondViewVisible;
        public bool SecondViewVisible { get => _secondViewVisible; set => SetProperty(ref _secondViewVisible, value); }
        private bool _thirdViewVisible;
        public bool ThirdViewVisible { get => _thirdViewVisible; set => SetProperty(ref _thirdViewVisible, value); }

        // Field properties and validation flags (matching Xamarin)
        private string _firstName;
        public string FirstName { get => _firstName; set { SetProperty(ref _firstName, value); ClearFormErrors(); } }
        private string _lastName;
        public string LastName { get => _lastName; set { SetProperty(ref _lastName, value); ClearFormErrors(); } }
        private string _dOB;
        public string DOB { get => _dOB; set { SetProperty(ref _dOB, value); ClearFormErrors(); } }
        private string _nINumber;
        public string NINumber { get => _nINumber; set { SetProperty(ref _nINumber, value); ClearFormErrors(); } }
        private string _motherMaidenName;
        public string MotherMaidenName { get => _motherMaidenName; set { SetProperty(ref _motherMaidenName, value); ClearFormErrors(); } }
        private string _email;
        public string Email { get => _email; set { SetProperty(ref _email, value); ClearFormErrors(); } }
        private string _phoneNumber;
        public string PhoneNumber { get => _phoneNumber; set { SetProperty(ref _phoneNumber, value); ClearFormErrors(); } }
        private string _password;
        public string Password { get => _password; set { SetProperty(ref _password, value); ClearFormErrors(); } }
        private string _confirmPassword;
        public string ConfirmPassword { get => _confirmPassword; set => SetProperty(ref _confirmPassword, value); }
        private string _otherTrade;
        public string OtherTrade { get => _otherTrade; set { SetProperty(ref _otherTrade, value); ClearFormErrors(); } }
        private string _otherContractor;
        public string OtherContractor { get => _otherContractor; set { SetProperty(ref _otherContractor, value); ClearFormErrors(); } }

        // Validation flags
        private bool _isFirstNameNotValid;
        public bool IsFirstNameNotValid { get => _isFirstNameNotValid; set => SetProperty(ref _isFirstNameNotValid, value); }
        private bool _isLastNameNotValid;
        public bool IsLastNameNotValid { get => _isLastNameNotValid; set => SetProperty(ref _isLastNameNotValid, value); }
        private bool _isNINumberNotValid;
        public bool IsNINumberNotValid { get => _isNINumberNotValid; set => SetProperty(ref _isNINumberNotValid, value); }
        private bool _isMotherNameNotValid;
        public bool IsMotherNameNotValid { get => _isMotherNameNotValid; set => SetProperty(ref _isMotherNameNotValid, value); }
        private bool _isDobNotValid;
        public bool IsDobNotValid { get => _isDobNotValid; set => SetProperty(ref _isDobNotValid, value); }
        private bool _isEmailNotValid;
        public bool IsEmailNotValid { get => _isEmailNotValid; set => SetProperty(ref _isEmailNotValid, value); }
        private bool _isPasswordNotValid;
        public bool IsPasswordNotValid { get => _isPasswordNotValid; set => SetProperty(ref _isPasswordNotValid, value); }
        private bool _isPhoneNumberNotValid;
        public bool IsPhoneNumberNotValid { get => _isPhoneNumberNotValid; set => SetProperty(ref _isPhoneNumberNotValid, value); }
        private bool _isOtherTradeNotValid;
        public bool IsOtherTradeNotValid { get => _isOtherTradeNotValid; set => SetProperty(ref _isOtherTradeNotValid, value); }
        private bool _isOtherContractorNotValid;
        public bool IsOtherContractorNotValid { get => _isOtherContractorNotValid; set => SetProperty(ref _isOtherContractorNotValid, value); }
        private bool _isTradeNotValid;
        public bool IsTradeNotValid
        {
            get => _isTradeNotValid;
            set => SetProperty(ref _isTradeNotValid, value);
        }
        private bool _isContractorNotValid;
        public bool IsContractorNotValid
        {
            get => _isContractorNotValid;
            set => SetProperty(ref _isContractorNotValid, value);
        }
        // ... keep other validation flags as needed ...

        // Dropdowns and popup logic (Contractor, Trade, EmployeeType)
        // ... ensure loading and "Other" logic matches Xamarin ...

        // Step navigation commands
        public ICommand NextStepCommand => new Command(() => {
            if (ValidateFirstStep()) {
                FirstViewVisible = false;
                SecondViewVisible = true;
            }
        });
        public ICommand BackStepCommand => new Command(() => {
            FirstViewVisible = true;
            SecondViewVisible = false;
        });
        public ICommand ProceedButtonCommand => new Command(() => {
            if (ValidateSecondStep()) {
                // Build registration request as in Xamarin
                var request = new RegistrationRequestModel {
                    Dob = !string.IsNullOrEmpty(DOB) ? Convert.ToDateTime(DOB) : DateTime.Now,
                    Email = Email,
                    Password = Password,
                    PhoneNumber = PhoneNumber,
                    ConfirmPassword = Password,
                    FirstName = FirstName,
                    LastName = LastName,
                    Role = "Employee",
                    //NiNumber = NINumber,
                    MotherMaidenName = MotherMaidenName,
                    TradeId = SelectedTrade?.Id != null ? long.Parse(SelectedTrade.Id) : 0,
                    EmployeeTypeId = SelectedEmployeeType?.Id != null ? long.Parse(SelectedEmployeeType.Id) : 0,
                    ContractorId = SelectedContractor?.Id != null ? long.Parse(SelectedContractor.Id) : 0,
                    OtherTrade = IsOtherTradeFieldVisible ? OtherTrade : null,
                    OtherContractor = IsOtherContractorFieldVisible ? OtherContractor : null
                };
                // ... navigate to camera or next step as in Xamarin ...
            }
        });

        // Validation methods
        public bool ValidateFirstStep() {
            // ... copy logic from Xamarin ValidateFirstStep ...
            if (!string.IsNullOrEmpty(FirstName) && !string.IsNullOrEmpty(LastName) && !string.IsNullOrEmpty(DOB) && !string.IsNullOrEmpty(NINumber) && !string.IsNullOrEmpty(MotherMaidenName)) {
                if (!IsFirstNameNotValid && !IsLastNameNotValid && !IsNINumberNotValid && !IsMotherNameNotValid) {
                    return true;
                } else {
                    return false;
                }
            } else {
                //first Name
                if (!String.IsNullOrEmpty(FirstName)) {
                    if (ValidationHelpers.NameRegex.IsMatch(FirstName)) {
                        IsFirstNameNotValid = false;
                    } else {
                        IsFirstNameNotValid = true;
                    }
                } else {
                    IsFirstNameNotValid = true;
                }
                //lastname
                if (!String.IsNullOrEmpty(LastName)) {
                    if (ValidationHelpers.NameRegex.IsMatch(LastName)) {
                        IsLastNameNotValid = false;
                    } else {
                        IsLastNameNotValid = true;
                    }
                } else {
                    IsLastNameNotValid = true;
                }
                //Dob
                if (String.IsNullOrEmpty(DOB)) {
                    IsDobNotValid = true;
                } else {
                    IsDobNotValid = false;
                }
                // NiNumber
                if (!String.IsNullOrEmpty(NINumber)) {
                    if (ValidationHelpers.AlphaNumber.IsMatch(NINumber)) {
                        if (IsNiNumberAvailable) {
                            IsNINumberNotValid = false;
                        } else {
                            IsNINumberNotValid = true;
                        }
                    } else {
                        IsNINumberNotValid = true;
                    }
                } else {
                    IsNINumberNotValid = true;
                }
                //MotherMaidenName
                if (!String.IsNullOrEmpty(MotherMaidenName)) {
                    if (ValidationHelpers.NameRegex.IsMatch(MotherMaidenName)) {
                        IsMotherNameNotValid = false;
                    } else {
                        IsMotherNameNotValid = true;
                    }
                } else {
                    IsMotherNameNotValid = true;
                }
                return false;
            }
        }
        public bool ValidateSecondStep() {
            // ... copy logic from Xamarin ValidateSecondStep ...
            if (!string.IsNullOrEmpty(Email) && !string.IsNullOrEmpty(Password) && !string.IsNullOrEmpty(PhoneNumber) && SelectedContractor!=null && SelectedEmployeeType!=null && SelectedTrade!=null && !IsEmailNotValid&& !IsPasswordNotValid && !IsPhoneNumberNotValid && CheckOtherFields()) {
                return true;
            } else {
                //Email
                if (!String.IsNullOrEmpty(Email)) {
                    if (ValidationHelpers.EmailRegex.IsMatch(Email)) {
                        if (IsEmailAvailable) {
                            IsEmailNotValid = false;
                        }
                    } else {
                        IsEmailNotValid = true;
                    }
                } else {
                    IsEmailNotValid = true;
                }
                //Password
                if (!String.IsNullOrEmpty(Password)) {
                    if (Password.Length > 3) {
                        IsPasswordNotValid = false;
                    } else {
                        IsPasswordNotValid = true;
                    }
                } else {
                    IsPasswordNotValid = true;
                }
                //PhoneNumber
                if (!String.IsNullOrEmpty(PhoneNumber)) {
                    if (PhoneNumber.Length == 11 && ValidationHelpers.Number.IsMatch(PhoneNumber)) {
                        IsPhoneNumberNotValid = false;
                    } else {
                        IsPhoneNumberNotValid = true;
                    }
                } else {
                    IsPhoneNumberNotValid = true;
                }
                //Contractor
                if (SelectedContractor!=null) {
                    if(SelectedContractor.Title == "Other") {
                        if (!String.IsNullOrEmpty(OtherContractor)) {
                            IsOtherContractorNotValid = false;
                        } else {
                            IsOtherContractorNotValid = true;
                        }
                    }
                } else {
                    IsContractorNotValid = true;
                }
                //Trade
                if (SelectedTrade != null) {
                    if (SelectedTrade.Title == "Other") {
                        if (!String.IsNullOrEmpty(OtherTrade)) {
                            IsOtherTradeNotValid = false;
                        } else {
                            IsOtherTradeNotValid = true;
                        }
                    }
                } else {
                    IsTradeNotValid = true;
                }
                return false;
            }
        }
        private bool CheckOtherFields() {
            if (IsOtherContractorFieldVisible && IsOtherTradeFieldVisible) {
                if (!String.IsNullOrEmpty(OtherContractor) && !String.IsNullOrEmpty(OtherTrade)) {
                    return true;
                } else {
                    return false;
                }
            } else if (IsOtherContractorFieldVisible && !IsOtherTradeFieldVisible) {
                if (!String.IsNullOrEmpty(OtherContractor)) {
                    return true;
                } else {
                    return false;
                }
            } else if(!IsOtherContractorFieldVisible && IsOtherTradeFieldVisible) {
                if (!String.IsNullOrEmpty(OtherTrade)) {
                    return true;
                } else {
                    return false;
                }
            } else {
                return true;
            }
        }
        private void ClearFormErrors() { /* Optionally implement as in Xamarin */ }

        // Collection data
        public ObservableCollection<PopupPicker> Trades { get; } = new ObservableCollection<PopupPicker>();
        public ObservableCollection<PopupPicker> Contractors { get; } = new ObservableCollection<PopupPicker>();
        public ObservableCollection<PopupPicker> EmployeeTypes { get; } = new ObservableCollection<PopupPicker>();

        // Commands
        public ICommand CapturePhotoCommand { get; }
        public ICommand RegisterCommand { get; }
        public ICommand CancelCommand { get; }
        public ICommand CheckEmailOrNiExistsCommand { get; }
        public ICommand OpenPopUpCommand { get; }
        public ICommand HidePickerPopupCommand { get; }
        public ICommand NavigateToCameraCommand { get; }

        private bool _isLoading;
        public bool IsLoading
        {
            get => _isLoading;
            set => SetProperty(ref _isLoading, value);
        }

        private string _capturedPhotoPath;
        public string CapturedPhotoPath
        {
            get => _capturedPhotoPath;
            set => SetProperty(ref _capturedPhotoPath, value);
        }

        private string _facePersistencyId;
        public string FacePersistencyId { get => _facePersistencyId; set => SetProperty(ref _facePersistencyId, value); }
        private byte[] _capturedPhotoBytes;

        private bool _isEmailAvailable = true;
        public bool IsEmailAvailable { get => _isEmailAvailable; set => SetProperty(ref _isEmailAvailable, value); }

        private string _emailValidationMessage;
        public string EmailValidationMessage { get => _emailValidationMessage; set => SetProperty(ref _emailValidationMessage, value); }

        private bool _isNiNumberAvailable = true;
        public bool IsNiNumberAvailable
        {
            get => _isNiNumberAvailable;
            set => SetProperty(ref _isNiNumberAvailable, value);
        }

        private string _niNumberValidationMessage = "Please enter a valid NI number";
        public string NINumberValidationMessage
        {
            get => _niNumberValidationMessage;
            set => SetProperty(ref _niNumberValidationMessage, value);
        }

        public DateTime MaximumDate => DateTime.Now.Date;
        public DateTime CurrentDate => DateTime.Now.Date;

        // Validation methods for each field
        public void ValidateFirstName()
        {
            IsFirstNameNotValid = string.IsNullOrWhiteSpace(FirstName) || !ValidationHelpers.NameRegex.IsMatch(FirstName);
        }

        public void ValidateLastName()
        {
            IsLastNameNotValid = string.IsNullOrWhiteSpace(LastName) || !ValidationHelpers.NameRegex.IsMatch(LastName);
        }

        public void ValidateMotherName()
        {
            IsMotherNameNotValid = string.IsNullOrWhiteSpace(MotherMaidenName) || !ValidationHelpers.NameRegex.IsMatch(MotherMaidenName);
        }

        public void ValidateEmail()
        {
            IsEmailNotValid = string.IsNullOrWhiteSpace(Email) || !ValidationHelpers.EmailRegex.IsMatch(Email);
            if (!IsEmailNotValid && !string.IsNullOrWhiteSpace(Email))
            {
                // Optionally, trigger email uniqueness check here if desired
            }
        }

        public void ValidatePassword()
        {
            IsPasswordNotValid = string.IsNullOrWhiteSpace(Password) || Password.Length < 4;
        }

        public void ValidatePhoneNumber()
        {
            IsPhoneNumberNotValid = string.IsNullOrWhiteSpace(PhoneNumber) || PhoneNumber.Length != 11 || !ValidationHelpers.Number.IsMatch(PhoneNumber);
        }

        public void ValidateNINumber()
        {
            // No-op: validation is now only via API
        }

        public void ValidateDateOfBirth()
        {
            if (DateTime.TryParse(DOB, out var dobValue))
            {
                var age = DateTime.Today.Year - dobValue.Year;
                if (dobValue > DateTime.Today.AddYears(-age))
                {
                    age--;
                }

                if (age < 16)
                {
                    IsDobNotValid = true;
                    DobValidationMessage = "You must be at least 16 years old to register";
                }
                else if (age > 100)
                {
                    IsDobNotValid = true;
                    DobValidationMessage = "Please enter a valid date of birth";
                }
                else
                {
                    IsDobNotValid = false;
                    DobValidationMessage = string.Empty;
                }
            }
            else
            {
                IsDobNotValid = true;
                DobValidationMessage = "Please enter a valid date of birth";
            }
        }

        // Email uniqueness check (to be called on unfocus)
        public async Task CheckEmailUniquenessAsync()
        {
            if (!IsEmailNotValid && !string.IsNullOrWhiteSpace(Email))
            {
                try
                {
                    var req = new EmailExistCheck
                    {
                        EmailOrNiNumber = Email,
                        Type = 1
                    };
                    var response = await ApiHelper.CallApi(HttpMethod.Post, ConstantHelper.Constants.CheckEmailOrNiNumberExist, JsonConvert.SerializeObject(req), false);
                    if (response.Status)
                    {
                        IsEmailAvailable = true;
                        EmailValidationMessage = string.Empty;
                    }
                    else
                    {
                        IsEmailAvailable = false;
                        IsEmailNotValid = true;
                        EmailValidationMessage = "Email Already Exists";
                    }
                }
                catch
                {
                    IsEmailAvailable = false;
                    IsEmailNotValid = true;
                    EmailValidationMessage = "Error checking email";
                }
            }
        }

        private bool _isConfirmPasswordNotValid;
        public bool IsConfirmPasswordNotValid
        {
            get => _isConfirmPasswordNotValid;
            set => SetProperty(ref _isConfirmPasswordNotValid, value);
        }

        public void ValidateConfirmPassword()
        {
            IsConfirmPasswordNotValid = string.IsNullOrWhiteSpace(ConfirmPassword) || Password != ConfirmPassword;
        }

        private bool _isPickerPopUpVisible = false;
        public bool IsPickerPopUpVisible
        {
            get => _isPickerPopUpVisible;
            set => SetProperty(ref _isPickerPopUpVisible, value);
        }

        private string _photoBytes;
        public string PhotoBytes { get => _photoBytes; set => SetProperty(ref _photoBytes, value); }

        private string _dobValidationMessage = "";
        public string DobValidationMessage
        {
            get => _dobValidationMessage;
            set => SetProperty(ref _dobValidationMessage, value);
        }

        private PopupPicker _selectedEmployeeType;
        public PopupPicker SelectedEmployeeType
        {
            get => _selectedEmployeeType;
            set => SetProperty(ref _selectedEmployeeType, value);
        }

        private PopupPicker _selectedTrade;
        public PopupPicker SelectedTrade
        {
            get => _selectedTrade;
            set => SetProperty(ref _selectedTrade, value);
        }

        private PopupPicker _selectedContractor;
        public PopupPicker SelectedContractor
        {
            get => _selectedContractor;
            set => SetProperty(ref _selectedContractor, value);
        }

        private bool _isOtherContractorFieldVisible;
        public bool IsOtherContractorFieldVisible
        {
            get => _isOtherContractorFieldVisible;
            set => SetProperty(ref _isOtherContractorFieldVisible, value);
        }

        private bool _isOtherTradeFieldVisible;
        public bool IsOtherTradeFieldVisible
        {
            get => _isOtherTradeFieldVisible;
            set => SetProperty(ref _isOtherTradeFieldVisible, value);
        }

        private ObservableCollection<PopupPicker> _employers = new ObservableCollection<PopupPicker>();
        public ObservableCollection<PopupPicker> Employers { get => _employers; set => SetProperty(ref _employers, value); }

        private PopupPicker _selectedEmployer;
        public PopupPicker SelectedEmployer { get => _selectedEmployer; set => SetProperty(ref _selectedEmployer, value); }

        public async Task GetEmployerAsync()
        {
            try
            {
                var response = await ApiHelper.CallApi(HttpMethod.Get, Constants.Employer, null, true);
                if (response.Status)
                {
                    var employers = response.GetData<List<PopupPicker>>();
                    if (employers != null)
                    {
                        Employers.Clear();
                        foreach (var employer in employers)
                        {
                            Employers.Add(employer);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Handle/log error
            }
        }

        private bool _isDobPickerVisible;
        public bool IsDobPickerVisible
        {
            get => _isDobPickerVisible;
            set { _isDobPickerVisible = value; OnPropertyChanged(nameof(IsDobPickerVisible)); }
        }

        public RegistrationViewModel(
            IFaceRecognitionService faceRecognitionService,
            IDeviceInfoService deviceInfoService,
            IImageSyncService imageSyncService)
        {
            _faceRecognitionService = faceRecognitionService;
            _deviceInfoService = deviceInfoService;
            _imageSyncService = imageSyncService;

            // Initialize view visibility
            FirstViewVisible = true;
            SecondViewVisible = false;
            ThirdViewVisible = false;
            IsPickerPopUpVisible = false;

            // Initialize commands
            CapturePhotoCommand = new Command(async () => await CapturePhoto());
            RegisterCommand = new Command(async () => await Register());
            CancelCommand = new Command(async () => await NavigateBack());
            CheckEmailOrNiExistsCommand = new Command<string>(async (type) => await CheckEmailOrNiExists(type));
            OpenPopUpCommand = new Command<string>(OpenPopUp);
            HidePickerPopupCommand = new Command(() => IsPickerPopUpVisible = false);
            NavigateToCameraCommand = new Command(async () => await NavigateToCamera());

            // Load initial data
            _ = LoadTradesAndContractors();
            _ = GetEmployerAsync();
        }

        private async Task CheckEmailOrNiExists(string type)
        {
            try
            {
                App.ShowLoader();
                
                EmailExistCheck emailExistCheck = new EmailExistCheck
                {
                    EmailOrNiNumber = type == "email" ? Email : NINumber,
                    Type = type == "email" ? 0 : 1
                };
                
                var response = await ApiHelper.CallApi(HttpMethod.Post, "account/checkemail", JsonConvert.SerializeObject(emailExistCheck));
                
                if (response.Status)
                {
                    var result = response.GetData<bool>();
                    if (result)
                    {
                        await DisplayAlert("Error", type == "email" ? "Email already exists" : "NI number already exists", "OK");
                    }
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

        private async Task LoadTradesAndContractors()
        {
            try
            {
                App.ShowLoader();
                
                // Load Trades
                var tradesResponse = await ApiHelper.CallApi(HttpMethod.Get, Constants.Trade, null, true);
                if (tradesResponse.Status)
                {
                    var trades = tradesResponse.GetData<List<PopupPicker>>();
                    if (trades != null)
                    {
                        Trades.Clear();
                        foreach (var trade in trades)
                        {
                            Trades.Add(trade);
                        }
                    }
                }
                
                // Load Contractors
                var contractorsResponse = await ApiHelper.CallApi(HttpMethod.Get, Constants.EmployeeType, null, true);
                if (contractorsResponse.Status)
                {
                    var contractors = contractorsResponse.GetData<List<PopupPicker>>();
                    if (contractors != null)
                    {
                        Contractors.Clear();
                        foreach (var contractor in contractors)
                        {
                            Contractors.Add(contractor);
                        }
                    }
                }
                
                // Load Employee Types
                var employeeTypesResponse = await ApiHelper.CallApi(HttpMethod.Get, "account/employeetypes");
                if (employeeTypesResponse.Status)
                {
                    var employeeTypes = employeeTypesResponse.GetData<List<PopupPicker>>();
                    if (employeeTypes != null)
                    {
                        EmployeeTypes.Clear();
                        foreach (var employeeType in employeeTypes)
                        {
                            EmployeeTypes.Add(employeeType);
                        }
                    }
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

        private async Task Register()
        {
            try
            {
                if (!ValidateRegistration())
                    return;

                App.ShowLoader();

                if (string.IsNullOrEmpty(this.PhotoBytes) || string.IsNullOrEmpty(this. FacePersistencyId))
                {
                    await DisplayAlert("Error", "Please capture your photo", "OK");
                    return;
                }

                var registrationRequest = new RegistrationRequestModel
                {
                    Email = Email,
                    PhoneNumber = PhoneNumber,
                    FirstName = FirstName,
                    LastName = LastName,
                    Role = "User",
                    ZipByte = null,
                    MotherMaidenName = MotherMaidenName,
                    TradeId = SelectedTrade?.Id != null ? long.Parse(SelectedTrade.Id) : 0,
                    EmployeeTypeId = SelectedEmployeeType?.Id != null ? long.Parse(SelectedEmployeeType.Id) : 0,
                    ContractorId = SelectedContractor?.Id != null ? long.Parse(SelectedContractor.Id) : 0,
                    //NiNumber = NINumber,
                    Dob = !string.IsNullOrEmpty(DOB) ? Convert.ToDateTime(DOB) : DateTime.Now,
                    Password = Password,
                    ConfirmPassword = ConfirmPassword,
                    FacePersistencyId = this.FacePersistencyId,
                    OtherTrade = OtherTrade,
                    OtherContractor = OtherContractor,
                    PhotoBytes = this.PhotoBytes
                };

                var response = await ApiHelper.CallApi(HttpMethod.Post, "account/register", JsonConvert.SerializeObject(registrationRequest));
                
                if (response.Status)
                {
                    var registrationResponse = response.GetData<RegistrationResponseModel>();
                    if (registrationResponse != null)
                    {
                        // Start image sync service
                        await _imageSyncService.StartSync(registrationResponse.EmpId.ToString(), FacePersistencyId);
                        await DisplayAlert("Success", "Registration successful", "OK");
                        await NavigateBack();
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

        private bool ValidateRegistration()
        {
            if (string.IsNullOrWhiteSpace(FirstName) ||
                string.IsNullOrWhiteSpace(LastName) ||
                string.IsNullOrWhiteSpace(Email) ||
                string.IsNullOrWhiteSpace(PhoneNumber) ||
                string.IsNullOrWhiteSpace(NINumber) ||
                string.IsNullOrWhiteSpace(MotherMaidenName) ||
                string.IsNullOrWhiteSpace(Password) ||
                string.IsNullOrWhiteSpace(ConfirmPassword))
            {
                DisplayAlert("Error", "Please fill in all required fields", "OK").Wait();
                return false;
            }

            if (Password != ConfirmPassword)
            {
                DisplayAlert("Error", "Passwords do not match", "OK").Wait();
                return false;
            }

            if (SelectedTrade == null || SelectedContractor == null || SelectedEmployeeType == null)
            {
                DisplayAlert("Error", "Please select Trade, Contractor and Employee Type", "OK").Wait();
                return false;
            }

            return true;
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

        private async Task NavigateBack()
        {
            try
            {
                await Shell.Current.GoToAsync("..");
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", ex.Message, "OK");
            }
        }

        private void OpenPopUp(string type)
        {
            // Set up popup data here if needed
            IsPickerPopUpVisible = true;
        }

        public async Task CheckNINumberUniquenessAsync()
        {
            if (!string.IsNullOrWhiteSpace(NINumber))
            {
                try
                {
                    var req = new EmailExistCheck
                    {
                        EmailOrNiNumber = NINumber,
                        Type = 2
                    };
                    var response = await ApiHelper.CallApi(HttpMethod.Post, ConstantHelper.Constants.CheckEmailOrNiNumberExist, JsonConvert.SerializeObject(req), false);
                    if (response.Status)
                    {
                        IsNiNumberAvailable = true;
                        IsNINumberNotValid = false;
                        NINumberValidationMessage = string.Empty;
                    }
                    else
                    {
                        IsNiNumberAvailable = false;
                        IsNINumberNotValid = true;
                        NINumberValidationMessage = "NI Number Already Exists";
                    }
                }
                catch
                {
                    IsNiNumberAvailable = false;
                    IsNINumberNotValid = true;
                    NINumberValidationMessage = "Error checking NI number";
                }
            }
            else
            {
                IsNINumberNotValid = true;
                NINumberValidationMessage = "Please enter NI number";
            }
        }

        private async Task NavigateToCamera()
        {
            // Just await, don't assign to var result
            await Shell.Current.GoToAsync($"CameraCapturePage?email={Email}");
            // The CameraCapturePage should set PhotoBytes and FacePersistencyId on return
        }
    }
} 