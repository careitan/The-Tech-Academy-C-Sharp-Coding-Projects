using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Insurance.Models;
using Insurance.ViewModels;
using System.Data.SqlClient;
using System.Data.SqlTypes;

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
                



                return View("Success");
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