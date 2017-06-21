using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using System.Data;
using System.Data.Entity.Validation;
using System.Diagnostics;
using System.Security.Cryptography;
using System.Text;
using zzLibrary.Models;
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
        public Object Get(string token)
        {
            var userdao = new UserDAO();
            var usr = userdao.GetByToken(token);
            if(usr!=null && usr.isadmin)
            {
                return userdao.GetAll();
            }
            else
            {
                var resp = new HttpResponseMessage(HttpStatusCode.Unauthorized);
                resp.Content = new StringContent("Please log in as Admin.");
                return resp;
            }
        }

        /// <summary>
        /// 登录
        /// </summary>
        /// <returns>用户信息</returns>
        [HttpPost]
        [ActionName("login")]
        
        public Object Login([FromBody]AccountMsg value)
        {
            var userDAO = new UserDAO();
            user usr = userDAO.Get(value.Username);
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
                userDAO.Update(usr, usr.user1);
                return new
                {
                    user = usr.user1,
                    token = usr.token,
                    isadmin = usr.isadmin,
                    duration = usr.duration
                };
            }
            else return new HttpResponseMessage(HttpStatusCode.Unauthorized);
        }

        /// <summary>
        /// 注册
        /// </summary>
        /// <returns>注册成功的用户信息</returns>
        [HttpPost]
        [ActionName("signup")]
        public Object Signup([FromBody]AccountMsg info)
        {
            user newUser = new user
            {
                user1 = info.Username,
                password = info.Password,
                isadmin = (info.Admincode == "rootAdmin"),
                duration = 30
            };
            var result = new UserDAO().Add(newUser);
            if (result == null)
            {
                var resp = new HttpResponseMessage(HttpStatusCode.NotImplemented);
                resp.Content = new StringContent("Username existed.");
                return resp;
            }
            else return new { user=result.user1, duration=result.duration, isadmin=result.isadmin};
        }

        /// <summary>
        /// 验证用户token
        /// </summary>
        /// <returns>有效则返回用户信息</returns>
        [HttpGet]
        [ActionName("validate")]
        public Object Validate(string token)
        {
            var usr = new UserDAO().GetByToken(token);
            if (usr != null) return new
            {
                user = usr.user1,
                token = usr.token,
                isadmin = usr.isadmin,
                duration = usr.duration
            };
            else
            {
                var resp = new HttpResponseMessage(HttpStatusCode.NotFound);
                resp.Content = new StringContent("User unfound");
                return resp;
            }
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