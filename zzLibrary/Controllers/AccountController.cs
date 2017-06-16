using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Data;
using System.Data.Entity.Validation;
using System.Diagnostics;
using System.Security.Cryptography;
using System.Text;
using zzLibrary.Models;
using zzLibrary.DAOs;

namespace zzLibrary.Controllers
{
    public class AccountController : ApiController
    {
        // GET api/<controller>
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/<controller>/5
        public string Get(int id)
        {
            return "value";
        }

        [HttpPost]
        [ActionName("login")]
        public Object Login([FromBody]AccountMsg value)
        {
            var userDAO = new BaseDAO<user>();
            user _user = userDAO.Get(value.Username);
            if (_user.password == value.Password)
            {
                string token = value.Username + value.Password;
                byte[] bytes = Encoding.UTF8.GetBytes(token);
                SHA256Managed hasher = new SHA256Managed();
                bytes = hasher.ComputeHash(bytes);
                string hashString = string.Empty;
                foreach (byte x in bytes)
                {
                    hashString += String.Format("{0:x2}", x);
                }
                _user.token = hashString;
                userDAO.Update(_user, _user.user1);
                return new { token = hashString };
            }
            else return new HttpResponseMessage(HttpStatusCode.Unauthorized);
            return true;
        }

        [HttpPost]
        [ActionName("signup")]
        public Object Signup([FromBody]AccountMsg info)
        {
            user newUser = new user
            {
                user1 = info.Username,
                password = info.Password,
                isadmin = (info.Admincode == "rootAdmin")
            };
            var result = new BaseDAO<user>().Add(newUser);
            if (result == null)
            {
                var resp = new HttpResponseMessage(HttpStatusCode.NotImplemented);
                resp.Content = new StringContent("Username existed.");
                return resp;
            }
            else return result;
        }

        // POST api/<controller>
        public void Post([FromBody]string value)
        {

        }

        // PUT api/<controller>/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/<controller>/5
        public void Delete(int id)
        {
        }

        public class AccountMsg
        {
            public string Username { get; set; }
            public string Password { get; set; }
            public string Admincode { get; set; }
        }
    }
}