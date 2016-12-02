using System;

namespace DSManager.Model.Entities {
    public class ClassesDates : Dates {
        public virtual DateTime? EndDate { get; set; }
        public virtual decimal? Distance { get; set; }
    }
}
