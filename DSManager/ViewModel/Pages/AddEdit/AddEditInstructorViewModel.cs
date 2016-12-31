using System;
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
    public class AddEditInstructorViewModel : AddEditBaseViewModel, IDataErrorInfo {
        private Instructor _instructor;
        private DateTime? _birthDate;
        private bool _PESELValid;
        private InstructorPermissions _availableCategory;
        private InstructorPermissions _chosenCategory;
        private ObservableCollection<InstructorPermissions> _availableCategories;
        private ObservableCollection<InstructorPermissions> _chosenCategories;

        private RelayCommand _PESELToDate;
        private RelayCommand _moveCategoryToLeft;
        private RelayCommand _moveCategoryToRight;

        public AddEditInstructorViewModel() {
            Messenger.Default.Register<AddEditEntityMessage<Instructor>>(this, HandleMessage);
        }

        private void HandleMessage(AddEditEntityMessage<Instructor> message) {
            if(message.Entity != null) {
                if (message.Entity.GetType() != typeof(Instructor))
                    return;

                _instructor = (Instructor) message.Entity;
                _birthDate = _instructor.BirthDate;
                _PESELValid = PESELValidator.Validate(_instructor.PESEL);
                _chosenCategories = new ObservableCollection<InstructorPermissions>(_instructor.Permissions);
            } else {
                _instructor = new Instructor();
                _birthDate = null;
                _PESELValid = false;
                _chosenCategories = new ObservableCollection<InstructorPermissions>();
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
                    if(string.IsNullOrEmpty(_instructor.FirstName))
                        validationMessage = "Pole nie może być puste!";
                    else if(!Regex.IsMatch(_instructor.FirstName, @"^[a-ząćęłńóśźżA-ZĄĆĘŁŃÓŚŹŻ]+$"))
                        validationMessage = "Podano niepoprawne imię!";
                    break;

                case "SecondName":
                    if(!string.IsNullOrEmpty(_instructor.SecondName) && !Regex.IsMatch(_instructor.SecondName, @"^[a-ząćęłńóśźżA-ZĄĆĘŁŃÓŚŹŻ]+$"))
                        validationMessage = "Podano niepoprawne imię!";
                    break;

                case "LastName":
                    if(string.IsNullOrEmpty(_instructor.LastName))
                        validationMessage = "Pole nie może być puste!";
                    else if(!Regex.IsMatch(_instructor.LastName, @"^[a-ząćęłńóśźżA-ZĄĆĘŁŃÓŚŹŻ]+$"))
                        validationMessage = "Podano niepoprawne nazwisko!";
                    break;

                case "BirthDate":
                    if(_birthDate == null)
                        validationMessage = "Pole nie może być puste!";
                    else if(!string.IsNullOrEmpty(_instructor.PESEL))
                        if(!BirthDateValidator.Validate(_birthDate, _instructor.PESEL))
                            validationMessage = "Podana data urodzenia nie jest zgodna z numerem PESEL!";
                    break;

                case "PESEL":
                    if(!string.IsNullOrEmpty(_instructor.PESEL) && !PESELValidator.Validate(_instructor.PESEL))
                        validationMessage = "Podano niepoprawny numer PESEL!";
                    BirthDate = _birthDate;
                    break;

                case "Street":
                    if(string.IsNullOrEmpty(_instructor.Street))
                        validationMessage = "Pole nie może być puste!";
                    else if(!Regex.IsMatch(_instructor.Street, @"^[0-9a-ząćęłńóśźżA-ZĄĆĘŁŃÓŚŹŻ ]+$"))
                        validationMessage = "Podano niepoprawną nazwę ulicy!";
                    break;

                case "HouseNr":
                    if(string.IsNullOrEmpty(_instructor.HouseNr))
                        validationMessage = "Pole nie może być puste!";
                    else if(!Regex.IsMatch(_instructor.HouseNr, @"^[0-9a-ząćęłńóśźżA-ZĄĆĘŁŃÓŚŹŻ ]+$"))
                        validationMessage = "Podano niepoprawny numer domu!";
                    break;

                case "ApartmentNr":
                    if(!string.IsNullOrEmpty(_instructor.ApartmentNr) && !Regex.IsMatch(_instructor.ApartmentNr, @"^[0-9a-ząćęłńóśźżA-ZĄĆĘŁŃÓŚŹŻ ]+$"))
                        validationMessage = "Podano niepoprawny numer lokalu";
                    break;

                case "PostalCode":
                    if(!string.IsNullOrEmpty(_instructor.PostalCode) && !Regex.IsMatch(_instructor.PostalCode, @"^[0-9]{2}-[0-9]{3}$"))
                        validationMessage = "Podano niepoprawny kod pocztowy";
                    break;

                case "City":
                    if(string.IsNullOrEmpty(_instructor.City))
                        validationMessage = "Pole nie może być puste!";
                    else if(!Regex.IsMatch(_instructor.City, @"^[a-ząćęłńóśźżA-ZĄĆĘŁŃÓŚŹŻ]+$"))
                        validationMessage = "Podano niepoprawną nazwę miejscowości!";
                    break;

                case "PhoneNr":
                    if(!string.IsNullOrEmpty(_instructor.PhoneNr) && !Regex.IsMatch(_instructor.PhoneNr, @"^(\+?[0-9]+)?(\([0-9]+\))?[0-9]+$"))
                        validationMessage = "Podano niepoprawny numer telefonu";
                    break;

                case "PermissionsNr":
                    if(string.IsNullOrEmpty(_instructor.PermissionsNr))
                        validationMessage = "Pole nie może być puste!";
                    else if(!Regex.IsMatch(_instructor.PermissionsNr, @"^[0-9a-ząćęłńóśźżA-ZĄĆĘŁŃÓŚŹŻ ]+$"))
                        validationMessage = "Podano niepoprawny numer uprawnień";
                    break;

                case "DrivingLicenseNr":
                    if(!string.IsNullOrEmpty(_instructor.FirstName) && !Regex.IsMatch(_instructor.FirstName, @"^[a-zA-Z]+$"))
                        validationMessage = "Podano niepoprawny numer prawa jazdy";
                    break;

                case "ChosenCategories":
                    if(_chosenCategories.Count == 0)
                        validationMessage = "Pole nie może być puste!";
                    break;
            }

            return validationMessage;
        }
        #endregion

        #region Save Methods
        public override bool Validate() {
            #region Required
            if(_instructor.FirstName == null || !Regex.IsMatch(_instructor.FirstName, @"^[a-ząćęłńóśźżA-ZĄĆĘŁŃÓŚŹŻ]+$"))
                return false;

            if(_instructor.LastName == null || !Regex.IsMatch(_instructor.LastName, @"^[a-ząćęłńóśźżA-ZĄĆĘŁŃÓŚŹŻ]+$"))
                return false;

            if(_birthDate == null)
                return false;
            if(!string.IsNullOrEmpty(_instructor.PESEL)) {
                if(!BirthDateValidator.Validate(_birthDate, _instructor.PESEL))
                    return false;
                _instructor.BirthDate = (DateTime)_birthDate;
            } else
                _instructor.BirthDate = (DateTime)_birthDate;

            if(_instructor.Street == null || !Regex.IsMatch(_instructor.Street, @"^[0-9a-ząćęłńóśźżA-ZĄĆĘŁŃÓŚŹŻ ]+$"))
                return false;

            if(_instructor.HouseNr == null || !Regex.IsMatch(_instructor.HouseNr, @"^[0-9a-ząćęłńóśźżA-ZĄĆĘŁŃÓŚŹŻ ]+$"))
                return false;

            if(_instructor.City == null || !Regex.IsMatch(_instructor.City, @"^[a-ząćęłńóśźżA-ZĄĆĘŁŃÓŚŹŻ]+$"))
                return false;

            if(_instructor.PermissionsNr == null || !Regex.IsMatch(_instructor.PermissionsNr, @"^[0-9a-ząćęłńóśźżA-ZĄĆĘŁŃÓŚŹŻ ]+$"))
                return false;

            if(_chosenCategories.Count == 0)
                return false;
            _instructor.Permissions = _chosenCategories;
            #endregion

            #region Not Required
            if(string.IsNullOrEmpty(_instructor.SecondName))
                _instructor.SecondName = null;
            else if(!Regex.IsMatch(_instructor.SecondName, @"^[a-ząćęłńóśźżA-ZĄĆĘŁŃÓŚŹŻ]+$"))
                return false;

            if(string.IsNullOrEmpty(_instructor.PESEL))
                _instructor.PESEL = null;
            else if(!PESELValidator.Validate(_instructor.PESEL))
                return false;

            if(string.IsNullOrEmpty(_instructor.ApartmentNr))
                _instructor.ApartmentNr = null;
            else if(!Regex.IsMatch(_instructor.ApartmentNr, @"^[0-9a-ząćęłńóśźżA-ZĄĆĘŁŃÓŚŹŻ ]+$"))
                return false;

            if(string.IsNullOrEmpty(_instructor.PostalCode))
                _instructor.PostalCode = null;
            else if(!Regex.IsMatch(_instructor.PostalCode, @"^[0-9]{2}-[0-9]{3}$"))
                return false;

            if(string.IsNullOrEmpty(_instructor.PhoneNr))
                _instructor.PhoneNr = null;
            else if(!Regex.IsMatch(_instructor.PhoneNr, @"^(\+?[0-9]+)?(\([0-9]+\))?[0-9]+$"))
                return false;
            #endregion

            return true;
        }

        public override bool Save() {
            if(!Validate())
                return false;

            using(var repository = new BaseRepository()) {
                repository.Save(_instructor);
            }

            return true;
        }
        #endregion

        #region Properties
        public string FirstName {
            get { return _instructor.FirstName; }
            set {
                _instructor.FirstName = value;
                RaisePropertyChanged();
            }
        }

        public string SecondName {
            get { return _instructor.SecondName; }
            set {
                _instructor.SecondName = value;
                RaisePropertyChanged();
            }
        }

        public string LastName {
            get { return _instructor.LastName; }
            set {
                _instructor.LastName = value;
                RaisePropertyChanged();
            }
        }

        public string PESEL {
            get { return _instructor.PESEL; }
            set {
                _instructor.PESEL = value;
                PESELValid = PESELValidator.Validate(_instructor.PESEL);
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
            get { return _instructor.City; }
            set {
                _instructor.City = value;
                RaisePropertyChanged();
            }
        }

        public string PostalCode {
            get { return _instructor.PostalCode; }
            set {
                _instructor.PostalCode = value;
                RaisePropertyChanged();
            }
        }

        public string Street {
            get { return _instructor.Street; }
            set {
                _instructor.Street = value;
                RaisePropertyChanged();
            }
        }

        public string HouseNr {
            get { return _instructor.HouseNr; }
            set {
                _instructor.HouseNr = value;
                RaisePropertyChanged();
            }
        }

        public string ApartmentNr {
            get { return _instructor.ApartmentNr; }
            set {
                _instructor.ApartmentNr = value;
                RaisePropertyChanged();
            }
        }

        public string PhoneNr {
            get { return _instructor.PhoneNr; }
            set {
                _instructor.PhoneNr = value;
                RaisePropertyChanged();
            }
        }

        public string Email {
            get { return _instructor.Email; }
            set {
                _instructor.Email = value;
                RaisePropertyChanged();
            }
        }

        public string PermissionsNr {
            get { return _instructor.PermissionsNr; }
            set {
                _instructor.PermissionsNr = value;
                RaisePropertyChanged();
            }
        }

        public InstructorPermissions AvailableCategory {
            get { return _availableCategory; }
            set {
                _availableCategory = value;
                RaisePropertyChanged();
            }
        }

        public InstructorPermissions ChosenCategory {
            get { return _chosenCategory; }
            set {
                _chosenCategory = value;
                RaisePropertyChanged();
            }
        }

        public ObservableCollection<InstructorPermissions> AvailableCategories {
            get { return _availableCategories; }
            set {
                _availableCategories = value;
                RaisePropertyChanged();
            }
        }

        public ObservableCollection<InstructorPermissions> ChosenCategories {
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
            BirthDate = Utilities.PESELToDate.Translate(_instructor.PESEL);
        }));

        public RelayCommand MoveCategoryToRight => _moveCategoryToRight ?? (_moveCategoryToRight = new RelayCommand(() => {
            ChosenCategories.Add(_availableCategory);
            AvailableCategories.Remove(_availableCategory);
            RefreshTables();
        }));

        public RelayCommand MoveCategoryToLeft => _moveCategoryToLeft ?? (_moveCategoryToLeft = new RelayCommand(() => {
            AvailableCategories.Add(_chosenCategory);
            ChosenCategories.Remove(_chosenCategory);
            RefreshTables();
        }));
        #endregion

        #region Helpers
        private ObservableCollection<InstructorPermissions> FillCategories() {
            var availableCategories = new ObservableCollection<InstructorPermissions>();

            new EnumToList<DrivingLicenseCategory>().Enums.ForEach(x => availableCategories.Add(new InstructorPermissions {
                Instructor = _instructor,
                Category = x
            }));


            _chosenCategories.ForEach(x => {
                var availableCategory = availableCategories.FirstOrDefault(y => y.Category == x.Category);
                availableCategories.Remove(availableCategory);
            });

            return availableCategories;
        }

        private void RefreshTables() {
            ChosenCategories = new ObservableCollection<InstructorPermissions>(ChosenCategories.OrderBy(x => x.Category));
            AvailableCategories = new ObservableCollection<InstructorPermissions>(AvailableCategories.OrderBy(x => x.Category));
        }
        #endregion
    }
}