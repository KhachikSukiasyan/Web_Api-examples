using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace ServerSide.Controllers
{
    public class MainController : ApiController
    {


        // GET: api/Main/5
        public string Get(int id)
        {
            return "value";
        }



    }
}
