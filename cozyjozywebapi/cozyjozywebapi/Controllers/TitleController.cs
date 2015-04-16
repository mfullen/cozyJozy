using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using cozyjozywebapi.Infrastructure.Core;

namespace cozyjozywebapi.Controllers
{
    public class TitleController : ApiController
    {
        private readonly IUnitOfWork _unitOfWork;

        public TitleController(IUnitOfWork uow)
        {
            _unitOfWork = uow;
        }


        public IHttpActionResult Get()
        {
            var results = _unitOfWork.TitleRepository.All().Select(x => x.Name).ToList();
        
            return Ok(results);
        }
    }
}
