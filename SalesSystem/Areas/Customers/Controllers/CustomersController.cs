using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SalesSystem.Areas.Customers.Models;
using SalesSystem.Data;
using SalesSystem.Library;
using SalesSystem.Models;

namespace SalesSystem.Areas.Customers.Controllers
{
    [Authorize]
    [Area("Customers")]
    public class CustomersController : Controller
    {
        private LCustomers _customer;
        private SignInManager<IdentityUser> _signInManager;
        private static DataPaginador<InputModelRegister> models;

        public CustomersController(
           SignInManager<IdentityUser> signInManager,
           ApplicationDbContext context)
        {
            _signInManager = signInManager;
            _customer = new LCustomers(context);
        }
        public IActionResult Customers(int id, String filtrar)
        {
            if (_signInManager.IsSignedIn(User))
            {
                Object[] objects = new Object[3];
                var data = _customer.getTClients(filtrar, 0);
                if (0 < data.Count)
                {
                    var url = Request.Scheme + "://" + Request.Host.Value;
                    objects = new LPaginador<InputModelRegister>().paginador(data,
                        id, 10, "Customers", "Customers", "Customers", url);
                }
                else
                {
                    objects[0] = "No hay datos que mostrar";
                    objects[1] = "No hay datos que mostrar";
                    objects[2] = new List<InputModelRegister>();
                }
                models = new DataPaginador<InputModelRegister>
                {
                    List = (List<InputModelRegister>)objects[2],
                    Pagi_info = (String)objects[0],
                    Pagi_navegacion = (String)objects[1],
                    Input = new InputModelRegister(),
                };
                return View(models);
            }
            else
            {
                return Redirect("/");
            }
                
        }
        public String Fees(int fees, int idClient)
        {

            return fees.Equals(0) ? "" : _customer.AmountFees(fees, idClient);
            
        }
    }
}