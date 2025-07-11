using CCIMIGRATION.ApiModels;
using CCIMIGRATION.ConstantHelper;
using CCIMIGRATION.Models;
using CCIMIGRATION.MultilanguageHelper;
using CCIMIGRATION.Views;
using CCIMIGRATION.Views.SiteManagerViews;
using Newtonsoft.Json;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Windows.Input;

namespace CCIMIGRATION.ViewModels
{
    public class LanguageSelectionViewModel : BaseViewModel
    {
        #region Public/Private Variables
        private ObservableCollection<LanguageModel> _languageList;
        private LanguageModel _selectedLanguage;

        public LanguageModel SelectedLanguage
        {
            get { return _selectedLanguage; }
            set 
            {
                _selectedLanguage = value;
                OnPropertyChanged();
                if (SelectedLanguage != null)
                {
                    foreach (var item in LanguageList)
                    {
                        item.IsSelected = false;
                    }
                    LanguageList[LanguageList.IndexOf(SelectedLanguage)].IsSelected = true;
                }
            }
        }

        public ObservableCollection<LanguageModel> LanguageList
        {
            get { return _languageList; }
            set { _languageList = value; OnPropertyChanged(); }
        }

        #endregion
        #region Commands
        public ICommand NextButtonCommand
        {
            get
            {
                return new Command(() =>
                {
                    try
                    {
                        var data = LanguageList.Where(x => x.IsSelected).ToList().FirstOrDefault();
                        if (data != null)
                        {
                            Preferences.Set(Constants.AppLanguageCode, data.Code);
                            LocalizationResourceManager.Instance.SetCulture(CultureInfo.GetCultureInfo(data.Code));
                            CheckUser();
                        }
                    }
                    catch(Exception ex)
                    {
                        WriteLog(ex.Message);
                    }
                    
                });
            }
        }
        #endregion
        #region Contructor
        private void CheckUser()
        {
            if (Preferences.ContainsKey(Constants.UserData))
            {
                var user = Preferences.Get(Constants.UserData, "");
                App.LoggedInUser = JsonConvert.DeserializeObject<User>(user);
                App.Token = App.LoggedInUser.Token;
                if (App.LoggedInUser.RoleType == (int)StaticHelpers.EmployeeType.SiteManager)
                {
                    App.Current.MainPage = new NavigationPage(new SiteManagerMenuPage());
                }
                else
                {
                    App.Current.MainPage = new NavigationPage(new EmployeeMenuPage());
                }
            }
            else
            {
                App.Current.MainPage = new NavigationPage(new LoginPage());
            }
                //App.Current.MainPage = new NavigationPage(new StartupPage());
        }
        public LanguageSelectionViewModel() 
        {
            LanguageList = new ObservableCollection<LanguageModel>();
            //UpdateSelectedLanguage();
        }

        public void UpdateSelectedLanguage()
        {
            var language = Preferences.Get(Constants.AppLanguageCode, "");
            if (!String.IsNullOrEmpty(language))
            {
               LanguageList.Where(x => x.Code == language).FirstOrDefault().IsSelected = true;
            }
            else
            {
                LanguageList.FirstOrDefault().IsSelected = true;
                
            }
        }
        #endregion
        #region Methods
        public void GetLanguages()
        {
            LanguageList.Clear();
            LanguageList.Add(new LanguageModel()
            {
                Name = "English",
                IsSelected = false,
                Code = "en"
            });
            LanguageList.Add(new LanguageModel()
            {
                Name = "Romanian",
                IsSelected = false,
                Code = "ro"
            });
            LanguageList.Add(new LanguageModel()
            {
                Name = "Polish",
                IsSelected = false,
                Code = "pl"
            });
            LanguageList.Add(new LanguageModel()
            {
                Name = "Albanian",
                IsSelected = false,
                Code = "sq"
            });
        }
        #endregion
    }
}
