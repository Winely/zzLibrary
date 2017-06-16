using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
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

        [HttpGet]
        [ActionName("tryit")]
        public Object tryit(string detail, int id)
        {
            return new { detail = detail, id = id };
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
    }
}