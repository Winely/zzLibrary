﻿using System;
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
        /// <returns>相关记录列表</returns>
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
        /// <returns>相关记录列表</returns>
        [HttpGet]
        [ActionName("copy")]
        public Object CopyRecord(string token, int copyId)
        {
            var usr = new UserDAO().GetByToken(token);
            if (usr != null && usr.isadmin)
            {
                var records = new RecordDAO().GetByID(copyId);
                return records.ConvertAll(x => new
                {

                });
            }
            else return new HttpResponseMessage(HttpStatusCode.Unauthorized);
        }

        /// <summary>
        /// 借书，仅管理员有权
        /// </summary>
        /// <returns>操作信息</returns>
        [HttpPost]
        [ActionName("borrow")]
        public Object Borrow([FromBody]BorrowMsg body)
        {
            var usrdao = new UserDAO();
            var opt = usrdao.GetByToken(body.token);
            if (opt == null || !opt.isadmin)
                return new HttpResponseMessage(HttpStatusCode.Unauthorized);

            var usr = usrdao.Get(body.username);
            if (usr == null)
            {
                var resp = new HttpResponseMessage(HttpStatusCode.NotImplemented);
                resp.Content = new StringContent("User not existed.");
                return resp;
            }

            var rec = new RecordDAO().Add(new record
            {
                copy = body.copy,
                user = usr.user1,
                borrow_time = DateTime.Now,
                deadline = DateTime.Now.AddDays(usr.duration),
                @operator = opt.user1
            });

            if (rec == null)
                return new HttpResponseMessage(HttpStatusCode.NotImplemented);

            return new
            {
                id = rec.id,
                copy = rec.copy,
                user = rec.user,
                borrow_time = rec.borrow_time,
                deadline = rec.deadline,
                @operator = rec.@operator
            };

        }
        public class BorrowMsg
        {
            public string token { get; set; }
            public string username { get; set; }
            public int copy { get; set; }
        }
    }
}