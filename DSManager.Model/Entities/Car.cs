﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DSManager.Model.Entities {
    public class Car : BaseEntity {
        #region Relations
        public virtual IList<CarPermissions> Permissions { get; set; }
        public virtual IList<ClassesDates> ClassesDates { get; set; }
        public virtual IList<ExamsDates> ExamsDates { get; set; }
        #endregion

        public virtual string Brand { get; set; }
        public virtual string Model { get; set; }
        public virtual string RegistrationNr { get; set; }
        public virtual decimal DistanceTraveled { get; set; }
        public virtual DateTime InspectionDate { get; set; }
        public virtual DateTime InsuranceDate { get; set; }
    }
}