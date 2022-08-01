using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IncedoReservoir.Areas.AdminServices.Models
{
    public class EditPrivilegeModelView
    {
        public int GroupID { get; set; }
        public string GroupName { get; set; }
        public string FunctionalLevel { get; set; }
        public string CheckValue { get; set; }
    }
}