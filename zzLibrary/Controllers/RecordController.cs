using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using ZZLibDAO;
using ZZLibModel;
using System.Runtime.InteropServices;

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
        public async Task<RecordByPage> GetAll(string token, int page)
        {
            var usr = await new UserDAO().GetByToken(token);
            if (usr==null || !usr.isadmin)
            {
                var resp = new HttpResponseMessage(HttpStatusCode.Unauthorized);
                resp.Content = new StringContent("Please log in as admin.");
                throw new HttpResponseException(resp);
            }

            var recorddao = new RecordDAO();
            var len = await recorddao.CountAsync();
            var pageSize = 40; //每页长度
            var totalPage = (len + pageSize - 1) / pageSize;
            int startRow = (page - 1) * pageSize;
            var records = await recorddao.GetAllRecord(startRow, pageSize);
            return new RecordByPage
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
        public async Task<List<recordbook>> UserRecord(string token, string user)
        {
            var usr = await new UserDAO().GetByToken(token);
            if (usr != null)
            {
                if (usr.isadmin || usr.user1 == user)
                {
                    return await new RecordDAO().GetByUser(user);
                }
                else
                    throw new HttpResponseException(HttpStatusCode.Unauthorized);
            }
            else throw new HttpResponseException(HttpStatusCode.NotImplemented);
        }

        /// <summary>
        /// 获取某本副本的借阅记录，仅管理员可见
        /// </summary>
        /// <param name="token">管理员token</param>
        /// <param name="copyId">副本id</param>
        /// <returns>相关记录列表</returns>
        [HttpGet]
        [ActionName("copy")]
        public async Task<List<recordbook>> CopyRecord(string token, int copyId)
        {
            var usr = await new UserDAO().GetByToken(token);
            if (usr != null && usr.isadmin)
            {
                return await new RecordDAO().GetByID(copyId);
            }
            else throw new HttpResponseException(HttpStatusCode.Unauthorized);
        }

        /// <summary>
        /// 借书，仅管理员有权
        /// </summary>
        /// <param name="token">管理员token</param>
        /// <returns>操作信息</returns>
        [HttpPost]
        [ActionName("borrow")]
        public async Task<RecordMsg> Borrow(string token, [FromBody]BorrowMsg body)
        {
            // validate authorization
            var usrdao = new UserDAO();
            var recorddao = new RecordDAO();
            var opt = await usrdao.GetByToken(token);
            if (opt == null || !opt.isadmin)
                throw new HttpResponseException(HttpStatusCode.Unauthorized);

            // validate user
            var usr = await usrdao.GetAsync(body.Username);
            if (usr == null)
            {
                var resp = new HttpResponseMessage(HttpStatusCode.NotImplemented);
                resp.Content = new StringContent("User not existed.");
                throw new HttpResponseException(resp);
            }

            var credit = await recorddao.GetCredit(body.Username);
            if (credit.available <= 0 || credit.dated > 0)
            {
                var resp = new HttpResponseMessage(HttpStatusCode.Unauthorized);
                resp.Content = new StringContent("余额不足或有过期未还");
                throw new HttpResponseException(resp);
            }

            // validate copy
            var copy = await new CopyDAO().GetAsync(body.Copy);
            if (copy.status != 0)
            {
                var resp = new HttpResponseMessage(HttpStatusCode.NotImplemented);
                resp.Content = new StringContent("The copy is borrowed or missed.");
                throw new HttpResponseException(resp);
            }

            var rec = await recorddao.AddAsync(new record
            {
                copy = body.Copy,
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
                throw new HttpResponseException(resp);
            }

            copy.status = 1;
            await new CopyDAO().UpdateAsync(copy, copy.id);

            return new RecordMsg(rec);

        }

        [DllImport(@"Penalty.dll", EntryPoint = "penalty", SetLastError = true, CharSet = CharSet.Ansi, ExactSpelling = false, CallingConvention = CallingConvention.StdCall)]
        extern static double penalty(double time);

        /// <summary>
        /// 还书，仅管理员
        /// </summary>
        /// <param name="token">管理员token</param>
        /// <returns>返回过期时间（为负则过期）</returns>
        [HttpPost]
        [ActionName("return")]
        public async Task<Dated> Return(string token, [FromBody]BorrowMsg msg)
        {
            var usrdao = new UserDAO();
            var recorddao = new RecordDAO();
            var opt = await usrdao.GetByToken(token);
            if (opt == null || !opt.isadmin)
            {
                var resp = new HttpResponseMessage(HttpStatusCode.Unauthorized);
                resp.Content = new StringContent("Not admin.");
                throw new HttpResponseException(resp);
            }

            var record = await recorddao.FindAsync(x => x.user == msg.Username && x.copy == msg.Copy && !x.isclosed);
            if (record == null)
            {
                var resp = new HttpResponseMessage(HttpStatusCode.NotImplemented);
                resp.Content = new StringContent("Record not found");
                throw new HttpResponseException(resp);
            }

            var copydao = new CopyDAO();
            var copy = await copydao.GetAsync(msg.Copy);
            copy.status = 0;
            await copydao.UpdateAsync(copy, copy.id);
            record.isclosed = true;
            await recorddao.UpdateAsync(record, record.id);
            var outdated = -1*record.deadline.Subtract(DateTime.Now).TotalDays;
            return new Dated
            {
                dated = outdated,
                penalty = penalty(outdated)
            };

        }

        /// <summary>
        /// 续借书本，仅限本人或管理员
        /// </summary>
        /// <param name="token">用户token</param>
        /// <returns></returns>
        [HttpPost]
        [ActionName("renew")]
        public async Task<RecordMsg> Renew(string token, [FromBody]BorrowMsg msg)
        {
            var usrdao = new UserDAO();
            var recorddao = new RecordDAO();
            var opt = await usrdao.GetByToken(token);
            var usr = await usrdao.GetAsync(msg.Username);
            if(opt==null || usr==null || (opt!=usr && !opt.isadmin))
            {
                var resp = new HttpResponseMessage(HttpStatusCode.Unauthorized);
                resp.Content = new StringContent("Unauthorized.");
                throw new HttpResponseException(resp);
            }

            var record = await recorddao.FindAsync(x => x.user == usr.user1 && x.copy == msg.Copy && !x.isclosed);
            if (record == null)
            {
                var resp = new HttpResponseMessage(HttpStatusCode.NotImplemented);
                resp.Content = new StringContent("Record not found.");
                throw new HttpResponseException(resp);
            }

            if (record.renew < 1)
            {
                var resp = new HttpResponseMessage(HttpStatusCode.NotImplemented);
                resp.Content = new StringContent("Cannot renew any more");
                throw new HttpResponseException(resp);
            }

            record.renew--;
            record.deadline = record.deadline.AddDays(15);
            await recorddao.UpdateAsync(record, record.id);

            return new RecordMsg(record);
        }

        /// <summary>
        /// 借还书提交信息
        /// </summary>
        public class BorrowMsg
        {
            /// <summary>
            /// 借书人用户名
            /// </summary>
            public string Username { get; set; }

            /// <summary>
            /// 复本id
            /// </summary>
            public int Copy { get; set; }
        }

        /// <summary>
        /// 分页查看借书记录
        /// </summary>
        public class RecordByPage
        {
            /// <summary>
            /// 总页数
            /// </summary>
            public int total { get; set; }

            /// <summary>
            /// 该页记录列表
            /// </summary>
            public List<RecordMsg> records { get; set; }
        }

        /// <summary>
        /// 过期情况
        /// </summary>
        public class Dated
        {
            /// <summary>
            /// 距离还书日的时间（为负则过期）
            /// </summary>
            public double dated { get; set; }

            /// <summary>
            /// 应交罚款
            /// </summary>
            public double penalty { get; set; }
        }
    }
}