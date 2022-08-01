using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace IncedoReservoir.DBContext
{
    public class AccountUsers
    {
        [Key]
        public int iUserID { get; set; }
        public int iAccountID { get; set; }
        public string sAccountType { get; set; }
        public int iEmployeeID { get; set; }
        public string sEmployeeCode { get; set; }       
        public string sDisplayName { get; set; }
        public string sLoginName { get; set; }
        public string sUserType { get; set; }
        public string sLoginPassword { get; set; }
        public Nullable<bool> bCNBasedLogin { get; set; }
        public Nullable<bool> bCNPoolBasedLogin { get; set; }
        public Nullable<short> iStatus { get; set; }
        public Nullable<System.DateTime> dServicesUsesStartDate { get; set; }
        public Nullable<System.DateTime> dServicesUsesExpiryDate { get; set; }
        public Nullable<System.DateTime> dCreatedOn { get; set; }
        public Nullable<int> iCreatedBy { get; set; }
        public Nullable<System.DateTime> dModifiedOn { get; set; }
        public Nullable<int> iModifiedBy { get; set; }
        public Nullable<System.DateTime> dDeletedOn { get; set; }
        public Nullable<int> iDeletedBy { get; set; }
        public Nullable<int> iLockCount { get; set; }
        public Nullable<int> iLockMReason { get; set; }
        public Nullable<int> iLockMChildReason { get; set; }
    }
}