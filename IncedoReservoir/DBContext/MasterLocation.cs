using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace IncedoReservoir.DBContext
{
    public class MasterLocation
    {
        [Key]
        public int iLocationId { get; set; }
        public string sLocationName { get; set; }
        public bool bStatus { get; set; }
        public int iCreatedBy { get; set; }
        public DateTime dCreatedOn { get; set; }
    }
}