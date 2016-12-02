using System.Collections.Generic;

namespace DSManager.Model.Entities {
    public class Student : Person {
        #region Relations
        public virtual DrivingLicense DrivingLicense { get; set; }
        public virtual User User { get; set; }
        public virtual IList<Participant> Participants { get; set; }
        #endregion
    }
}
