using EvernoteClone.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EvernoteClone.ViewModel.Helpers
{
    public class LocalAuthHelper
    {
        MsSqlDbProvider _dbProvider;

        public LocalAuthHelper()
        {
            _dbProvider = new MsSqlDbProvider();
        }

        public async Task<User> LoginAsync(User user)
        {
            return  (from c in await _dbProvider.GetAll<User>()
                    where c .Username == user.Username && c .Password == user.Password
                    select c).FirstOrDefault();
        }
    }
}
