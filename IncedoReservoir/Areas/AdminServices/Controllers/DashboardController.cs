using IncedoReservoir.Areas.AdminServices.Models;
using IncedoReservoir.Models;
using LoggerUtility;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace IncedoReservoir.Areas.AdminServices.Controllers
{
    public class DashboardController : Controller
    {
        // GET: AdminServices/Dashboard
        public ActionResult Index(string serviceID)
        {
            if (Session["UserID"] != null)
            {
                ViewBag.ServiceID = serviceID;
                Session["ServiceID"] = serviceID;
                return View();
            }
            else
            {
                return Redirect("/Login/SessionLogOut");
            }
        }
        public ActionResult TreeMenuNodes(string serviceID)
        {
            string abc = ViewBag.ServiceID;
            if (Session["UserID"] != null)
            {
                Session["ServiceID"] = serviceID.ToString();
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
                        sqlCommand.Parameters.Add("@iServiceDefinitionID", SqlDbType.Int).Value = Convert.ToInt32(serviceID);
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