using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IncedoReservoir.Areas.InventoryServices.Models
{
    public class DevicesDetailsViewModel
    {
        public long DeviceId { get; set; }
        public int ServiceId { get; set; }
        public string ServiceName { get; set; }
        public string DeviceName { get; set; }
        public string DeviceUserName { get; set; }
        public int LocationId { get; set; }
        public string LocationName { get; set; }
        public int DeviceStatusId { get; set; }
        public string ActiveDate { get; set; }
        public string EncryptedPasswordOne { get; set; }
        public string EncryptedPasswordTwo { get; set; }
        public string Remarks { get; set; }
        public string DeviceStatus { get; set; }
    }
}