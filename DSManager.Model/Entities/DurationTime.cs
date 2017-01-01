using System;

using DSManager.Model.Enums;

namespace DSManager.Model.Entities {
    public class DurationTime : BaseEntity<DurationTime> {
        #region Unique
        public virtual DrivingLicenseCategory Category { get; set; }
        public virtual CourseKind CourseKind { get; set; }
        public virtual DateTime StartDate { get; set; }
        #endregion

        public virtual int Time { get; set; }
    }
}
