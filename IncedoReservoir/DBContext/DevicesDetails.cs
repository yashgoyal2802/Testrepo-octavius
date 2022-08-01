using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace IncedoReservoir.DBContext
{
    public class DevicesDetails 
    {
        [Key]
        public long iDeviceId { get; set; }
        public int iServiceId { get; set; }
        public string sDeviceName { get; set; }
        public string sDeviceUserName { get; set; }
        public int iLocationId { get; set; }
        public int iDeviceStatusId { get; set; }
        public DateTime? dActiveDate { get; set; }
        public Byte[] sEncryptedPasswordOne { get; set; }
        public Byte[] sEncryptedPasswordTwo { get; set; }
        public string sRemarks { get; set; }


        public bool bStatus { get; set; }
        public string sSourceDescription { get; set; }
        public int iCreatedBy { get; set; }
        public DateTime dCreatedOn { get; set; }
        public int iModifiedBy { get; set; }
        public DateTime dModifiedOn { get; set; }
    }
}