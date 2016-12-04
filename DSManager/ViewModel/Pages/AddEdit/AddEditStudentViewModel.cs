using System;
using System.ComponentModel;
using System.Text.RegularExpressions;

using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;

using DSManager.Messengers;
using DSManager.Model.Entities;
using DSManager.Model.Services;
using DSManager.Validators;

namespace DSManager.ViewModel.Pages.AddEdit {
    public class AddEditStudentViewModel : AddEditBaseViewModel, IDataErrorInfo {
        private Student _student;
        private DateTime? _birthDate;
        private bool _PESELValid;

        private RelayCommand _PESELToDate;

        public AddEditStudentViewModel() {
            Messenger.Default.Register<AddEditEntityMessage>(this, HandleMessage);
        }

        private void HandleMessage(AddEditEntityMessage message) {
            if (message.Entity != null) {
                _student = (Student) message.Entity;
                _birthDate = _student.BirthDate;
                _PESELValid = PESELValidator.Validate(_student.PESEL);
            } else {
                _student = new Student();
                _birthDate = null;
                _PESELValid = false;
            }
        }

        #region IDataErrorInfo Methods
        public string this[string columnName] => Validate(columnName);

        public string Error => "Błąd";

        private string Validate(string propertyName) {
            var validationMessage = string.Empty;

            switch(propertyName) {
                case "FirstName":
                    if(string.IsNullOrEmpty(_student.FirstName))
                        validationMessage = "Pole nie może być puste!";
                    else if (!Regex.IsMatch(_student.FirstName, @"^[a-ząćęłńóśźżA-ZĄĆĘŁŃÓŚŹŻ]+$"))
                        validationMessage = "Podano niepoprawne imię!";
                    break;

                case "SecondName":
                    if(!string.IsNullOrEmpty(_student.SecondName) && !Regex.IsMatch(_student.SecondName, @"^[a-ząćęłńóśźżA-ZĄĆĘŁŃÓŚŹŻ]+$"))
                        validationMessage = "Podano niepoprawne imię!";
                    break;

                case "LastName":
                    if(string.IsNullOrEmpty(_student.LastName))
                        validationMessage = "Pole nie może być puste!";
                    else if(!Regex.IsMatch(_student.LastName, @"^[a-ząćęłńóśźżA-ZĄĆĘŁŃÓŚŹŻ]+$"))
                        validationMessage = "Podano niepoprawne nazwisko!";
                    break;

                case "BirthDate":
                    if(_birthDate == null)
                        validationMessage = "Pole nie może być puste!";
                    else if (!string.IsNullOrEmpty(_student.PESEL))
                        if (!BirthDateValidator.Validate(_birthDate, _student.PESEL))
                            validationMessage = "Podana data urodzenia nie jest zgodna z numerem PESEL!";
                    break;

                case "PESEL":
                    if(!string.IsNullOrEmpty(_student.PESEL) && !PESELValidator.Validate(_student.PESEL))
                        validationMessage = "Podano niepoprawny numer PESEL!";
                    BirthDate = _birthDate;
                    break;

                case "Street":
                    if(string.IsNullOrEmpty(_student.Street))
                        validationMessage = "Pole nie może być puste!";
                    else if(!Regex.IsMatch(_student.Street, @"^[0-9a-ząćęłńóśźżA-ZĄĆĘŁŃÓŚŹŻ ]+$"))
                        validationMessage = "Podano niepoprawną nazwę ulicy!";
                    break;

                case "HouseNr":
                    if(string.IsNullOrEmpty(_student.HouseNr))
                        validationMessage = "Pole nie może być puste!";
                    else if(!Regex.IsMatch(_student.HouseNr, @"^[0-9a-ząćęłńóśźżA-ZĄĆĘŁŃÓŚŹŻ ]+$"))
                        validationMessage = "Podano niepoprawny numer domu!";
                    break;

                case "ApartmentNr":
                    if(!string.IsNullOrEmpty(_student.ApartmentNr) && !Regex.IsMatch(_student.ApartmentNr, @"^[0-9a-ząćęłńóśźżA-ZĄĆĘŁŃÓŚŹŻ ]+$"))
                        validationMessage = "Podano niepoprawny numer lokalu";
                    break;

                case "PostalCode":
                    if(!string.IsNullOrEmpty(_student.PostalCode) && !Regex.IsMatch(_student.PostalCode, @"^[0-9]{2}-[0-9]{3}$"))
                        validationMessage = "Podano niepoprawny kod pocztowy";
                    break;

                case "City":
                    if(string.IsNullOrEmpty(_student.City))
                        validationMessage = "Pole nie może być puste!";
                    else if(!Regex.IsMatch(_student.City, @"^[a-ząćęłńóśźżA-ZĄĆĘŁŃÓŚŹŻ]+$"))
                        validationMessage = "Podano niepoprawną nazwę miejscowości!";
                    break;

                case "PhoneNr":
                    if(!string.IsNullOrEmpty(_student.PhoneNr) && !Regex.IsMatch(_student.PhoneNr, @"^(\+?[0-9]+)?(\([0-9]+\))?[0-9]+$"))
                        validationMessage = "Podano niepoprawny numer telefonu";
                    break;

                case "DrivingLicenseIssueDate":
                    if(!string.IsNullOrEmpty(_student.FirstName) && !Regex.IsMatch(_student.FirstName, @"^[a-zA-Z]+$"))
                        validationMessage = "Podano niepoprawną datę wydania prawa jazdy";
                    break;

                case "DrivingLicenseNr":
                    if(!string.IsNullOrEmpty(_student.FirstName) && !Regex.IsMatch(_student.FirstName, @"^[a-zA-Z]+$"))
                        validationMessage = "Podano niepoprawny numer prawa jazdy";
                    break;
            }

            return validationMessage;
        }
        #endregion
        
