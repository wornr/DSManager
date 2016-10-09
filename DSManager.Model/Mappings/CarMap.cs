using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using FluentNHibernate.Mapping;

using DSManager.Model.Entities;

namespace DSManager.Model.Mappings {
    class CarMap : ClassMap<Car> {
        public CarMap() {
            Id(x => x.Id);
            Map(x => x.Brand).Not.Nullable().Length(25);
            Map(x => x.Model).Not.Nullable().Length(25);
            Map(x => x.RegistrationNr).Not.Nullable().Length(10);
            Map(x => x.DistanceTraveled).Nullable().Precision(2);
            Map(x => x.InspectionDate).Nullable();
            Map(x => x.InsuranceDate).Nullable();

            HasMany(x => x.Permissions);
            HasMany(x => x.ClassesDates);
            HasMany(x => x.ExamsDates);
        }
    }
}
