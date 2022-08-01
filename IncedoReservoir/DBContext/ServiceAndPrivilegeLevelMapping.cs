using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace IncedoReservoir.DBContext
{
    public class ServiceAndPrivilegeLevelMapping
    {
        [Key]
        public int iTID { get; set; }
        public Nullable<int> iGroupID { get; set; }
        public Nullable<int> iPrivilegeID { get; set; }
        public Nullable<byte> iStatus { get; set; }
        public string sPageFunctionalLevel { get; set; }
        public Nullable<int> iParentGroupID { get; set; }
        public Nullable<System.DateTime> dCreatedOn { get; set; }
        public Nullable<int> iAccountIdCreatedBy { get; set; }
        public Nullable<int> iCreatedBy { get; set; }
        public Nullable<System.DateTime> dModifiedOn { get; set; }
        public Nullable<int> iAccountIdModifiedBy { get; set; }
        public Nullable<int> iModifiedBy { get; set; }
        public Nullable<System.DateTime> dDeletedOn { get; set; }
        public Nullable<int> iAccountIdDeletedBy { get; set; }
        public Nullable<int> iDeletedBy { get; set; }
        public Nullable<int> iServiceDefinitionID { get; set; }
    }
}