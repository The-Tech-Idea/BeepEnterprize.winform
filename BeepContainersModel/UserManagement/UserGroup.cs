using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using TheTechIdea.Beep.Containers.Models.UserManagement;

namespace TheTechIdea.Beep.Containers.UserManagement
{
    public class UserGroup
    {
        public int UserGroupID { get; set; }
        [ForeignKey("UserName")]
        public ApplicationUser User { get; set; }
        public GroupPriv GroupData { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
    }
}
