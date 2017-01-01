using System;
using System.Collections.ObjectModel;
using System.ComponentModel;

using GalaSoft.MvvmLight.Messaging;

using DSManager.Messengers;
using DSManager.Model.Entities;
using DSManager.Model.Enums;
using DSManager.Model.Services;
using DSManager.Utilities;

namespace DSManager.ViewModel.Pages.AddEdit {
    public class AddEditMinimalAgeViewModel : AddEditBaseViewModel, IDataErrorInfo {
        private MinimalAge _minimalAge;
        private DrivingLicenseCategory? _category;
        private DateTime? _startDate;
        private int? _age;

        public AddEditMinimalAgeViewModel() {
            Messenger.Default.Register<AddEditEntityMessage<MinimalAge>>(this, HandleMessage);
        }

        private void HandleMessage(AddEditEntityMessage<MinimalAge> message) {
            if(message.Entity != null) {
                if(message.Entity.GetType() != typeof(MinimalAge))
                    return;

                _minimalAge = (MinimalAge)message.Entity;
                _category = _minimalAge.Category;
                _startDate = _minimalAge.StartDate;
                _age = _minimalAge.Age;
            } else {
                _minimalAge = new MinimalAge();
                _category = null;
                _startDate = null;
                _age = null;
            }
        }

        #region IDataErrorInfo Methods
        public string this[string columnName] => Validate(columnName);

        public string Error => "Błąd";

        private string Validate(string propertyName) {
            var validationMessage = string.Empty;

            switch(propertyName) {
                case "Category":
                    if(_category == null)
                        validationMessage = "Pole nie może być puste!";
                    break;

                case "StartDate":
                    if(_startDate == null)
                        validationMessage = "Pole nie może być puste!";
                    break;

                case "Age":
                    if(_age == null)
                        validationMessage = "Pole nie może być puste!";
                    else if(_age <= 0)
                        validationMessage = "Wartość musi być większa od zera!";
                    break;
            }

            return validationMessage;
        }
        #endregion

        #region Save Methods
        public override bool Validate() {
            #region Required
            if(_category == null)
                return false;
            _minimalAge.Category = (DrivingLicenseCategory)_category;

            if(_startDate == null)
                return false;
            _minimalAge.StartDate = (DateTime)_startDate;

            if(_age == null || _age <= 0)
                return false;
            _minimalAge.Age = (int) _age;
            #endregion

            return true;
        }

        public override bool Save() {
            if(!Validate())
                return false;

            using(var repository = new BaseRepository()) {
                repository.Save(_minimalAge);
            }

            return true;
        }
        #endregion

        #region ViewElements
        public DrivingLicenseCategory? Category {
            get { return _category; }
            set {
                _category = value;
                RaisePropertyChanged();
            }
        }

        public DateTime? StartDate {
            get { return _startDate; }
            set {
                _startDate = value;
                RaisePropertyChanged();
            }
        }

        public int? Age {
            get { return _age; }
            set {
                _age = value;
                RaisePropertyChanged();
            }
        }

        public ObservableCollection<DrivingLicenseCategory> Categories => new ObservableCollection<DrivingLicenseCategory>(new EnumToList<DrivingLicenseCategory>().Enums);
        #endregion
    }
}