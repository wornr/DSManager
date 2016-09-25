using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DSManager.Model.Entities {
    public class Payment : BaseEntity {
        #region Relations
        public virtual Participant Participant { get; set; }
        #endregion

        public virtual decimal Amount { get; set; }
        public virtual string PaymentNr { get; set; }
        public virtual DateTime Date { get; set; }
    }
}
