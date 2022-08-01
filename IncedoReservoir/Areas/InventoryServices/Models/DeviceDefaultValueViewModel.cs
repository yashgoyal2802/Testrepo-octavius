using IncedoReservoir.Areas.InventoryServices.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IncedoReservoir.Areas.InventoryServices.Models
{ 
    public class DeviceDefaultValueViewModel
    {
        public List<MasterLocationViewModel> lstMasterLocation = new List<MasterLocationViewModel>();
        public List<MasterServicesViewModel> lstMasterService = new List<MasterServicesViewModel>();
    }
}