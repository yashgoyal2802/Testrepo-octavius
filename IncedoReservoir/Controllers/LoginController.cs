using LoggerUtility;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace IncedoReservoir.Controllers
{
    public class LoginController : Controller
    {
        // GET: Login
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult UserLogin()
        {
            ModelState.Clear();
            Session.Abandon();
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        public int UserLogin(string username, string password)
        {
            using (SqlConnection sqlConn = new SqlConnection(ConfigurationManager.ConnectionStrings["IncedoConnection"].ConnectionString))
            {
                try
                {

                    DataSet dsLoginDetails = new DataSet();
                    sqlConn.Open();
                    SqlCommand sqlComm = sqlConn.CreateCommand();
                    sqlComm.CommandText = "LoginDetails_GetAndValidate";
                    sqlComm.CommandType = CommandType.StoredProcedure;
                    sqlComm.CommandTimeout = 180;
                    sqlComm.Parameters.Add("@sAccountName", SqlDbType.NVarChar, 50).Value = "SO-Account";
                    sqlComm.Parameters.Add("@sLoginName", SqlDbType.NVarChar, 50).Value = username;
                    sqlComm.Parameters.Add("@sPassword", SqlDbType.NVarChar, 50).Value = password;
                    sqlComm.Parameters.Add("@sUserReferenceID", SqlDbType.NVarChar, 200).Direction = ParameterDirection.Output;
                    sqlComm.Parameters.Add("@sOutMsg", SqlDbType.NVarChar, 500).Direction = ParameterDirection.Output;
                    sqlComm.Parameters.Add("@iAccountStatus", SqlDbType.Int).Direction = ParameterDirection.Output;
                    sqlComm.Parameters.Add("@iAction", SqlDbType.Int).Value = 1;
                    SqlDataAdapter da = new SqlDataAdapter();
                    da.SelectCommand = sqlComm;
                    da.Fill(dsLoginDetails);
                    sqlConn.Close();
                    Session["UserReferenceID"] = sqlComm.Parameters["@sUserReferenceID"].Value.ToString();

                    DataTable dtLoginDetails = new DataTable();
                    dtLoginDetails = dsLoginDetails.Tables[0];

                    if (dtLoginDetails.Rows.Count > 0)
                    {
                        //if (dtLoginDetails.Rows[0]["LoginPassword"].ToString().Equals(password, StringComparison.InvariantCulture))

                        FormsAuthentication.SetAuthCookie(username.ToString(), true);
                        Session["AccountID"] = dtLoginDetails.Rows[0]["AccountID"].ToString();
                        Session["UserID"] = dtLoginDetails.Rows[0]["UserID"].ToString();
                        Session["AccountType"] = dtLoginDetails.Rows[0]["AccountType"].ToString();
                        Session["UserType"] = dtLoginDetails.Rows[0]["UserType"].ToString();
                        Session["UserName"] = username.ToString();                       
                        return 1;
                    }
                    else
                    {
                        return 3;
                        // objUser.Msg = "Login fail. Try Again !!!";
                    }
                }
                catch (Exception Ex)
                {
                    Logger.Log(new LogEntry(LogType.ERROR, "Error : - " + Ex.Message + "------------"
                        + Ex.InnerException + Ex.StackTrace));
                }
            }
            return 0;
        }

        public ActionResult SessionLogOut()
        {
            Session["AccountID"] = "";
            Session["UserID"] = "";
            Session["UserName"] = "";
            Session["AccountType"] = "";
            Session["UserType"] = "";

            FormsAuthentication.SignOut();
            return RedirectToAction("UserLogin", "Login");
        }
    }
}