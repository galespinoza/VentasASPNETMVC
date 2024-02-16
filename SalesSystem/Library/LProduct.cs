using Microsoft.AspNetCore.Http;
using SalesSystem.Areas.Product.Models;
using SalesSystem.Data;
using SalesSystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SalesSystem.Library
{
    public class LProduct : ListObject
    {
        public LProduct(ApplicationDbContext context)
        {
            _context = context;
        }
        public DataPaginador<InputModelRegister> GetTemporary_product(int page, int num, String valor, HttpRequest request, String idUser)
        {
            var objects = new Object[3];
            var url = request.Scheme + "://" + request.Host.Value;
            var data = getTemporary_product(valor,idUser);
            if (0 < data.Count)
            {
                data.Reverse();
                objects = new LPaginador<InputModelRegister>().paginador(data, page, num, "Product", "Product", "AddProduct", url);
            }
            else
            {
                objects[0] = "No data";
                objects[1] = "No data";
                objects[2] = new List<InputModelRegister>();
            }
            var models = new DataPaginador<InputModelRegister>
            {
                List = (List<InputModelRegister>)objects[2],
                Pagi_info = (String)objects[0],
                Pagi_navegacion = (String)objects[1],
                Input = new InputModelRegister()
            };
            return models;
        }
        public List<InputModelRegister> getTemporary_product(String valor, String idUser)
        {
            var TemporaryList = new List<InputModelRegister>();
            var query = valor == null ? _context.TTemporary_product.Join(_context.TShopping,
                p => p.IdShopping, r => r.IdShopping, (p, r) => new
                {
                    p.IdShopping,
                    p.IdUser,
                    r.Description,
                    r.Quantity,
                    r.Price,
                    r.Amount,
                    r.Image,
                    r.Ticket,

                }).Where(u => u.IdUser.Equals(idUser)).ToList()

                :
            _context.TTemporary_product.Join(_context.TShopping,
                p => p.IdShopping, r => r.IdShopping, (p, r) => new
                {
                    p.IdShopping,
                    p.IdUser,
                    r.Description,
                    r.Quantity,
                    r.Price,
                    r.Amount,
                    r.Image,
                    r.Ticket,

                }).Where(u => u.Description.StartsWith(valor) && u.IdUser.Equals(idUser)).ToList();
            if (!query.Count.Equals(0))
            {
                foreach (var item in query)
                {
                    TemporaryList.Add(new InputModelRegister {
                        IdShopping = item.IdShopping,
                        Description = item.Description,
                        Quantity = item.Quantity,
                        Price = item.Price,
                        Amount = item.Amount,
                        Image = item.Image,
                        Ticket = item.Ticket,
                    });
                }
            }
            return TemporaryList;
        }
        public List<InputModelRegister> getProducts(String valor, int id)
        {
            List<TProduct> listProduct;
            var productList = new List<InputModelRegister>();
            if (valor == null && id.Equals(0))
            {
                listProduct = _context.TProduct.ToList();
            }
            else
            {
                if (id.Equals(0))
                {
                    listProduct = _context.TProduct.Where(u => u.Description.StartsWith(valor)).ToList();
                }
                else
                {
                    listProduct = _context.TProduct.Where(u => u.IdProduct.Equals(id)).ToList();
                }
            }
            if (!listProduct.Count.Equals(0))
            {
                foreach (var item in listProduct)
                {
                    productList.Add(new InputModelRegister
                    {
                        IdProduct = item.IdProduct,
                        Description = item.Description,
                        Price = item.Price,
                        Barcode = item.Barcode,
                        Image = item.Image,
                        Department = item.Department,
                        Categories = item.Categories,
                        Date = item.Date,
                        Ticket = item.Ticket,
                    });
                }
            }
            return productList;
        }
    }
}
