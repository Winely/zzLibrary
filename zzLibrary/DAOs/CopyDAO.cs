using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ZZLibModel;
using System.Threading.Tasks;
using System.Data.Entity;

namespace zzLibrary.DAOs
{
    public class CopyDAO : BaseDAO<copy>
    {
        public async Task<List<copyMsg>> GetAllCopy()
        {
            return await db.copy.Select(x => new copyMsg {
                id=x.id,
                book = x.book
            }).ToListAsync();
        }
    }

    /// <summary>
    /// 复本的本体信息
    /// </summary>
    public class copyMsg
    {
        public copyMsg() { }
        public copyMsg(copy c)
        {
            id = c.id;
            book = c.book;
        }
        /// <summary>
        /// 复本id
        /// </summary>
        public int id { get; set; }

        /// <summary>
        /// 复本所属书isbn
        /// </summary>
        public string book { get; set; }
    }
}