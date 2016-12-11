using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text.RegularExpressions;

using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;

using NHibernate.Util;

using DSManager.Messengers;
using DSManager.Model.Entities;
using DSManager.Model.Enums;
using DSManager.Model.Services;
using DSManager.Utilities;
using DSManager.Validators;

namespace DSManager.ViewModel.Pages.AddEdit {
    public class AddEditStudentViewModel : AddEditBaseViewModel, IDataErrorInfo {
        private Student _student;
        private DrivingLicense _drivingLicense;
        private DateTime? _birthDate;
        private DateTime? _drivingLicenseIssueDate;
        private bool _PESELValid;
        private DrivingLicensePermissions _availableCategory;
        private DrivingLicensePermissions _chosenCategory;
        private ObservableCollection<DrivingLicensePermissions> _availableCategories;
        private ObservableCollection<DrivingLicensePermissions> _chosenCategories;

        private RelayCommand _PESELToDate;
        private RelayCommand _moveCategoryToLeft;
        private RelayCommand _moveCategoryToRight;

        public AddEditStudentViewModel() {
            Messenger.Default.Register<AddEditEntityMessage>(this, HandleMessage);
        }

        private void HandleMessage(AddEditEntityMessage message) {
            if (message.Entity != null) {
                if (message.Entity.GetType() != typeof(Student))
                    return;

                _student = (Student) message.Entity;
                _birthDate = _student.BirthDate;
                _PESELValid = PESELValidator.Validate(_student.PESEL);
                _drivingLicense = _student.DrivingLicense ?? new DrivingLicense {
                    DrivingLicensePermissions = new List<DrivingLicensePermissions>()
                };
                _chosenCategories = new ObservableCollection<DrivingLicensePermissions>(_drivingLicense.DrivingLicensePermissions);
            } else {
                _student = new Student();
                _birthDate = null;
                _PESELValid = false;
                _drivingLicense = new DrivingLicense {
                    DrivingLicensePermissions = new List<DrivingLicensePermissions>()
                };
                _chosenCategories = new ObservableCollection<DrivingLicensePermissions>();
            }

            _availableCategories = FillCategories();
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
            }

            return validationMessage;
        }
        #endregion
        
        #region Save Methods
        public override bool Validate() {
            #region Required
            if (_student.FirstName == null || !Regex.IsMatch(_student.FirstName, @"^[a-ząćęłńóśźżA-ZĄĆĘŁŃÓŚŹŻ]+$"))
                return false;
            
            if (_student.LastName == null || !Regex.IsMatch(_student.LastName, @"^[a-ząćęłńóśźżA-ZĄĆĘŁŃÓŚŹŻ]+$"))
                return false;

            if (_birthDate == null)
                return false;
            if (!string.IsNullOrEmpty(_student.PESEL)) {
                if (!BirthDateValidator.Validate(_birthDate, _student.PESEL))
                    return false;
                _student.BirthDate = (DateTime)_birthDate;
            } else
                _student.BirthDate = (DateTime) _birthDate;
            
            if(_student.Street == null || !Regex.IsMatch(_student.Street, @"^[0-9a-ząćęłńóśźżA-ZĄĆĘŁŃÓŚŹŻ ]+$"))
                return false;

            if(_student.HouseNr == null || !Regex.IsMatch(_student.HouseNr, @"^[0-9a-ząćęłńóśźżA-ZĄĆĘŁŃÓŚŹŻ ]+$"))
                return false;

            if(_student.City == null || !Regex.IsMatch(_student.City, @"^[a-ząćęłńóśźżA-ZĄĆĘŁŃÓŚŹŻ]+$"))
                return false;
            #endregion

            #region Not Required
            if(string.IsNullOrEmpty(_student.SecondName))
                _student.SecondName = null;
            else if(!Regex.IsMatch(_student.SecondName, @"^[a-ząćęłńóśźżA-ZĄĆĘŁŃÓŚŹŻ]+$"))
                return false;

            if (string.IsNullOrEmpty(_student.PESEL))
                _student.PESEL = null;
            else if(!PESELValidator.Validate(_student.PESEL))
                return false;

            if(string.IsNullOrEmpty(_student.ApartmentNr))
                _student.ApartmentNr = null;
            else if(!Regex.IsMatch(_student.ApartmentNr, @"^[0-9a-ząćęłńóśźżA-ZĄĆĘŁŃÓŚŹŻ ]+$"))
                return false;

            if(string.IsNullOrEmpty(_student.PostalCode))
                _student.PostalCode = null;
            else if(!Regex.IsMatch(_student.PostalCode, @"^[0-9]{2}-[0-9]{3}$"))
                return false;

            if(string.IsNullOrEmpty(_student.PhoneNr))
                _student.PhoneNr = null;
            else if(!Regex.IsMatch(_student.PhoneNr, @"^(\+?[0-9]+)?(\([0-9]+\))?[0-9]+$"))
                return false;

            if (_drivingLicenseIssueDate != null || !string.IsNullOrEmpty(_drivingLicense.DrivingLicenseNr) || _chosenCategories.Count != 0) {
                if (_drivingLicenseIssueDate == null)
                    return false;
                if (string.IsNullOrEmpty(_drivingLicense.DrivingLicenseNr))
                    return false;
                if (_chosenCategories.Count == 0)
                    return false;

                _drivingLicense.IssueDate = (DateTime)_drivingLicenseIssueDate;
                _drivingLicense.DrivingLicensePermissions = _chosenCategories;
                _drivingLicense.Student = _student;
                _student.DrivingLicense = _drivingLicense;
            } else {
                _student.DrivingLicense = null;
            }
            #endregion

            return true;
        }

