using System;

namespace DSManager.Model.Entities {
    public class Payment : BaseEntity<Payment> {
        #region Relations
        public virtual Participant Participant { get; set; }
        #endregion

        public virtual decimal Amount { get; set; }
        public virtual string PaymentNr { get; set; }
        public virtual DateTime PaymentDate { get; set; }
    }
}
