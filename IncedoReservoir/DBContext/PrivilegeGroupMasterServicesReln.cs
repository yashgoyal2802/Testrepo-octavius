using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace IncedoReservoir.DBContext
{
    public class PrivilegeGroupMasterServicesReln
    {
        [Key]
        public int iRelnId { get; set; }
        public int iServiceId { get; set; }
        public int iPrivilegeGroup { get; set; }
        public bool bStatus { get; set; }
    }
}