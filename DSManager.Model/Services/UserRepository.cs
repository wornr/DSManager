using System;
using System.Linq;
using System.Diagnostics;

using DSManager.Model.Entities;

namespace DSManager.Model.Services {
    public static class UserRepository {
        
        public static User GetUser(string login, string password) {
            using(var repository = new BaseRepository()) {
                try {
                    var user = repository.ToList<User>().Where(entity => entity.Login == login && entity.Password == password).ToList();

                    if(user.Count == 1)
                        return user.FirstOrDefault();
                } catch(Exception ex) {
                    Debug.WriteLine(ex.StackTrace, "ERROR");
                }
            }
            return null;
        }
    }
}
