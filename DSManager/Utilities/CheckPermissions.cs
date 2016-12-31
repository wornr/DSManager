using System.Linq;

using DSManager.Model.Entities.Dictionaries;
using DSManager.Model.Enums;
using DSManager.Model.Services;

namespace DSManager.Utilities {
    public class CheckPermissions {
        public static bool CheckPermission(AccountType accountType, string permissionName) {
            using (var repository = new BaseRepository()) {
                return repository.ToList<AccountPermissions>().Count(x => x.AccountType == accountType && x.Permission == permissionName) != 0;
            }
        }
    }
}