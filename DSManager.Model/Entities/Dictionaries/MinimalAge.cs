using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using DSManager.Model.Enums;

namespace DSManager.Model.Entities.Dictionaries {
    public class MinimalAge {
        #region Composite Key
        public virtual DrivingLicenseCategory Category { get; set; }
        public virtual DateTime StartDate { get; set; }
        #endregion

        public virtual int Age { get; set; }

        public override bool Equals(object obj) {
            return base.Equals(obj);
        }

        public override int GetHashCode() {
            return base.GetHashCode();
        }
    }
}
