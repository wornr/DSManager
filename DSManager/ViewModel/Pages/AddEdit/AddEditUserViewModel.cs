using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Text.RegularExpressions;

using GalaSoft.MvvmLight.Messaging;

using DSManager.Messengers;
using DSManager.Model.Entities;
using DSManager.Model.Enums;
using DSManager.Model.Services;
using DSManager.Utilities;

namespace DSManager.ViewModel.Pages.AddEdit {
    public class AddEditUserViewModel : AddEditBaseViewModel, IDataErrorInfo {
        private User _user;
        private AccountType? _accountType;
        private bool _editMode;
        private string _password;

        public AddEditUserViewModel() {
            Messenger.Default.Register<AddEditEntityMessage<User>>(this, HandleMessage);
        }

        private void HandleMessage(AddEditEntityMessage<User> message) {
            if(message.Entity != null) {
                if(message.Entity.GetType() != typeof(User))
                    return;

                _user = (User)message.Entity;
                _accountType = _user.AccountType;
                _editMode = true;
            } else {
                _user = new User();
                _accountType = null;
                _editMode = false;
            }
            _password = null;
        }

        #region IDataErrorInfo Methods
        public string this[string columnName] => Validate(columnName);

        public string Error => "Błąd";

        private string Validate(string propertyName) {
            var validationMessage = string.Empty;

            switch(propertyName) {
                case "FirstName":
                    if(string.IsNullOrEmpty(_user.FirstName))
                        validationMessage = "Pole nie może być puste!";
                    else if(!Regex.IsMatch(_user.FirstName, @"^[a-ząćęłńóśźżA-ZĄĆĘŁŃÓŚŹŻ]+$"))
                        validationMessage = "Podano niepoprawne imię!";
                    break;

                case "LastName":
                    if(string.IsNullOrEmpty(_user.LastName))
                        validationMessage = "Pole nie może być puste!";
                    else if(!Regex.IsMatch(_user.LastName, @"^[a-ząćęłńóśźżA-ZĄĆĘŁŃÓŚŹŻ]+$"))
                        validationMessage = "Podano niepoprawne nazwisko!";
                    break;

                case "Login":
                    if(string.IsNullOrEmpty(_user.Login))
                        validationMessage = "Pole nie może być puste!";
                    else if (!Regex.IsMatch(_user.Login, @"^[\S]+$"))
                        validationMessage = "Podano niepoprawny login!";
                    break;

                case "Password":
                    if(!_editMode && string.IsNullOrEmpty(_password))
                        validationMessage = "Pole nie może być puste!";
                    break;
            }

            return validationMessage;
        }
        #endregion

        #region Save Methods
        public override bool Validate() {
            #region Required
            if(_user.FirstName == null || !Regex.IsMatch(_user.FirstName, @"^[a-ząćęłńóśźżA-ZĄĆĘŁŃÓŚŹŻ]+$"))
                return false;

            if(_user.LastName == null || !Regex.IsMatch(_user.LastName, @"^[a-ząćęłńóśźżA-ZĄĆĘŁŃÓŚŹŻ]+$"))
                return false;

            if(_user.Login == null || !Regex.IsMatch(_user.Login, @"^[\S]+$"))
                return false;

            if (!_editMode && string.IsNullOrEmpty(_password))
                return false;

            if ((_editMode && !string.IsNullOrEmpty(_password)) || !_editMode)
                _user.Password = MD5Encrypter.Encrypt(_password);

            if (AccountType == null)
                return false;
            _user.AccountType = (AccountType) _accountType;
            #endregion

            return true;
        }

        public override int Save() {
            if(!Validate())
                return 1;

            using(var repository = new BaseRepository()) {
                repository.Save(_user);
            }

            return 0;
        }
        #endregion

        #region ViewElements
        public string Login {
            get { return _user.Login; }
            set {
                _user.Login = value;
                RaisePropertyChanged();
            }
        }

        public string Password {
            get { return _password; }
            set {
                _password = value;
                RaisePropertyChanged();
            }
        }

        public string FirstName {
            get { return _user.FirstName; }
            set {
                _user.FirstName = value;
                RaisePropertyChanged();
            }
        }

        public string LastName {
            get { return _user.LastName; }
            set {
                _user.LastName = value;
                RaisePropertyChanged();
            }
        }

        public string Email {
            get { return _user.Email; }
            set {
                _user.Email = value;
                RaisePropertyChanged();
            }
        }

        public ObservableCollection<AccountType> AccountTypes => new ObservableCollection<AccountType>(new EnumToList<AccountType>().Enums);

        public AccountType? AccountType {
            get { return _accountType; }
            set {
                _accountType = value;
                RaisePropertyChanged();
            }
        }

        public bool IsActive {
            get { return _user.Active; }
            set {
                _user.Active = value;
                RaisePropertyChanged();
            }
        }
        #endregion
    }
}