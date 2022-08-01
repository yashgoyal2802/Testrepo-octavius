using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IncedoReservoir.Areas.AdminServices.Models
{
    public class ServiceDefinitionViewModel
    {
        public int iServiceDefinitionID { get; set; }
        public string sServiceName { get; set; }
        public string sServiceDescription { get; set; }
        public string sServiceStartURL { get; set; }
        public string sServiceVersionNo { get; set; }
        public string sHostName { get; set; }
        public Nullable<int> iStatus { get; set; }
        public Nullable<int> iOrderBy { get; set; }
        public string sServiceLogo { get; set; }
    }
}