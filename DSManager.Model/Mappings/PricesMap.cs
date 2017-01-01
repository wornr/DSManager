using FluentNHibernate.Mapping;

using DSManager.Model.Entities;

namespace DSManager.Model.Mappings {
    class PricesMap : ClassMap<Prices> {
        public PricesMap() {
            Id(x => x.Id);
            Map(x => x.Category).Not.Nullable();
            Map(x => x.CourseType).Not.Nullable();
            Map(x => x.CourseKind).Not.Nullable();
            Map(x => x.StartDate).Not.Nullable();
            Map(x => x.EndDate).Nullable();
            Map(x => x.Price).Not.Nullable();
        }
    }
}
