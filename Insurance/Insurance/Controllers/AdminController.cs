using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Insurance.ViewModels;
using System.Data;
using System.Data.SqlClient;

namespace Insurance.Controllers
{
    public class AdminController : Controller
    {
        private string connectionString = @"Data Source=ALLAN-YOGA3;Initial Catalog=Insurance;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";

        // GET: Admin
        public ActionResult Index()
        {
            List<QuoteVm> Quotes = new List<QuoteVm>();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand(@"PROC_Get_User_Premiums",connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    connection.Open();

                    SqlDataReader sql = command.ExecuteReader();

                    while (sql.Read())
                    {
                        QuoteVm quote = new QuoteVm();

                        quote.Id = int.Parse(sql["id"].ToString());
                        quote.FirstName = sql["FirstName"].ToString();
                        quote.LastName = sql["LastName"].ToString();
                        quote.Email = sql["Email"].ToString();
                        quote.Premium = decimal.Parse(sql["Premium"].ToString());

                        Quotes.Add(quote);
                    }

                    sql.Close();
                }
                connection.Close();
            }

            return View(Quotes);
        }
    }
}