using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using zzLibrary.Models;
using zzLibrary.DAOs;

namespace zzLibrary.Controllers
{
    public class CopyController : ApiController
    {
        /// <summary>
        /// 获取某本书的copy列表
        /// </summary>
        /// <returns></returns>
        public Object Get(string isbn)
        {
            var db = new CopyDAO().db;
            var copies = db.copy
                .Select(x => new { book = x.book, id = x.id })
                .Where(cp => cp.book == isbn)
                .ToList();
            return copies;
        }

        /// <summary>
        /// 添加副本
        /// </summary>
        /// <param name="token">管理员token</param>
        /// <param name="isbn">书本isbn</param>
        /// <param name="num">副本量</param>
        /// <returns></returns>
        [HttpPut]
        [ActionName("add")]
        public Object Add(string token, string isbn, int num)
        {
            var usr = new UserDAO().GetByToken(token);
            if (usr != null && usr.isadmin)
            {
                foreach (var i in Enumerable.Range(1, num))
                {
                    var bi = new CopyDAO().Add(new copy { book = isbn });
                    if (bi != null) continue;
                    else return new HttpResponseMessage(HttpStatusCode.NotImplemented);
                }
                return Ok();
            }
            else return new HttpResponseMessage(HttpStatusCode.Unauthorized);
        }

        /// <summary>
        /// 删除复本
        /// </summary>
        /// <param name="token">管理员token</param>
        /// <param name="id">复本id</param>
        public void Delete(string token, int id)
        {
            var usr = new UserDAO().GetByToken(token);
            if(usr!=null && usr.isadmin)
            {
                var copydao = new CopyDAO();
                copydao.Delete(copydao.Get(id));
            }
        }
    }
}