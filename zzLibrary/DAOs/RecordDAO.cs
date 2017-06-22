using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ZZLibModel;
using System.Threading.Tasks;
using System.Data.Entity;

namespace zzLibrary.DAOs
{
    public class RecordDAO : BaseDAO<record>
    {
        public async Task<List<recordbook>> GetByUser(string username)
        {
            return await db.record.Where(x => x.user == username)
                   .Join(db.copy, r => r.copy, c => c.id, (r, c) => new recordcopy(r, c))
                   .Join(db.book, r => r.isbn, b => b.isbn, (r, b) => new recordbook(r, b))
                   .ToListAsync();
        }

        public async Task<List<recordbook>> GetByID(int copyId)
        {
            var copy = await new CopyDAO().GetAsync(copyId);
            if (copy == null) return null;

            var book = await new BookDAO().GetAsync(copy.book);

            return await db.record.Where(x => x.copy == copyId)
                   .Select(r => new recordbook(r, book))
                   .ToListAsync();
        }

        public async Task<UserCredit> GetCredit(string username)
        {
            var borrowed = await FindAllAsync(x => x.user == username && !x.isclosed);
            var dated = borrowed.Where(x => x.deadline.CompareTo(DateTime.Now) < 0).Count();
            return new UserCredit { available = 10 - borrowed.Count(), dated = dated };
        }

        public async Task<List<RecordMsg>> GetAllRecord(int startRow, int pageSize)
        {
            return await db.record
                .OrderBy(x => x.borrow_time)
                .Skip(startRow).Take(pageSize)
                .Select(x => new RecordMsg(x))
                .ToListAsync();
        }
    }

    public class RecordMsg
    {
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
        public int id { get; set; }
        public string user { get; set; }
        public int copy { get; set; }
        public System.DateTime borrow_time { get; set; }
        public System.DateTime deadline { get; set; }
        public sbyte renew { get; set; }
        public bool isclosed { get; set; }
        public string @operator { get; set; }
    }

    public class recordcopy
    {
        public int id { get; set; }
        public string user { get; set; }
        public int copy { get; set; }
        public System.DateTime borrow_time { get; set; }
        public System.DateTime deadline { get; set; }
        public sbyte renew { get; set; }
        public bool isclosed { get; set; }
        public string @operator { get; set; }
        public string isbn { get; set; }

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

    public class recordbook
    {
        public int id { get; set; }
        public string user { get; set; }
        public int copy { get; set; }
        public System.DateTime borrow_time { get; set; }
        public System.DateTime deadline { get; set; }
        public sbyte renew { get; set; }
        public bool isclosed { get; set; }
        public string @operator { get; set; }
        public string book { get; set; }

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

    public class UserCredit
    {
        public int available { get; set; }
        public int dated { get; set; }
    }
}