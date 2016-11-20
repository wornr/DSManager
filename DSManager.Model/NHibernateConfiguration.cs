using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

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
        private static readonly string _dbPath;
        private static bool _createAdmin = false;

        static NHibernateConfiguration() {
            _dbPath = Path.Combine(Directory.GetCurrentDirectory(), "database.sqlite");
            _sessionFactory = CreateSessionFactory(_dbPath);
        }

        public static void Initialize() {
            if(_createAdmin) {
                using(var session = _sessionFactory.OpenSession()) {
                    User admin = new User();
                    admin.Login = "admin";
                    admin.Password = "21232f297a57a5a743894a0e4a801fc3"; // md5(admin)
                    admin.AccountType = AccountType.Boss;
                    admin.Active = true;
                    session.Save(admin);
                }
            }
        }

        public static ISessionFactory SessionFactory {
            get { return _sessionFactory ?? (_sessionFactory = CreateSessionFactory(_dbPath)); }
        }

        private static ISessionFactory CreateSessionFactory(string _dbPath) {

            return Fluently.Configure()
                .Database(SQLiteConfiguration.Standard.UsingFile(_dbPath))
                .Mappings(m => m.FluentMappings
                            .AddFromAssemblyOf<BaseEntity>()
                            .Conventions.Add(DefaultLazy.Never())
                         )
                .ExposeConfiguration(BuildSchema)
                .BuildSessionFactory();
        }

        private static void BuildSchema(NHibernate.Cfg.Configuration config) {
            if(!File.Exists(_dbPath)) {
                new SchemaExport(config)
                    .Create(false, true);
                _createAdmin = true;
            }
        }

        public static ISession OpenSession() {
            return SessionFactory.OpenSession();
        }
    }
}