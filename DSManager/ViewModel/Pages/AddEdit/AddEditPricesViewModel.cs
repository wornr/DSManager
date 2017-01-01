using System;
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
    public class AddEditPricesViewModel : AddEditBaseViewModel, IDataErrorInfo {
        private Prices _prices;
        private DrivingLicenseCategory? _category;
        private CourseType? _courseType;
        private CourseKind? _courseKind;
        private DateTime? _startDate;
        private DateTime? _endDate;
        private decimal? _price;

        public AddEditPricesViewModel() {
            Messenger.Default.Register<AddEditEntityMessage<Prices>>(this, HandleMessage);
        }

        private void HandleMessage(AddEditEntityMessage<Prices> message) {
            if(message.Entity != null) {
                if(message.Entity.GetType() != typeof(Prices))
                    return;

                _prices = (Prices)message.Entity;
                _category = _prices.Category;
                _courseType = _prices.CourseType;
                _courseKind = _prices.CourseKind;
                _startDate = _prices.StartDate;
                _endDate = _prices.EndDate;
                _price = _prices.Price;
            } else {
                _prices = new Prices();
                _category = null;
                _courseType = null;
                _courseKind = null;
                _startDate = null;
                _endDate = null;
                _price = null;
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

                case "CourseType":
                    if(_courseType == null)
                        validationMessage = "Pole nie może być puste!";
                    break;

                case "CourseKind":
                    if(_courseKind == null)
                        validationMessage = "Pole nie może być puste!";
                    break;

                case "StartDate":
                    if(_startDate == null)
                        validationMessage = "Pole nie może być puste!";
                    break;

                case "EndDate":
                    if(_endDate == null)
                        validationMessage = "Pole nie może być puste!";
                    break;

                case "Price":
                    if(_price == null)
                        validationMessage = "Pole nie może być puste!";
                    else if(_price <= 0m)
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
            _prices.Category = (DrivingLicenseCategory) _category;

            if(_courseType == null)
                return false;
            _prices.CourseType = (CourseType) _courseType;

            if(_courseKind == null)
                return false;
            _prices.CourseKind = (CourseKind) _courseKind;

            if(_startDate == null)
                return false;
            _prices.StartDate = (DateTime) _startDate;

            if(_endDate == null)
                return false;
            _prices.EndDate = (DateTime) _endDate;

            if(_price == null || _price <= 0m)
                return false;
            _prices.Price = (decimal)_price;
            #endregion

            return true;
        }

        public override bool Save() {
            if(!Validate())
                return false;

            using(var repository = new BaseRepository()) {
                repository.Save(_prices);
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

        public CourseType? CourseType {
            get { return _courseType; }
            set {
                _courseType = value;
                RaisePropertyChanged();
            }
        }

        public CourseKind? CourseKind {
            get { return _courseKind; }
            set {
                _courseKind = value;
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

        public DateTime? EndDate {
            get { return _endDate; }
            set {
                _endDate = value;
                RaisePropertyChanged();
            }
        }

        public decimal? Price {
            get { return _price; }
            set {
                _price = value;
                RaisePropertyChanged();
            }
        }

        public ObservableCollection<DrivingLicenseCategory> Categories => new ObservableCollection<DrivingLicenseCategory>(new EnumToList<DrivingLicenseCategory>().Enums);

        public ObservableCollection<CourseType> CourseTypes => new ObservableCollection<CourseType>(new EnumToList<CourseType>().Enums);

        public ObservableCollection<CourseKind> CourseKinds => new ObservableCollection<CourseKind>(new EnumToList<CourseKind>().Enums);
        #endregion
    }
}