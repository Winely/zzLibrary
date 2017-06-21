using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using zzLibrary.DAOs;
using zzLibrary.Models;

namespace zzLibrary.Controllers
{
    public class RecordController : ApiController
    {
        // GET api/<controller>
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        /// <summary>
        /// 查询某一用户的借阅记录，仅本人或管理员可见。
        /// </summary>
        /// <param name="token">用户token</param>
        /// <param name="user">被查询用户名</param>
        /// <returns></returns>
        [HttpGet]
        [ActionName("user")]
        public Object UserRecord(string token, string user)
        {
            var usr = new UserDAO().GetByToken(token);
            if (usr != null)
            {
                if (usr.isadmin || usr.user1 == user)
                    return new RecordDAO().GetByUser(user);
                else
                    return new HttpResponseMessage(HttpStatusCode.Unauthorized);
            }
            else return new HttpResponseMessage(HttpStatusCode.NotImplemented);
        }

        /// <summary>
        /// 获取某本副本的借阅记录，仅管理员可见
        /// </summary>
        /// <param name="token">管理员token</param>
        /// <param name="copyId">副本id</param>
        /// <returns></returns>
        [HttpGet]
        [ActionName("copy")]
        public Object CopyRecord(string token, int copyId)
        {
            var usr = new UserDAO().GetByToken(token);
            if (usr != null && usr.isadmin)
            {
                return new RecordDAO().GetByID(copyId);
            }
            else return new HttpResponseMessage(HttpStatusCode.Unauthorized);
        }

        [HttpPost]
        [ActionName("borrow")]
        public void Post(string token, [FromBody]string values)
        {

        }

        // PUT api/<controller>/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/<controller>/5
        public void Delete(int id)
        {
        }

        class BorrowMsg
        {
            public string username;
            public int copy;
        }
    }
}