using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using DSManager.Model.Enums;

namespace DSManager.Model.Entities.Dictionaries {
    public class MinimalAge {
        public virtual DrivingLicenseCategory Category { get; set; }
        public virtual DateTime StartDate { get; set; }
        public virtual int Age { get; set; }
    }
}
