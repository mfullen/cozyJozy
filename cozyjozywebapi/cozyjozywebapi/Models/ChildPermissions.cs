using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.Identity.EntityFramework;

namespace cozyjozywebapi.Models
{
    public class ChildPermissions
    {
        public int Id { get; set; }

        public int ChildId { get; set; }
        public virtual Child Child { get; set; }
        public string IdentityUserId { get; set; }
        public virtual IdentityUser IdentityUser { get; set; }
        public virtual IdentityRole Role { get; set; }
    }
}