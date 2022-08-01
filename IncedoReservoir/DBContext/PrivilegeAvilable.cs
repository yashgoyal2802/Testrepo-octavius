using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace IncedoReservoir.DBContext
{
    public class PrivilegeAvilable
    {
        [Key]
        public int iPrivilegeID { get; set; }
        public int iServiceDefinitionID { get; set; }
        public string sDescription { get; set; }
        public string sFileName { get; set; }
        public Nullable<int> iParentID { get; set; }
        public string sFunctionalLevel { get; set; }
        public string sPageFunctionalLevel { get; set; }
        public Nullable<bool> iIsPagefunctionality { get; set; }
        public Nullable<int> iOrderBySequence { get; set; }
        public string sCssClassName { get; set; }
        public Nullable<System.DateTime> dCreatedOn { get; set; }
        public Nullable<int> iAccountIdCreatedBy { get; set; }
        public Nullable<int> iCreatedBy { get; set; }
        public Nullable<System.DateTime> dModifiedOn { get; set; }
        public Nullable<int> iAccountIdModifiedBy { get; set; }
        public Nullable<int> iModifiedBy { get; set; }
        public Nullable<System.DateTime> dDeletedOn { get; set; }
        public Nullable<int> iAccountIdDeletedBy { get; set; }
        public Nullable<int> iDeletedBy { get; set; }
        public Nullable<bool> iStatus { get; set; }
    }
}