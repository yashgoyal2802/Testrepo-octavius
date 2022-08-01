using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IncedoReservoir.Areas.AdminServices.Models
{
    public class UserAddEditViewModel
    {
        public List<ServiceDefinitionViewModel> lstServiceDefinition = new List<ServiceDefinitionViewModel>();
        public List<PrivilegeGroupDetails> lstPrivilegeGroup = new List<PrivilegeGroupDetails>();
    }
}