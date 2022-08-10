﻿using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Incedo_Octavius_Demo_2.Data;
using Incedo_Octavius_Demo_2.Models;
using MySql.Data.MySqlClient;

namespace Incedo_Octavius_Demo_2.Controllers
{
    
    public class BusinessUserDegreeModelsController : Controller
    {

        private Incedo_Octavius_Demo_2_Rules_BU_Deg_Context db = new Incedo_Octavius_Demo_2_Rules_BU_Deg_Context();
        public string constr = ConfigurationManager.ConnectionStrings["Incedo_Octavius_Demo_2_Rules_BU_Deg_Context"].ConnectionString;

        // GET: BusinessUserDegreeModels
        public ActionResult Index()
        {
            List<BusinessUserDegreeModel> RuleDegBU_List = new List<BusinessUserDegreeModel>();
            //string constr = ConfigurationManager.ConnectionStrings["Incedo_Octavius_Demo_2_kol_table_Context"].ConnectionString;
            // Stored Procedures
            using (MySqlConnection dbConnection = new MySqlConnection(constr))
            {
                try
                {
                    dbConnection.Open();
                    MySqlCommand cmd = new MySqlCommand();
                    cmd.Connection = dbConnection;
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "Rules_BU_Deg";

                    MySqlDataAdapter dataAdapter = new MySqlDataAdapter(cmd);
                    DataSet dataSetObject = new DataSet();
                    dataAdapter.Fill(dataSetObject);

                    if (dataSetObject.Tables[0].Rows.Count > 0)
                    {
                        for (int iCout = 0; iCout < dataSetObject.Tables[0].Rows.Count; iCout++)
                        {
                            BusinessUserDegreeModel RuleDegBU = new BusinessUserDegreeModel();
                            RuleDegBU.MapID = Convert.ToInt32(dataSetObject.Tables[0].Rows[iCout]["MapID"]);
                            RuleDegBU.DegreeID = Convert.ToInt32(dataSetObject.Tables[0].Rows[iCout]["DegreeID"]);
                            RuleDegBU.Degree_Map = dataSetObject.Tables[0].Rows[iCout]["Degree_Map"].ToString();
                            RuleDegBU.Parent_Degree_ID = Convert.ToInt32(dataSetObject.Tables[0].Rows[iCout]["Parent_Degree_ID"]);
                            RuleDegBU.Degree_Master = dataSetObject.Tables[0].Rows[iCout]["Degree_Master"].ToString();

                            RuleDegBU_List.Add(RuleDegBU);
                        }
                    }
                    
                }
                catch (Exception Ex)
                {

                    Console.WriteLine("Error : " + Ex.Message);
                }

            }
            return View(RuleDegBU_List);
            //return View(db.BusinessUserDegreeModels.ToList());
        }

        // GET: BusinessUserDegreeModels/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            BusinessUserDegreeModel businessUserDegreeModel = db.BusinessUserDegreeModels.Find(id);
            if (businessUserDegreeModel == null)
            {
                return HttpNotFound();
            }
            return View(businessUserDegreeModel);
        }

        // GET: BusinessUserDegreeModels/Create
        public ActionResult Create()
        {
            //BusinessUserDegreeModel model = new BusinessUserDegreeModel();
            //model.DegreesList = PopulateDegrees();

            List<DegreeModel> degreeList = GetDegrees();
            ViewBag.DegreeListItem = ToSelectList(degreeList);

            return View();
        }

