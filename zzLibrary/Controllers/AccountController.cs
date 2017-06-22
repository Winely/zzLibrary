using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using ZZLibModel;
using zzLibrary.DAOs;

namespace zzLibrary.Controllers
{
    /// <summary>
    /// 账号用户管理系统
    /// </summary>
    public class AccountController : ApiController
    {
        /// <summary>
        /// 全系统用户信息（管理员可见）
        /// </summary>
        /// <param name="token">管理员token</param>
        /// <returns>一张列表</returns>
        public async Task<ICollection<UserMsg>> Get(string token)
        {
            var userdao = new UserDAO();
            var usr = await userdao.GetByToken(token);
            if(usr!=null && usr.isadmin)
            {
                return await userdao.GetAllUser();
            }
            else
            {
                var resp = new HttpResponseMessage(HttpStatusCode.Unauthorized);
                resp.Content = new StringContent("Please log in as Admin.");
                throw new HttpResponseException(resp);
            }
        }

        /// <summary>
        /// 登录
        /// </summary>
        /// <returns>用户信息</returns>
        [HttpPost]
        [ActionName("login")]
        public async Task<UserMsg> Login([FromBody]AccountMsg value)
        {
            var userDAO = new UserDAO();
            user usr = await userDAO.GetAsync(value.Username);
            if (usr!=null && usr.password == value.Password)
            {
                string token = value.Username + value.Password;
                byte[] bytes = Encoding.UTF8.GetBytes(token);
                SHA256Managed hasher = new SHA256Managed();
                bytes = hasher.ComputeHash(bytes);
                string hashString = string.Empty;
                foreach (byte x in bytes)
                {
                    hashString += String.Format("{0:x2}", x);
                }
                usr.token = hashString;
                await userDAO.UpdateAsync(usr, usr.user1);
                return new UserMsg(usr);
            }
            else throw new HttpResponseException(HttpStatusCode.Unauthorized);
        }

        /// <summary>
        /// 注册
        /// </summary>
        /// <returns>注册成功的用户信息</returns>
        [HttpPost]
        [ActionName("signup")]
        public async Task<UserMsg> Signup([FromBody]AccountMsg info)
        {
            user newUser = new user
            {
                user1 = info.Username,
                password = info.Password,
                isadmin = (info.Admincode == "rootAdmin"),
                duration = 30
            };
            var result = await new UserDAO().AddAsync(newUser);
            if (result == null)
            {
                var resp = new HttpResponseMessage(HttpStatusCode.NotImplemented);
                resp.Content = new StringContent("Username existed.");
                throw new HttpResponseException(resp);
            }
            else return new UserMsg(newUser);
        }

        /// <summary>
        /// 验证用户token
        /// </summary>
        /// <returns>有效则返回用户信息</returns>
        [HttpGet]
        [ActionName("validate")]
        public async Task<UserMsgPlus> Validate(string token)
        {
            var usr = await new UserDAO().GetByToken(token);
            if (usr == null)
            {
                var resp = new HttpResponseMessage(HttpStatusCode.NotFound);
                resp.Content = new StringContent("User not found");
                throw new HttpResponseException(resp);
            }

            var info = await new RecordDAO().GetCredit(usr.user1);
            return new UserMsgPlus(usr, info);
        }

        /// <summary>
        /// 获取用户借书相关信息，仅本人或管理员可见
        /// </summary>
        /// <param name="token">用户的token</param>
        /// <returns></returns>
        [HttpGet]
        [ActionName("info")]
        public async Task<UserCredit> Info(string token, string username)
        {
            var usrdao = new UserDAO();
            var opt = await usrdao.GetByToken(token);
            var usr = await usrdao.GetAsync(username);
            
            if(opt==null || usr==null || (!opt.isadmin && opt != usr))
            {
                var resp = new HttpResponseMessage(HttpStatusCode.Unauthorized);
                resp.Content = new StringContent("Please validate the token and username.");
                throw new HttpResponseException(resp);
            }

            var borrowed = await new RecordDAO().FindAllAsync(x => x.user == usr.user1 && !x.isclosed);
            var dated = borrowed.Where(x => x.deadline.CompareTo(DateTime.Now) < 0).Count();

            return new UserCredit{ available = 10 - borrowed.Count(), dated = dated };
        }


        /// <summary>
        /// 注册用的各种信息
        /// </summary>
        public class AccountMsg
        {
            /// <summary>
            /// 用户名
            /// </summary>
            public string Username { get; set; }

            /// <summary>
            /// 密码
            /// </summary>
            public string Password { get; set; }

            /// <summary>
            /// 成为管理员所需要的神秘代码
            /// </summary>
            public string Admincode { get; set; }
        }
    }
}