using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.Identity.EntityFramework;

namespace cozyjozywebapi.Models
{
    public class ChildPermissions
    {
        /// <summary>
        /// Identifier of the Childpermission
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// The Child Identifier in which the permission is for.
        /// </summary>
        public int ChildId { get; set; }
        /// <summary>
        /// Virtual Child object to allow navigation of the Child record
        /// </summary>
        public virtual Child Child { get; set; }
        /// <summary>
        /// Which user has the permission.
        /// </summary>
        public string IdentityUserId { get; set; }
        /// <summary>
        /// Virtual Object to allow navigation of the IdentityUser 
        /// </summary>
        public virtual IdentityUser IdentityUser { get; set; }
        /// <summary>
        /// This user can only read the associated childs records. False value allows created / edit / delete.
        /// </summary>
        public bool ReadOnly { get; set; }
    }
}