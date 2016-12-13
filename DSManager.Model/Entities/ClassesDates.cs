using System;

namespace DSManager.Model.Entities {
    public class ClassesDates : Dates<ClassesDates> {
        public virtual DateTime? EndDate { get; set; }
        public virtual decimal? Distance { get; set; }
    }
}
