using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SalesSystem.Areas.Shopping.Models;
using SalesSystem.Data;
using SalesSystem.Library;

namespace SalesSystem.Areas.Shopping.Pages.Account
{
    [Authorize]
    [Area("Shoppin")]
    public class DetailsShoppingModel : PageModel
    {
        private LShopping _shopping;
        public DetailsShoppingModel(
            ApplicationDbContext context)
        {
            _shopping = new LShopping(context);
        }
        public void OnGet(int id)
        {
            var data = _shopping.getShoppings(null, id);
            if (0 < data.Count)
            {
                Input = new InputModel
                {
                    DataShopping = data.ToList().Last(),
                };
            }
        }
        [BindProperty]
        public InputModel Input { get; set; }
        public class InputModel
        {
            public InputModelRegister DataShopping { get; set; }
        }
    }
}
