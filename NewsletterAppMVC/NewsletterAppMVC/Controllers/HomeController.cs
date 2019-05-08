using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using NewsletterAppMVC.Models;
using NewsletterAppMVC.ViewModels;

namespace NewsletterAppMVC.Controllers
{
    public class HomeController : Controller
    {
        private readonly string connectionString = @"Data Source=ALLAN-YOGA3;Initial Catalog=Newsletter;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";

        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult SignUp(string firstName, string lastName, string emailAddress)
        {
            if (string.IsNullOrEmpty(firstName) || string.IsNullOrEmpty(lastName) || string.IsNullOrEmpty(emailAddress))
            {
                return View("~/Views/Shared/Error.cshtml");
            }
            else
            {
                

                string queryString = @"INSERT INTO Signups (FirstName, LastName, EmailAddress) VALUES (@FN, @LN, @EMAIL)";

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    SqlCommand command = new SqlCommand(queryString, connection);
                    command.Parameters.Add("@FN",SqlDbType.VarChar);
                    command.Parameters.Add("@LN", SqlDbType.VarChar);
                    command.Parameters.Add("@EMAIL", SqlDbType.VarChar);

                    command.Parameters["@FN"].Value = firstName;
                    command.Parameters["@LN"].Value = lastName;
                    command.Parameters["@EMAIL"].Value = emailAddress;

                    connection.Open();
                    command.ExecuteNonQuery();
                    connection.Close();
                }

                return View("Success");
            }
        }

        public ActionResult Admin()
        {
            string querySstring = @"SELECT Id, FirstName, LastName, EmailAddress, SocialSecurityNumber FROM Signups";
            List<NewsletterSignUp> signUps = new List<NewsletterSignUp>();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(querySstring, connection);

                connection.Open();

                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    var signup = new NewsletterSignUp();
                    signup.Id = Convert.ToInt32(reader["Id"]);
                    signup.FirstName = reader["FirstName"].ToString();
                    signup.LastName = reader["LastName"].ToString();
                    signup.EmailAddress = reader["EmailAddress"].ToString();
                    signup.SocialSecurityNumber = reader["SocialSecurityNumber"].ToString();
                    signUps.Add(signup);
                }
            }

            var signupVms = new List<SignupVm>();
            foreach (var signup in signUps)
            {
                var signupVm = new SignupVm();
                signupVm.FirstName = signup.FirstName;
                signupVm.LastName = signup.LastName;
                signupVm.EmailAddress = signup.EmailAddress;
                signupVms.Add(signupVm);
            }

            return View(signupVms);
        }

    }
}