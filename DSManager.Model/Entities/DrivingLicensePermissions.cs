using DSManager.Model.Enums;

namespace DSManager.Model.Entities {
    public class DrivingLicensePermissions : BaseEntity<DrivingLicensePermissions> {
        #region Relations
        public virtual DrivingLicense DrivingLicense { get; set; }
        #endregion

        public virtual DrivingLicenseCategory Category { get; set; }
    }
}
