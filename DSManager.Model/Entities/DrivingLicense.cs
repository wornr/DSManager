using System;
using System.Collections.Generic;

namespace DSManager.Model.Entities {
    public class DrivingLicense : BaseEntity {
        #region Relations
        public virtual IList<DrivingLicensePermissions> DrivingLicensePermissions { get; set; }
        public virtual Student Student { get; set; }
        #endregion

        public virtual DateTime IssueDate { get; set; }
        public virtual string DrivingLicenseNr { get; set; }
    }
}
