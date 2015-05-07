using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.WebPages;
using cozyjozywebapi.Infrastructure.Core;
using cozyjozywebapi.Models;
using Microsoft.AspNet.Identity.EntityFramework;

namespace cozyjozywebapi.Controllers
{
    public class SearchController : ApiController
    {
        private readonly IUnitOfWork _unitOfWork;

        public SearchController(IUnitOfWork uow)
        {
            _unitOfWork = uow;
        }

        class UserResponse
        {
            public string Id { get; set; }
            public string UserName { get; set; }
        }

        [AllowAnonymous]
        public async Task<IHttpActionResult> GetUsers(string username)
        {
            if (username == null || username.IsEmpty())
            {
                return BadRequest();
            }
            var userSearch = _unitOfWork.UserRepository.Where(u => u.UserName.ToLower().Contains(username.ToLower())).Select(c => new
            UserResponse
            {
                UserName = c.UserName,
                Id = c.Id
            });
            var users = _unitOfWork.UserRepository.All().ToList();



            return Ok(userSearch);
        }
    }
}
