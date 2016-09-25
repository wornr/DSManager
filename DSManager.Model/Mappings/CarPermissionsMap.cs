using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using FluentNHibernate.Mapping;
using DSManager.Model.Entities;

namespace DSManager.Model.Mappings {
    class CarPermissionsMap : ClassMap<CarPermissions> {
        public CarPermissionsMap() {
            Id(x => x.Id);
            Map(x => x.Category);

            References(x => x.Car);
        }
    }
}
