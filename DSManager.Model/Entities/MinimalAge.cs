using System;

using DSManager.Model.Enums;

namespace DSManager.Model.Entities {
    public class MinimalAge : BaseEntity<MinimalAge> {
        #region Unique
        public virtual DrivingLicenseCategory Category { get; set; }
        public virtual DateTime StartDate { get; set; }
        #endregion

        public virtual int Age { get; set; }
    }
}
