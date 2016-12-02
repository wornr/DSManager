using DSManager.Model.Enums;

namespace DSManager.Model.Entities {
    public class User : BaseEntity {
        #region Relations
        public virtual Instructor Instructor { get; set; }
        public virtual Student Student { get; set; }
        #endregion

        public virtual string Login { get; set; }
        public virtual string Password { get; set; }
        public virtual string Email { get; set; }
        public virtual string FirstName { get; set; }
        public virtual string LastName { get; set; }
        public virtual AccountType AccountType { get; set; }
        public virtual bool Active { get; set; }
    }
}
