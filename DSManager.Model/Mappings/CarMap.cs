using FluentNHibernate.Mapping;

using DSManager.Model.Entities;

namespace DSManager.Model.Mappings {
    class CarMap : ClassMap<Car> {
        public CarMap() {
            Id(x => x.Id);
            Map(x => x.Brand).Not.Nullable().Length(25);
            Map(x => x.Model).Not.Nullable().Length(25);
            Map(x => x.RegistrationNr).Not.Nullable().Length(10);
            Map(x => x.DistanceTraveled).Not.Nullable().Precision(2);
            Map(x => x.InspectionDate).Not.Nullable();
            Map(x => x.InsuranceDate).Not.Nullable();

            HasMany(x => x.Permissions).Cascade.All();
            HasMany(x => x.ClassesDates);
            HasMany(x => x.ExamsDates);
        }
    }
}
