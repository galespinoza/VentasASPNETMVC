using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SalesSystem.Areas.Principal.Models;
using SalesSystem.Data;
using SalesSystem.Library;
using SalesSystem.Models;

namespace SalesSystem.Areas.Principal.Controllers
{
    [Area("Principal")]
    public class PrincipalController : Controller
    {
        private LSetting _setting;
        private LCustomers _customer;
        private static InputModels _model, _dataPayment;
        private UserManager<IdentityUser> _userManager;
        private static DataPaginador<InputModels> models;
        private SignInManager<IdentityUser> _signInManager;
        private static String ErrorMessage1 = null;
        private LSales _sale;
        private static int _idTemporary, _box;
        private static String _idUser = null;

        public PrincipalController(
          ApplicationDbContext context,
          UserManager<IdentityUser> userManager,
          SignInManager<IdentityUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _setting = new LSetting(context);
            _customer = new LCustomers(context);
            _sale = new LSales(context);
            _box = LUser.boxData != null ? LUser.boxData.Box : 0;
        }
        public IActionResult Principal(int id, String filtrar, int registros)
        {
            if (_signInManager.IsSignedIn(User))
            {
                _idUser = _userManager.GetUserId(User);
                Object[] objects = new Object[3];
                var data = _sale.getTSales(filtrar, 0, _box, _idUser);
                if (0 < data.Count)
                {
                    var url = Request.Scheme + "://" + Request.Host.Value;
                    objects = new LPaginador<InputModels>().paginador(data, id, registros, "Principal", "Principal", "Principal", url);
                }
                else
                {
                    objects[0] = "No hay datos que mostrar";
                    objects[1] = "No hay datos que mostrar";
                    objects[2] = new List<InputModels>();
                }
                models = new DataPaginador<InputModels>
                {
                    List = (List<InputModels>)objects[2],
                    Pagi_info = (String)objects[0],
                    Pagi_navegacion = (String)objects[1],
                    Input = new InputModels(),
                };
                models.Input.IdPage = id;
                if (ErrorMessage1 != null)
                {
                    models.Input.Search = _model.Search;
                    models.Input.ErrorMessage1 = ErrorMessage1;
                }
                if (_dataPayment != null)
                {
                    models.Input.Payments = _dataPayment.Payments;
                    models.Input.Credit = _dataPayment.Credit;
                }
                    _model = null;
                ErrorMessage1 = null;
                _dataPayment = null;
                _customer.getTClientReport(0);
                models.Input.TotalDebt = _sale.getTotalAmount(_box, _idUser);
                models.Input.TotalIncome = _sale.getIncomeBox(LUser.boxData.IdBox, _idUser);
                return View(models);
            }
            else
            {
                return Redirect("/");
            }
        }
        [HttpPost]
        public IActionResult Search(InputModels model)
        {
            _model = model;
            var data = _sale.temporarySalesAsync(model, _idUser,_idTemporary);
            if (data != null)
            {
                ErrorMessage1 = data.Result;
            }
            return RedirectToAction(nameof(PrincipalController.Principal), "Principal");
        }
        public IActionResult GetSales(int id, int IdTemporary)
        {
            _idTemporary = IdTemporary;
            models.Input.IdPage = id;
            var data = _sale.getTSales(null, IdTemporary, _box, _idUser);
            if (0 < data.Count)
            {
                models.Input.Search = data.First().Barcode;
                models.Input.Quantity = data.First().Quantity;
            }
            return View("Principal", models);
        }
        public IActionResult DeleteSales(int id, int idTemporary)
        {
            //_idTemporary = idTemporary;
            models.Input.IdPage = id;
            _ = _sale.DeleteAsync(idTemporary);
            return Redirect("/Principal/Principal");
        }
        public IActionResult CollectSales(DataPaginador<InputModels> data)
        {
            if (data.Input != null)
            {
                var value = _sale.CustomerPayment(data.Input);
                if (value != null)
                {
                    _dataPayment = data.Input;
                    ErrorMessage1 = value;
                }
                else
                {
                    _dataPayment = null;
                }
            }
            return RedirectToAction(nameof(PrincipalController.Principal), "Principal");
        }
    }
}