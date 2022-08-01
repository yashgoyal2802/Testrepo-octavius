using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IncedoReservoir.Areas.AdminServices.Models
{
    public class ApplicationUsersViewModel
    {
        public int iUserID { get; set; }
        public int iAccountID { get; set; }
        public string sAccountType { get; set; }
        public int iEmployeeID { get; set; }
        public string sEmployeeCode { get; set; }
        //public Nullable<System.DateTime> dCreationDate { get; set; }
        public string sDisplayName { get; set; }
        public string sLoginName { get; set; }
        public string sUserType { get; set; }
        public string sLoginPassword { get; set; }
        public string tempAssignedServices { get; set; }
        public string tempAssignedPG { get; set; }
        public int iStatus { get; set; }
        public int iAction { get; set; }
        public string dCreatedOn { get; set; }
        public string iCreatedBy { get; set; }

    }
}