﻿using FluentNHibernate.Mapping;

using DSManager.Model.Entities;

namespace DSManager.Model.Mappings {
    class CarPermissionsMap : ClassMap<CarPermissions> {
        public CarPermissionsMap() {
            Id(x => x.Id);
            Map(x => x.Category).Not.Nullable();

            References(x => x.Car).Nullable();
        }
    }
}
