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
using System.Threading.Tasks;
using System.Windows.Input;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Storage;
using System.Net.Http;

namespace CloudCheckInMaui.ViewModels
{
    public class RegistrationViewModel : BaseViewModel
    {
        private readonly IFaceRecognitionService _faceRecognitionService;
        private readonly IDeviceInfoService _deviceInfoService;
        private readonly IImageSyncService _imageSyncService;

        // Step visibility properties
        private bool _firstViewVisible = true;
        public bool FirstViewVisible { get => _firstViewVisible; set => SetProperty(ref _firstViewVisible, value); }
        private bool _secondViewVisible;
        public bool SecondViewVisible { get => _secondViewVisible; set => SetProperty(ref _secondViewVisible, value); }
        private bool _thirdViewVisible;
        public bool ThirdViewVisible { get => _thirdViewVisible; set => SetProperty(ref _thirdViewVisible, value); }

        // Field properties
        private string _firstName;
        public string FirstName { get => _firstName; set { SetProperty(ref _firstName, value); ValidateFirstName(); } }
        private string _lastName;
        public string LastName { get => _lastName; set { SetProperty(ref _lastName, value); ValidateLastName(); } }
        private DateTime? _dob;
        public DateTime? DOB { get => _dob; set { SetProperty(ref _dob, value); ValidateDateOfBirth(); } }
        private string _nINumber;
        public string NINumber { get => _nINumber; set { SetProperty(ref _nINumber, value); ValidateNINumber(); } }
        private string _motherMaidenName;
        public string MotherMaidenName { get => _motherMaidenName; set { SetProperty(ref _motherMaidenName, value); ValidateMotherName(); } }
        private string _email;
        public string Email { get => _email; set { SetProperty(ref _email, value); ValidateEmail(); } }
        private string _phoneNumber;
        public string PhoneNumber { get => _phoneNumber; set { SetProperty(ref _phoneNumber, value); ValidatePhoneNumber(); } }
        private string _password;
        public string Password { get => _password; set { SetProperty(ref _password, value); ValidatePassword(); } }
        private string _confirmPassword;
        public string ConfirmPassword { get => _confirmPassword; set { SetProperty(ref _confirmPassword, value); ValidateConfirmPassword(); } }
        private string _otherTrade;
        public string OtherTrade { get => _otherTrade; set => SetProperty(ref _otherTrade, value); }
        private string _otherContractor;
        public string OtherContractor { get => _otherContractor; set => SetProperty(ref _otherContractor, value); } 

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
        private bool _isConfirmPasswordNotValid;
        public bool IsConfirmPasswordNotValid { get => _isConfirmPasswordNotValid; set => SetProperty(ref _isConfirmPasswordNotValid, value); }
        private bool _isEmployerNotValid;
        public bool IsEmployerNotValid { get => _isEmployerNotValid; set => SetProperty(ref _isEmployerNotValid, value); }

        private bool _isDobPickerVisible;
        public bool IsDobPickerVisible
        {
            get => _isDobPickerVisible;
            set => SetProperty(ref _isDobPickerVisible, value);
        }

        private string _dobValidationMessage = "";
        public string DobValidationMessage
        {
            get => _dobValidationMessage;
            set => SetProperty(ref _dobValidationMessage, value);
        }

        private string _confirmPasswordValidationMessage = "";
        public string ConfirmPasswordValidationMessage
        {
            get => _confirmPasswordValidationMessage;
            set => SetProperty(ref _confirmPasswordValidationMessage, value);
        }

        private ObservableCollection<PopupPicker> _employers = new ObservableCollection<PopupPicker>();
        public ObservableCollection<PopupPicker> Employers { get => _employers; set => SetProperty(ref _employers, value); }

        private PopupPicker _selectedEmployer;
        public PopupPicker SelectedEmployer { get => _selectedEmployer; set => SetProperty(ref _selectedEmployer, value); }

