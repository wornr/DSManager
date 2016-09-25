using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using DSManager.Model.Enums;

namespace DSManager.Model.Entities {
    public class ExamsDates : Dates {
        public virtual bool IsPassed { get; set; }
    }
}
