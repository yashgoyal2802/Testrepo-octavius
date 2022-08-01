using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IncedoReservoir.Models
{
    public class ServiceAccountUserPrivilege
    {
        public int iServiceDefinitionID { get; set; }
        public string sServiceName { get; set; }
        public string sServiceDescription { get; set; }
        public string sServiceStartURL { get; set; }
        public string sServiceVersionNo { get; set; }
        public string sHostName { get; set; }
        public int itid { get; set; }
        public int iPrivilegeGroup { get; set; }
        public string dServicesUsesStartDate { get; set; }
        public string dServicesUsesExpiryDate { get; set; }
        public string sServiceLogo { get; set; }
    }
}