        // Other properties (Trades, Contractors, etc.)
        public ObservableCollection<PopupPicker> Trades { get; } = new ObservableCollection<PopupPicker>();
        public ObservableCollection<PopupPicker> Contractors { get; } = new ObservableCollection<PopupPicker>();
        public ObservableCollection<PopupPicker> EmployeeTypes { get; } = new ObservableCollection<PopupPicker>();
        private PopupPicker _selectedTrade;
        public PopupPicker SelectedTrade { get => _selectedTrade; set => SetProperty(ref _selectedTrade, value); }
        private PopupPicker _selectedContractor;
        public PopupPicker SelectedContractor { get => _selectedContractor; set => SetProperty(ref _selectedContractor, value); }
        private PopupPicker _selectedEmployeeType;
        public PopupPicker SelectedEmployeeType { get => _selectedEmployeeType; set => SetProperty(ref _selectedEmployeeType, value); }
        private bool _isOtherContractorFieldVisible;
        public bool IsOtherContractorFieldVisible { get => _isOtherContractorFieldVisible; set => SetProperty(ref _isOtherContractorFieldVisible, value); }
        private bool _isOtherTradeFieldVisible;
        public bool IsOtherTradeFieldVisible { get => _isOtherTradeFieldVisible; set => SetProperty(ref _isOtherTradeFieldVisible, value); }
        private bool _isPickerPopUpVisible;
        public bool IsPickerPopUpVisible { get => _isPickerPopUpVisible; set => SetProperty(ref _isPickerPopUpVisible, value); }
        private string _pickerPopupPlaceholder;
        public string PickerPopupPlaceholder { get => _pickerPopupPlaceholder; set => SetProperty(ref _pickerPopupPlaceholder, value); }
        private string _pickerPopupInput;
        public string PickerPopupInput { get => _pickerPopupInput; set => SetProperty(ref _pickerPopupInput, value); }
        private bool _isTradeListVisible;
        public bool IsTradeListVisible { get => _isTradeListVisible; set => SetProperty(ref _isTradeListVisible, value); }
        private bool _isContractorListVisible;
        public bool IsContractorListVisible { get => _isContractorListVisible; set => SetProperty(ref _isContractorListVisible, value); }
        private bool _isEmployerListVisible;
        public bool IsEmployerListVisible { get => _isEmployerListVisible; set => SetProperty(ref _isEmployerListVisible, value); }

        // Commands
        public ICommand ShowDatePickerCommand => new Command(() =>
        {
            IsDobPickerVisible = true;
        });

        public ICommand BackCommand => new Command(async () => await NavigateBack());
        public ICommand NextStepCommand => new Command(() =>
        {
            if (ValidateFirstStep())
            {
                FirstViewVisible = false;
                SecondViewVisible = true;
            }
        });
        public ICommand BackStepCommand => new Command(() =>
        {
            if (SecondViewVisible)
            {
                SecondViewVisible = false;
                FirstViewVisible = true;
            }
            else if (ThirdViewVisible)
            {
                ThirdViewVisible = false;
                SecondViewVisible = true;
            }
        });
        public ICommand ProceedButtonCommand => new Command(() =>
        {
            if (ValidateSecondStep())
            {
                SecondViewVisible = false;
                ThirdViewVisible = true;
            }
        });
        public ICommand RegisterCommand { get; }
        public ICommand NavigateToCameraCommand { get; }
        public ICommand OpenPopUpCommand { get; }
        public ICommand HidePickerPopupCommand { get; }

        private bool _isEmailAvailable = true;
        public bool IsEmailAvailable { get => _isEmailAvailable; set => SetProperty(ref _isEmailAvailable, value); }
        private string _emailValidationMessage;
        public string EmailValidationMessage { get => _emailValidationMessage; set => SetProperty(ref _emailValidationMessage, value); }
        private bool _isNiNumberAvailable = true;
        public bool IsNiNumberAvailable { get => _isNiNumberAvailable; set => SetProperty(ref _isNiNumberAvailable, value); }
        private string _niNumberValidationMessage = "Please enter a valid NI number";
        public string NINumberValidationMessage { get => _niNumberValidationMessage; set => SetProperty(ref _niNumberValidationMessage, value); }

