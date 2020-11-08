//////////////////////////////////////////////////////////
///
/// Name: HomeController.cs
/// Author: William Kurth
/// Description: 
/// Class:  HomeController
/// Base Class: Controller
/// Notes:
/// 



using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;
using Microsoft.SqlServer;




using RADAR.Models;
namespace RADAR.Controllers
{

    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
    public class MultipleButtonAttribute : ActionNameSelectorAttribute
    {
        public string Name { get; set; }
        public string Argument { get; set; }

        public override bool IsValidName(ControllerContext controllerContext, string actionName, MethodInfo aMethod)
        {
            var isValidName = false;
            var keyValue = string.Format("{0}:{1}", Name, Argument);
            var value = controllerContext.Controller.ValueProvider.GetValue(keyValue);

            if (value != null)
            {
                controllerContext.Controller.ControllerContext.RouteData.Values[Name] = Argument;
                isValidName = true;
            }

            return isValidName;
        }
    }

    public class HomeController : Controller
    {

        static List<LocationsData> myLocations = new List<LocationsData>();
        static List<ShiftData> myShifts = new List<ShiftData>();
        static List<TypeData> myTypes = new List<TypeData>();
        static List<BehaviorData> myBehaviors = new List<BehaviorData>();

        static List<ADUserData> myUserData = new List<ADUserData>();
        static public Widget myWidget;
        [HttpPost]
        [MultipleButton(Name = "action", Argument = "Create")]
        public ActionResult Create(Widget rdrrecord)
        {

            try
            {
                using (SqlConnection con = new SqlConnection())
                {
                    String sql = @"RADAR_AddRecord";
                    con.ConnectionString = @"Data Source=roadrunner;Initial Catalog=SACME;User ID=secure;Password=secure";

                    if ( rdrrecord.MentorReview == null)
                    {
                        rdrrecord.MentorReview = "";
                    }
                    using (SqlCommand cmd = new SqlCommand(sql, con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.Add("@Name", SqlDbType.NVarChar).Value = rdrrecord.UserName;
                        cmd.Parameters.Add("@SelectListName", SqlDbType.NVarChar).Value = rdrrecord.UserName;
                        cmd.Parameters.Add("@obsDate", SqlDbType.Date).Value = rdrrecord.CreatedOn.ToShortDateString();
                        cmd.Parameters.Add("@BehaviorName", SqlDbType.NVarChar).Value = rdrrecord.Behavior;
                        cmd.Parameters.Add("@obsType", SqlDbType.NVarChar).Value = rdrrecord.Observation;
                        cmd.Parameters.Add("@obsShift", SqlDbType.NVarChar).Value = rdrrecord.Shift;
                        cmd.Parameters.Add("@obsLocation", SqlDbType.NVarChar).Value = rdrrecord.Location;
                        cmd.Parameters.Add("@safeatrisk", SqlDbType.Int).Value = rdrrecord.Safe.ToString() == "Safe"? 1:0;
                        cmd.Parameters.Add("@intercede", SqlDbType.Int).Value = rdrrecord.intercede.ToString() == "Yes"? 1:0;
                        cmd.Parameters.Add("@mentorreviewed", SqlDbType.Int).Value = rdrrecord.mentorreviewed.ToString() == "Yes"? 1:0;
                        cmd.Parameters.Add("@comments", SqlDbType.NVarChar).Value = rdrrecord.Comments;
                        cmd.Parameters.Add("@mentornotest", SqlDbType.NVarChar).Value = rdrrecord.MentorReview;
                        con.Open();
                        cmd.ExecuteNonQuery();
                    }
                   
                }
                //  Widget _objuserloginmodel = new Widget();
                /*    List<StudentDetailViewModel> _objStudent = new List<StudentDetailViewModel>();
                    _objStudent = _objuserloginmodel.GetStudentList();
                    _objuserloginmodel.Studentmodel = _objStudent;
                    */
            }
            catch (  System.Exception e)
            {
                string mess = e.Message;
            }
            ModelState.Clear();
            ViewBag.Locations = myLocations;
            ViewBag.selectLocations = new SelectList(myLocations.OrderBy(g => g.DispOrder), "Location", "LocationDescription");


            ViewBag.Shifts = myShifts;

            ViewBag.Types = myTypes;

            ViewBag.Behaviors = myBehaviors;

            ViewBag.ADUsers = myUserData;


            ViewBag.Widget = new Widget();
            myWidget = new Widget();
            myWidget.UserName = "aaaa";
            return View("Index", myWidget);
        }


        [HttpPost]
        [MultipleButton(Name = "action", Argument = "Clear")]
        public ActionResult Clear(Widget rdrrecord)
        {

            try
            {
            }
            catch (System.Exception e)
            {
                string mess = e.Message;

            }
            ModelState.Clear();
            ViewBag.Locations = myLocations;
            ViewBag.selectLocations = new SelectList(myLocations.OrderBy(g => g.DispOrder), "Location", "LocationDescription");


            ViewBag.Shifts = myShifts;

            ViewBag.Types = myTypes;

            ViewBag.Behaviors = myBehaviors;

            ViewBag.ADUsers = myUserData;


            ViewBag.Widget = new Widget();
            myWidget = new Widget();
            myWidget.UserName = "aaaa";
            return View("Index", myWidget);
        }


        public ActionResult Index()
        {
            try
            {

                using (SqlConnection con = new SqlConnection())
                {

                    String sql = @"RADAR_GetLocations";
                    con.ConnectionString = @"Data Source=roadrunner;Initial Catalog=SACME;User ID=secure;Password=secure";

                    DataTable dt = new DataTable();
                    SqlDataAdapter da = new SqlDataAdapter("RADAR_GetLocations", con);
                    da.SelectCommand.CommandType = CommandType.StoredProcedure;

                    da.SelectCommand = new SqlCommand(sql, con);
                    if (myLocations.Count == 0)
                    {
                        da.Fill(dt);
                        foreach (DataRow row in dt.Rows)
                        {
                            LocationsData loc = new LocationsData();
                            loc.Location = row["RDR_Location"].ToString();
                            loc.LocationDescription = row["RDR_Notes"].ToString();
                            loc.DispOrder = Int32.Parse(row["RDR_Location_DispOrder"].ToString());
                            myLocations.Add(loc);
                        }

                    }


                    sql = @"RADAR_GetShifts";
                    dt = new DataTable();
                    if (myShifts.Count == 0)
                    {
                        da = new SqlDataAdapter("RADAR_GetShifts", con);
                        da.SelectCommand.CommandType = CommandType.StoredProcedure;
                        da.SelectCommand = new SqlCommand(sql, con);
                        da.Fill(dt);
                        foreach (DataRow row in dt.Rows)
                        {
                            ShiftData shift = new ShiftData();
                            shift.ShiftName = row["RDR_Shift"].ToString();
                            shift.ShiftDescription = row["RDR_Notes"].ToString();
                            shift.DispOrder = Int32.Parse(row["RDR_ShiftDispOrder"].ToString());
                            myShifts.Add(shift);
                        }
                    }


                    sql = @"RADAR_GetTypes";
                    dt = new DataTable();
                    if (myTypes.Count == 0)
                    {
                        da = new SqlDataAdapter("RADAR_GetTypes", con);
                        da.SelectCommand.CommandType = CommandType.StoredProcedure;
                        da.SelectCommand = new SqlCommand(sql, con);
                        da.Fill(dt);
                        foreach (DataRow row in dt.Rows)
                        {
                            TypeData type = new TypeData();
                            type.ObservationType = row["RDR_Type"].ToString();
                            type.ObservationDescription = row["RDR_Notes"].ToString();
                            type.DispOrder = Int32.Parse(row["RDR_Type_DispOrder"].ToString());
                            myTypes.Add(type);
                        }
                    }




                    sql = @"RADAR_GetBehaviors";
                    dt = new DataTable();
                    if (myBehaviors.Count == 0)
                    {
                        da = new SqlDataAdapter("RADAR_GetBehaviors", con);
                        da.SelectCommand.CommandType = CommandType.StoredProcedure;
                        da.SelectCommand = new SqlCommand(sql, con);
                        da.Fill(dt);
                        foreach (DataRow row in dt.Rows)
                        {
                            BehaviorData behavior = new BehaviorData();
                            behavior.Behavior = row["RDR_BehaviorName"].ToString();
                            behavior.BehaviorDescription = row["RDR_BehaviorNotes"].ToString();
                            behavior.DispOrder = Int32.Parse(row["RDR_Behavior_DispOrder"].ToString());
                            myBehaviors.Add(behavior);
                        }
                    }


                    sql = @"RADAR_GetADUsers"; // EmailAddress, Name, SamAccountName, UserPrincipalName,FullName";
                    dt = new DataTable();
                    if (myUserData.Count == 0)
                    {
                        da = new SqlDataAdapter("RADAR_GetADUsers", con);
                        da.SelectCommand.CommandType = CommandType.StoredProcedure;
                        da.SelectCommand = new SqlCommand(sql, con);
                        da.Fill(dt);
                        foreach (DataRow row in dt.Rows)
                        {
                            ADUserData aUser = new ADUserData();
                            aUser.email = row["EmailAddress"].ToString();
                            aUser.ename = row["Name"].ToString();
                            aUser.SAMAccountName = row["SamAccountName"].ToString();
                            aUser.UserPrincipalName = row["UserPrincipalName"].ToString();
                            aUser.FullName = row["FullName"].ToString();
                            myUserData.Add(aUser);
                        }
                    }

                }
            }
            catch (Exception ex)
            {
                string mess = ex.Message;
            }

            ViewBag.Locations = myLocations;
            ViewBag.selectLocations = new SelectList(myLocations.OrderBy(g => g.DispOrder), "Location", "LocationDescription");


            ViewBag.Shifts = myShifts;

            ViewBag.Types = myTypes;

            ViewBag.Behaviors = myBehaviors;

            ViewBag.ADUsers = myUserData;


            ViewBag.Widget = new Widget();
            myWidget = new Widget();
            if (myUserData.Count > 0)
            {
                myWidget.UserName = myUserData[0].FullName; // "aaaa";
            }
            else
            {
                myWidget.UserName = "aaa";
            }
            return View(myWidget);
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}