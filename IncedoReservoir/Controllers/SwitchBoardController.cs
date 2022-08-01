using IncedoReservoir.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace IncedoReservoir.Controllers
{
    public class SwitchBoardController : Controller
    {
        // GET: SwitchBoard
        public ActionResult Index()
        {
            ServiceAccountUserPrivilegeViewModel lstServiceAccountUserPrivilegeViewModel = new ServiceAccountUserPrivilegeViewModel();

            if (Session["UserID"] != null)
            {
                using (SqlConnection dbConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["IncedoConnection"].ConnectionString))
                {
                    try
                    {
                        dbConnection.Open();
                        SqlCommand sqlCommand = new SqlCommand();
                        sqlCommand.Connection = dbConnection;
                        sqlCommand.CommandType = CommandType.StoredProcedure;
                        sqlCommand.CommandText = "ServiceDefinition_GetAllAndUserAssignedService";
                        sqlCommand.Parameters.Add("@iAccountID", SqlDbType.Int).Value = Convert.ToInt32(Session["AccountID"].ToString());
                        sqlCommand.Parameters.Add("@iUserID", SqlDbType.Int).Value = Convert.ToInt32(Session["UserID"].ToString());
                        sqlCommand.Parameters.Add("@iServiceDefinitionID", SqlDbType.Int).Value = 1;
                        sqlCommand.Parameters.Add("@sAccountType", SqlDbType.NVarChar, 50).Value = Session["AccountType"].ToString();
                        sqlCommand.Parameters.Add("@iAction", SqlDbType.Int).Value = 1;
                        SqlDataAdapter dataAdapter = new SqlDataAdapter(sqlCommand);
                        DataSet dataSetObject = new DataSet();
                        dataAdapter.Fill(dataSetObject);

                        if (dataSetObject.Tables[0].Rows.Count > 0)//Case of Electric Data
                        {
                            for (int iCout = 0; iCout < dataSetObject.Tables[0].Rows.Count; iCout++)
                            {
                                ServiceAccountUserPrivilege objRowData = new ServiceAccountUserPrivilege();
                                objRowData.dServicesUsesExpiryDate = dataSetObject.Tables[0].Rows[iCout]["dServicesUsesExpiryDate"].ToString();
                                objRowData.dServicesUsesStartDate = dataSetObject.Tables[0].Rows[iCout]["dServicesUsesStartDate"].ToString();
                                objRowData.iPrivilegeGroup = Convert.ToInt32(dataSetObject.Tables[0].Rows[iCout]["iPrivilegeGroup"].ToString());
                                objRowData.iServiceDefinitionID = Convert.ToInt32(dataSetObject.Tables[0].Rows[iCout]["iServiceDefinitionID"].ToString());
                                objRowData.itid = Convert.ToInt32(dataSetObject.Tables[0].Rows[iCout]["itid"].ToString());
                                objRowData.sHostName = dataSetObject.Tables[0].Rows[iCout]["sHostName"].ToString();
                                objRowData.sServiceDescription = dataSetObject.Tables[0].Rows[iCout]["sServiceDescription"].ToString();
                                objRowData.sServiceName = dataSetObject.Tables[0].Rows[iCout]["sServiceName"].ToString();
                                objRowData.sServiceStartURL = dataSetObject.Tables[0].Rows[iCout]["sServiceStartURL"].ToString();
                                objRowData.sServiceVersionNo = dataSetObject.Tables[0].Rows[iCout]["sServiceVersionNo"].ToString();
                                objRowData.sServiceLogo = dataSetObject.Tables[0].Rows[iCout]["sServiceLogo"].ToString();
                                lstServiceAccountUserPrivilegeViewModel.lstServiceAccountUserPrivilege.Add(objRowData);
                            }
                        }
                    }
                    catch (Exception Ex)
                    {
                        //Logger.Log(new LogEntry(LogType.ERROR, "Error : - " + Ex.InnerException + Ex.StackTrace));
                    }
                }
                //CustomerSearchViewModels returnModel = new CustomerSearchViewModels();
                //returnModel = CustomerDetailsReturn(strUtilityAccountNo, strPhoneNo);
                //ViewBag.NextSearchCondition = 0;               
            }
            else
            {
                return Redirect("/Login/SessionLogOut");
            }
            return View(lstServiceAccountUserPrivilegeViewModel);
        }
    }
}