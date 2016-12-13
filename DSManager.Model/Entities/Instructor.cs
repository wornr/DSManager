using System.Collections.Generic;

namespace DSManager.Model.Entities {
    public class Instructor : Person<Instructor> {
        #region Relations
        public virtual User User { get; set; }
        public virtual IList<Participant> Participants { get; set; }
        public virtual IList<InstructorPermissions> Permissions { get; set; }
        public virtual IList<ClassesDates> ClassesDates { get; set; }
        public virtual IList<ExamsDates> ExamsDates { get; set; }
        #endregion

        public virtual string PermissionsNr { get; set; }
    }
}
