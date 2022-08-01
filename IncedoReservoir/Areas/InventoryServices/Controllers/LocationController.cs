using IncedoReservoir.Areas.InventoryServices.Models;
using IncedoReservoir.DBContext;
using IncedoReservoir.Models;
using LoggerUtility;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace IncedoReservoir.Areas.InventoryServices.Controllers
{
    public class LocationController : Controller
    {
        // GET: InventoryServices/Location
        public ActionResult Index()
        {
            if (Session["UserID"] != null)
            {                
                return View();
            }
            else
            {
                return Redirect("/Login/SessionLogOut");
            }
        }

        public JsonResult SaveLocationDetails()
        {
            int iActionStatus = 0;
            var resolveRequest = HttpContext.Request;
            string jsonString = new StreamReader(resolveRequest.InputStream).ReadToEnd();
            var JSONObj = new JavaScriptSerializer().Deserialize<Dictionary<string, string>>(jsonString);

            if ((Session["UserID"] != null) && (Session["AccountID"] != null))
            {
                try
                {
                    if (JSONObj != null)
                    {
                        using (SqlConnection dbConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["IncedoConnection"].ConnectionString))
                        {
                            dbConnection.Open();
                            SqlCommand sqlCommand = new SqlCommand();
                            sqlCommand.Connection = dbConnection;
                            sqlCommand.CommandType = CommandType.StoredProcedure;
                            sqlCommand.CommandText = "SetLocationDetails";
                            sqlCommand.Parameters.Add("@iRecordId", SqlDbType.Int).Value = Convert.ToInt32(JSONObj["iRecordId"].ToString());
                            sqlCommand.Parameters.Add("@sLocationName", SqlDbType.NVarChar, 200).Value = JSONObj["strLocationName"].ToString();
                            sqlCommand.Parameters.Add("@iAccountID", SqlDbType.Int).Value = Convert.ToInt32(Session["AccountID"].ToString());
                            sqlCommand.Parameters.Add("@iUserId", SqlDbType.Int).Value = Convert.ToInt32(Session["UserId"].ToString());
                            sqlCommand.Parameters.Add("@iAction", SqlDbType.Int).Value = Convert.ToInt32(JSONObj["iAction"].ToString());
                            sqlCommand.Parameters.Add("@iActionStatus", SqlDbType.Int);
                            sqlCommand.Parameters["@iActionStatus"].Direction = ParameterDirection.Output;
                            sqlCommand.ExecuteNonQuery();
                            sqlCommand.Dispose();
                            iActionStatus = Convert.ToInt32(sqlCommand.Parameters["@iActionStatus"].Value);
                        }
                    }
                }

                catch (Exception Ex)
                {
                    Logger.Log(new LogEntry(LogType.ERROR, "Error : - " + Ex.Message + "------------"
                        + Ex.InnerException + Ex.StackTrace));
                }
            }
            return Json(iActionStatus, JsonRequestBehavior.AllowGet);
        }

        public JsonResult ValidateLocationName(string strLocationName)
        {
            int iUserStatus = 0;
            if (Session["UserID"] != null)
            {
                IncedoReservoirDBContext objIncedoReservoirDBContext = new IncedoReservoirDBContext();
                var userName = objIncedoReservoirDBContext.MasterLocation.Where(p => p.sLocationName == strLocationName && p.bStatus == true).FirstOrDefault();
                if (userName != null && userName.iLocationId > 0)
                { iUserStatus = 1; }
            }
            return Json(iUserStatus, JsonRequestBehavior.AllowGet);
        }

        public ActionResult LocationDetails(int iLocationId = 0, string sLocation = "", int iAction = 1)
        {
            if (Session["UserID"] != null)
            {
                List<MasterLocationViewModel> objMasterLocationViewModel = new List<MasterLocationViewModel>();
                IncedoReservoirDBContext objIncedoReservoirDBContext = new IncedoReservoirDBContext();
                if ((Session["UserID"] != null) && (Session["AccountID"] != null))
                {


                    var lstLocation = from p in objIncedoReservoirDBContext.MasterLocation
                                      where p.bStatus == true
                                      select p;
                    if (iLocationId != 0) lstLocation = lstLocation.Where(x => x.iLocationId == iLocationId);
                    if (!string.IsNullOrEmpty(sLocation)) lstLocation = lstLocation.Where(x => x.sLocationName.Contains(sLocation));


                    int iUserID = Convert.ToInt32(Session["UserID"].ToString());
                    if (Session["UserType"].ToString() != "SO-ADMIN")
                    {
                        lstLocation = lstLocation.Where(x => x.iCreatedBy == iUserID).OrderBy(a => a.dCreatedOn);
                    }

                    foreach (var item in lstLocation)
                    {
                        MasterLocationViewModel lstServiceDefinition = new MasterLocationViewModel();
                        lstServiceDefinition.LocationId = item.iLocationId;
                        lstServiceDefinition.LocationName = item.sLocationName;
                        objMasterLocationViewModel.Add(lstServiceDefinition);
                    }
                }
                return View(objMasterLocationViewModel);
            }
            else
            {
                return Redirect("/Login/SessionLogOut");
            }
        }

        public JsonResult EditLocationDataLoad(int iLocationId = 0)
        {
            MasterLocationViewModel objMasterLocationViewModel = new MasterLocationViewModel();
            using (SqlConnection dbConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["IncedoConnection"].ConnectionString))
            {
                try
                { 
                    IncedoReservoirDBContext objIncedoReservoirDBContext = new IncedoReservoirDBContext();
                    if ((Session["UserID"] != null) && (Session["AccountID"] != null))
                    {
                        var lslService = objIncedoReservoirDBContext.MasterLocation.Where(a => a.bStatus == true && a.iLocationId== iLocationId).FirstOrDefault();  
                        objMasterLocationViewModel.LocationId = lslService.iLocationId;
                        objMasterLocationViewModel.LocationName = lslService.sLocationName;
                    }
                }
                catch (Exception Ex)
                {
                    Logger.Log(new LogEntry(LogType.ERROR, "Error : - " + Ex.Message + "------------"
                        + Ex.InnerException + Ex.StackTrace));
                }
            }
            ViewBag.NextSearchCondition = 1;
            return Json(objMasterLocationViewModel, JsonRequestBehavior.AllowGet);
        }

        public ActionResult TreeMenuNodes()
        {
            if (Session["UserID"] != null)
            {
                InventoryDashboardViewModel lstTreeNodeListViewModel = new InventoryDashboardViewModel();
                using (SqlConnection dbConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["IncedoConnection"].ConnectionString))
                {
                    try
                    {
                        dbConnection.Open();
                        SqlCommand sqlCommand = new SqlCommand();
                        sqlCommand.Connection = dbConnection;
                        sqlCommand.CommandType = CommandType.StoredProcedure;
                        sqlCommand.CommandText = "GetMasterTreeParentNode";
                        sqlCommand.Parameters.Add("@iAccountID", SqlDbType.Int).Value = Convert.ToInt32(Session["AccountID"].ToString());
                        sqlCommand.Parameters.Add("@iUserID", SqlDbType.Int).Value = Convert.ToInt32(Session["UserID"].ToString());
                        sqlCommand.Parameters.Add("@iGroupID", SqlDbType.Int).Value = 1;
                        sqlCommand.Parameters.Add("@iServiceDefinitionID", SqlDbType.Int).Value = Convert.ToInt32(Session["ServiceID"].ToString());
                        sqlCommand.Parameters.Add("@iAction", SqlDbType.Int).Value = 1;

                        SqlDataAdapter dataAdapter = new SqlDataAdapter(sqlCommand);
                        DataSet dataSetObject = new DataSet();
                        dataAdapter.Fill(dataSetObject);

                        if (dataSetObject.Tables[0].Rows.Count > 0)
                        {
                            for (int iCout = 0; iCout < dataSetObject.Tables[0].Rows.Count; iCout++)
                            {
                                TreeNodeList objTreeNodeList = new TreeNodeList();

                                objTreeNodeList.iParentID = Convert.ToInt32(dataSetObject.Tables[0].Rows[iCout]["iParentID"].ToString());
                                objTreeNodeList.iPrivilegeID = Convert.ToInt32(dataSetObject.Tables[0].Rows[iCout]["iPrivilegeID"].ToString());
                                objTreeNodeList.sDescription = dataSetObject.Tables[0].Rows[iCout]["sDescription"].ToString();
                                objTreeNodeList.sFileName = dataSetObject.Tables[0].Rows[iCout]["sFileName"].ToString();
                                objTreeNodeList.sFunctionalLevel = dataSetObject.Tables[0].Rows[iCout]["sFunctionalLevel"].ToString();
                                objTreeNodeList.spagefunctionallevel = dataSetObject.Tables[0].Rows[iCout]["spagefunctionallevel"].ToString();
                                objTreeNodeList.SPLevelMappingID = dataSetObject.Tables[0].Rows[iCout]["SPLevelMappingID"].ToString();
                                objTreeNodeList.sCssClassName = dataSetObject.Tables[0].Rows[iCout]["sCssClassName"].ToString();
                                lstTreeNodeListViewModel.lstTreeNodeList.Add(objTreeNodeList);
                            }
                        }

                    }
                    catch (Exception Ex)
                    {
                        Logger.Log(new LogEntry(LogType.ERROR, "Error : - " + Ex.InnerException + Ex.StackTrace));
                    }
                }

                ViewBag.NextSearchCondition = 1;
                return PartialView(lstTreeNodeListViewModel);
            }
            else
            {
                return Redirect("/Login/SessionLogOut");
            }
        }
    }
}