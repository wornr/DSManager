using System;

using DSManager.Model.Enums;
// ReSharper disable RedundantOverriddenMember
// ReSharper disable BaseObjectEqualsIsObjectEquals
// ReSharper disable BaseObjectGetHashCodeCallInGetHashCode

namespace DSManager.Model.Entities.Dictionaries {
    public class Prices {
        #region Composite Key
        public virtual DrivingLicenseCategory Category { get; set; }
        public virtual CourseType CourseType { get; set; }
        public virtual DateTime StartDate { get; set; }
        #endregion

        public virtual DateTime? EndDate { get; set; }
        public virtual decimal Price { get; set; }

        public override bool Equals(object obj) {
            return base.Equals(obj);
        }

        public override int GetHashCode() {
            return base.GetHashCode();
        }
    }
}
