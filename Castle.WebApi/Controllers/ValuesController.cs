using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Castle.WebApi.Controllers
{
    [RoutePrefix("api/v1/vlaues")]
    public class ValuesController : ApiController
    {
        [HttpGet, Route("")]
        public IHttpActionResult Test()
        {
            return Ok(new { Name = "Ahmad", Age = 29 });
        }
    }
}
