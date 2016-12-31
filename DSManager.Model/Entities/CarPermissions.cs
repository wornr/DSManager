using DSManager.Model.Enums;

namespace DSManager.Model.Entities {
    public class CarPermissions : BaseEntity<CarPermissions> {
        #region Relations
        public virtual Car Car { get; set; }
        #endregion

        public virtual DrivingLicenseCategory Category { get; set; }
    }
}
