using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Diagnostics;

using DSManager.Model.Entities;

namespace DSManager.Model.Services {
    public static class UserRepository {
        
        public static User getUser(string login, string password) {
            IQueryable<User> user = null;
            using(BaseRepository repository = new BaseRepository()) {
                try {
                    user = repository.ToList<User>().Where(entity => entity.Login == login && entity.Password == password);

                    if(user != null && user.Count() == 1)
                        return user.FirstOrDefault();
                } catch(Exception ex) {
                    Debug.WriteLine(ex.StackTrace, "ERROR");
                }
            }
            return null;
        }
    }
}