        // POST: BusinessUserDegreeModels/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "MapID,Degree_Map,Parent_Degree_ID,DegreeID,Degree_Master")] BusinessUserDegreeModel businessUserDegreeModel)
        {

            if (ModelState.IsValid)
            {
                using (MySqlConnection dbConnection = new MySqlConnection(constr))
                {
                    try
                    {
                        dbConnection.Open();
                        MySqlCommand cmd = new MySqlCommand();
                        cmd.Connection = dbConnection;
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.CommandText = "Rules_BU_Deg_INSERT";
                        cmd.Parameters.AddWithValue("deg_text", businessUserDegreeModel.Degree_Map);
                        cmd.Parameters.AddWithValue("deg_id", businessUserDegreeModel.Parent_Degree_ID);
                        cmd.ExecuteNonQuery();
                        dbConnection.Close();

                        /*MySqlDataAdapter dataAdapter = new MySqlDataAdapter(cmd);
                        DataSet dataSetObject = new DataSet();
                        dataAdapter.Fill(dataSetObject);

                        if (dataSetObject.Tables[0].Rows.Count > 0)
                        {
                            for (int iCout = 0; iCout < dataSetObject.Tables[0].Rows.Count; iCout++)
                            {
                                BusinessUserDegreeModel RuleDegBU = new BusinessUserDegreeModel();
                                RuleDegBU.MapID = Convert.ToInt32(dataSetObject.Tables[0].Rows[iCout]["MapID"]);
                                RuleDegBU.DegreeID = Convert.ToInt32(dataSetObject.Tables[0].Rows[iCout]["DegreeID"]);
                                RuleDegBU.Degree_Map = dataSetObject.Tables[0].Rows[iCout]["Degree_Map"].ToString();
                                RuleDegBU.Parent_Degree_ID = Convert.ToInt32(dataSetObject.Tables[0].Rows[iCout]["Parent_Degree_ID"]);
                                RuleDegBU.Degree_Master = dataSetObject.Tables[0].Rows[iCout]["Degree_Master"].ToString();

                                RuleDegBU_List.Add(RuleDegBU);
                            }
                        }*/

                    }
                    catch (Exception Ex)
                    {

                        Console.WriteLine("Error : " + Ex.Message);
                    }

                }

                return RedirectToAction("Index");
            }

            return View(businessUserDegreeModel);
        }

        // GET: BusinessUserDegreeModels/Edit/5
        public ActionResult Edit(int? id)
        {
            List<BusinessUserDegreeModel> RuleDegBU_List = new List<BusinessUserDegreeModel>();
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ViewBag.msg = id;
            //BusinessUserDegreeModel businessUserDegreeModel = db.BusinessUserDegreeModels.Find(id);
            //BusinessUserDegreeModel model = new BusinessUserDegreeModel();
            //model.DegreesList = PopulateDegrees();
            List<DegreeModel> degreeList = GetDegrees();
            ViewBag.DegreeListItem = ToSelectList(degreeList);

            using (MySqlConnection dbConnection = new MySqlConnection(constr))
            {
                try
                {
                    dbConnection.Open();
                    MySqlCommand cmd = new MySqlCommand();
                    cmd.Connection = dbConnection;
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "Rules_BU_Deg_pm";
                    cmd.Parameters.AddWithValue("id", id);

                    MySqlDataAdapter dataAdapter = new MySqlDataAdapter(cmd);
                    DataSet dataSetObject = new DataSet();
                    dataAdapter.Fill(dataSetObject);

                    if (dataSetObject.Tables[0].Rows.Count > 0)
                    {
                        for (int iCout = 0; iCout < dataSetObject.Tables[0].Rows.Count; iCout++)
                        {
                            BusinessUserDegreeModel RuleDegBU = new BusinessUserDegreeModel();
                            RuleDegBU.MapID = Convert.ToInt32(dataSetObject.Tables[0].Rows[iCout]["MapID"]);
                            RuleDegBU.DegreeID = Convert.ToInt32(dataSetObject.Tables[0].Rows[iCout]["DegreeID"]);
                            RuleDegBU.Degree_Map = dataSetObject.Tables[0].Rows[iCout]["Degree_Map"].ToString();
                            RuleDegBU.Parent_Degree_ID = Convert.ToInt32(dataSetObject.Tables[0].Rows[iCout]["Parent_Degree_ID"]);
                            RuleDegBU.Degree_Master = dataSetObject.Tables[0].Rows[iCout]["Degree_Master"].ToString();

                            RuleDegBU_List.Add(RuleDegBU);
                        }
                    }

                }
                catch (Exception Ex)
                {

                    Console.WriteLine("Error : " + Ex.Message);
                }

            }


            return View(RuleDegBU_List[0]);
            //return View();
        }

