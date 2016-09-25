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
            Map(x => x.Brand);
            Map(x => x.Model);
            Map(x => x.RegistrationNr);
            Map(x => x.DistanceTraveled);
            Map(x => x.InspectionDate);
            Map(x => x.InsuranceDate);

            HasMany(x => x.Permissions);
            HasMany(x => x.ClassesDates);
            HasMany(x => x.ExamsDates);
        }
    }
}
