using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Insurance.Models;
using Insurance.ViewModels;
using System.Data.SqlClient;
using System.Data;

namespace Insurance.Controllers
{
    public class HomeController : Controller
    {
        private string connectionString = @"Data Source=ALLAN-YOGA3;Initial Catalog=Insurance;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";

        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Quote()
        {
            v_UserQuotes userQuote = new v_UserQuotes();
            userQuote.FirstName = Request["FirstName"];
            userQuote.LastName = Request["LastName"];
            userQuote.Email = Request["EmailAddress"];
            var Make = Request["Make"];
            var Model = Request["Model"];
            userQuote.Year = int.Parse(Request["CarYear"].ToString());
            userQuote.SpeedingTickets = int.Parse(Request["SpeedTickets"].ToString());
            userQuote.DOB = DateTime.Parse(Request["DOB"].ToString());
            userQuote.DUI = string.IsNullOrEmpty(Request["DUI"]) ? false : true;
            userQuote.IsFullCoverage = string.IsNullOrEmpty(Request["IsFullCoverage"]) ? false : true;


            if (string.IsNullOrEmpty(userQuote.FirstName) || string.IsNullOrEmpty(userQuote.LastName) ||
                string.IsNullOrEmpty(userQuote.Email) || string.IsNullOrEmpty(Make) ||
                string.IsNullOrEmpty(Model)
                )
            {
                return View("~/Views/Shared/Error.cshtml");
            }
            else
            {
                List<v_CarMakeModelMasterList> LookupCars = GetVehicleMasterList();
                QuoteVm quoteVm = new QuoteVm();

                v_CarMakeModelMasterList Temp = LookupCars.FirstOrDefault(c => c.Manufacturer == Make && c.Name == Model);
                userQuote.CarMakeModelId = (int)Temp.Id;

                quoteVm.FirstName = userQuote.FirstName;
                quoteVm.LastName = userQuote.LastName;
                quoteVm.Premium = 0.00m;

                // Calculate premium
                userQuote.Premium = 50.0m;

                userQuote.Premium += ((DateTime.Now.Year - userQuote.DOB.Year) < 18) ? 100.0m : 0;
                if ((((DateTime.Now.Year - userQuote.DOB.Year) >= 18) 
                    && ((DateTime.Now.Year - userQuote.DOB.Year) < 25)) 
                    || ((DateTime.Now.Year - userQuote.DOB.Year) > 25)) { userQuote.Premium += 25.0m; }
                if (userQuote.Year <2000 || userQuote.Year > 2015) { userQuote.Premium += 25.0m; }
                userQuote.Premium += (Make == "Porsche") ? 25.0m : 0;
                userQuote.Premium += (Model == "911 Carrera") ? 25.0m : 0;
                userQuote.Premium += userQuote.SpeedingTickets * 10.0m;
                userQuote.Premium += (userQuote.DUI) ? userQuote.Premium * 1.25m : 0;
                userQuote.Premium += (userQuote.IsFullCoverage) ? userQuote.Premium * 1.50m : 0;

                quoteVm.Premium = (decimal)userQuote.Premium;

                using (SqlConnection connection = new SqlConnection(connectionString))
                { 
                    SqlCommand command = new SqlCommand(@"PROC_Save_User_Info_for_Quote",connection);
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    command.Parameters.Clear();

                    command.Parameters.Add("FirstName", SqlDbType.VarChar);
                    command.Parameters.Add("LastName", SqlDbType.VarChar);
                    command.Parameters.Add("Email", SqlDbType.VarChar);
                    command.Parameters.Add("DOB", SqlDbType.DateTime);
                    command.Parameters.Add("CarMakeModelId", SqlDbType.Int);
                    command.Parameters.Add("Year", SqlDbType.Int);
                    command.Parameters.Add("SpeedingTickets", SqlDbType.Int);
                    command.Parameters.Add("IsFullCoverage", SqlDbType.Bit);
                    command.Parameters.Add("DUI", SqlDbType.Bit);
                    command.Parameters.Add("Premium", SqlDbType.Decimal);

                    command.Parameters["FirstName"].Value = userQuote.FirstName;
                    command.Parameters["LastName"].Value = userQuote.LastName;
                    command.Parameters["Email"].Value = userQuote.Email;
                    command.Parameters["DOB"].Value = userQuote.DOB;
                    command.Parameters["CarMakeModelId"].Value = userQuote.CarMakeModelId;
                    command.Parameters["Year"].Value = userQuote.Year;
                    command.Parameters["SpeedingTickets"].Value = userQuote.SpeedingTickets;
                    command.Parameters["IsFullCoverage"].Value = userQuote.IsFullCoverage;
                    command.Parameters["DUI"].Value = userQuote.DUI;
                    command.Parameters["Premium"].Value = (decimal)userQuote.Premium;

                    connection.Open();
                    command.ExecuteNonQuery();
                    connection.Close();
                }

                return View("Success", quoteVm);
            }
        }

        private List<v_CarMakeModelMasterList> GetVehicleMasterList()
        {
            List<v_CarMakeModelMasterList> tempList = new List<v_CarMakeModelMasterList>();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand("PROC_Car_Master_List", connection);
                command.CommandType = System.Data.CommandType.StoredProcedure;

                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    v_CarMakeModelMasterList Car = new v_CarMakeModelMasterList();
                    Car.Id = int.Parse(reader["Id"].ToString());
                    Car.Manufacturer = reader["Manufacturer"].ToString();
                    Car.Name = reader["Name"].ToString();
                    Car.ModelRisk = int.Parse(reader["ModelRisk"].ToString());
                    Car.MakeRisk = int.Parse(reader["MakeRisk"].ToString());
                    tempList.Add(Car);
                }
                reader.Close();
                connection.Close();
            }

            return tempList;
        }
    }
}