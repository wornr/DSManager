using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using FluentNHibernate.Mapping;
using DSManager.Model.Entities.Dictionaries;

namespace DSManager.Model.Mappings {
    class DurationTimeMap :ClassMap<DurationTime> {
        public DurationTimeMap() {
            CompositeId().KeyProperty(x => x.Category)
                .KeyProperty(x => x.CourseKind)
                .KeyProperty(x => x.StartDate);
            Map(x => x.Time).Not.Nullable();
        }
    }
}
