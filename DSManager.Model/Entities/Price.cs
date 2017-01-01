using System;

using DSManager.Model.Enums;

namespace DSManager.Model.Entities {
    public class Prices : BaseEntity<Prices> {
        #region Unique
        public virtual DrivingLicenseCategory Category { get; set; }
        public virtual CourseType CourseType { get; set; }
        public virtual CourseKind CourseKind { get; set; }
        public virtual DateTime StartDate { get; set; }
        #endregion

        public virtual DateTime? EndDate { get; set; }
        public virtual decimal Price { get; set; }
    }
}
