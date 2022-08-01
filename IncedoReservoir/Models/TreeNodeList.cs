using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IncedoReservoir.Models
{
    public class TreeNodeList
    {
        public int iPrivilegeID { get; set; }
        public int? iParentID { get; set; }
        public string sDescription { get; set; }
        public string sFileName { get; set; }
        public string sFunctionalLevel { get; set; }
        public string SPLevelMappingID { get; set; }
        public string spagefunctionallevel { get; set; }
        public string sCssClassName { get; set; }
    }
}