using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SalesSystem.Areas.Customers.Models;
using SalesSystem.Data;
using SalesSystem.Library;

namespace SalesSystem.Areas.Customers.Pages.Account
{
    [Authorize]
    public class DetailsModel : PageModel
    {
        private LCustomers _customer;
        public DetailsModel(
            ApplicationDbContext context)
        {
            _customer = new LCustomers(context);
        }
        public void OnGet(int id)
        {
            var data = _customer.getTClients(null, id);
            if (0 < data.Count)
            {
                Input = new InputModel
                {
                    DataClient = data.ToList().Last(),
                };
            }
        }
        [BindProperty]
        public InputModel Input { get; set; }
        public class InputModel
        {
            public InputModelRegister DataClient { get; set; }
        }
    }
}
