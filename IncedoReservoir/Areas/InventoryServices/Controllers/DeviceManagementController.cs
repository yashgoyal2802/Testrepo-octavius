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
    public class DeviceManagementController : Controller
    {
        // GET: InventoryServices/DeviceManagement
        public ActionResult Index()
        {
            DeviceDefaultValueViewModel deviceDefaultValue = new DeviceDefaultValueViewModel();
            if (Session["AccountID"] != null)
            {
                IncedoReservoirDBContext objInventoryDBContext = new IncedoReservoirDBContext();
                int iAccountIDVal = Convert.ToInt32(Session["AccountID"].ToString());

                var masterLocationlst = objInventoryDBContext.MasterLocation.Where(a => a.bStatus == true).ToList();

                foreach (var item in masterLocationlst)
                {
                    MasterLocationViewModel objMasterLocation = new MasterLocationViewModel();
                    objMasterLocation.LocationId = item.iLocationId;
                    objMasterLocation.LocationName = item.sLocationName;
                    deviceDefaultValue.lstMasterLocation.Add(objMasterLocation);
                }

                using (SqlConnection dbConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["IncedoConnection"].ConnectionString))
                {
                    try
                    {
                        dbConnection.Open();
                        SqlCommand sqlCommand = new SqlCommand();
                        sqlCommand.Connection = dbConnection;
                        sqlCommand.CommandType = CommandType.StoredProcedure;
                        sqlCommand.CommandText = "GetMasterServicesAssigned";
                        sqlCommand.Parameters.Add("@iAccountID", SqlDbType.Int).Value = Convert.ToInt32(Session["AccountID"].ToString());
                        sqlCommand.Parameters.Add("@iUserID", SqlDbType.Int).Value = Convert.ToInt32(Session["UserID"].ToString());
                        sqlCommand.Parameters.Add("@iGroupID", SqlDbType.Int).Value = 1;
                        sqlCommand.Parameters.Add("@iAction", SqlDbType.Int).Value = 1;

                        SqlDataAdapter dataAdapter = new SqlDataAdapter(sqlCommand);
                        DataSet dataSetObject = new DataSet();
                        dataAdapter.Fill(dataSetObject);
                        if (dataSetObject.Tables[0].Rows.Count > 0)
                        {
                            for (int iCout = 0; iCout < dataSetObject.Tables[0].Rows.Count; iCout++)
                            {
                                MasterServicesViewModel objServiceLocation = new MasterServicesViewModel();
                                objServiceLocation.ServiceId = Convert.ToInt32(dataSetObject.Tables[0].Rows[iCout]["iServiceId"].ToString()); ;
                                objServiceLocation.ServiceName = dataSetObject.Tables[0].Rows[iCout]["sServiceName"].ToString();
                                deviceDefaultValue.lstMasterService.Add(objServiceLocation);
                            }
                        }
                    }
                    catch (Exception Ex)
                    {
                        Logger.Log(new LogEntry(LogType.ERROR, "Error : - " + Ex.InnerException + Ex.StackTrace));
                    }
                }
                //var masterServicelst = objInventoryDBContext.MasterServices.Where(a => a.bStatus == true).ToList();

                //foreach (var item in masterServicelst)
                //{
                //    MasterServicesViewModel objServiceLocation = new MasterServicesViewModel();
                //    objServiceLocation.ServiceId = item.iServiceId;
                //    objServiceLocation.ServiceName = item.sServiceName;
                //    deviceDefaultValue.lstMasterService.Add(objServiceLocation);
                //}
            }
            else { return Redirect("/Login/SessionLogOut"); }
            return View(deviceDefaultValue);
        }

        public JsonResult ValidateDeviceName(string strDeviceName)
        {
            int iUserStatus = 0;
            if (Session["UserID"] != null)
            {
                IncedoReservoirDBContext objInventoryDBContext = new IncedoReservoirDBContext();
                var deviceName = objInventoryDBContext.DevicesDetails.Where(p => p.sDeviceName == strDeviceName).FirstOrDefault();
                if (deviceName != null && deviceName.iDeviceId > 0)
                { iUserStatus = 1; }
            }
            return Json(iUserStatus, JsonRequestBehavior.AllowGet);
        }

        public ActionResult DeviceDetails(int iDeviceId = 0, int iServiceId = 0, int iLocationId = 0, string sDeviceName = "", int iAction = 1)
        {
            if ((Session["UserID"] != null) && (Session["UserType"] != null))
            {
                List<DevicesDetailsViewModel> lstTreeNodeListViewModel = new List<DevicesDetailsViewModel>();
                using (SqlConnection dbConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["IncedoConnection"].ConnectionString))
                {
                    try
                    {
                        dbConnection.Open();
                        SqlCommand sqlCommand = new SqlCommand();
                        sqlCommand.Connection = dbConnection;
                        sqlCommand.CommandType = CommandType.StoredProcedure;
                        sqlCommand.CommandText = "GetDevicesDetails";
                        sqlCommand.Parameters.Add("@iUserID", SqlDbType.Int).Value = Convert.ToInt32(Session["UserID"].ToString());
                        sqlCommand.Parameters.Add("@iDeviceId", SqlDbType.Int).Value = iDeviceId;
                        sqlCommand.Parameters.Add("@iServiceId", SqlDbType.Int).Value = iServiceId;
                        sqlCommand.Parameters.Add("@iLocationId", SqlDbType.Int).Value = iLocationId;
                        sqlCommand.Parameters.Add("@sDeviceName", SqlDbType.NVarChar).Value = sDeviceName;
                        sqlCommand.Parameters.Add("@UserType", SqlDbType.NVarChar).Value = Session["UserType"].ToString();
                        sqlCommand.Parameters.Add("@iAction", SqlDbType.Int).Value = iAction;

                        SqlDataAdapter dataAdapter = new SqlDataAdapter(sqlCommand);
                        DataSet dataSetObject = new DataSet();
                        dataAdapter.Fill(dataSetObject);

                        if (dataSetObject.Tables[0].Rows.Count > 0)
                        {
                            for (int iCout = 0; iCout < dataSetObject.Tables[0].Rows.Count; iCout++)
                            {
                                DevicesDetailsViewModel objInventoryDashboardViewModel = new DevicesDetailsViewModel();

                                objInventoryDashboardViewModel.DeviceId = Convert.ToInt32(dataSetObject.Tables[0].Rows[iCout]["DeviceId"].ToString());
                                objInventoryDashboardViewModel.ServiceName = dataSetObject.Tables[0].Rows[iCout]["ServiceName"].ToString();
                                objInventoryDashboardViewModel.DeviceName = dataSetObject.Tables[0].Rows[iCout]["DeviceName"].ToString();
                                objInventoryDashboardViewModel.LocationName = dataSetObject.Tables[0].Rows[iCout]["LocationName"].ToString();
                                objInventoryDashboardViewModel.DeviceStatus = dataSetObject.Tables[0].Rows[iCout]["DeviceStatus"].ToString();
                                objInventoryDashboardViewModel.ActiveDate = dataSetObject.Tables[0].Rows[iCout]["ActiveDate"].ToString();
                                objInventoryDashboardViewModel.EncryptedPasswordOne = dataSetObject.Tables[0].Rows[iCout]["EncryptedPasswordOne"].ToString();
                                objInventoryDashboardViewModel.EncryptedPasswordTwo = dataSetObject.Tables[0].Rows[iCout]["EncryptedPasswordTwo"].ToString();
                                objInventoryDashboardViewModel.DeviceUserName = dataSetObject.Tables[0].Rows[iCout]["DeviceUserName"].ToString();

                                objInventoryDashboardViewModel.Remarks = dataSetObject.Tables[0].Rows[iCout]["Remarks"].ToString();
                                lstTreeNodeListViewModel.Add(objInventoryDashboardViewModel);
                            }
                        }

                    }
                    catch (Exception Ex)
                    {
                        Logger.Log(new LogEntry(LogType.ERROR, "Error : - " + Ex.Message + "------------"
                            + Ex.InnerException + Ex.StackTrace));
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

        public JsonResult EditDeviceDataLoad(int iDeviceId = 0)
        {

            DevicesDetailsViewModel objInventoryDashboardViewModel = new DevicesDetailsViewModel();
            using (SqlConnection dbConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["IncedoConnection"].ConnectionString))
            {
                try
                {
                    dbConnection.Open();
                    SqlCommand sqlCommand = new SqlCommand();
                    sqlCommand.Connection = dbConnection;
                    sqlCommand.CommandType = CommandType.StoredProcedure;
                    sqlCommand.CommandText = "GetDevicesDetails";
                    sqlCommand.Parameters.Add("@iUserID", SqlDbType.Int).Value = Convert.ToInt32(Session["UserID"].ToString());
                    sqlCommand.Parameters.Add("@iDeviceId", SqlDbType.Int).Value = iDeviceId;
                    sqlCommand.Parameters.Add("@iServiceId", SqlDbType.Int).Value = 0;
                    sqlCommand.Parameters.Add("@iLocationId", SqlDbType.Int).Value = 0;
                    sqlCommand.Parameters.Add("@sDeviceName", SqlDbType.NVarChar).Value = "";
                    sqlCommand.Parameters.Add("@UserType", SqlDbType.NVarChar).Value = Session["UserType"].ToString();
                    sqlCommand.Parameters.Add("@iAction", SqlDbType.Int).Value = 2;

                    SqlDataAdapter dataAdapter = new SqlDataAdapter(sqlCommand);
                    DataSet dataSetObject = new DataSet();
                    dataAdapter.Fill(dataSetObject);

                    if (dataSetObject.Tables[0].Rows.Count > 0)
                    {
                        for (int iCout = 0; iCout < dataSetObject.Tables[0].Rows.Count; iCout++)
                        {
                            objInventoryDashboardViewModel.DeviceId = Convert.ToInt32(dataSetObject.Tables[0].Rows[iCout]["DeviceId"].ToString());
                            objInventoryDashboardViewModel.ServiceName = dataSetObject.Tables[0].Rows[iCout]["ServiceId"].ToString();
                            objInventoryDashboardViewModel.DeviceName = dataSetObject.Tables[0].Rows[iCout]["DeviceName"].ToString();
                            objInventoryDashboardViewModel.LocationName = dataSetObject.Tables[0].Rows[iCout]["LocationId"].ToString();
                            objInventoryDashboardViewModel.DeviceStatus = dataSetObject.Tables[0].Rows[iCout]["DeviceStatus"].ToString();
                            objInventoryDashboardViewModel.ActiveDate = dataSetObject.Tables[0].Rows[iCout]["ActiveDate"].ToString();

                            objInventoryDashboardViewModel.EncryptedPasswordOne = dataSetObject.Tables[0].Rows[iCout]["EncryptedPasswordOne"].ToString();
                            objInventoryDashboardViewModel.EncryptedPasswordTwo = dataSetObject.Tables[0].Rows[iCout]["EncryptedPasswordTwo"].ToString();
                            objInventoryDashboardViewModel.DeviceUserName = dataSetObject.Tables[0].Rows[iCout]["DeviceUserName"].ToString();


                            objInventoryDashboardViewModel.Remarks = dataSetObject.Tables[0].Rows[iCout]["Remarks"].ToString();

                            Logger.Log(new LogEntry(LogType.INFORMATION, "EN Password " + dataSetObject.Tables[0].Rows[iCout]["ActiveDate"].ToString()));
                            Logger.Log(new LogEntry(LogType.INFORMATION, "EN Password " + dataSetObject.Tables[0].Rows[iCout]["EncryptedPassword"].ToString()));
                        }
                    }
                }
                catch (Exception Ex)
                {
                    Logger.Log(new LogEntry(LogType.ERROR, "Error : - " + Ex.Message + "------------"
                        + Ex.InnerException + Ex.StackTrace));
                }
            }

            ViewBag.NextSearchCondition = 1;
            return Json(objInventoryDashboardViewModel, JsonRequestBehavior.AllowGet);

        }

        [HttpPost]
        public JsonResult SaveDeviceDetails()
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
                            sqlCommand.CommandText = "SetDevicesDetails";
                            sqlCommand.Parameters.Add("@iRecordId", SqlDbType.Int).Value = Convert.ToInt32(JSONObj["RecordId"].ToString());
                            sqlCommand.Parameters.Add("@iServiceId", SqlDbType.Int).Value = JSONObj["ServiceId"].ToString();
                            sqlCommand.Parameters.Add("@sDeviceName", SqlDbType.NVarChar, 200).Value = JSONObj["DeviceName"].ToString();
                            sqlCommand.Parameters.Add("@sDeviceUserName", SqlDbType.NVarChar, 200).Value = JSONObj["DeviceUserName"].ToString();
                            sqlCommand.Parameters.Add("@sPasswordOne", SqlDbType.NVarChar, 200).Value = JSONObj["PasswordOne"].ToString();
                            sqlCommand.Parameters.Add("@sPasswordTwo", SqlDbType.NVarChar, 200).Value = JSONObj["PasswordTwo"].ToString();
                            sqlCommand.Parameters.Add("@iLocationId", SqlDbType.Int).Value = Convert.ToInt32(JSONObj["LocationId"].ToString());
                            sqlCommand.Parameters.Add("@iDeviceStatusId", SqlDbType.Int).Value = Convert.ToInt32(JSONObj["DeviceStatusId"].ToString());
                            sqlCommand.Parameters.Add("@dActiveDate", SqlDbType.NVarChar).Value = JSONObj["ActiveDate"].ToString();
                            sqlCommand.Parameters.Add("@sRemarks", SqlDbType.NVarChar, 500).Value = JSONObj["Remarks"].ToString();
                            sqlCommand.Parameters.Add("@iUserId", SqlDbType.Int).Value = Convert.ToInt32(Session["UserId"].ToString());
                            sqlCommand.Parameters.Add("@iAction", SqlDbType.Int).Value = Convert.ToInt32(JSONObj["Action"].ToString());
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
                        Logger.Log(new LogEntry(LogType.ERROR, "Error : - " + Ex.Message + "------------"
                            + Ex.InnerException + Ex.StackTrace));
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