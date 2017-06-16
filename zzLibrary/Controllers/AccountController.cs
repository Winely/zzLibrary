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

namespace zzLibrary.Controllers
{
    public class AccountController : ApiController
    {
        zzLibraryEntities db = new zzLibraryEntities();

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
            try
            {
                user _user = db.user.Find(value.Username);
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
                    db.SaveChanges();
                    return new { token = hashString };
                }
                else return new HttpResponseMessage(HttpStatusCode.Unauthorized);
            }
            catch (DbEntityValidationException e)
            {
                foreach (var validationErrors in e.EntityValidationErrors)
                {
                    foreach (var validationError in validationErrors.ValidationErrors)
                    {
                        Trace.TraceInformation("Property: {0} Error: {1}",
                                                validationError.PropertyName,
                                                validationError.ErrorMessage);
                    }
                }
            }
            catch (DataException)
            {
                return new HttpResponseMessage(HttpStatusCode.Unauthorized);
            }
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
            try
            {
                db.user.Add(newUser);
                db.SaveChanges();
            }
            catch (DbEntityValidationException e)
            {
                foreach (var validationErrors in e.EntityValidationErrors)
                {
                    foreach (var validationError in validationErrors.ValidationErrors)
                    {
                        Trace.TraceInformation("Property: {0} Error: {1}",
                                                validationError.PropertyName,
                                                validationError.ErrorMessage);
                    }
                }
            }
            catch (DataException e)
            {
                Console.WriteLine(e.Message);
                Console.WriteLine(e.Source);
                return new HttpResponseMessage(HttpStatusCode.NotImplemented);
            }

            return true;
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

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }

    }
}