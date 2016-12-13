using System;
using System.Collections.Generic;

using DSManager.Model.Enums;

namespace DSManager.Model.Entities {
    public class Course : BaseEntity<Course> {
        #region Relations
        public virtual IList<Participant> Participants { get; set; }
        #endregion

        public virtual DrivingLicenseCategory Category { get; set; }
        public virtual CourseType CourseType { get; set; }
        public virtual DateTime StartDate { get; set; }
    }
}
