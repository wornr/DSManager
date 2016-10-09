using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using DSManager.Model.Enums;

namespace DSManager.Model.Entities.Dictionaries {
    public class AccountPermissions {
        #region Composite Key
        public virtual AccountType AccountType { get; set; }
        public virtual string Permission { get; set; }
        #endregion

        public virtual string Description { get; set; }

        public override bool Equals(object obj) {
            return base.Equals(obj);
        }

        public override int GetHashCode() {
            return base.GetHashCode();
        }
    }
}
