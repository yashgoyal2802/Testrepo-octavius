using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IncedoReservoir.DBContext
{
    public class BaseDBModel
    {
        public bool bStatus { get; set; }
        public string sSourceDescription { get; set; }
        public int? iCreatedBy { get; set; }
        public DateTime? dCreatedDate { get; set; }
        public int? iModifiedBy { get; set; }
        public DateTime? dModifiedDate { get; set; }
    }
}