using DSManager.Model.Enums;

namespace DSManager.Model.Entities {
    public class InstructorPermissions : BaseEntity<InstructorPermissions> {
        #region Relations
        public virtual Instructor Instructor { get; set; }
        #endregion

        public virtual DrivingLicenseCategory Category { get; set; }
    }
}
