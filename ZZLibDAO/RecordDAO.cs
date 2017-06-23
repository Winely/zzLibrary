using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ZZLibModel;
using System.Threading.Tasks;
using System.Data.Entity;

namespace ZZLibDAO
{
    /// <summary>
    /// 借书记录相关数据库操作
    /// </summary>
    public class RecordDAO : BaseDAO<record>
    {
        /// <summary>
        /// 按用户获取
        /// </summary>
        /// <param name="username">用户名</param>
        /// <returns>用户记录</returns>
        //public async Task<List<recordbook>> GetByUser(string username)
        public async Task<List<recordbook>> GetByUser(string username)
        {
            return await db.record.Where(x => x.user == username)
                   .Join(db.copy, r => r.copy, c => c.id, (r, c) => new recordcopy
                   {
                       id = r.id,
                       user = r.user,
                       copy = r.copy,
                       borrow_time = r.borrow_time,
                       deadline = r.deadline,
                       renew = r.renew,
                       isclosed = r.isclosed,
                       @operator = r.@operator,
                       isbn = c.book
                   })
                   .Join(db.book, r => r.isbn, b => b.isbn, (r, b) => new recordbook
                   {
                       id = r.id,
                       user = r.user,
                       copy = r.copy,
                       borrow_time = r.borrow_time,
                       deadline = r.deadline,
                       renew = r.renew,
                       isclosed = r.isclosed,
                       @operator = r.@operator,
                       book = b.title
                   })
                   .ToListAsync();
        }

        /// <summary>
        /// 按复本获取记录
        /// </summary>
        /// <param name="copyId">复本id</param>
        /// <returns>复本记录</returns>
        public async Task<List<recordbook>> GetByID(int copyId)
        {
            var copy = await new CopyDAO().GetAsync(copyId);
            if (copy == null) return null;

            var book = await new BookDAO().GetAsync(copy.book);

            return await db.record.Where(x => x.copy == copyId)
                   .Select(r => new recordbook
                   {
                       id = r.id,
                       user = r.user,
                       copy = r.copy,
                       borrow_time = r.borrow_time,
                       deadline = r.deadline,
                       renew = r.renew,
                       isclosed = r.isclosed,
                       @operator = r.@operator,
                       book = book.title
                   })
                   .ToListAsync();
        }

        /// <summary>
        /// 获取用户信用相关分数
        /// </summary>
        /// <param name="username">用户名</param>
        /// <returns>。</returns>
        public async Task<UserCredit> GetCredit(string username)
        {
            var borrowed = await FindAllAsync(x => x.user == username && !x.isclosed);
            var dated = borrowed.Where(x => x.deadline.CompareTo(DateTime.Now) < 0).Count();
            return new UserCredit { available = 10 - borrowed.Count(), dated = dated };
        }

        /// <summary>
        /// 获取所有记录
        /// </summary>
        /// <param name="startRow">开始行</param>
        /// <param name="pageSize">一页的长度</param>
        /// <returns>记录列表</returns>
        public async Task<List<RecordMsg>> GetAllRecord(int startRow, int pageSize)
        {
            return await db.record
                .OrderBy(x => x.borrow_time)
                .Skip(startRow).Take(pageSize)
                .Select(r => new RecordMsg
                {
                    id = r.id,
                    user = r.user,
                    copy = r.copy,
                    borrow_time = r.borrow_time,
                    deadline = r.deadline,
                    renew = r.renew,
                    @operator = r.@operator,
                    isclosed = r.isclosed
                })
                .ToListAsync();
        }
    }

    /// <summary>
    /// 一条记录的本体信息
    /// </summary>
    public class RecordMsg
    {
        /// <summary>
        /// 从record构造
        /// </summary>
        /// <param name="r">源record</param>
        public RecordMsg(record r)
        {
            id = r.id;
            user = r.user;
            copy = r.copy;
            borrow_time = r.borrow_time;
            deadline = r.deadline;
            renew = r.renew;
            @operator = r.@operator;
        }
        public RecordMsg() { }

        /// <summary>
        /// record id
        /// </summary>
        public int id { get; set; }

        /// <summary>
        /// 借书人
        /// </summary>
        public string user { get; set; }

        /// <summary>
        /// 被借复本
        /// </summary>
        public int copy { get; set; }

        /// <summary>
        /// 借书时间
        /// </summary>
        public System.DateTime borrow_time { get; set; }

        /// <summary>
        /// 应还时间
        /// </summary>
        public System.DateTime deadline { get; set; }

        /// <summary>
        /// 剩余可续次数
        /// </summary>
        public sbyte renew { get; set; }

        /// <summary>
        /// 已还书
        /// </summary>
        public bool isclosed { get; set; }

        /// <summary>
        /// 操作人
        /// </summary>
        public string @operator { get; set; }
    }

    public class recordcopy : RecordMsg
    {
        /// <summary>
        /// 书本的isbn号码
        /// </summary>
        public string isbn { get; set; }

        public recordcopy() { }
        public recordcopy(record r, copy c)
        {
            id = r.id;
            user = r.user;
            copy = r.copy;
            borrow_time = r.borrow_time;
            deadline = r.deadline;
            renew = r.renew;
            isclosed = r.isclosed;
            @operator = r.@operator;
            isbn = c.book;
        }
    }

    public class recordbook : RecordMsg
    {
        /// <summary>
        /// 书本名称
        /// </summary>
        public string book { get; set; }
        public recordbook() { }
        public recordbook(recordcopy r, book b)
        {
            id = r.id;
            user = r.user;
            copy = r.copy;
            borrow_time = r.borrow_time;
            deadline = r.deadline;
            renew = r.renew;
            isclosed = r.isclosed;
            @operator = r.@operator;
            book = b.title;
        }


        public recordbook(record r, book b)
        {
            id = r.id;
            user = r.user;
            copy = r.copy;
            borrow_time = r.borrow_time;
            deadline = r.deadline;
            renew = r.renew;
            isclosed = r.isclosed;
            @operator = r.@operator;
            book = b.title;
        }
    }

    /// <summary>
    /// 用户当前借书余额等
    /// </summary>
    public class UserCredit
    {
        /// <summary>
        /// 剩余可借书本数量
        /// </summary>
        public int available { get; set; }

        /// <summary>
        /// 已过期书本数
        /// </summary>
        public int dated { get; set; }
    }
}