using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using zzLibrary.Models;

namespace zzLibrary.DAOs
{
    public class RecordDAO : BaseDAO<record>
    {
        public List<recordbook> GetByUser(string username)
        {
            return FindAll(x => x.user == username)
                   .Join(db.copy, r => r.copy, c => c.id, (r, c) => new
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
                   .ToList();
        }

        public List<recordbook> GetByID(int copyId)
        {
            var copy = new CopyDAO().Get(copyId);
            if (copy == null) return null;

            var book = new BookDAO().Get(copy.book);

            return FindAll(x => x.copy == copyId)
                   .ToList()
                   .ConvertAll(r => new recordbook
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
                   });
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
        }
    }
}