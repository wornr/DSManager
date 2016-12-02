using System;
using System.Linq;

namespace DSManager.Model.Services.Interfaces {
    public interface IRepository {
        void Save(object obj);
        void Delete(object obj);
        object GetById(Type objType, object objId);
        IQueryable<TEntity> ToList<TEntity>();
    }
}
