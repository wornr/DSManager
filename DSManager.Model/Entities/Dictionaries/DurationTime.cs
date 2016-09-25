using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using DSManager.Model.Enums;

namespace DSManager.Model.Entities.Dictionaries {
    public class DurationTime {
        #region Composite Key
        public virtual DrivingLicenseCategory Category { get; set; }
        public virtual CourseKind CourseKind { get; set; }
        public virtual DateTime StartDate { get; set; }
        #endregion

        public virtual int Time { get; set; }
    }
}