        public DateTime CurrentDate => DateTime.Now.Date;

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
            RegisterCommand = new Command(async () => await Register());
            NavigateToCameraCommand = new Command(async () => await NavigateToCamera());
            OpenPopUpCommand = new Command<string>(OpenPopUp);
            HidePickerPopupCommand = new Command(() => IsPickerPopUpVisible = false);

            // Load initial data
            _ = LoadTradesAndContractors();
            _ = GetEmployerAsync();
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
                await DisplayAlert("Error", "Failed to load data. Please try again.", "OK");
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
                if (!await ValidateRegistration())
                    return;

                App.ShowLoader();

                if (string.IsNullOrEmpty(PhotoBytes) || string.IsNullOrEmpty(FacePersistencyId))
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
                    NiNumber = NINumber,
                    MotherMaidenName = MotherMaidenName,
                    TradeId = SelectedTrade?.Id != null ? long.Parse(SelectedTrade.Id) : 0,
                    EmployeeTypeId = SelectedEmployeeType?.Id != null ? long.Parse(SelectedEmployeeType.Id) : 0,
                    ContractorId = SelectedContractor?.Id != null ? long.Parse(SelectedContractor.Id) : 0,
                    Dob = DOB ?? DateTime.Now,
                    Password = Password,
                    ConfirmPassword = ConfirmPassword,
                    FacePersistencyId = FacePersistencyId,
                    OtherTrade = OtherTrade,
                    OtherContractor = OtherContractor,
                    PhotoBytes = PhotoBytes
                };

                var response = await ApiHelper.CallApi(HttpMethod.Post, "account/register", JsonConvert.SerializeObject(registrationRequest));

                if (response.Status)
                {
                    var registrationResponse = response.GetData<RegistrationResponseModel>();
                    if (registrationResponse != null)
                    {
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
                await DisplayAlert("Error", "An error occurred during registration. Please try again.", "OK");
            }
            finally
            {
                App.HideLoader();
            }
        }

        private async Task<bool> ValidateRegistration()
        {
            ValidateFirstStep();
            ValidateSecondStep();
            ValidateConfirmPassword();

            if (IsFirstNameNotValid || IsLastNameNotValid || IsDobNotValid || IsNINumberNotValid || IsMotherNameNotValid ||
                IsEmailNotValid || IsPasswordNotValid || IsPhoneNumberNotValid || IsConfirmPasswordNotValid || IsEmployerNotValid)
            {
                await DisplayAlert("Error", "Please correct all errors before registering", "OK");
                return false;
            }

            if (SelectedTrade == null || SelectedContractor == null || SelectedEmployeeType == null)
            {
                await DisplayAlert("Error", "Please select Trade, Contractor, and Employee Type", "OK");
                return false;
            }

            return true;
        }

        public bool ValidateFirstStep()
        {
            ValidateFirstName();
            ValidateLastName();
            ValidateDateOfBirth();
            ValidateNINumber();
            ValidateMotherName();

            return !IsFirstNameNotValid && !IsLastNameNotValid && !IsDobNotValid && !IsNINumberNotValid && !IsMotherNameNotValid;
        }

