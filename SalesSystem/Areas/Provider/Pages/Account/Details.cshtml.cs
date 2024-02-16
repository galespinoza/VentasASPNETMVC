using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SalesSystem.Areas.Provider.Models;
using SalesSystem.Data;
using SalesSystem.Library;

namespace SalesSystem.Areas.Provider.Pages.Account
{
    public class DetailsModel : PageModel
    {
        private LProvider _provider;

        public DetailsModel(
            ApplicationDbContext context)
        {
            _provider = new LProvider(context);
            ReportsModel.idProvider = 0;
        }


        public void OnGet(int id)
        {
            var data = _provider.getTProviders(null, id);
            if (0 < data.Count)
            {
                Input = new InputModel
                {
                    DataProvider = data.ToList().Last(),
                };
            }
        }
        [BindProperty]
        public InputModel Input { get; set; }
        public class InputModel
        {
            public InputModelRegister DataProvider { get; set; }
        }
    }
}
