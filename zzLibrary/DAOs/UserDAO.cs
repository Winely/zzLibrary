using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ZZLibModel;
using System.Threading.Tasks;
using System.Data.Entity;

namespace zzLibrary.DAOs
{
    public class UserDAO : BaseDAO<user>
    {
        public async Task<user> GetByToken(string token)
        {
            return await FindAsync(u=>u.token==token);
        }

        public async Task<ICollection<UserMsg>> GetAllUser()
        {
            return await db.user.Select(u => new UserMsg(u)).ToListAsync();
        }

    }

    public class UserMsg
    {
        /// <summary>
        /// 脱去密码的user类
        /// </summary>
        /// <param name="u"></param>
        public UserMsg(user u)
        {
            user1 = u.user1;
            duration = u.duration;
            isadmin = u.isadmin;
            token = u.token;
        }
        public string user1 { get; set; }
        public sbyte duration { get; set; }
        public bool isadmin { get; set; }
        public string token { get; set; }
    }
}