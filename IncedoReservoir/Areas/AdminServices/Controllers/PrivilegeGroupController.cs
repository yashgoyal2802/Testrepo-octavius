using IncedoReservoir.Areas.AdminServices.Models;
using IncedoReservoir.DBContext;
using IncedoReservoir.Models;
using LoggerUtility;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace IncedoReservoir.Areas.AdminServices.Controllers
{
    public class PrivilegeGroupController : Controller
    {
        // GET: AdminServices/PrivilegeGroup
        // GET: AdminServices/Dashboard
        // GET: ECLDataServices/Dashboard
        public ActionResult Index()
        {
            if (Session["UserID"] != null)
            {
                PrivilegeGroupMainModel objPrivilegeGroupMainModel = new PrivilegeGroupMainModel();
                if (Session["UserID"] != null)
                {
                    IncedoReservoirDBContext objIncedoReservoirDBContext = new IncedoReservoirDBContext();
                    List<GetPrivilegeGroupNameViewModel> intList = new List<GetPrivilegeGroupNameViewModel>();


                    var ServiceDefList = (from p in objIncedoReservoirDBContext.ServiceDefinition select p).ToList();
                    List<ServiceDefinitionViewModel> lstServiceDefinitionViewModel = new List<Models.ServiceDefinitionViewModel>();
                    foreach (var item in ServiceDefList)
                    {
                        ServiceDefinitionViewModel objServiceDefinitionViewModel = new Models.ServiceDefinitionViewModel();
                        objServiceDefinitionViewModel.iOrderBy = item.iOrderBy;
                        objServiceDefinitionViewModel.iServiceDefinitionID = item.iServiceDefinitionID;
                        objServiceDefinitionViewModel.sHostName = item.sHostName;
                        objServiceDefinitionViewModel.sServiceDescription = item.sServiceDescription;
                        lstServiceDefinitionViewModel.Add(objServiceDefinitionViewModel);
                    }

                    var PrivilegeList = objIncedoReservoirDBContext.PrivilegeAvilable.ToList();
                    List<PrivilegeAvilableViewModel> lstPrivilegeAvilableViewModel = new List<PrivilegeAvilableViewModel>();

                    foreach (var item in PrivilegeList)
                    {
                        PrivilegeAvilableViewModel objPrivilegeAvilableViewModel = new PrivilegeAvilableViewModel();
                        objPrivilegeAvilableViewModel.iPrivilegeID = item.iPrivilegeID;
                        objPrivilegeAvilableViewModel.iServiceDefinitionID = item.iServiceDefinitionID;
                        objPrivilegeAvilableViewModel.sDescription = item.sDescription;
                        objPrivilegeAvilableViewModel.iParentID = item.iParentID;
                        lstPrivilegeAvilableViewModel.Add(objPrivilegeAvilableViewModel);
                    }

                    List<MasterServicesViewModelAdmin> lstMasterServicesViewModelAdmin = new List<MasterServicesViewModelAdmin>();
                    using (IncedoReservoirDBContext objInventoryDBContext = new IncedoReservoirDBContext())
                    {
                        var masterServicelst = objInventoryDBContext.MasterServices
                            .Where(a => a.bStatus == true)
                            .OrderBy(a=>a.sServiceName)
                            .ToList();

                        foreach (var item in masterServicelst)
                        {
                            MasterServicesViewModelAdmin objServiceLocation = new MasterServicesViewModelAdmin();
                            objServiceLocation.iServiceId = item.iServiceId;
                            objServiceLocation.sServiceName = item.sServiceName;
                            lstMasterServicesViewModelAdmin.Add(objServiceLocation);
                        }
                    }
                    objPrivilegeGroupMainModel.lstGetPrivilegeGroupName = intList;
                    objPrivilegeGroupMainModel.lstPrivilegeAvilable = lstPrivilegeAvilableViewModel;
                    objPrivilegeGroupMainModel.lstServiceDefinition = lstServiceDefinitionViewModel;
                    objPrivilegeGroupMainModel.lstMasterServices = lstMasterServicesViewModelAdmin;
                }
                else
                {
                    return Redirect("/Login/SessionLogOut");
                }
                return View(objPrivilegeGroupMainModel);
            }
            else
            {
                return Redirect("/Login/SessionLogOut");
            }
        }

        public ActionResult PrivilegeGroupList(string sSearchGroupName = "", int iAction = 0)
        {
            PrivilegeGroupMainModel objPrivilegeGroupMainModel = new PrivilegeGroupMainModel();
            if (Session["UserID"] != null)
            {
                if (string.IsNullOrEmpty(sSearchGroupName))
                {
                    sSearchGroupName = "NA";
                }
                IncedoReservoirDBContext objIncedoReservoirDBContext = new IncedoReservoirDBContext();
                List<GetPrivilegeGroupNameViewModel> intList = new List<GetPrivilegeGroupNameViewModel>();
                SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["IncedoConnection"].ToString());
                SqlCommand cmd = new SqlCommand("GetPrivilegeGroupName", con);
                SqlDataAdapter da = new SqlDataAdapter();
                DataTable dt = new DataTable();
                cmd.CommandType = CommandType.StoredProcedure;
                SqlParameter param;
                param = cmd.Parameters.Add("@iRecordID", SqlDbType.Int);
                param.Value = 0;
                param = cmd.Parameters.Add("@iLoggedUserId", SqlDbType.Int);
                param.Value = Convert.ToInt32(Session["UserID"].ToString());
                param = cmd.Parameters.Add("@sUserType", SqlDbType.NVarChar);
                param.Value = Session["UserType"];
                param = cmd.Parameters.Add("@sGroupName", SqlDbType.NVarChar);
                param.Value = sSearchGroupName;
                param = cmd.Parameters.Add("@iAction", SqlDbType.Int);
                param.Value = iAction;
                // Execute the command.
                con.Open();
                da.SelectCommand = cmd;
                da.Fill(dt);

                con.Close();
                foreach (DataRow i in dt.Rows)
                {
                    GetPrivilegeGroupNameViewModel obj = new GetPrivilegeGroupNameViewModel();
                    obj.GroupID = Convert.ToInt32(i["GroupID"]);
                    obj.GroupName = i["GroupName"].ToString();
                    obj.FunctionalLevel = i["FunctionalLevel"].ToString();
                    obj.ActiveStatus = i["ActiveStatus"].ToString();
                    intList.Add(obj);
                }
                objPrivilegeGroupMainModel.lstGetPrivilegeGroupName = intList;
            }
            else
            {
                return Redirect("/Login/SessionLogOut");
            }
            return View(objPrivilegeGroupMainModel);
        }


        public JsonResult ValidatePrivilegeGroupName(string strPrivilegeGroupName)
        {
            int iPGNameStatus = 0;
            if (Session["UserID"] != null)
            {
                IncedoReservoirDBContext objIncedoReservoirDBContext = new IncedoReservoirDBContext();
                var privilegeGroup = objIncedoReservoirDBContext.PrivilegeGroup.Where(p => p.sGroupName == strPrivilegeGroupName).FirstOrDefault();
                if (privilegeGroup != null && privilegeGroup.iGroupID > 0)
                { iPGNameStatus = 1; }

            }
            return Json(iPGNameStatus, JsonRequestBehavior.AllowGet);
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

        [HttpPost]
        public JsonResult AddPrivilegeGroup(string SGN,  string Checkval, string MasterServiceCheckval)
        {
            try
            {
                var iCheckval = Checkval.Split(',').ToList().Distinct();
                var iMSCheckval = MasterServiceCheckval.Split(',').ToList().Distinct();

                using (var db = new IncedoReservoirDBContext())
                {
                    PrivilegeGroup objGetPrivilegeGroupNameViewModel = new PrivilegeGroup();
                    objGetPrivilegeGroupNameViewModel.sGroupName = SGN;
                    objGetPrivilegeGroupNameViewModel.sFunctionalLevel = "SO-USER";
                    objGetPrivilegeGroupNameViewModel.iStatus = 1;
                    objGetPrivilegeGroupNameViewModel.iAccountID = 1;
                    objGetPrivilegeGroupNameViewModel.dCreatedOn = DateTime.Now;
                    objGetPrivilegeGroupNameViewModel.iCreatedBy = Convert.ToInt32(Session["UserID"].ToString());
                    db.PrivilegeGroup.Add(objGetPrivilegeGroupNameViewModel);
                    db.SaveChanges();
                    int id = objGetPrivilegeGroupNameViewModel.iGroupID;

                    if (iCheckval != null)
                    {
                        foreach (var item in iCheckval)
                        {
                            if (item != null && item != "")
                            {
                                int itemval = Convert.ToInt32(item);
                                var req = (from s in db.PrivilegeAvilable
                                           where s.iPrivilegeID == itemval
                                           select s).FirstOrDefault();
                                int? nor = Convert.ToInt32(req.iServiceDefinitionID);
                                ServiceAndPrivilegeLevelMapping objServiceAndPrivilegeLevelMapping = new ServiceAndPrivilegeLevelMapping();
                                objServiceAndPrivilegeLevelMapping.iGroupID = id;
                                objServiceAndPrivilegeLevelMapping.iPrivilegeID = Convert.ToInt32(item);
                                objServiceAndPrivilegeLevelMapping.iServiceDefinitionID = nor;
                                objServiceAndPrivilegeLevelMapping.iStatus = 1;
                                db.ServiceAndPrivilegeLevelMapping.Add(objServiceAndPrivilegeLevelMapping);
                                db.SaveChanges();
                            }
                        }
                    }

                    if (iMSCheckval != null)
                    {
                        foreach (var item in iMSCheckval)
                        {
                            if (item != null && item != "")
                            {
                                int itemval = Convert.ToInt32(item);

                                PrivilegeGroupMasterServicesReln objServiceAndPrivilegeLevelMapping = new PrivilegeGroupMasterServicesReln();
                                objServiceAndPrivilegeLevelMapping.iServiceId = Convert.ToInt32(item);
                                objServiceAndPrivilegeLevelMapping.iPrivilegeGroup = id;
                                objServiceAndPrivilegeLevelMapping.bStatus = true;
                                db.PrivilegeGroupMasterServicesReln.Add(objServiceAndPrivilegeLevelMapping);
                                db.SaveChanges();
                            }
                        }
                    }
                    return Json("Success");
                }
            }
            catch (Exception Ex)
            {
                Logger.Log(new LogEntry(LogType.ERROR, "Error : - " + Ex.InnerException + Ex.StackTrace));
                return Json(Ex);
            }
        }

        [HttpPost]
        public JsonResult EditPrivilegeGroup(int id)
        {
            EditPrivilegeModelView objEditPrivilegeModelView = new EditPrivilegeModelView();
            //IncedoReservoirDBContext objed = new EscoTools.IncedoReservoirDBContext();
            using (var db = new IncedoReservoirDBContext())
            {
                var query = (from st in db.PrivilegeGroup
                             where st.iGroupID == id && st.iStatus == 1
                             select new { st.iGroupID, st.sGroupName, st.sFunctionalLevel }).FirstOrDefault();
                objEditPrivilegeModelView.GroupID = query.iGroupID;
                objEditPrivilegeModelView.GroupName = query.sGroupName;
                objEditPrivilegeModelView.FunctionalLevel = query.sFunctionalLevel;

                var pQuery = (from st in db.ServiceAndPrivilegeLevelMapping
                              where st.iGroupID == id && st.iStatus == 1
                              select new { st.iPrivilegeID }).ToList();
                string strarr = null;

                foreach (var i in pQuery)
                {
                    strarr = strarr + i.iPrivilegeID.ToString() + ",";
                }
                objEditPrivilegeModelView.CheckValue = strarr;
            }
            return Json(objEditPrivilegeModelView, JsonRequestBehavior.AllowGet);
        }
        public JsonResult EditMasterServicePG(int id)
        {
            PrivilegeGroupMasterServicesRelnViewModel objEditPrivilegeModelView = new PrivilegeGroupMasterServicesRelnViewModel();
            //IncedoReservoirDBContext objed = new EscoTools.IncedoReservoirDBContext();
            using (var db = new IncedoReservoirDBContext())
            {
                var query = (from st in db.PrivilegeGroupMasterServicesReln
                             where st.iPrivilegeGroup == id && st.bStatus == true
                             select new { st.iServiceId }).ToList();



                string strarr = null;

                foreach (var i in query)
                {
                    strarr = strarr + i.iServiceId.ToString() + ",";
                }
                objEditPrivilegeModelView.CheckValue = strarr;
            }
            return Json(objEditPrivilegeModelView, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult UpdatePrivilegeGroup(string SGN,  string Checkval, int GroupID, string PrivilegesID, string MasterServiceCheckval, string msExistingIdsToDelete)
        {
            try
            {
                var i = Checkval.Split(',').ToList().Distinct();
                var ExsistPrivilegeIDs = PrivilegesID.Split(',').ToList();

                var MasterServiceIds = MasterServiceCheckval.Split(',').ToList();
                var MSExistingIdsToDelete = msExistingIdsToDelete.Split(',').ToList();

                using (var db = new IncedoReservoirDBContext())
                {
                    var editPG = db.PrivilegeGroup.FirstOrDefault(item => item.iGroupID == GroupID);
                    if (editPG != null)
                    {
                        editPG.sGroupName = SGN;
                        editPG.sFunctionalLevel = "SO-USER";
                        db.PrivilegeGroup.Add(editPG);
                        db.Entry(editPG).State = EntityState.Modified;
                        db.SaveChanges();
                    }

                    foreach (var item in i)
                    {
                        if (item != null && item != "")
                        {
                            if (ExsistPrivilegeIDs.Contains(item))
                            {
                                ExsistPrivilegeIDs.Remove(item);
                            }
                            else
                            {
                                int itemval = Convert.ToInt32(item);
                                var req = (from s in db.PrivilegeAvilable
                                           where s.iPrivilegeID == itemval
                                           select s).FirstOrDefault();
                                int? nor = Convert.ToInt32(req.iServiceDefinitionID);

                                ServiceAndPrivilegeLevelMapping objServiceAndPrivilegeLevelMapping = new ServiceAndPrivilegeLevelMapping();
                                objServiceAndPrivilegeLevelMapping.iGroupID = GroupID;
                                objServiceAndPrivilegeLevelMapping.iPrivilegeID = itemval;
                                objServiceAndPrivilegeLevelMapping.iServiceDefinitionID = nor;
                                objServiceAndPrivilegeLevelMapping.iStatus = 1;
                                db.ServiceAndPrivilegeLevelMapping.Add(objServiceAndPrivilegeLevelMapping);
                            }
                            db.SaveChanges();
                        }
                    }

                    foreach (var remo in ExsistPrivilegeIDs)
                    {
                        if (remo != null && remo != "")
                        {
                            int rem = Convert.ToInt32(remo);
                            var ExsistRemove = db.ServiceAndPrivilegeLevelMapping.FirstOrDefault(pg => pg.iGroupID == GroupID && pg.iPrivilegeID == rem);
                            if (ExsistRemove != null)
                            {
                                ExsistRemove.iStatus = 0;
                                db.ServiceAndPrivilegeLevelMapping.Add(ExsistRemove);
                                db.Entry(ExsistRemove).State = EntityState.Modified;
                            }
                            db.SaveChanges();
                        }
                    }

                    foreach (var item in MasterServiceIds)
                    {
                        if (item != null && item != "")
                        {
                            if (MSExistingIdsToDelete.Contains(item))
                            {
                                MSExistingIdsToDelete.Remove(item);
                            }
                            else
                            {
                                int itemval = Convert.ToInt32(item);

                                PrivilegeGroupMasterServicesReln objPGMSR = new PrivilegeGroupMasterServicesReln();
                                objPGMSR.iServiceId = itemval;
                                objPGMSR.iPrivilegeGroup = GroupID;
                                objPGMSR.bStatus = true;
                                db.PrivilegeGroupMasterServicesReln.Add(objPGMSR);
                            }
                            db.SaveChanges();
                        }
                    }

                    foreach (var remo in MSExistingIdsToDelete)
                    {
                        if (remo != null && remo != "")
                        {
                            int rem = Convert.ToInt32(remo);
                            var ExsistRemove = db.PrivilegeGroupMasterServicesReln.FirstOrDefault(pg => pg.iPrivilegeGroup == GroupID && pg.iServiceId == rem);
                            if (ExsistRemove != null)
                            {
                                ExsistRemove.bStatus = false;
                                db.PrivilegeGroupMasterServicesReln.Add(ExsistRemove);
                                db.Entry(ExsistRemove).State = EntityState.Modified;
                            }
                            db.SaveChanges();
                        }
                    }

                    return Json("Success");
                }
            }
            catch (Exception Ex)
            {
                Logger.Log(new LogEntry(LogType.ERROR, "Error : - " + Ex.InnerException + Ex.StackTrace));
                return Json(Ex);
            }
        }
    }
}