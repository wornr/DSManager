using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using DSManager.Model.Enums;

namespace DSManager.Model.Entities {
    public class Dates : BaseEntity {
        #region Relations
        public virtual Participant Participant { get; set; }
        public virtual Instructor Instructor { get; set; }
        public virtual Car Car { get; set; }
        #endregion

        public virtual CourseKind CourseKind { get; set; }
        public virtual DateTime StartDate { get; set; }
    }
}
