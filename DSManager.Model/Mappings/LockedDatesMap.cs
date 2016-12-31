using FluentNHibernate.Mapping;

using DSManager.Model.Entities;

namespace DSManager.Model.Mappings {
    class LockedDatesMap : ClassMap<LockedDates> {
        public LockedDatesMap() {
            Id(x => x.Id);
            Map(x => x.StartDate).Not.Nullable();
            Map(x => x.EndDate).Not.Nullable();
            Map(x => x.Description).Not.Nullable();

            References(x => x.Participant).Nullable();
            References(x => x.Instructor).Nullable();
            References(x => x.Car).Nullable();
        }
    }
}
