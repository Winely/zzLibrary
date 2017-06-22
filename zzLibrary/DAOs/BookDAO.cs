using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ZZLibModel;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Threading.Tasks;

namespace zzLibrary.DAOs
{
    public class BookDAO : BaseDAO<book>
    {
        public async Task<ICollection<BookMsg>> SearchAsync(string title, int page)
        {
            var pageSize = 20;
            int startRow = (page - 1) * pageSize;
            return await db.book
                .Where(x => x.title.Contains(title))
                .OrderBy(x => x.title)
                .Skip(startRow).Take(pageSize)
                .Select(x => new BookMsg(x))
                .ToListAsync();
        }

        public async Task<ICollection<BookMsg>> GetALLAsync()
        {
            return await db.book.Select(x => new BookMsg(x)).ToListAsync();
        }
    }

    /// <summary>
    /// 只有本体信息的book
    /// </summary>
    public class BookMsg
    {
        public BookMsg(book b)
        {
            isbn = b.isbn;
            title = b.title;
            author = b.author;
            price = b.price;
            edition = b.edition;
        }
        public string isbn { get; set; }
        public string title { get; set; }
        public string author { get; set; }
        public string price { get; set; }
        public string edition { get; set; }
    }
}