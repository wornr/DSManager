using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using NHibernate;
using NHibernate.Tool.hbm2ddl;
using DSManager.Model.Entities;

namespace DSManager.Model {
    public class NHibernateConfiguration {
        private ISessionFactory _sessionFactory;
        private readonly string _dbPath;

        public NHibernateConfiguration() {
            _dbPath = Path.Combine(Directory.GetCurrentDirectory(), "database.sqlite");
            _sessionFactory = CreateSessionFactory(_dbPath);
        }

        public ISessionFactory SessionFactory {
            get { return _sessionFactory ?? (_sessionFactory = CreateSessionFactory(_dbPath)); }
        }

        private ISessionFactory CreateSessionFactory(string _dbPath) {
            Assembly assembly = Assembly.GetExecutingAssembly();

            return Fluently.Configure()
                .Database(SQLiteConfiguration.Standard.UsingFile(_dbPath))
                .Mappings(m => m.FluentMappings.AddFromAssemblyOf<BaseEntity>())
                .ExposeConfiguration(BuildSchema)
                .BuildSessionFactory();
        }

        private void BuildSchema(NHibernate.Cfg.Configuration config) {
            if(!File.Exists(_dbPath))
                    new SchemaExport(config)
                        .Create(false, true);
        }
    }
}