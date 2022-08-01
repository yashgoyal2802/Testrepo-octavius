using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IncedoReservoir.Areas.AdminServices.Models
{
    public class PrivilegeGroupMasterServicesRelnViewModel
    {
        public int iRelnId { get; set; }
        public int iServiceId { get; set; }
        public int iPrivilegeGroup { get; set; }
        public bool bStatus { get; set; }

        public string CheckValue { get; set; }
    }
}