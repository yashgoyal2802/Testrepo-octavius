using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IncedoReservoir.Areas.AdminServices.Models
{
    public class PrivilegeGroupDetails
    {
        public int GroupID { get; set; }
        public string GroupName { get; set; }
        public string FunctionalLevel { get; set; }
        public bool ServiceName { get; set; }
        public bool ServiceDescription { get; set; }
        public bool Status { get; set; }
    }
}