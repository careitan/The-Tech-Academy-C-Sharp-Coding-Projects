using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Insurance.ViewModels
{
    public class QuoteVm
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public decimal Premium { get; set; }
    }
}