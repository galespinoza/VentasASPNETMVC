using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SalesSystem.Areas.Product.Models;
using SalesSystem.Data;
using SalesSystem.Library;

namespace SalesSystem.Areas.Product.Pages.Account
{
    public class DetailsProductModel : PageModel
    {
        private LProduct _product;
        public DetailsProductModel(
            ApplicationDbContext context)
        {
            _product = new LProduct(context);
        }
        public void OnGet(int id)
        {
            var data = _product.getProducts(null, id);
            if (0 < data.Count)
            {
                Input = new InputModel
                {
                    DataProduct = data.Last(),
                };
            }
        }
        [BindProperty]
        public InputModel Input { get; set; }
        public class InputModel
        {
            public InputModelRegister DataProduct { get; set; }
        }
    }
}
