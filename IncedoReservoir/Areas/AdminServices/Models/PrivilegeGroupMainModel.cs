using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IncedoReservoir.Areas.AdminServices.Models
{
    public class PrivilegeGroupMainModel
    {
        public List<GetPrivilegeGroupNameViewModel> lstGetPrivilegeGroupName { get; set; }
        public List<ServiceDefinitionViewModel> lstServiceDefinition { get; set; }
        public List<PrivilegeAvilableViewModel> lstPrivilegeAvilable { get; set; }

        public List<MasterServicesViewModelAdmin> lstMasterServices { get; set; }
    }
}