using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using DSManager.Model.Enums;

namespace DSManager.Model.Entities {
    public class ClassesDates : Dates {
        public virtual DateTime EndDate { get; set; }
        public virtual decimal Distance { get; set; }
    }
}
