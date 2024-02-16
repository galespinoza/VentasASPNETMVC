using Microsoft.EntityFrameworkCore;
using SalesSystem.Areas.Principal.Models;
using SalesSystem.Areas.Product.Models;
using SalesSystem.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SalesSystem.Library
{
    public class LSales : ListObject
    {
        public LSales(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<string> temporarySalesAsync(InputModels model, string idUser, int idTemporary)
        {
            String value = null;
            var box = LUser.boxData != null ? LUser.boxData.Box : 0;
            var product = _context.TProduct.Join(_context.TGrocery_store, p => p.IdProduct, r => r.IdProduct
            , (p, r) => new
            {
                p.IdProduct,
                p.Barcode,
                p.Description,
                p.Price,
                r.Stock,

            }).Where(c => c.Barcode.Equals(model.Search) && c.Stock > 0).ToList();
            if (0 < product.Count)
            {
                var data1 = 0;
                var data2 = 0;
                List<Temporary_Sales> tempsales;
                List<TGrocery_store> store;
                using (var dbContext = new ApplicationDbContext())
                {
                    tempsales = _context.Temporary_Sales.Where(t => t.Barcode.Equals(model.Search) && t.Box.Equals(box) && t.IdUser.Equals(idUser)).ToList();
                    store = _context.TGrocery_store.Where(u => u.IdProduct.Equals(product.First().IdProduct)).ToList();

                }
                if (0 < tempsales.Count())
                {
                    if (tempsales.First().Quantity > model.Quantity)
                    {
                        if (product.First().Stock > 0)
                        {
                            if (idTemporary.Equals(0))
                            {
                                data2 = product.First().Stock - model.Quantity;
                            }
                            else
                            {
                                data1 = tempsales.First().Quantity - model.Quantity;
                                data2 = product.First().Stock + data1;
                            }
                        }
                        else
                        {
                            return "Productos sin existencia";
                        }
                    }
                    else if (model.Quantity > tempsales.First().Quantity)
                    {
                        if (product.First().Stock > 0)
                        {
                            data1 = model.Quantity - tempsales.First().Quantity;
                            if (data1 <= product.First().Stock)
                            {
                                data2 = product.First().Stock - data1;
                            }
                            else
                            {
                                return "Se sobrepaso de la cantidad de existencia del producto";
                            }
                        }
                        else
                        {
                            return "Productos sin existencia";
                        }
                    }
                    else if (model.Quantity == tempsales.First().Quantity)
                    {
                        if (idTemporary.Equals(0))
                        {
                            data2 = product.First().Stock - model.Quantity;
                        }
                        else
                        {
                            return null;
                        }
                    }
                }
                else
                {
                    if (product.First().Stock > 0)
                    {
                        if (model.Quantity <= product.First().Stock)
                        {
                            data2 = product.First().Stock - model.Quantity;
                        }
                        else
                        {
                            return "Se sobrepaso de la cantidad de existencia del producto";
                        }
                    }
                    else
                    {
                        return "Productos sin existencia";
                    }
                }

                var precio = product.First().Price;
                var strategy = _context.Database.CreateExecutionStrategy();
                await strategy.ExecuteAsync(async () =>
                {
                    using (var transaction = _context.Database.BeginTransaction())
                    {
                        try
                        {
                            if (idTemporary.Equals(0))
                            {
                                if (0 < tempsales.Count())
                                {
                                    var importe = precio * model.Quantity;
                                    var temporary = tempsales.First();
                                    temporary.IdTempo = temporary.IdTempo;
                                    temporary.Quantity = model.Quantity;
                                    temporary.Amount = importe;

                                    _context.Update(temporary);
                                    _context.SaveChanges();
                                }
                                else
                                {
                                    var importe = precio * model.Quantity;
                                    var temporary = new Temporary_Sales
                                    {
                                        Barcode = product.First().Barcode,
                                        Description = product.First().Description,
                                        Price = precio,
                                        Quantity = model.Quantity,
                                        Amount = importe,
                                        Box = box,
                                        IdUser = idUser,
                                    };
                                    await _context.AddAsync(temporary);
                                    _context.SaveChanges();
                                }
                            }
                            else
                            {
                                var importe = precio * model.Quantity;
                                var temporary = tempsales.First();
                                temporary.IdTempo = idTemporary;
                                temporary.Quantity = model.Quantity;
                                temporary.Amount = importe;
                                _context.Update(temporary);
                                _context.SaveChanges();
                            }
                            if (!store.Count().Equals(0))
                            {
                                var storeData = store.First();
                                storeData.Stock = data2;
                                _context.Update(storeData);
                                _context.SaveChanges();
                            }
                            transaction.Commit();
                        }
                        catch (Exception ex)
                        {
                            value = ex.Message;
                            transaction.Rollback();
                        }
                    }
                });
            }
            else
            {
                value = "Producto sin existencia";
            }
            return value;
        }
        public List<InputModels> getTSales(String valor, int id, int box, String user)
        {
            List<Temporary_Sales> listTemporary;
            var salesList = new List<InputModels>();
            if (valor == null && id.Equals(0))
            {
                listTemporary = _context.Temporary_Sales.Where(u => u.IdUser.Equals(user) && u.Box.Equals(box)).ToList();
            }
            else
            {
                if (id.Equals(0))
                {
                    listTemporary = _context.Temporary_Sales.Where(u => u.Description.StartsWith(valor) || u.Barcode.StartsWith(valor) && u.IdUser.Equals(user) && u.Box.Equals(box)).ToList();
                }
                else
                {
                    listTemporary = _context.Temporary_Sales.Where(u => u.IdTempo.Equals(id) && u.IdUser.Equals(user) && u.Box.Equals(box)).ToList();
                }
            }
            if (!listTemporary.Count.Equals(0))
            {
                foreach (var item in listTemporary)
                {
                    salesList.Add(new InputModels
                    {
                        IdTempo = item.IdTempo,
                        Barcode = item.Barcode,
                        Description = item.Description,
                        Quantity = item.Quantity,
                        Price = item.Price,
                        Box = item.Box,
                        Amount = item.Amount,
                        IdUser = item.IdUser,
                    });
                }
            }
            return salesList;
        }
        public async Task DeleteAsync(int id)
        {
            Temporary_Sales dataShoppin = null;
            var cant = 0;
            var idProduct = 0;
            var strategy = _context.Database.CreateExecutionStrategy();
            await strategy.ExecuteAsync(async () =>
            {
                using (var transaction = _context.Database.BeginTransaction())
                {
                    try
                    {
                        using (var dbContext = new ApplicationDbContext())
                        {
                            dataShoppin = dbContext.Temporary_Sales
                           .Where(u => u.IdTempo.Equals(id)).ToList().Last();

                           var grocery = dbContext.TProduct.Join(dbContext.TGrocery_store, p => p.IdProduct, r => r.IdProduct, (p, r) => new
                            {
                                p.IdProduct,
                                p.Barcode,
                                p.Description,
                                p.Price,
                                r.Stock,

                            }).Where(c => c.Barcode.Equals(dataShoppin.Barcode)).ToList();
                            cant = grocery.First().Stock;
                            idProduct = grocery.First().IdProduct;
                        }
                        _context.Remove(dataShoppin);
                        _context.SaveChanges();
                        var dataGrocery =_context.TGrocery_store.Where(c => c.IdProduct.Equals(idProduct)).ToList().First();
                        dataGrocery.Stock += cant;
                        _context.Update(dataGrocery);
                        _context.SaveChanges();

                        transaction.Commit();
                    }
                    catch (Exception)
                    {

                        transaction.Rollback();
                    }
                }
            });
        }
        private static Decimal amount = 0.0m;
        private List<Temporary_Sales> listTemporary;
        public Decimal getTotalAmount(int box, String user)
        {
            amount = 0.0m;
            listTemporary = _context.Temporary_Sales.Where(u => u.IdUser.Equals(user) && u.Box.Equals(box)).ToList();
            if (!listTemporary.Count.Equals(0))
            {
                listTemporary.ForEach(item => {
                    amount += item.Amount;
                });
            }
            return amount;
        }
        private Decimal totalIncome = 0.0m;
        public List<Decimal> getIncomeBox(int idBox, String user)
        {
            totalIncome = 0.0m;
            var entry = new List<Decimal>();
            var initialIncome = _context.TIncome_boxes.Where(t => t.TBoxesIdBox.Equals(idBox) && t.IdUser.Equals(user)).ToList();
            if (!initialIncome.Count.Equals(0))
            {
                totalIncome = initialIncome.First().Entry;
            }
            entry.Add(totalIncome);
            var salesIncome = _context.TReport_boxes.Where(t => t.IdBox.Equals(idBox) && t.IdUser.Equals(user) && t.IncomeType.Equals("venta") && t.Fecha.Date.Equals(DateTime.Now.Date)).ToList();
            if (!salesIncome.Count.Equals(0))
            {
                var data = salesIncome.Last().Entry;
                entry.Add(data);
                totalIncome += data;
            }
            else
            {
                entry.Add(0.0m);
            }
            entry.Add(totalIncome);
            return entry;
        }
        private Decimal _change = 0.0m;
        private static Decimal _payment = 0.0m;
        private bool _check = false, _yourChange = false;
        internal String CustomerPayment(InputModels input)
        {
            string value = null;

            if (!amount.Equals(0.0M))
            {
                _payment = input.Payments;
                if (_payment > 0)
                {
                    if (_payment > amount)
                    {
                        _change = _payment - amount;
                        if (_change > totalIncome)
                        {
                            _check = false;
                            _yourChange = false;
                            value = "No hay suficientes ingresos en caja";
                        }
                        else if (input.Credit)
                        {
                            _check = false;
                            _yourChange = false;
                            value = "Desmarque la opción crédito";
                        }
                        else
                        {
                            _check = true;
                            _yourChange = true;
                        }
                    }
                    else if (_payment < amount)
                    {

                    }
                }
            }
            return value;
        }
    }
}
