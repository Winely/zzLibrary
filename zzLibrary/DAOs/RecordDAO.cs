using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using zzLibrary.Models;

namespace zzLibrary.DAOs
{
    public class RecordDAO :BaseDAO<record>
    {
        public List<record> GetByUser(string username)
        {
            return db.record.Where(x => x.user == username).ToList();
        }

        public List<record> GetByID(int copyId)
        {
            return db.record
                            .Where(x => x.copy == copyId)
                            .Select(x=>new record{
                                copy=x.copy,
                                borrow_time=x.borrow_time,
                            })
                            .ToList();
        }
    }
}