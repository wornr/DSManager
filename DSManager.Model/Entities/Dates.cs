using System;

using DSManager.Model.Enums;

namespace DSManager.Model.Entities {
    public class Dates<T> : BaseEntity<T> where T : BaseEntity<T> {
        #region Relations
        public virtual Participant Participant { get; set; }
        public virtual Instructor Instructor { get; set; }
        public virtual Car Car { get; set; }
        #endregion

        public virtual CourseKind CourseKind { get; set; }
        public virtual DateTime StartDate { get; set; }
    }
}
