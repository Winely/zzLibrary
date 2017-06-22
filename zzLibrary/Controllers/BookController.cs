using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using ZZLibModel;
using zzLibrary.DAOs;

namespace zzLibrary.Controllers
{
    public class BookController : ApiController
    {
        /// <summary>
        /// 获取所有的书本列表
        /// </summary>
        /// <returns>全部书信息的列表</returns>
        public Object Get()
        {
            return new BookDAO().db.book.Select(x => new
            {
                isbn = x.isbn,
                title = x.title,
                author = x.author,
                edition = x.edition,
                price = x.price
            }).ToList();
        }

        /// <summary>
        /// 通过ISBN获取馆藏某本书的信息
        /// </summary>
        /// <param name="isbn">书的ISBN</param>
        /// <returns>某本书的信息</returns>
        public Object Get(string isbn)
        {
            var bk = new BookDAO().Get(isbn);
            if (bk == null) return new HttpResponseMessage(HttpStatusCode.NotFound);
            else return new
            {
                title = bk.title,
                author = bk.author,
                edition = bk.edition,
                price = bk.price,
                isbn = bk.isbn
            };
        }

        /// <summary>
        /// 更新书本信息
        /// </summary>
        /// <param name="token">管理员token</param>
        /// <param name="bookInfo">更新后的书本信息</param>
        /// <returns>更新后的书本信息</returns>
        [HttpPost]
        [ActionName("update")]
        public Object UpdateBook(string token, [FromBody]book bookInfo)
        {
            var usr = new UserDAO().GetByToken(token);
            if (usr != null && usr.isadmin)
            {
                var bk = new BookDAO().Update(bookInfo, bookInfo.isbn);
                if (bk == null) return new HttpResponseMessage(HttpStatusCode.NotImplemented);
                else return bk;
            }
            else return new HttpResponseMessage(HttpStatusCode.Unauthorized);
        }

        /// <summary>
        /// 添加书本
        /// </summary>
        /// <param name="token">管理员token</param>
        /// <param name="bookInfo">书本信息</param>
        /// <returns>新增成功的书本信息</returns>
        [HttpPut]
        [ActionName("add")]
        public Object AddNewBook(string token, [FromBody]book bookInfo)
        {
            var usr = new UserDAO().GetByToken(token);
            if (usr != null && usr.isadmin)
            {
                if (bookInfo.price == null) bookInfo.price = "";
                if (bookInfo.edition == null) bookInfo.edition = "";
                if (bookInfo.author == null) bookInfo.author = "";
                var bk = new BookDAO().Add(bookInfo);
                if (bk == null) return new HttpResponseMessage(HttpStatusCode.NotImplemented);
                else return new
                {
                    title = bk.title,
                    author = bk.author,
                    edition = bk.edition,
                    price = bk.price,
                    isbn = bk.isbn
                };
            }
            else return new HttpResponseMessage(HttpStatusCode.Unauthorized);
        }
        
        /// <summary>
        /// 删除书本
        /// </summary>
        /// <param name="token">管理员token</param>
        /// <param name="isbn">被删的书本isbn</param>
        /// <returns>状态信息</returns>
        [HttpDelete]
        [ActionName("Delete")]
        public Object Delete(string token, string isbn)
        {
            var usr = new UserDAO().GetByToken(token);
            if (usr != null && usr.isadmin)
            {
                var bookdao = new BookDAO();
                bookdao.Delete(bookdao.Get(isbn));
                return Ok();
            }
            return new HttpResponseMessage(HttpStatusCode.Unauthorized);
        }

        /// <summary>
        /// 搜索书目，每页20条结果
        /// </summary>
        /// <param name="title">书本标题</param>
        /// <param name="page">结果页码</param>
        /// <returns></returns>
        [HttpGet]
        [ActionName("search")]
        public Object Search(string title, int page)
        {
            var bookdao = new BookDAO();
            var len = bookdao.FindAll(x => x.title.Contains(title)).Count();
            var pageSize = 20; //每页长度
            var totalPage = (len + pageSize - 1) / pageSize;
            int startRow = (page - 1) * pageSize;
            var books = bookdao.FindAll(x => x.title.Contains(title))
                .OrderBy(x => x.title)
                .Skip(startRow).Take(pageSize)
                .Select(x => new
                {
                    title = x.title,
                    author = x.author,
                    edition = x.edition,
                    isbn = x.isbn,
                    price = x.price
                })
                .ToList();
            return new
            {
                total = totalPage,
                books = books
            };
        }
    }
}