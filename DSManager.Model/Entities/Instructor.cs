using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DSManager.Model.Entities {
    public class Instructor : Person {
        #region Relations
        public virtual IList<Participant> Participants { get; set; }
        public virtual IList<InstructorPermissions> Permissions { get; set; }
        public virtual IList<ClassesDates> ClassesDates { get; set; }
        public virtual IList<ExamsDates> ExamsDates { get; set; }
        #endregion

        public virtual string PermissionsNr { get; set; }
    }
}
