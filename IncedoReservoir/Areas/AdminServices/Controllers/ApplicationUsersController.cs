using IncedoReservoir.Areas.AdminServices.Models;
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

namespace IncedoReservoir.Areas.AdminServices.Controllers
{
    public class ApplicationUsersController : Controller
    {
        // GET: AdminServices/ApplicationUsers
        public ActionResult Index()
        {
            if (Session["UserID"] != null)
            {
                UserAddEditViewModel objUserAddEditViewModel = new Models.UserAddEditViewModel();
                IncedoReservoirDBContext objIncedoReservoirDBContext = new IncedoReservoirDBContext();
                if ((Session["UserID"] != null) && (Session["AccountID"] != null))
                {
                    var lslService = objIncedoReservoirDBContext.ServiceDefinition.Where(a => a.iStatus == 1).ToList();

                    foreach (var item in lslService)
                    {
                        ServiceDefinitionViewModel lstServiceDefinition = new ServiceDefinitionViewModel();
                        lstServiceDefinition.iServiceDefinitionID = item.iServiceDefinitionID;
                        lstServiceDefinition.sServiceDescription = item.sServiceDescription;
                        objUserAddEditViewModel.lstServiceDefinition.Add(lstServiceDefinition);
                    }

                    var lstPrivilege = objIncedoReservoirDBContext.PrivilegeGroup.Where(a => a.iStatus == 1).ToList();

                    foreach (var item in lstPrivilege)
                    {
                        PrivilegeGroupDetails lstPrivilegeGroup = new PrivilegeGroupDetails();
                        lstPrivilegeGroup.GroupID = item.iGroupID;
                        lstPrivilegeGroup.GroupName = item.sGroupName;
                        objUserAddEditViewModel.lstPrivilegeGroup.Add(lstPrivilegeGroup);
                    }
                }
                return View(objUserAddEditViewModel);
            }
            else
            {
                return Redirect("/Login/SessionLogOut");
            }
        }

        public JsonResult ValidateUserName(string strUserLoginName)
        {
            int iUserStatus = 0;
            if (Session["UserID"] != null)
            {
                IncedoReservoirDBContext objIncedoReservoirDBContext = new IncedoReservoirDBContext();
                var userName = objIncedoReservoirDBContext.AccountUsers.Where(p => p.sLoginName == strUserLoginName).FirstOrDefault();
                if (userName != null && userName.iUserID > 0)
                { iUserStatus = 1; }
            }
            return Json(iUserStatus, JsonRequestBehavior.AllowGet);
        }
        public ActionResult UserDetails(string strUserDisplayNameSearch = "", string strUserLoginNameSearch = "")
        {
            if ((Session["UserID"] != null) && (Session["AccountID"] != null))
            {
                List<ApplicationUsersViewModel> lstApplicationUsers = new List<ApplicationUsersViewModel>();
                IncedoReservoirDBContext objIncedoReservoirDBContext = new IncedoReservoirDBContext();
                int iAccountIDVal = Convert.ToInt32(Session["AccountID"].ToString());
                ApplicationUsersViewModel dataAccountViewModel = new ApplicationUsersViewModel();

                var appUsersDetails = from p in objIncedoReservoirDBContext.AccountUsers
                                      where p.iAccountID == iAccountIDVal
                                      select p;
                int iUserID = Convert.ToInt32(Session["UserID"].ToString());
                if (Session["UserType"].ToString() != "SO-ADMIN")
                {
                    appUsersDetails = appUsersDetails.Where(x => x.iCreatedBy == iUserID).OrderBy(a=> a.dCreatedOn);
                }

                if (strUserDisplayNameSearch != null) appUsersDetails = appUsersDetails.Where(x => x.sDisplayName.Contains(strUserDisplayNameSearch));
                if (strUserLoginNameSearch != null) appUsersDetails = appUsersDetails.Where(x => x.sLoginName.Contains(strUserLoginNameSearch));

                if (appUsersDetails != null)
                {
                    foreach (var item in appUsersDetails.ToList().OrderByDescending(a => a.iUserID))
                    {
                        ApplicationUsersViewModel dataApplicationUsers = new ApplicationUsersViewModel();
                        dataApplicationUsers.iUserID = item.iUserID;
                        dataApplicationUsers.iAccountID = item.iAccountID;
                        dataApplicationUsers.sAccountType = item.sAccountType;
                        dataApplicationUsers.iEmployeeID = item.iEmployeeID;
                        dataApplicationUsers.sEmployeeCode = item.sEmployeeCode;
                        dataApplicationUsers.sDisplayName = item.sDisplayName;
                        dataApplicationUsers.sLoginName = item.sLoginName;
                        dataApplicationUsers.sUserType = item.sUserType;
                        dataApplicationUsers.sLoginPassword = item.sLoginPassword;
                        dataApplicationUsers.iStatus = Convert.ToInt32(item.iStatus);
                        lstApplicationUsers.Add(dataApplicationUsers);
                    }
                }
                return PartialView(lstApplicationUsers);
            }
            else
            {
                return Redirect("/Login/SessionLogOut");
            }
        }