        // POST: BusinessUserDegreeModels/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "MapID,Degree_Map,Parent_Degree_ID,DegreeID,Degree_Master")] BusinessUserDegreeModel businessUserDegreeModel)
        {
            if (ModelState.IsValid)
            {
                ViewBag.map_id = businessUserDegreeModel.MapID;
                ViewBag.deg_text = businessUserDegreeModel.Degree_Map;
                ViewBag.deg_id = businessUserDegreeModel.Parent_Degree_ID;
                /*if (businessUserDegreeModel == null)
                {
                    return HttpNotFound();
                }*/
                using (MySqlConnection dbConnection = new MySqlConnection(constr))
                {
                    try
                    {
                        dbConnection.Open();
                        MySqlCommand cmd = new MySqlCommand();
                        cmd.Connection = dbConnection;
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.CommandText = "Rules_BU_Deg_UPDATE";
                        cmd.Parameters.AddWithValue("map_id", businessUserDegreeModel.MapID);
                        cmd.Parameters.AddWithValue("deg_text", businessUserDegreeModel.Degree_Map);
                        cmd.Parameters.AddWithValue("deg_id", businessUserDegreeModel.Parent_Degree_ID);

                        cmd.ExecuteNonQuery();
                        dbConnection.Close();

                        /*MySqlDataAdapter dataAdapter = new MySqlDataAdapter(cmd);
                        DataSet dataSetObject = new DataSet();
                        dataAdapter.Fill(dataSetObject);

                        if (dataSetObject.Tables[0].Rows.Count > 0)
                        {
                            for (int iCout = 0; iCout < dataSetObject.Tables[0].Rows.Count; iCout++)
                            {
                                BusinessUserDegreeModel RuleDegBU = new BusinessUserDegreeModel();
                                RuleDegBU.MapID = Convert.ToInt32(dataSetObject.Tables[0].Rows[iCout]["MapID"]);
                                RuleDegBU.DegreeID = Convert.ToInt32(dataSetObject.Tables[0].Rows[iCout]["DegreeID"]);
                                RuleDegBU.Degree_Map = dataSetObject.Tables[0].Rows[iCout]["Degree_Map"].ToString();
                                RuleDegBU.Parent_Degree_ID = Convert.ToInt32(dataSetObject.Tables[0].Rows[iCout]["Parent_Degree_ID"]);
                                RuleDegBU.Degree_Master = dataSetObject.Tables[0].Rows[iCout]["Degree_Master"].ToString();

                                RuleDegBU_List.Add(RuleDegBU);
                            }
                        }*/

                    }
                    catch (Exception Ex)
                    {

                        Console.WriteLine("Error : " + Ex.Message);
                    }

                }
                //return View();
                //db.Entry(businessUserDegreeModel).State = EntityState.Modified;
                //db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(businessUserDegreeModel);
        }

        // GET: BusinessUserDegreeModels/Delete/5
        public ActionResult Delete(int? id)
        {
            List<BusinessUserDegreeModel> RuleDegBU_List = new List<BusinessUserDegreeModel>();
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            /*BusinessUserDegreeModel businessUserDegreeModel = db.BusinessUserDegreeModels.Find(id);
            if (businessUserDegreeModel == null)
            {
                return HttpNotFound();
            }*/

            using (MySqlConnection dbConnection = new MySqlConnection(constr))
            {
                try
                {
                    dbConnection.Open();
                    MySqlCommand cmd = new MySqlCommand();
                    cmd.Connection = dbConnection;
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "Rules_BU_Deg_pm";
                    cmd.Parameters.AddWithValue("id", id);

                    MySqlDataAdapter dataAdapter = new MySqlDataAdapter(cmd);
                    DataSet dataSetObject = new DataSet();
                    dataAdapter.Fill(dataSetObject);

                    if (dataSetObject.Tables[0].Rows.Count > 0)
                    {
                        for (int iCout = 0; iCout < dataSetObject.Tables[0].Rows.Count; iCout++)
                        {
                            BusinessUserDegreeModel RuleDegBU = new BusinessUserDegreeModel();
                            RuleDegBU.MapID = Convert.ToInt32(dataSetObject.Tables[0].Rows[iCout]["MapID"]);
                            RuleDegBU.DegreeID = Convert.ToInt32(dataSetObject.Tables[0].Rows[iCout]["DegreeID"]);
                            RuleDegBU.Degree_Map = dataSetObject.Tables[0].Rows[iCout]["Degree_Map"].ToString();
                            RuleDegBU.Parent_Degree_ID = Convert.ToInt32(dataSetObject.Tables[0].Rows[iCout]["Parent_Degree_ID"]);
                            RuleDegBU.Degree_Master = dataSetObject.Tables[0].Rows[iCout]["Degree_Master"].ToString();

                            RuleDegBU_List.Add(RuleDegBU);
                        }
                    }

                }
                catch (Exception Ex)
                {

                    Console.WriteLine("Error : " + Ex.Message);
                }

            }


            return View(RuleDegBU_List[0]);
        }

        // POST: BusinessUserDegreeModels/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            /*BusinessUserDegreeModel businessUserDegreeModel = db.BusinessUserDegreeModels.Find(id);
            db.BusinessUserDegreeModels.Remove(businessUserDegreeModel);
            db.SaveChanges();*/
            using (MySqlConnection dbConnection = new MySqlConnection(constr))
            {
                try
                {
                    dbConnection.Open();
                    MySqlCommand cmd = new MySqlCommand();
                    cmd.Connection = dbConnection;
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "Rules_BU_Deg_DELETE";
                    cmd.Parameters.AddWithValue("map_id", id);

                    cmd.ExecuteNonQuery();
                    dbConnection.Close();

                }
                catch (Exception Ex)
                {

                    Console.WriteLine("Error : " + Ex.Message);
                }

            }
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private List<DegreeModel> GetDegrees()
        {
            List<DegreeModel> items = new List<DegreeModel>();
            //string constr = ConfigurationManager.ConnectionStrings["Incedo_Octavius_Demo_2_kol_table_Context"].ConnectionString;
            // Stored Procedures
            using (MySqlConnection dbConnection = new MySqlConnection(constr))
            {
                try
                {
                    string query = "Select DegreeID, Degree from degree_master_table";
                    using (MySqlCommand cmd = new MySqlCommand(query))
                    {
                        cmd.Connection = dbConnection;
                        dbConnection.Open();
                        using (MySqlDataReader sdr = cmd.ExecuteReader())
                        {
                            while(sdr.Read())
                            {
                                items.Add(new DegreeModel
                                {
                                    DegreeID = Convert.ToInt32(sdr["DegreeID"]),
                                    Degree = sdr["Degree"].ToString()
                                });

                            }
                        }
                        dbConnection.Close();
                    }
                    
                }
                catch (Exception Ex)
                {

                    Console.WriteLine("Error : " + Ex.Message);
                }

            }
            return items;
        }

        private SelectList ToSelectList(List<DegreeModel> degree)
        {
            List<SelectListItem> degreeList = new List<SelectListItem>();
            foreach (DegreeModel deg in degree)
            {
                degreeList.Add(new SelectListItem()
                {
                    Text = deg.Degree,
                    Value = Convert.ToString(deg.DegreeID)
                });
            }
            return new SelectList(degreeList, "Value", "Text");
        }
    }
}
