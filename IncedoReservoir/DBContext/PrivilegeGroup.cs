using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace IncedoReservoir.DBContext
{
    public class PrivilegeGroup
    {
        [Key]
        public int iGroupID { get; set; }
        public string sGroupName { get; set; }
        public string sFunctionalLevel { get; set; }
        public Nullable<int> iAccountID { get; set; }
        public Nullable<int> iServiceDefinitionID { get; set; }
        public Nullable<byte> iStatus { get; set; }
        public Nullable<int> iUserID { get; set; }
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