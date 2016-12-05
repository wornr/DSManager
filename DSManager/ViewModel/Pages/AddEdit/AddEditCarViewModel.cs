using System;
using System.ComponentModel;
using System.Text.RegularExpressions;

using GalaSoft.MvvmLight.Messaging;

using DSManager.Messengers;
using DSManager.Model.Entities;
using DSManager.Model.Services;

namespace DSManager.ViewModel.Pages.AddEdit {
    public class AddEditCarViewModel : AddEditBaseViewModel, IDataErrorInfo {
        private Car _car;
        private decimal? _distanceTraveled;
        private DateTime? _inspectionDate;
        private DateTime? _insuranceDate;

        public AddEditCarViewModel() {
            Messenger.Default.Register<AddEditEntityMessage>(this, HandleMessage);
        }

        private void HandleMessage(AddEditEntityMessage message) {
            if(message.Entity != null) {
                _car = (Car)message.Entity;
                _distanceTraveled = _car.DistanceTraveled;
                _inspectionDate = _car.InspectionDate;
                _insuranceDate = _car.InsuranceDate;
            } else {
                _car = new Car();
                _distanceTraveled = null;
                _inspectionDate = null;
                _insuranceDate = null;
            }
        }

        #region IDataErrorInfo Methods
        public string this[string columnName] => Validate(columnName);

        public string Error => "Błąd";

        private string Validate(string propertyName) {
            var validationMessage = string.Empty;

            switch(propertyName) {
                case "Brand":
                    if(string.IsNullOrEmpty(_car.Brand))
                        validationMessage = "Pole nie może być puste!";
                    else if(!Regex.IsMatch(_car.Brand, @"^[a-ząćęłńóśźżA-ZĄĆĘŁŃÓŚŹŻ]+$"))
                        validationMessage = "Podano niepoprawną markę pojazdu!";
                    break;

                case "Model":
                    if(string.IsNullOrEmpty(_car.Model))
                        validationMessage = "Pole nie może być puste!";
                    else if(!Regex.IsMatch(_car.Model, @"^[0-9a-ząćęłńóśźżA-ZĄĆĘŁŃÓŚŹŻ ]+$"))
                        validationMessage = "Podano niepoprawny model pojazdu!";
                    break;

                case "RegistrationNr":
                    if(string.IsNullOrEmpty(_car.RegistrationNr))
                        validationMessage = "Pole nie może być puste!";
                    else if(!Regex.IsMatch(_car.RegistrationNr, @"^[0-9a-ząćęłńóśźżA-ZĄĆĘŁŃÓŚŹŻ ]+$"))
                        validationMessage = "Podano niepoprawny numer rejestracyjny!";
                    break;

                case "DistanceTraveled":
                    if(_distanceTraveled == null)
                        validationMessage = "Pole nie może być puste!";
                    else if(_distanceTraveled < 0m || _distanceTraveled > 999999.99m)
                        validationMessage = "Podano przebieg spoza zakresu 0 - 999999,99";
                    break;
                
                case "InspectionDate":
                    if(_inspectionDate == null)
                        validationMessage = "Pole nie może być puste!";
                    break;

                case "InsuranceDate":
                    if(_insuranceDate == null)
                        validationMessage = "Pole nie może być puste!";
                    break;

                // TODO dodać walidację uprawnień pojazdu
            }

            return validationMessage;
        }
        #endregion

        #region Save Methods
        public override bool Validate() {
            #region Required
            if (_car.Brand == null || !Regex.IsMatch(_car.Brand, @"^[a-ząćęłńóśźżA-ZĄĆĘŁŃÓŚŹŻ]+$"))
                return false;

            if (_car.Model == null || !Regex.IsMatch(_car.Model, @"^[0-9a-ząćęłńóśźżA-ZĄĆĘŁŃÓŚŹŻ ]+$"))
                return false;

            if (_car.RegistrationNr == null || !Regex.IsMatch(_car.RegistrationNr, @"^[0-9a-ząćęłńóśźżA-ZĄĆĘŁŃÓŚŹŻ ]+$"))
                return false;

            if (_distanceTraveled == null)
                return false;

            _car.DistanceTraveled = (decimal) _distanceTraveled;

            if (_inspectionDate == null)
                return false;
            _car.InspectionDate = (DateTime) _inspectionDate;

            if (_insuranceDate == null)
                return false;
            _car.InsuranceDate = (DateTime) _insuranceDate;

            // TODO dodać walidację uprawnień pojazdu
            #endregion

            return true;
        }

        public override bool Save() {
            if(!Validate())
                return false;

            using(var repository = new BaseRepository()) {
                repository.Save(_car);
            }

            return true;
        }
        #endregion

        #region Properties
        public string Brand {
            get { return _car.Brand; }
            set {
                _car.Brand = value;
                RaisePropertyChanged();
            }
        }

        public string Model {
            get { return _car.Model; }
            set {
                _car.Model = value;
                RaisePropertyChanged();
            }
        }

        public string RegistrationNr {
            get { return _car.RegistrationNr; }
            set {
                _car.RegistrationNr = value;
                RaisePropertyChanged();
            }
        }

        public decimal? DistanceTraveled {
            get { return _distanceTraveled; }
            set {
                _distanceTraveled = value;
                RaisePropertyChanged();
            }
        }

        public DateTime? InspectionDate {
            get { return _inspectionDate; }
            set {
                _inspectionDate = value;
                RaisePropertyChanged();
            }
        }

        public DateTime? InsuranceDate {
            get { return _insuranceDate; }
            set {
                _insuranceDate = value;
                RaisePropertyChanged();
            }
        }

        // TODO dodać właściwości odpowiedzialne za uprawnienia pojazdu (dostępne/wybrane)
        #endregion
    }
}