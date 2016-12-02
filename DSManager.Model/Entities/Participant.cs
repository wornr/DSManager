using System;
using System.Collections.Generic;

namespace DSManager.Model.Entities {
    public class Participant : BaseEntity {
        #region Relations
        public virtual Student Student { get; set; }
        public virtual Instructor Instructor { get; set; }
        public virtual Course Course { get; set; }
        public virtual IList<Payment> Payments { get; set; }
        public virtual IList<ClassesDates> ClassesDates { get; set; }
        public virtual IList<ExamsDates> ExamsDates { get; set; }
        #endregion

        public virtual string KEOSNr { get; set; }
        public virtual string PKKNr { get; set; }
        public virtual decimal CoursePrice { get; set; }
        public virtual DateTime? EndDate { get; set; }
        public virtual string CertificateNr { get; set; }
        public virtual bool IsTheory { get; set; }
    }
}
