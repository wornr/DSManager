using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DSManager.Model.Entities {
    public class Student : Person {
        #region Relations
        public virtual DrivingLicense DrivingLicense { get; set; }
        public virtual IList<Participant> Participants { get; set; }
        #endregion
    }
}
