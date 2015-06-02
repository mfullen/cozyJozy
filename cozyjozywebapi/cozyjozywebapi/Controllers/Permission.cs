using cozyjozywebapi.Models;

namespace cozyjozywebapi.Controllers
{
    public class Permission
    {
        public int Id { get; set; }
        public UserRestModel User { get; set; }
        public Child Child { get; set; }
        public bool ReadOnly { get; set; }
        public string Title { get; set; }
        public bool? FeedingWriteAccess { get; set; }
        public bool? DiaperChangeWriteAccess { get; set; }
        public bool? SleepWriteAccess { get; set; }
        public bool? MeasurementWriteAccess { get; set; }
        public bool? ChildManagementWriteAccess { get; set; }
        public bool? PermissionsWriteAccess { get; set; }
        public bool FeedingStatAccess { get; set; }
        public bool DiaperStatAccess { get; set; }
    }
}