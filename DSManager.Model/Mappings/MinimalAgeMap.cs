using FluentNHibernate.Mapping;

using DSManager.Model.Entities.Dictionaries;

namespace DSManager.Model.Mappings {
    class MinimalAgeMap : ClassMap<MinimalAge> {
        public MinimalAgeMap() {
            CompositeId().KeyProperty(x => x.Category)
                .KeyProperty(x => x.StartDate);
            Map(x => x.Age).Not.Nullable();
        }
    }
}
