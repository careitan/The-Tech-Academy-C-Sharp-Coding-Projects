using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Insurance.Models;
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
        public ActionResult Quote(string FirstName, string LastName, 
            string EmailAddress, DateTime DOB, 
            string Make, string Model, string CarYear,
            bool DUI, bool FullCoverage, string SpeedTickets
            )
        {
            if (string.IsNullOrEmpty(FirstName) || string.IsNullOrEmpty(LastName) ||
                string.IsNullOrEmpty(EmailAddress) || string.IsNullOrEmpty(Make) ||
                string.IsNullOrEmpty(Model) || string.IsNullOrEmpty(CarYear) ||
                string.IsNullOrEmpty(SpeedTickets)
                )
            {
                return View("~/Views/Shared/Error.cshtml");
            }
            else
            {
                List<v_CarMakeModelMasterList> LookupCars = GetVehicleMasterList();


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