        public bool ValidateSecondStep()
        {
            ValidateEmail();
            ValidatePassword();
            ValidatePhoneNumber();
            IsEmployerNotValid = SelectedEmployer == null;

            if (IsOtherContractorFieldVisible && string.IsNullOrWhiteSpace(OtherContractor))
            {
                IsOtherContractorNotValid = true;
            }
            if (IsOtherTradeFieldVisible && string.IsNullOrWhiteSpace(OtherTrade))
            {
                IsOtherTradeNotValid = true;
            }

            return !IsEmailNotValid && !IsPasswordNotValid && !IsPhoneNumberNotValid && !IsEmployerNotValid &&
                   !IsOtherContractorNotValid && !IsOtherTradeNotValid;
        }

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
            IsNINumberNotValid = string.IsNullOrWhiteSpace(NINumber) || !ValidationHelpers.AlphaNumber.IsMatch(NINumber);
        }

        public void ValidateConfirmPassword()
        {
            if (string.IsNullOrWhiteSpace(ConfirmPassword) || ConfirmPassword != Password)
            {
                IsConfirmPasswordNotValid = true;
                ConfirmPasswordValidationMessage = "PINs do not match or are invalid";
            }
            else
            {
                IsConfirmPasswordNotValid = false;
                ConfirmPasswordValidationMessage = string.Empty;
            }
        }

        public void ValidateDateOfBirth()
        {
            if (!DOB.HasValue)
            {
                IsDobNotValid = true;
                DobValidationMessage = "Please select a date of birth";
                return;
            }

            var age = DateTime.Today.Year - DOB.Value.Year;
            if (DOB.Value > DateTime.Today.AddYears(-age))
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

        public async Task CheckEmailUniquenessAsync()
        {
            if (!IsEmailNotValid && !string.IsNullOrWhiteSpace(Email))
            {
                try
                {
                    App.ShowLoader();
                    var req = new EmailExistCheck { EmailOrNiNumber = Email, Type = 1 };
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
                        EmailValidationMessage = "Email already exists";
                    }
                }
                catch
                {
                    IsEmailAvailable = false;
                    IsEmailNotValid = true;
                    EmailValidationMessage = "Unable to verify email. Please check your connection.";
                }
                finally
                {
                    App.HideLoader();
                }
            }
        }

        public async Task CheckNINumberUniquenessAsync()
        {
            if (!string.IsNullOrWhiteSpace(NINumber))
            {
                try
                {
                    App.ShowLoader();
                    var req = new EmailExistCheck { EmailOrNiNumber = NINumber, Type = 2 };
                    var response = await ApiHelper.CallApi(HttpMethod.Post, ConstantHelper.Constants.CheckEmailOrNiNumberExist, JsonConvert.SerializeObject(req), false);
                    if (response.Status)
                    {
                        IsNiNumberAvailable = true;
                        NINumberValidationMessage = string.Empty;
                    }
                    else
                    {
                        IsNiNumberAvailable = false;
                        IsNINumberNotValid = true;
                        NINumberValidationMessage = "NI Number already exists";
                    }
                }
                catch
                {
                    IsNiNumberAvailable = false;
                    IsNINumberNotValid = true;
                    NINumberValidationMessage = "Unable to verify NI number. Please check your connection.";
                }
                finally
                {
                    App.HideLoader();
                }
            }
            else
            {
                IsNINumberNotValid = true;
                NINumberValidationMessage = "Please enter NI number";
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
                await DisplayAlert("Error", "Failed to navigate back. Please try again.", "OK");
            }
        }

        private void OpenPopUp(string type)
        {
            IsPickerPopUpVisible = true;
            PickerPopupPlaceholder = $"Select {type}";
            IsEmployerListVisible = type == "Employer";
            IsTradeListVisible = type == "Trade";
            IsContractorListVisible = type == "Contractor";
        }

        private string _capturedPhotoPath;
        public string CapturedPhotoPath { get => _capturedPhotoPath; set => SetProperty(ref _capturedPhotoPath, value); }
        private string _facePersistencyId;
        public string FacePersistencyId { get => _facePersistencyId; set => SetProperty(ref _facePersistencyId, value); }
        private string _photoBytes;
        public string PhotoBytes { get => _photoBytes; set => SetProperty(ref _photoBytes, value); }

        private async Task NavigateToCamera()
        {
            try
            {
                await Shell.Current.GoToAsync($"CameraCapturePage?email={Email}");
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", "Failed to open camera. Please try again.", "OK");
            }
        }

        public async Task GetEmployerAsync()
        {
            try
            {
                App.ShowLoader();
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
                await DisplayAlert("Error", "Failed to load employers. Please try again.", "OK");
            }
            finally
            {
                App.HideLoader();
            }
        }
    }
}