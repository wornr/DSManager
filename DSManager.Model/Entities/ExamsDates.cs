using DSManager.Model.Enums;

namespace DSManager.Model.Entities {
    public class ExamsDates : Dates<ExamsDates> {
        public virtual CourseKind CourseKind { get; set; }
        public virtual bool? IsPassed { get; set; }
    }
}
