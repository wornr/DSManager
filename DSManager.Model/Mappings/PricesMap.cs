using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using FluentNHibernate.Mapping;

using DSManager.Model.Entities.Dictionaries;

namespace DSManager.Model.Mappings {
    class PricesMap : ClassMap<Prices> {
        public PricesMap() {
            CompositeId().KeyProperty(x => x.Category)
                .KeyProperty(x => x.CourseType)
                .KeyProperty(x => x.StartDate);
            Map(x => x.EndDate);
            Map(x => x.Price).Not.Nullable();
        }
    }
}
