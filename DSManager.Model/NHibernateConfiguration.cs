using System.IO;
using System.Reflection;

using NHibernate;
using NHibernate.Dialect;
using NHibernate.Tool.hbm2ddl;

using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using FluentNHibernate.Conventions.Helpers;

using DSManager.Model.Entities;
using DSManager.Model.Entities.Dictionaries;
using DSManager.Model.Enums;

namespace DSManager.Model {
    public static class NHibernateConfiguration {
        private static ISessionFactory _sessionFactory;
        private static readonly string DbPath;

        static NHibernateConfiguration() {
            DbPath = Path.Combine(Directory.GetCurrentDirectory(), "database.sqlite");
            _sessionFactory = CreateSessionFactory(DbPath);
        }

        public static void Initialize() {
            GenerateAdminAndDefaultPermissions();
        }

        public static ISessionFactory SessionFactory => _sessionFactory ?? (_sessionFactory = CreateSessionFactory(DbPath));

        private static ISessionFactory CreateSessionFactory(string dbPath) {

            // MSSql
            /*return Fluently
                .Configure()
                .Database(
                    MsSqlConfiguration
                        .MsSql2012.Dialect<MsSql2012Dialect>()
                        .ConnectionString("Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=\"F:\\Moje dokumenty\\Studia\\Engineering\\DSManager_DB.mdf\";Integrated Security=True;Connect Timeout=30")
                        .DefaultSchema("dbo")
                        //.ShowSql
                )
                .Mappings(m => m.FluentMappings
                    .AddFromAssembly(Assembly.Load("DSManager.Model"))
                    .Conventions.Add(DefaultLazy.Never())
                )
                .ExposeConfiguration(BuildSchema)
                .BuildSessionFactory();*/

            // MySQL
            if (!Properties.Settings.Default.DeveloperMode)
                return Fluently
                    .Configure()
                    .Database(
                        MySQLConfiguration
                            .Standard
                            .Dialect<MySQL5Dialect>()
                            .ConnectionString(cs => cs
                                .Server(Properties.Settings.Default.DBHost)
                                .Database(Properties.Settings.Default.DBName)
                                .Username(Properties.Settings.Default.DBUser)
                                .Password(Properties.Settings.Default.DBPassword)
                            )
                            .ShowSql
                    )
                    .Mappings(m => m.FluentMappings
                            .AddFromAssembly(Assembly.Load("DSManager.Model"))
                            .Conventions.Add(DefaultLazy.Never())
                    )
                    .ExposeConfiguration(BuildMySQLSchema)
                    .BuildSessionFactory();

            // SQLite
            return Fluently.Configure()
                .Database(SQLiteConfiguration
                    .Standard
                    .UsingFile(dbPath)
                    //.ShowSql
                )
                .Mappings(m => m.FluentMappings
                    .AddFromAssembly(Assembly.Load("DSManager.Model"))
                    .Conventions.Add(DefaultLazy.Never())
                )
                .ExposeConfiguration(BuildSchema)
                .BuildSessionFactory();
        }

        // Method for MySQL
        private static void BuildMySQLSchema(NHibernate.Cfg.Configuration config) {
            new SchemaUpdate(config).Execute(false, true);
        }

        // Method for SQLite
        private static void BuildSchema(NHibernate.Cfg.Configuration config) {
            if (File.Exists(DbPath))
                return;

            new SchemaExport(config).Create(false, true);
        }

        public static ISession OpenSession() => SessionFactory.OpenSession();

        private static void GenerateAdminAndDefaultPermissions() {
            var session = _sessionFactory.OpenSession();
            if(session.QueryOver<User>().Where(x => x.Login == "admin").RowCount() != 0)
                return;

            var admin = new User {
                Login = "admin",
                Password = "21232f297a57a5a743894a0e4a801fc3", // md5(admin)
                AccountType = AccountType.Boss,
                Active = true
            };
            session.Save(admin);

            var permission = new AccountPermissions {
                AccountType = AccountType.Boss,
                Permission = "AgendaManagement",
                Description = "Uprawnienie do zarządzania terminarzem"
            };
            session.Save(permission);

            permission = new AccountPermissions {
                AccountType = AccountType.Boss,
                Permission = "CarsManagement",
                Description = "Uprawnienie do zarządzania flotą pojazdów"
            };
            session.Save(permission);

            permission = new AccountPermissions {
                AccountType = AccountType.Boss,
                Permission = "CoursesManagement",
                Description = "Uprawnienie do zarządzania kursami"
            };
            session.Save(permission);

            permission = new AccountPermissions {
                AccountType = AccountType.Boss,
                Permission = "InstructorsManagement",
                Description = "Uprawnienie do zarządzania instruktorami"
            };
            session.Save(permission);

            permission = new AccountPermissions {
                AccountType = AccountType.Boss,
                Permission = "StudentsManagement",
                Description = "Uprawnienie do zarządzania kursantami"
            };
            session.Save(permission);

            permission = new AccountPermissions {
                AccountType = AccountType.Boss,
                Permission = "UsersManagement",
                Description = "Uprawnienie do zarządzania użytkownikami"
            };
            session.Save(permission);

            permission = new AccountPermissions {
                AccountType = AccountType.Coordinator,
                Permission = "AgendaManagement",
                Description = "Uprawnienie do zarządzania terminarzem"
            };
            session.Save(permission);

            permission = new AccountPermissions {
                AccountType = AccountType.Coordinator,
                Permission = "CarsManagement",
                Description = "Uprawnienie do zarządzania flotą pojazdów"
            };
            session.Save(permission);

            permission = new AccountPermissions {
                AccountType = AccountType.Coordinator,
                Permission = "CoursesManagement",
                Description = "Uprawnienie do zarządzania kursami"
            };
            session.Save(permission);

            permission = new AccountPermissions {
                AccountType = AccountType.Coordinator,
                Permission = "InstructorsManagement",
                Description = "Uprawnienie do zarządzania instruktorami"
            };
            session.Save(permission);

            permission = new AccountPermissions {
                AccountType = AccountType.Coordinator,
                Permission = "StudentsManagement",
                Description = "Uprawnienie do zarządzania kursantami"
            };
            session.Save(permission);

            session.Flush();
        }
    }
}