using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace cozyjozywebapi.Controllers
{
    public class DeveloperController : ApiController
    {
        [HttpGet]
        [ActionName("forbidden")]
        public IHttpActionResult GetForbidden()
        {
            return ResponseMessage(Request.CreateResponse(HttpStatusCode.Forbidden));
        }

        [HttpGet]
        [ActionName("serverError")]
        public IHttpActionResult GetServerError()
        {
            return ResponseMessage(Request.CreateResponse(HttpStatusCode.InternalServerError));
        }

        [HttpGet]
        [ActionName("notfound")]
        public IHttpActionResult Get404()
        {
            return ResponseMessage(Request.CreateResponse(HttpStatusCode.NotFound));
        }

        [HttpGet]
        [ActionName("badrequest")]
        //[Authorize(Roles = "blaha")]
        public IHttpActionResult GetBadRequest()
        {
            return ResponseMessage(Request.CreateResponse(HttpStatusCode.BadRequest));
        }

    }
}
