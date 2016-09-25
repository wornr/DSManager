using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using FluentNHibernate.Mapping;
using DSManager.Model.Entities;

namespace DSManager.Model.Mappings {
    class ClassesDatesMap : ClassMap<ClassesDates> {
        public ClassesDatesMap() {
            Id(x => x.Id);
            Map(x => x.CourseKind);
            Map(x => x.StartDate);
            Map(x => x.EndDate);
            Map(x => x.Distance);

            References(x => x.Participant);
            References(x => x.Instructor);
            References(x => x.Car);
        }
    }
}
