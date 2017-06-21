using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using zzLibrary.Models;

namespace zzLibrary.DAOs
{
    public class UserDAO : BaseDAO<user>
    {
        public user GetByToken(string token)
        {
            return Find(u => u.token == token);
        }
    }
}