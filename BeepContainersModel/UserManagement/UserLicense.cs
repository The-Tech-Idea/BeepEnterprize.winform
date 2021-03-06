using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using TheTechIdea.Beep.Containers.Models;
namespace TheTechIdea.Beep.Containers.UserManagement
{
    public class UserLicense
    {
        public int UserLicenseID { get; set; }
        public string LicenceID { get; set; }
        public Product Product { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public bool AutoRenewal { get; set; }

        [ForeignKey("UserName")]
        public ApplicationUser User { get; set; }
    }
}
