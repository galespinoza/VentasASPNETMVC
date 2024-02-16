using Microsoft.AspNetCore.Http;
using SalesSystem.Areas.Shopping.Models;
using SalesSystem.Data;
using SalesSystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SalesSystem.Library
{
    public class LShopping : ListObject
    {
        public LShopping(ApplicationDbContext context)
        {
            _context = context;
        }
        public DataPaginador<TTemporary_shopping> GetTemporary_shopping(int page, int num, String valor, HttpRequest request)
        {
            Object[] objects = new Object[3];
            var url = request.Scheme + "://" + request.Host.Value;
            var data = getTemporary_shopping(valor);
            if (0 < data.Count)
            {
                data.Reverse();
                objects = new LPaginador<TTemporary_shopping>().paginador(data, page, num, "Shopping", "Shopping", "AddShopping", url);
            }
            else
            {
                objects[0] = "No data";
                objects[1] = "No data";
                objects[2] = new List<TTemporary_shopping>();
            }
            var models = new DataPaginador<TTemporary_shopping>
            {
                List = (List<TTemporary_shopping>)objects[2],
                Pagi_info = (String)objects[0],
                Pagi_navegacion = (String)objects[1],
                Input = new TTemporary_shopping()
            };
            return models;
        }
        public List<TTemporary_shopping> getTemporary_shopping(String valor)
        {
            List<TTemporary_shopping> listTemporary;
            var TemporaryList = new List<TTemporary_shopping>();
            if (valor == null)
            {
                listTemporary = _context.TTemporary_shopping.ToList();
            }
            else
            {
                listTemporary = _context.TTemporary_shopping.Where(u => u.Description.StartsWith(valor)).ToList();
            }
            if (!listTemporary.Count.Equals(0))
            {
                foreach (var item in listTemporary)
                {
                    TemporaryList.Add(new TTemporary_shopping
                    {
                        IdTemporary = item.IdTemporary,
                        Description = item.Description,
                        Quantity = item.Quantity,
                        Price = item.Price,
                        IdProvider = item.IdProvider,
                        Provider = item.Provider,
                        Image = item.Image,
                        Amount = item.Amount,
                        IdUser = item.IdUser,
                        Date = item.Date,
                    });
                }
            }
            return TemporaryList;
        }
        public TTemporary_shopping getShopping(int id)
        {
            var TemporaryList = new TTemporary_shopping();
            var listTemporary = _context.TTemporary_shopping.Where(u => u.IdTemporary.Equals(id)).ToList();
            if (!listTemporary.Count.Equals(0))
            {
                foreach (var item in listTemporary)
                {
                    TemporaryList = new TTemporary_shopping
                    {
                        IdTemporary = item.IdTemporary,
                        Description = item.Description,
                        Quantity = item.Quantity,
                        Price = item.Price,
                        IdProvider = item.IdProvider,
                        Provider = item.Provider,
                        Image = item.Image,
                        Amount = item.Amount,
                        IdUser = item.IdUser,
                        Date = item.Date,
                    };
                }
            }
                return TemporaryList;
        }
        public Decimal getTotalAmount()
        {
            var amount = 0.0m;
            var listTemporary = _context.TTemporary_shopping.ToList();
            if (0 < listTemporary.Count)
            {
                listTemporary.ForEach(item => {
                    amount += item.Amount;
                });
            }
            return amount;
        }
        public List<InputModelRegister> getShoppings(String valor, int id)
        {
            List<TShopping> listShopping;
            var shoppingList = new List<InputModelRegister>();
            if (valor == null && id.Equals(0))
            {
                listShopping = _context.TShopping.ToList();
            }
            else
            {
                if (id.Equals(0))
                {
                    listShopping = _context.TShopping.Where(u => u.Description.StartsWith(valor)).ToList();
                }
                else
                {
                    listShopping = _context.TShopping.Where(u => u.IdShopping.Equals(id)).ToList();
                }
                
            }
            if (!listShopping.Count.Equals(0))
            {
                foreach (var item in listShopping)
                {
                    var providerData = _context.TProviders.Where(u => u.IdProvider.Equals(item.IdProvider)).ToList().Last();
                    shoppingList.Add(new InputModelRegister
                    {
                        IdTemporary = item.IdShopping,
                        Description = item.Description,
                        Quantity = item.Quantity,
                        Price = item.Price,
                        IdProvider = item.IdProvider,
                        Image = item.Image,
                        Amount = item.Amount,
                        IdUser = item.IdUser,
                        Date = item.Date,
                        Provider = providerData.Provider,
                        Ticket = item.Ticket,
                    });
                }
            }
            return shoppingList;
        }
    }
}
