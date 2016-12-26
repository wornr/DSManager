using DSManager.Model.Enums;

namespace DSManager.Model.Entities {
    public class ClassesDates : Dates<ClassesDates> {
        public virtual CourseKind CourseKind { get; set; }
        public virtual decimal? Distance { get; set; }
        public virtual bool IsAdditional { get; set; }
        public virtual decimal? Price { get; set; }
    }
}
