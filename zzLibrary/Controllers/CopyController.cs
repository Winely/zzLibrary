using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using ZZLibModel;
using zzLibrary.DAOs;
using System.Threading.Tasks;
using System.Data.Entity;

namespace zzLibrary.Controllers
{
    public class CopyController : ApiController
    {
        /// <summary>
        /// 获取某本书的copy列表
        /// </summary>
        /// <returns></returns>
        public async Task<ICollection<copyMsg>> Get(string isbn)
        {
            var copies = await new CopyDAO().FindAllAsync(x=>x.book==isbn);
            return copies.Select(x => new copyMsg(x)).ToList();
        }

        /// <summary>
        /// 添加副本
        /// </summary>
        /// <param name="token">管理员token</param>
        /// <param name="isbn">书本isbn</param>
        /// <param name="num">副本量</param>
        /// <returns>添加成功则返回200</returns>
        [HttpPut]
        [ActionName("add")]
        public async Task<Object> Add(string token, string isbn, int num)
        {
            var usr = await new UserDAO().GetByToken(token);
            if (usr != null && usr.isadmin)
            {
                foreach (var i in Enumerable.Range(1, num))
                {
                    var bi = await new CopyDAO().AddAsync(new copy { book = isbn });
                    if (bi != null) continue;
                    else throw new HttpResponseException(HttpStatusCode.NotImplemented);
                }
                return Ok();
            }
            else throw new HttpResponseException(HttpStatusCode.Unauthorized);
        }

        /// <summary>
        /// 删除复本
        /// </summary>
        /// <param name="token">管理员token</param>
        /// <param name="id">复本id</param>
        public async void Delete(string token, int id)
        {
            var usr = await new UserDAO().GetByToken(token);
            if (usr != null && usr.isadmin)
            {
                var copydao = new CopyDAO();
                await copydao.DeleteAsync(await copydao.GetAsync(id));
            }
            else throw new HttpResponseException(HttpStatusCode.Unauthorized);
        }
    }
}