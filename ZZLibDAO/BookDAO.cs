using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ZZLibModel;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Threading.Tasks;

namespace ZZLibDAO
{
    /// <summary>
    /// 书本相关数据库操作
    /// </summary>
    public class BookDAO : BaseDAO<book>
    {
        /// <summary>
        /// 模糊搜索书目
        /// </summary>
        /// <param name="title">书名</param>
        /// <param name="page">页码</param>
        /// <returns>书目列表</returns>
        public async Task<ICollection<BookMsg>> SearchAsync(string title, int page)
        {
            var pageSize = 20;
            int startRow = (page - 1) * pageSize;
            return await db.book
                .Where(x => x.title.Contains(title))
                .OrderBy(x => x.title)
                .Skip(startRow).Take(pageSize)
                .Select(x => new BookMsg
                {
                    title = x.title,
                    isbn = x.isbn,
                    author = x.author,
                    price = x.price,
                    edition = x.edition
                })
                .ToListAsync();
        }

        public async Task<ICollection<BookMsg>> GetByPage(int startRow, int pageSize)
        {
            return await db.book.OrderBy(x => x.title)
                .Skip(startRow).Take(pageSize)
                .Select(x => new BookMsg
                {
                    title = x.title,
                    isbn = x.isbn,
                    author = x.author,
                    price = x.price,
                    edition = x.edition
                })
                .ToListAsync();
        }

        /// <summary>
        /// 所有馆藏书目
        /// </summary>
        /// <returns>书目列表</returns>
        public async Task<ICollection<BookMsg>> GetALLAsync()
        {
            return await db.book.Select(x => new BookMsg
            {
                title = x.title,
                isbn = x.isbn,
                author = x.author,
                price = x.price,
                edition = x.edition
            }).ToListAsync();
        }
    }

    /// <summary>
    /// 只有本体信息的book
    /// </summary>
    public class BookMsg
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="b"></param>
        public BookMsg(book b)
        {
            isbn = b.isbn;
            title = b.title;
            author = b.author;
            price = b.price;
            edition = b.edition;
        }
        public BookMsg() { }

        /// <summary>
        /// 书的isbn
        /// </summary>
        public string isbn { get; set; }

        /// <summary>
        /// 书名
        /// </summary>
        public string title { get; set; }

        /// <summary>
        /// 作者
        /// </summary>
        public string author { get; set; }

        /// <summary>
        /// 价格
        /// </summary>
        public string price { get; set; }

        /// <summary>
        /// 出版社/版本信息
        /// </summary>
        public string edition { get; set; }
    }
}