using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using FluentNHibernate.Mapping;

using DSManager.Model.Entities;

namespace DSManager.Model.Mappings {
    class ExamsDatesMap : ClassMap<ExamsDates> {
        public ExamsDatesMap() {
            Id(x => x.Id);
            Map(x => x.CourseKind).Not.Nullable();
            Map(x => x.StartDate).Not.Nullable();
            Map(x => x.IsPassed).Nullable();

            References(x => x.Participant);
            References(x => x.Instructor);
            References(x => x.Car);
        }
    }
}
