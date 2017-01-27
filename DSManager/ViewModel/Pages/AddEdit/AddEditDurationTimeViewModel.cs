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
    public class AddEditDurationTimeViewModel : AddEditBaseViewModel, IDataErrorInfo {
        private DurationTime _durationTime;
        private DrivingLicenseCategory? _category;
        private CourseKind? _courseKind;
        private DateTime? _startDate;
        private int? _time;

        public AddEditDurationTimeViewModel() {
            Messenger.Default.Register<AddEditEntityMessage<DurationTime>>(this, HandleMessage);
        }

        private void HandleMessage(AddEditEntityMessage<DurationTime> message) {
            if(message.Entity != null) {
                if(message.Entity.GetType() != typeof(DurationTime))
                    return;

                _durationTime = (DurationTime)message.Entity;
                _category = _durationTime.Category;
                _courseKind = _durationTime.CourseKind;
                _startDate = _durationTime.StartDate;
                _time = _durationTime.Time;
            } else {
                _durationTime = new DurationTime();
                _category = null;
                _courseKind = null;
                _startDate = null;
                _time = null;
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

                case "CourseKind":
                    if(_courseKind == null)
                        validationMessage = "Pole nie może być puste!";
                    break;

                case "StartDate":
                    if(_startDate == null)
                        validationMessage = "Pole nie może być puste!";                    
                    break;

                case "Time":
                    if(_time == null)
                        validationMessage = "Pole nie może być puste!";
                    else if (_time <= 0)
                        validationMessage = "Wartość musi być większa od zera!";
                    break;
            }

            return validationMessage;
        }
        #endregion

        #region Save Methods
        public override bool Validate() {
            #region Required
            if (_category == null)
                return false;
            _durationTime.Category = (DrivingLicenseCategory) _category;

            if (_courseKind == null)
                return false;
            _durationTime.CourseKind = (CourseKind) _courseKind;

            if (_startDate == null)
                return false;
            _durationTime.StartDate = (DateTime) _startDate;

            if (_time == null || _time <= 0)
                return false;
            _durationTime.Time = (int) _time;
            #endregion

            return true;
        }

        public override int Save() {
            if(!Validate())
                return 1;

            using(var repository = new BaseRepository()) {
                repository.Save(_durationTime);
            }

            return 0;
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

        public int? Time {
            get { return _time; }
            set {
                _time = value;
                RaisePropertyChanged();
            }
        }

        public ObservableCollection<DrivingLicenseCategory> Categories => new ObservableCollection<DrivingLicenseCategory>(new EnumToList<DrivingLicenseCategory>().Enums);

        public ObservableCollection<CourseKind> CourseKinds => new ObservableCollection<CourseKind>(new EnumToList<CourseKind>().Enums);
        #endregion
    }
}