﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using ZZLibDAO;
using ZZLibModel;


namespace zzLibrary.Controllers
{
    public class BookController : ApiController
    {
        /// <summary>
        /// 按分页获取所有的书本列表
        /// </summary>
        /// <param name="page">页码</param>
        /// <returns>全部书信息的列表</returns>
        public async Task<SearchResult> Get(int page)
        {
            var bookdao = new BookDAO();
            var len = await bookdao.CountAsync();
            int pageSize = 20;
            var totalPage = (len + pageSize - 1) / pageSize;
            int startRow = (page - 1) * pageSize;
            var books = await bookdao.GetByPage(startRow, pageSize);
            return new SearchResult
            {
                total = totalPage,
                books = books
            };
        }

        /// <summary>
        /// 通过ISBN获取馆藏某本书的信息
        /// </summary>
        /// <param name="isbn">书的ISBN</param>
        /// <returns>某本书的信息</returns>
        public async Task<BookMsg> Get(string isbn)
        {
            var bk = await new BookDAO().GetAsync(isbn);
            if (bk == null)
            {
                var resp = new HttpResponseMessage(HttpStatusCode.NotFound);
                resp.Content = new StringContent("ISBN not found.");
                throw new HttpResponseException(resp);
            }
            else return new BookMsg(bk);
        }

        /// <summary>
        /// 更新书本信息
        /// </summary>
        /// <param name="token">管理员token</param>
        /// <param name="bookInfo">更新后的书本信息</param>
        /// <returns>更新后的书本信息</returns>
        [HttpPost]
        [ActionName("update")]
        public async Task<BookMsg> UpdateBook(string token, [FromBody]book bookInfo)
        {
            var usr = await new UserDAO().GetByToken(token);
            if (usr != null && usr.isadmin)
            {
                var bk = await new BookDAO().UpdateAsync(bookInfo, bookInfo.isbn);
                if (bk == null)
                {
                    var resp = new HttpResponseMessage(HttpStatusCode.NotImplemented);
                    resp.Content = new StringContent("Update failed.");
                    throw new HttpResponseException(resp);
                }
                else return new BookMsg(bk);
            }
            else throw new HttpResponseException(HttpStatusCode.Unauthorized);
        }

        /// <summary>
        /// 添加书本
        /// </summary>
        /// <param name="token">管理员token</param>
        /// <param name="bookInfo">书本信息</param>
        /// <returns>新增成功的书本信息</returns>
        [HttpPut]
        [ActionName("add")]
        public async Task<BookMsg> AddNewBook(string token, [FromBody]book bookInfo)
        {
            var usr = await new UserDAO().GetByToken(token);
            if (usr != null && usr.isadmin)
            {
                if (bookInfo.price == null) bookInfo.price = "";
                if (bookInfo.edition == null) bookInfo.edition = "";
                if (bookInfo.author == null) bookInfo.author = "";
                var bk = new BookDAO().Add(bookInfo);
                if (bk == null) throw new HttpResponseException(HttpStatusCode.NotImplemented);
                else return new BookMsg(bk);
            }
            else throw new HttpResponseException(HttpStatusCode.Unauthorized);
        }
        
        /// <summary>
        /// 删除书本
        /// </summary>
        /// <param name="token">管理员token</param>
        /// <param name="isbn">被删的书本isbn</param>
        /// <returns>状态信息</returns>
        [HttpDelete]
        [ActionName("Delete")]
        public async Task<Object> Delete(string token, string isbn)
        {
            var usr = await new UserDAO().GetByToken(token);
            if (usr != null && usr.isadmin)
            {
                var bookdao = new BookDAO();
                await bookdao.DeleteAsync(bookdao.Get(isbn));
                return Ok();
            }
            return new HttpResponseMessage(HttpStatusCode.Unauthorized);
        }

        /// <summary>
        /// 搜索书目，每页20条结果
        /// </summary>
        /// <param name="title">书本标题</param>
        /// <param name="page">结果页码</param>
        /// <returns>搜索结果和搜索页数</returns>
        [HttpGet]
        [ActionName("search")]
        public async Task<SearchResult> Search(string title, int page)
        {
            var bookdao = new BookDAO();
            var allResult = await bookdao.FindAllAsync(x => x.title.Contains(title));
            var len = allResult.Count();
            var pageSize = 20; //每页长度
            var totalPage = (len + pageSize - 1) / pageSize;
            int startRow = (page - 1) * pageSize;
            var books = await bookdao.SearchAsync(title, page);
            return new SearchResult
            {
                total = totalPage,
                books = books
            };
        }

        /// <summary>
        /// 通过isbn获得某本书的信息（外网API转发）
        /// </summary>
        /// <param name="token">管理员token</param>
        /// <param name="isbn">isbn号码</param>
        /// <returns>查询结果json</returns>
        [HttpGet]
        [ActionName("info")]
        public async Task<Object> info(string token, string isbn)
        {
            var usr = await new UserDAO().GetByToken(token);
            if (usr == null || !usr.isadmin)
            {
                var resp = new HttpResponseMessage(HttpStatusCode.Unauthorized);
                resp.Content = new StringContent("Please log in as admin.");
                throw new HttpResponseException(resp);
            }

            var request = WebRequest.Create("http://isbn.szmesoft.com/isbn/query?isbn=" + isbn) as WebRequest;
            request.Method = "GET";
            var re =  await request.GetResponseAsync();
            using (var reader = new StreamReader(re.GetResponseStream()))
            {
                string result = reader.ReadToEnd();
                var json = JsonConvert.DeserializeObject(result);
                return json;
            }
        }
    }

    /// <summary>
    /// 搜索结果
    /// </summary>
    public class SearchResult
    {
        /// <summary>
        /// 总页数
        /// </summary>
        public int total { get; set; }

        /// <summary>
        /// 结果列表
        /// </summary>
        public ICollection<BookMsg> books { get; set; }
    }
}