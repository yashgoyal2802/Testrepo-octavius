using System.Web.Mvc;

namespace IncedoReservoir.Areas.InventoryServices
{
    public class InventoryServicesAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "InventoryServices";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "InventoryServices_default",
                "InventoryServices/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}