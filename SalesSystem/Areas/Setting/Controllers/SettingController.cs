using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SalesSystem.Areas.Setting.Models;
using SalesSystem.Data;
using SalesSystem.Library;

namespace SalesSystem.Areas.Setting.Controllers
{
    [Authorize]
    [Area("Setting")]
    public class SettingController : Controller
    {
        private LSetting _setting;
        private ApplicationDbContext _context;
        private static string _errorMessage;

        public SettingController(
           ApplicationDbContext context)
        {
            //_context = context;
            _setting = new LSetting(context);
        }
        public IActionResult Setting()
        {
            var model = new InputModelSetting();
            model.Type_Money = LSetting.Mony;
            model.FormatInterests = "%" + String.Format("{0:#,###,###,##0.00####}", LSetting.Interests);
            if (_errorMessage != null)
            {
                model.ErrorMessage = _errorMessage;
            }
            _errorMessage = null;
            return View(model);
        }
        [HttpPost]
        public IActionResult TypeMoney(InputModelSetting model)
        {
            var data = _ = _setting.TypeMoneyAsync(model.RadioOptions);
            _errorMessage = data.Result;
            return Redirect("/Setting/Setting?area=Setting");
        }
        [HttpPost]
        public IActionResult SetInterests(InputModelSetting model)
        {
            var data = _ = _setting.SetInterests(model.Interests);
            _errorMessage = data.Result;
            return Redirect("/Setting/Setting?area=Setting");
        }
    }
}