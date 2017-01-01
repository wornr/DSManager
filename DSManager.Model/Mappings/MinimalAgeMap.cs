using FluentNHibernate.Mapping;

using DSManager.Model.Entities;

namespace DSManager.Model.Mappings {
    class MinimalAgeMap : ClassMap<MinimalAge> {
        public MinimalAgeMap() {
            Id(x => x.Id);
            Map(x => x.Category).Not.Nullable();
            Map(x => x.StartDate).Not.Nullable();
            Map(x => x.Age).Not.Nullable();
        }
    }
}