        public override bool Save() {
            if(!Validate())
                return false;

            using(var repository = new BaseRepository()) {
                if(_student.DrivingLicense != null)
                    repository.Save(_drivingLicense);
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

        public DateTime? DrivingLicenseIssueDate {
            get { return _drivingLicenseIssueDate; }
            set {
                _drivingLicenseIssueDate = value;
                RaisePropertyChanged();
            }
        }

        public string DrivingLicenseNr {
            get { return _drivingLicense.DrivingLicenseNr; }
            set {
                _drivingLicense.DrivingLicenseNr = value;
                RaisePropertyChanged();
            }
        }

        public DrivingLicensePermissions AvailableCategory {
            get { return _availableCategory; }
            set {
                _availableCategory = value;
                RaisePropertyChanged();
            }
        }

        public DrivingLicensePermissions ChosenCategory {
            get { return _chosenCategory; }
            set {
                _chosenCategory = value;
                RaisePropertyChanged();
            }
        }

        public ObservableCollection<DrivingLicensePermissions> AvailableCategories {
            get { return _availableCategories; }
            set {
                _availableCategories = value;
                RaisePropertyChanged();
            }
        }

        public ObservableCollection<DrivingLicensePermissions> ChosenCategories {
            get { return _chosenCategories; }
            set {
                _chosenCategories = value;
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

        public RelayCommand MoveCategoryToRight => _moveCategoryToRight ?? (_moveCategoryToRight = new RelayCommand(() => {
            ChosenCategories.Add(_availableCategory);
            AvailableCategories.Remove(_availableCategory);
            ChosenCategories = new ObservableCollection<DrivingLicensePermissions>(ChosenCategories.OrderBy(x => x.Category));
        }));

        public RelayCommand MoveCategoryToLeft => _moveCategoryToLeft ?? (_moveCategoryToLeft = new RelayCommand(() => {
            AvailableCategories.Add(_chosenCategory);
            ChosenCategories.Remove(_chosenCategory);
            AvailableCategories = new ObservableCollection<DrivingLicensePermissions>(AvailableCategories.OrderBy(x => x.Category));
        }));
        #endregion

        #region Helpers
        private ObservableCollection<DrivingLicensePermissions> FillCategories() {
            var availableCategories = new ObservableCollection<DrivingLicensePermissions>();

            new EnumToList<DrivingLicenseCategory>().Enums.ForEach(x => availableCategories.Add(new DrivingLicensePermissions {
                DrivingLicense = _drivingLicense,
                Category = x
            }));


            _chosenCategories.ForEach(x => {
                var availableCategory = availableCategories.FirstOrDefault(y => y.Category == x.Category);
                availableCategories.Remove(availableCategory);
            });

            return availableCategories;
        }
        #endregion
    }
}