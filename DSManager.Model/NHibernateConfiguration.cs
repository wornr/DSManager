using System.IO;
using System.Reflection;

using NHibernate;
using NHibernate.Tool.hbm2ddl;

using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;

using DSManager.Model.Entities;
using DSManager.Model.Enums;
using FluentNHibernate.Conventions.Helpers;

namespace DSManager.Model {
    public static class NHibernateConfiguration {
        private static ISessionFactory _sessionFactory;
        private static readonly string DbPath;
        private static bool _createAdmin;

        static NHibernateConfiguration() {
            DbPath = Path.Combine(Directory.GetCurrentDirectory(), "database.sqlite");
            _sessionFactory = CreateSessionFactory(DbPath);
        }

        public static void Initialize() {
            if (!_createAdmin) return;
            using(var session = _sessionFactory.OpenSession()) {
                var admin = new User {
                    Login = "admin",
                    Password = "21232f297a57a5a743894a0e4a801fc3", // md5(admin)
                    AccountType = AccountType.Boss,
                    Active = true
                };
                session.Save(admin);
            }
        }

        public static ISessionFactory SessionFactory => _sessionFactory ?? (_sessionFactory = CreateSessionFactory(DbPath));

        private static ISessionFactory CreateSessionFactory(string dbPath) {

            return Fluently.Configure()
                .Database(SQLiteConfiguration.Standard.UsingFile(dbPath))
                .Mappings(m => m.FluentMappings
                            .AddFromAssembly(Assembly.Load("DSManager.Model"))
                            .Conventions.Add(DefaultLazy.Never())
                         )
                .ExposeConfiguration(BuildSchema)
                .BuildSessionFactory();
        }

        private static void BuildSchema(NHibernate.Cfg.Configuration config) {
            if (File.Exists(DbPath)) return;
            new SchemaExport(config).Create(false, true);
            _createAdmin = true;
        }

        public static ISession OpenSession() => SessionFactory.OpenSession();
    }
}