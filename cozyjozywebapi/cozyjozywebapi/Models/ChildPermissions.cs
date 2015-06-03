﻿using System;
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
        public virtual User IdentityUser { get; set; }
        /// <summary>
        /// The title identifier
        /// </summary>
        public int? TitleId { get; set; }
        /// <summary>
        /// Virtual Object to allow navigation to the Title table
        /// </summary>
        public virtual Title Title { get; set; }

        public bool? FeedingWriteAccess { get; set; }
        public bool? DiaperChangeWriteAccess { get; set; }
        public bool? SleepWriteAccess { get; set; }
        public bool? MeasurementWriteAccess { get; set; }
        public bool? ChildManagementWriteAccess { get; set; }
        public bool? PermissionsWriteAccess { get; set; }
    }
}