        [HttpPost]
        public JsonResult SaveUserDetails()
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
                            sqlCommand.CommandText = "SetAccountUsers";
                            sqlCommand.Parameters.Add("@RecordId", SqlDbType.Int).Value = Convert.ToInt32(JSONObj["iRecordId"].ToString());
                            sqlCommand.Parameters.Add("@AccountType", SqlDbType.NVarChar, 200).Value = JSONObj["sAccountType"].ToString();
                            sqlCommand.Parameters.Add("@PrivilegeGroupID", SqlDbType.Int).Value = Convert.ToInt32(JSONObj["iPrivilegeGroupId"].ToString());
                            sqlCommand.Parameters.Add("@AssignedServiceList", SqlDbType.NVarChar, 200).Value = JSONObj["sAssignedServiceList"].ToString();
                            sqlCommand.Parameters.Add("@DisplayName", SqlDbType.NVarChar, 200).Value = JSONObj["sDisplayName"].ToString();
                            sqlCommand.Parameters.Add("@LoginName", SqlDbType.NVarChar, 200).Value = JSONObj["sLoginName"].ToString();
                            sqlCommand.Parameters.Add("@UserType", SqlDbType.NVarChar, 200).Value = JSONObj["sUserType"].ToString();
                            sqlCommand.Parameters.Add("@LoginPassword", SqlDbType.NVarChar, 200).Value = JSONObj["sLoginPassword"].ToString();
                            sqlCommand.Parameters.Add("@Status", SqlDbType.Int).Value = Convert.ToInt32(JSONObj["iStatus"].ToString());
                            sqlCommand.Parameters.Add("@LoggedUserId", SqlDbType.Int).Value = Convert.ToInt32(Session["UserID"].ToString());
                            sqlCommand.Parameters.Add("@LoggedAccountId", SqlDbType.Int).Value = Convert.ToInt32(Session["AccountID"].ToString());
                            sqlCommand.Parameters.Add("@Action", SqlDbType.Int).Value = Convert.ToInt32(JSONObj["iAction"].ToString());
                            sqlCommand.Parameters.Add("@ActionStatus", SqlDbType.Int);
                            sqlCommand.Parameters["@ActionStatus"].Direction = ParameterDirection.Output;
                            sqlCommand.ExecuteNonQuery();
                            sqlCommand.Dispose();
                            iActionStatus = Convert.ToInt32(sqlCommand.Parameters["@ActionStatus"].Value);
                        }
                    }
                }

                catch (Exception ex)
                {
                    throw ex;
                }
            }
            return Json(iActionStatus, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult EditUser(int id)
        {
            ApplicationUsersViewModel dataApplicationUsers = new ApplicationUsersViewModel();
            //if ((Session["UserID"] != null) && (Session["AccountID"] != null))
            {
                if (id > 0)
                {

                    //if (( ! string.IsNullOrEmpty(strEmployeeCode)) || (! string.IsNullOrEmpty( strUserLoginName )))
                    //{
                    IncedoReservoirDBContext objIncedoReservoirDBContext = new IncedoReservoirDBContext();
                    int iAccountIDVal = Convert.ToInt32(Session["AccountID"].ToString());
                    ApplicationUsersViewModel dataAccountViewModel = new ApplicationUsersViewModel();

                    var appUsersDetails = objIncedoReservoirDBContext.AccountUsers
                        .Where(a => a.iUserID == id).FirstOrDefault();

                    if (appUsersDetails != null)
                    {
                        dataApplicationUsers.iUserID = appUsersDetails.iUserID;
                        dataApplicationUsers.iAccountID = appUsersDetails.iAccountID;
                        dataApplicationUsers.sAccountType = appUsersDetails.sAccountType;
                        dataApplicationUsers.iEmployeeID = appUsersDetails.iEmployeeID;
                        dataApplicationUsers.sEmployeeCode = appUsersDetails.sEmployeeCode;
                        dataApplicationUsers.sDisplayName = appUsersDetails.sDisplayName;
                        dataApplicationUsers.sLoginName = appUsersDetails.sLoginName;
                        dataApplicationUsers.sUserType = appUsersDetails.sUserType;
                        dataApplicationUsers.sLoginPassword = appUsersDetails.sLoginPassword;
                        dataApplicationUsers.iStatus = Convert.ToInt32(appUsersDetails.iStatus);
                    }

                    var appUsersServices = objIncedoReservoirDBContext.ServiceAccountUserPrivilegeGroupMapping
                       .Where(a => a.iUserID == id && a.iStatus == 1).ToList();

                    dataApplicationUsers.tempAssignedServices = String.Join(",", appUsersServices.Select(a => a.iServiceDefinitionID.ToString()));
                    dataApplicationUsers.tempAssignedPG = appUsersServices.Select(a => a.iPrivilegeGroup).FirstOrDefault().ToString();

                }
                else
                {
                    //return RedirectToAction("SessionLogOut", "Account");
                }
            }
            return Json(dataApplicationUsers, JsonRequestBehavior.AllowGet); ;
            //return JsonConvert.SerializeObject(dataApplicationUsers);
            // return Json(dataApplicationUsers, JsonRequestBehavior.AllowGet);
        }
        public ActionResult TreeMenuNodes()
        {
            if (Session["UserID"] != null)
            {
                AdminDashboardViewModel lstTreeNodeListViewModel = new AdminDashboardViewModel();
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