        #region IDataSave Methods
        public override bool Validate() {
            var valid = true;

            #region Required
            if (_student.FirstName == null || !Regex.IsMatch(_student.FirstName, @"^[a-ząćęłńóśźżA-ZĄĆĘŁŃÓŚŹŻ]+$"))
                valid = false;
            
            if (_student.LastName == null || !Regex.IsMatch(_student.LastName, @"^[a-ząćęłńóśźżA-ZĄĆĘŁŃÓŚŹŻ]+$"))
                valid = false;

            if (_birthDate == null) 
                valid = false;
            else if (!string.IsNullOrEmpty(_student.PESEL)) {
                if (!BirthDateValidator.Validate(_birthDate, _student.PESEL))
                    valid = false;
                else
                    _student.BirthDate = (DateTime)_birthDate;
            } else
                _student.BirthDate = (DateTime) _birthDate;
            
            if(_student.Street == null || !Regex.IsMatch(_student.Street, @"^[0-9a-ząćęłńóśźżA-ZĄĆĘŁŃÓŚŹŻ ]+$"))
                valid = false;
            
            if(_student.HouseNr == null || !Regex.IsMatch(_student.HouseNr, @"^[0-9a-ząćęłńóśźżA-ZĄĆĘŁŃÓŚŹŻ ]+$"))
                valid = false;
            
            if(_student.City == null || !Regex.IsMatch(_student.City, @"^[a-ząćęłńóśźżA-ZĄĆĘŁŃÓŚŹŻ]+$"))
                valid = false;
            #endregion

            #region Not Required
            if(string.IsNullOrEmpty(_student.SecondName))
                _student.SecondName = null;
            else if(!Regex.IsMatch(_student.SecondName, @"^[a-ząćęłńóśźżA-ZĄĆĘŁŃÓŚŹŻ]+$"))
                valid = false;

            if (string.IsNullOrEmpty(_student.PESEL))
                _student.PESEL = null;
            else if(!PESELValidator.Validate(_student.PESEL))
                valid = false;

            if(string.IsNullOrEmpty(_student.ApartmentNr))
                _student.ApartmentNr = null;
            else if(!Regex.IsMatch(_student.ApartmentNr, @"^[0-9a-ząćęłńóśźżA-ZĄĆĘŁŃÓŚŹŻ ]+$"))
                valid = false;

            if(string.IsNullOrEmpty(_student.PostalCode))
                _student.PostalCode = null;
            else if(!Regex.IsMatch(_student.PostalCode, @"^[0-9]{2}-[0-9]{3}$"))
                valid = false;

            if(string.IsNullOrEmpty(_student.PhoneNr))
                _student.PhoneNr = null;
            else if(!Regex.IsMatch(_student.PhoneNr, @"^(\+?[0-9]+)?(\([0-9]+\))?[0-9]+$"))
                valid = false;
            
            // TODO walidacja daty wydania prawa jazdy (pole nieobowiązkowe)

            // TODO walidacja numeru prawa jazdy (pole nieobowiązkowe)
            #endregion

            return valid;
        }

        public override bool Save() {
            if(!Validate()) {
                // TODO wyrzucić dialog o błędach walidacji
                return false;
            }

            using(var repository = new BaseRepository()) {
                repository.Save(_student);
            }

            return true;
        }
        #endregion

        #region Properties
        public string FirstName {
            get { return _student.FirstName; }
            set {
                _student.FirstName = value;
                RaisePropertyChanged();
            }
        }

        public string SecondName {
            get { return _student.SecondName; }
            set {
                _student.SecondName = value;
                RaisePropertyChanged();
            }
        }

        public string LastName {
            get { return _student.LastName; }
            set {
                _student.LastName = value;
                RaisePropertyChanged();
            }
        }

        public string PESEL {
            get { return _student.PESEL; }
            set {
                _student.PESEL = value;
                PESELValid = PESELValidator.Validate(_student.PESEL);
                RaisePropertyChanged();
            }
        }

        public DateTime? BirthDate {
            get { return _birthDate; }
            set {
                _birthDate = value;
                RaisePropertyChanged();
            }
        }

        public string City {
            get { return _student.City; }
            set {
                _student.City = value;
                RaisePropertyChanged();
            }
        }

        public string PostalCode {
            get { return _student.PostalCode; }
            set {
                _student.PostalCode = value;
                RaisePropertyChanged();
            }
        }

        public string Street {
            get { return _student.Street; }
            set {
                _student.Street = value;
                RaisePropertyChanged();
            }
        }

        public string HouseNr {
            get { return _student.HouseNr; }
            set {
                _student.HouseNr = value;
                RaisePropertyChanged();
            }
        }

        public string ApartmentNr {
            get { return _student.ApartmentNr; }
            set {
                _student.ApartmentNr = value;
                RaisePropertyChanged();
            }
        }

        public string PhoneNr {
            get { return _student.PhoneNr; }
            set {
                _student.PhoneNr = value;
                RaisePropertyChanged();
            }
        }

        public string Email {
            get { return _student.Email; }
            set {
                _student.Email = value;
                RaisePropertyChanged();
            }
        }

        public bool PESELValid {
            get { return _PESELValid; }
            set {
                _PESELValid = value;
                RaisePropertyChanged();
            }
        }
        #endregion

        #region Commands
        public RelayCommand PESELToDate => _PESELToDate ?? (_PESELToDate = new RelayCommand(() => {
            BirthDate = Utilities.PESELToDate.Translate(_student.PESEL);
        }));
        #endregion
    }
}