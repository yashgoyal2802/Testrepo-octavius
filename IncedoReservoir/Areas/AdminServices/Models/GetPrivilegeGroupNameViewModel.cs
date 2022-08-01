using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IncedoReservoir.Areas.AdminServices.Models
{
    public class GetPrivilegeGroupNameViewModel
    {
        public int GroupID { get; set; }
        public string GroupName { get; set; }
        public string FunctionalLevel { get; set; }
        public string ServiceName { get; set; }
        public string ServiceDescription { get; set; }
        public Nullable<byte> Status { get; set; }
        public string ActiveStatus { get; set; }
    }
}