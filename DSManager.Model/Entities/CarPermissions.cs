using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using DSManager.Model.Enums;

namespace DSManager.Model.Entities {
    public class CarPermissions : BaseEntity {
        #region Relations
        public virtual Car Car { get; set; }
        #endregion

        public virtual DrivingLicenseCategory Category { get; set; }
    }
}
