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
        /// <summary>
        /// 获取所有record，按时间排序，一页40条，仅管理员
        /// </summary>
        /// <param name="token">管理员token</param>
        /// <param name="page">页码</param>
        /// <returns>total为总页数；records为记录列表</returns>
        [HttpGet]
        [ActionName("Get")]
        public Object GetAll(string token, int page)
        {
            var usr = new UserDAO().GetByToken(token);
            if (usr==null || !usr.isadmin)
            {
                var resp = new HttpResponseMessage(HttpStatusCode.Unauthorized);
                resp.Content = new StringContent("Please log in as admin.");
                return resp;
            }

            var recorddao = new RecordDAO();
            var len = recorddao.Count();
            var pageSize = 40; //每页长度
            var totalPage = (len + pageSize - 1) / pageSize;
            int startRow = (page - 1) * pageSize;
            var records = recorddao.GetAll()
                .OrderBy(x => x.borrow_time)
                .Skip(startRow).Take(pageSize)
                .ToList()
                .ConvertAll(x => new
                {
                    id = x.id,
                    user = x.user,
                    copy = x.copy,
                    borrow_time = x.borrow_time,
                    deadline = x.deadline,
                    renew = x.renew,
                    @operator = x.@operator
                });
            return new
            {
                total = totalPage,
                records = records
            };
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
                {
                    return new RecordDAO().GetByUser(user);
                }
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
                return new RecordDAO().GetByID(copyId);
            }
            else return new HttpResponseMessage(HttpStatusCode.Unauthorized);
        }

        /// <summary>
        /// 借书，仅管理员有权
        /// </summary>
        /// <param name="token">管理员token</param>
        /// <returns>操作信息</returns>
        [HttpPost]
        [ActionName("borrow")]
        public Object Borrow(string token, [FromBody]BorrowMsg body)
        {
            // validate authorization
            var usrdao = new UserDAO();
            var recorddao = new RecordDAO();
            var opt = usrdao.GetByToken(token);
            if (opt == null || !opt.isadmin)
                return new HttpResponseMessage(HttpStatusCode.Unauthorized);

            // validate user
            var usr = usrdao.Get(body.username);
            if (usr == null)
            {
                var resp = new HttpResponseMessage(HttpStatusCode.NotImplemented);
                resp.Content = new StringContent("User not existed.");
                return resp;
            }

            var credit = recorddao.GetCredit(body.username);
            if (credit.available <= 0 || credit.dated > 0)
            {
                var resp = new HttpResponseMessage(HttpStatusCode.Unauthorized);
                resp.Content = new StringContent("余额不足或有过期未还");
                return resp;
            }
            
            // validate copy
            var copy = new CopyDAO().Get(body.copy);
            if (copy.status != 0)
            {
                var resp = new HttpResponseMessage(HttpStatusCode.NotImplemented);
                resp.Content = new StringContent("The copy is borrowed or missed.");
                return resp;
            }

            var rec = recorddao.Add(new record
            {
                copy = body.copy,
                user = usr.user1,
                borrow_time = DateTime.Now,
                deadline = DateTime.Now.AddDays(usr.duration),
                renew = 2,
                @operator = opt.user1
            });

            if (rec == null)
            {
                var resp = new HttpResponseMessage(HttpStatusCode.NotImplemented);
                resp.Content = new StringContent("Copy not existed");
                return resp;
            }

            copy.status = 1;
            new CopyDAO().Update(copy, copy.id);

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

        /// <summary>
        /// 还书，仅管理员
        /// </summary>
        /// <param name="token">管理员token</param>
        /// <returns>返回过期时间（为负则没有过期）</returns>
        [HttpPost]
        [ActionName("return")]
        public Object Return(string token, [FromBody]BorrowMsg msg)
        {
            var usrdao = new UserDAO();
            var recorddao = new RecordDAO();
            var opt = usrdao.GetByToken(token);
            if (opt == null || !opt.isadmin)
            {
                var resp = new HttpResponseMessage(HttpStatusCode.Unauthorized);
                resp.Content = new StringContent("Not admin.");
                return resp;
            }

            var record = recorddao.Find(x => x.user == msg.username && x.copy == msg.copy && !x.isclosed);
            if (record == null)
            {
                var resp = new HttpResponseMessage(HttpStatusCode.NotImplemented);
                resp.Content = new StringContent("Record not found");
                return resp;
            }

            var copydao = new CopyDAO();
            var copy = copydao.Get(msg.copy);
            copy.status = 0;
            copydao.Update(copy, copy.id);
            record.isclosed = true;
            recorddao.Update(record, record.id);

            return new
            {
                dated = record.deadline.Subtract(DateTime.Now).TotalDays
            };

        }

        /// <summary>
        /// 续借书本，仅限本人或管理员
        /// </summary>
        /// <param name="token">用户token</param>
        /// <returns></returns>
        [HttpPost]
        [ActionName("renew")]
        public Object Renew(string token, [FromBody]BorrowMsg msg)
        {
            var usrdao = new UserDAO();
            var recorddao = new RecordDAO();
            var opt = usrdao.GetByToken(token);
            var usr = usrdao.Get(msg.username);
            if(opt==null || usr==null || (opt!=usr && !opt.isadmin))
            {
                var resp = new HttpResponseMessage(HttpStatusCode.Unauthorized);
                resp.Content = new StringContent("Unauthorized.");
                return resp;
            }

            var record = recorddao.Find(x => x.user == usr.user1 && x.copy == msg.copy && !x.isclosed);
            if (record == null)
            {
                var resp = new HttpResponseMessage(HttpStatusCode.NotImplemented);
                resp.Content = new StringContent("Record not found.");
                return resp;
            }

            if (record.renew < 1)
            {
                var resp = new HttpResponseMessage(HttpStatusCode.NotImplemented);
                resp.Content = new StringContent("Cannot renew any more");
                return resp;
            }

            record.renew--;
            record.deadline = record.deadline.AddDays(15);
            recorddao.Update(record, record.id);

            return new
            {
                id = record.id,
                borrow_time = record.borrow_time,
                deadline = record.deadline,
                copy = record.copy,
                user = record.user
            };
        }

        /// <summary>
        /// 借还书提交信息
        /// </summary>
        public class BorrowMsg
        {
            /// <summary>
            /// 借书人用户名
            /// </summary>
            public string username { get; set; }

            /// <summary>
            /// 复本id
            /// </summary>
            public int copy { get; set; }
        }
    }
}