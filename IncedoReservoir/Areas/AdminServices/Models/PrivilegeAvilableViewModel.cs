using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IncedoReservoir.Areas.AdminServices.Models
{
    public class PrivilegeAvilableViewModel
    {
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
    }
}