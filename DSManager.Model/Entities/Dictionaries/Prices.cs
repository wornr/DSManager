﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using DSManager.Model.Enums;

namespace DSManager.Model.Entities.Dictionaries {
    public class Prices {
        #region Composite Key
        public virtual DrivingLicenseCategory Category { get; set; }
        public virtual CourseType CourseType { get; set; }
        public virtual DateTime StartDate { get; set; }
        #endregion

        public virtual DateTime EndDate { get; set; }
        public virtual decimal Price { get; set; }

        public override bool Equals(object obj) {
            return base.Equals(obj);
        }

        public override int GetHashCode() {
            return base.GetHashCode();
        }
    }
}
