using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace IncedoReservoir.DBContext
{
    public class ServiceDefinition
    {
        [Key]
        public int iServiceDefinitionID { get; set; }
        public string sServiceName { get; set; }
        public string sServiceDescription { get; set; }
        public string sServiceStartURL { get; set; }
        public string sServiceVersionNo { get; set; }
        public string sHostName { get; set; }
        public Nullable<int> iStatus { get; set; }
        public Nullable<int> iOrderBy { get; set; }
        public string sServiceLogo { get; set; }
        public Nullable<System.DateTime> dCreatedOn { get; set; }
        public Nullable<int> iAccountIdCreatedBy { get; set; }
        public Nullable<int> iCreatedBy { get; set; }
        public Nullable<System.DateTime> dModifiedOn { get; set; }
        public Nullable<int> iAccountIdModifiedBy { get; set; }
        public Nullable<int> iModifiedBy { get; set; }
        public Nullable<System.DateTime> dDeletedOn { get; set; }
        public Nullable<int> iAccountIdDeletedBy { get; set; }
        public Nullable<int> iDeletedBy { get; set; }
    }
}