using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.Identity.EntityFramework;

namespace cozyjozywebapi.Models
{
    public class User : IdentityUser
    {
        public virtual ICollection<Child> Child { get; set; } 
    }
}