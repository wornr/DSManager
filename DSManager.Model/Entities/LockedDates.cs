namespace DSManager.Model.Entities {
    public class LockedDates : Dates<LockedDates> {
        public virtual string Description { get; set; }
    }
}
