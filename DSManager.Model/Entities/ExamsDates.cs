namespace DSManager.Model.Entities {
    public class ExamsDates : Dates<ExamsDates> {
        public virtual bool? IsPassed { get; set; }
    }
}
