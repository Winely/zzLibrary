using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ZZLibModel;
using System.Threading.Tasks;
using System.Data.Entity;

namespace zzLibrary.DAOs
{
    /// <summary>
    /// 用户相关的DAO操作
    /// </summary>
    public class UserDAO : BaseDAO<user>
    {
        /// <summary>
        /// 获取token代表的用户信息
        /// </summary>
        /// <param name="token">用户token</param>
        /// <returns>一个用户信息（除密码）</returns>
        public async Task<user> GetByToken(string token)
        {
            return await FindAsync(u => u.token == token);
        }

        /// <summary>
        /// 获取所有用户
        /// </summary>
        /// <returns>全体用户列表</returns>
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
        public UserMsg() { }

        /// <summary>
        /// 用户名
        /// </summary>
        public string user1 { get; set; }

        /// <summary>
        /// 可借书时长
        /// </summary>
        public sbyte duration { get; set; }

        /// <summary>
        /// 是否管理员
        /// </summary>
        public bool isadmin { get; set; }

        /// <summary>
        /// 用户token
        /// </summary>
        public string token { get; set; }
    }

    public class UserMsgPlus : UserMsg
    {
        public UserMsgPlus() { }
        public UserMsgPlus(user usr, UserCredit info)
        {
            user1 = usr.user1;
            token = usr.token;
            isadmin = usr.isadmin;
            duration = usr.duration;
            available = info.available;
            dated = info.dated;
        }

        public int available { get; set; }
        public int dated { get; set; }
    }
}