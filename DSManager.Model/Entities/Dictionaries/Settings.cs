namespace DSManager.Model.Entities.Dictionaries {
    public class Settings : BaseEntity<Settings> {
        public virtual string SmtpHost { get; set; }
        public virtual int SmtpPort { get; set; }
        public virtual bool SmtpSsl { get; set; }
        public virtual string SmtpUsername { get; set; }
        public virtual string SmtpPassword { get; set; }
        public virtual string SmtpMail { get; set; }
        public virtual bool IsNotificationEnabled { get; set; }
    }
}
