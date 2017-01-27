using System;
using System.ComponentModel;
using System.Linq;

using GalaSoft.MvvmLight.Messaging;

using NHibernate.Util;

using DSManager.Messengers;
using DSManager.Model.Entities;
using DSManager.Model.Services;

namespace DSManager.ViewModel.Pages.AddEdit {
    public class AddEditPaymentViewModel : AddEditBaseViewModel, IDataErrorInfo {
        private Payment _payment;
        private DateTime? _paymentDate;
        private decimal? _amount;
        private decimal _overdueAmount;

        public AddEditPaymentViewModel() {
            Messenger.Default.Register<AddEditPaymentMessage<Payment>>(this, HandleMessage);
        }

        private void HandleMessage(AddEditPaymentMessage<Payment> message) {
            using(var repository = new BaseRepository()) {
                _overdueAmount = message.Participant.CoursePrice;
                repository.ToList<Payment>().Where(x => x.Participant == message.Participant).ForEach(x => _overdueAmount = decimal.Subtract(_overdueAmount, x.Amount));
            }

            if(message.Entity != null) {
                if(message.Entity.GetType() != typeof(Payment))
                    return;

                _payment = (Payment)message.Entity;
                _paymentDate = _payment.PaymentDate;
                _amount = _payment.Amount;
                _overdueAmount = decimal.Add(_overdueAmount, _payment.Amount);
            } else {
                _payment = new Payment();
                _paymentDate = null;
                _amount = null;
            }

            _payment.Participant = message.Participant;

            
        }

        #region IDataErrorInfo Methods
        public string this[string columnName] => Validate(columnName);

        public string Error => "Błąd";

        private string Validate(string propertyName) {
            var validationMessage = string.Empty;

            switch(propertyName) {
                case "PaymentNr":
                    if(string.IsNullOrEmpty(_payment.PaymentNr))
                        validationMessage = "Pole nie może być puste!";
                    break;

                case "PaymentDate":
                    if(_paymentDate == null)
                        validationMessage = "Pole nie może być puste!";
                    break;

                case "Amount":
                    if(_amount == null)
                        validationMessage = "Pole nie może być puste!";
                    else if(_amount <= 0m)
                        validationMessage = "Kwota musi być większa od zera!";
                    else if (_amount > _overdueAmount)
                        validationMessage = "Kwota musi być mniejsza od zaległości!";
                    break;
            }

            return validationMessage;
        }
        #endregion

        #region Save Methods
        public override bool Validate() {
            #region Required
            if(_payment.PaymentNr == null)
                return false;

            if(_paymentDate == null)
                return false;
            _payment.PaymentDate = (DateTime) _paymentDate;
            
            if(_amount == null || _amount < 0m)
                return false;
            _payment.Amount = (decimal) _amount;
            #endregion

            return true;
        }

        public override int Save() {
            if(!Validate())
                return 1;

            using(var repository = new BaseRepository()) {
                repository.Save(_payment);
            }

            return 0;
        }
        #endregion

        #region ViewElements
        public string PaymentNr {
            get { return _payment.PaymentNr; }
            set {
                _payment.PaymentNr = value;
                RaisePropertyChanged();
            }
        }

        public DateTime? PaymentDate {
            get { return _paymentDate; }
            set {
                _paymentDate = value;
                RaisePropertyChanged();
            }
        }

        public decimal? Amount {
            get { return _amount; }
            set {
                _amount = value;
                RaisePropertyChanged();
            }
        }
        #endregion
    }
}