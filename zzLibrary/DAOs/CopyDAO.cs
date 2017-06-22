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
            return await db.copy.Select(x => new copyMsg(x)).ToListAsync();
        }
        
    }

    public class copyMsg
    {
        public copyMsg(copy c)
        {
            id = c.id;
            book = c.book;
        }
        public int id { get; set; }
        public string book { get; set; }
    }
}