using SalesSystem.Areas.Setting.Models;
using SalesSystem.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SalesSystem.Library
{
    public class LSetting : ListObject
    {
        public static String Mony { get; set; }
        public static Decimal Interests { set; get; }
        private List<TSetting> setting;
        private string _errorMessage = null;

        public LSetting(ApplicationDbContext context)
        {
            _context = context;
            GetSetting();
        }
        public async Task<String> TypeMoneyAsync(int radioOptions)
        {
            try
            {
                using (var dbContext = new ApplicationDbContext())
                {
                    setting = dbContext.TSetting.ToList();
                }
                if (setting.Count.Equals(0))
                {
                    Mony = "L.";
                    var data = new TSetting
                    {
                        TypeMoney = "L.",
                        Interests = 0.0m,
                    };
                    await _context.AddAsync(data);
                    _context.SaveChanges();
                }
                else
                {
                    var data = setting.Last();
                    switch (radioOptions)
                    {
                        case 1:
                            Mony = "L.";
                            break;
                        case 2:
                            Mony = "$";
                            break;
                    }
                    using (var dbContext = new ApplicationDbContext())
                    {
                        var data1 = new TSetting
                        {
                            ID = data.ID,
                            TypeMoney = Mony,
                            Interests = data.Interests,
                        };
                        dbContext.Update(data1);
                        dbContext.SaveChanges();
                    }
                }
            }
            catch (Exception ex)
            {

                _errorMessage = ex.Message;
            }
            return _errorMessage;
        }
        public void GetSetting()
        {
            var setting = _context.TSetting.ToList();
            if (!setting.Count.Equals(0))
            {
                var data = setting.Last();
                Mony = data.TypeMoney;
                Interests = data.Interests;
            }
        }
        public async Task<String> SetInterests(Decimal? interests)
        {
            try
            {
                using (var dbContext = new ApplicationDbContext())
                {

                    setting = dbContext.TSetting.ToList();
                }
                if (setting.Count.Equals(0))
                {
                    Mony = "L";
                    var data = new TSetting
                    {
                        TypeMoney = "L.",
                        Interests = (Decimal)interests,
                    };
                    await _context.AddAsync(data);
                    _context.SaveChanges();
                }
                else
                {
                    var data = setting.Last();
                    using (var dbContext = new ApplicationDbContext())
                    {
                        var data1 = new TSetting
                        {
                            ID = data.ID,
                            TypeMoney = data.TypeMoney,
                            Interests = (Decimal)interests,
                        };
                        dbContext.Update(data1);
                        dbContext.SaveChanges();
                    }
                }
            }
            catch (Exception ex)
            {

                _errorMessage = ex.Message;
            }
            return _errorMessage;
        }
    }
}
