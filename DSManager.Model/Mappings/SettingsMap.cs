using DSManager.Model.Entities.Dictionaries;
using FluentNHibernate.Mapping;

namespace DSManager.Model.Mappings {
    class SettingsMap : ClassMap<Settings> {
        public SettingsMap() {
            Id(x => x.Id);
            Map(x => x.SmtpHost).Nullable();
            Map(x => x.SmtpPort).Nullable();
            Map(x => x.SmtpSsl).Nullable();
            Map(x => x.SmtpUsername).Nullable();
            Map(x => x.SmtpPassword).Nullable();
            Map(x => x.SmtpMail).Nullable();
            Map(x => x.IsNotificationEnabled);
        }
    }
}