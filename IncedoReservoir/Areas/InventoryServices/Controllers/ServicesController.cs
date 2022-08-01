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
    public class ServicesController : Controller
    {
        // GET: InventoryServices/Services
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

        public ActionResult ServiceDetails(int iServiceId = 0, string sServiceName = "", int iAction = 1)
        {
            if (Session["UserID"] != null)
            {
                List<MasterServicesViewModel> objMasterServicesViewModel = new List<MasterServicesViewModel>();
                IncedoReservoirDBContext objIncedoReservoirDBContext = new IncedoReservoirDBContext();
                if ((Session["UserID"] != null) && (Session["AccountID"] != null))
                {
                    // var lslService = objIncedoReservoirDBContext.MasterServices.Where(a => a.bStatus == true).ToList();

                    var lslService = from p in objIncedoReservoirDBContext.MasterServices
                                     where p.bStatus == true
                                     select p;

                    if (iServiceId != 0) lslService = lslService.Where(x => x.iServiceId == iServiceId);
                    if (!string.IsNullOrEmpty(sServiceName)) lslService = lslService.Where(x => x.sServiceName.Contains(sServiceName));

                    int iUserID = Convert.ToInt32(Session["UserID"].ToString());
                    if (Session["UserType"].ToString() != "SO-ADMIN")
                    {
                        lslService = lslService.Where(x => x.iCreatedBy == iUserID).OrderBy(a => a.dCreatedOn);
                    }


                    foreach (var item in lslService)
                    {
                        MasterServicesViewModel dataMasterServicesViewModel = new MasterServicesViewModel();
                        dataMasterServicesViewModel.ServiceId = item.iServiceId;
                        dataMasterServicesViewModel.ServiceName = item.sServiceName;
                        objMasterServicesViewModel.Add(dataMasterServicesViewModel);
                    }
                }
                return View(objMasterServicesViewModel);
            }
            else
            {
                return Redirect("/Login/SessionLogOut");
            }
        }

        public JsonResult ValidateServiceName(string strServiceName)
        {
            int iUserStatus = 0;
            if (Session["UserID"] != null)
            {
                IncedoReservoirDBContext objIncedoReservoirDBContext = new IncedoReservoirDBContext();
                var userName = objIncedoReservoirDBContext.MasterServices.Where(p => p.sServiceName == strServiceName && p.bStatus == true).FirstOrDefault();
                if (userName != null && userName.iServiceId > 0)
                { iUserStatus = 1; }
            }
            return Json(iUserStatus, JsonRequestBehavior.AllowGet);
        }
        public JsonResult SaveServiceDetails()
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
                            sqlCommand.CommandText = "SetServiceDetails";
                            sqlCommand.Parameters.Add("@iRecordId", SqlDbType.Int).Value = Convert.ToInt32(JSONObj["iRecordId"].ToString());
                            sqlCommand.Parameters.Add("@sServiceName", SqlDbType.NVarChar, 200).Value = JSONObj["strServiceName"].ToString();
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

        public JsonResult EditServiceDataLoad(int iServiceId = 0)
        {
            MasterServicesViewModel objMasterServiceViewModel = new MasterServicesViewModel();
            using (SqlConnection dbConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["IncedoConnection"].ConnectionString))
            {
                try
                {
                    IncedoReservoirDBContext objIncedoReservoirDBContext = new IncedoReservoirDBContext();
                    if ((Session["UserID"] != null) && (Session["AccountID"] != null))
                    {
                        var lslService = objIncedoReservoirDBContext.MasterServices.Where(a => a.bStatus == true && a.iServiceId == iServiceId).FirstOrDefault();
                        objMasterServiceViewModel.ServiceId = lslService.iServiceId;
                        objMasterServiceViewModel.ServiceName = lslService.sServiceName;
                    }
                }
                catch (Exception Ex)
                {
                    Logger.Log(new LogEntry(LogType.ERROR, "Error : - " + Ex.Message + "------------"
                        + Ex.InnerException + Ex.StackTrace));
                }
            }
            ViewBag.NextSearchCondition = 1;
            return Json(objMasterServiceViewModel, JsonRequestBehavior.AllowGet);
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