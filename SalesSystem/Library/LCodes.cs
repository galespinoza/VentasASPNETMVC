using BarcodeLib;
using Microsoft.AspNetCore.Hosting;
using SalesSystem.Areas.Product.Models;
using SalesSystem.Data;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace SalesSystem.Library
{
    public class LCodes
    {
        public Barcode barcode = new Barcode();
        public static Random rnd = new Random();
        private ApplicationDbContext _context;
        private IWebHostEnvironment _environment;
        public string Codigo { set; get; }
        public string BarcodeImage { set; get; }

        public LCodes()
        {

        }
        public LCodes(ApplicationDbContext context, IWebHostEnvironment environment)
        {
            _context = context;
            _environment = environment;
        }

        public String codesTickets(String Codigo)
        {
            String ticket = null;
            if (Codigo == null || Codigo.Equals("0000000000"))
            {
                ticket = "0000000001";
            }
            else
            {
                if (Codigo.Equals("9999999999"))
                {
                    ticket = "0000000001";
                }
                else
                {
                    var cod = Convert.ToInt64(Codigo);
                    cod++;
                    ticket = cod.ToString("D10");
                }
            }
            return ticket;
        }
        public String codigoBarra(
            string codigosBarra,
            String producto,
            Decimal precio)
        {
            int codigo = 0, count = 0;
            Codigo = codigosBarra;
            List<TProduct> product1;
            if (0 < _context.TProduct.Count())
            {
                product1 = _context.TProduct.Where(p => p.Description.Replace(" ", "").ToLower().Equals(producto.Replace(" ", "").ToLower()) && p.Price.Equals(precio)).ToList();
            }
            else
            {
                product1 = new List<TProduct>();
            }
            
            if (codigosBarra.Equals(""))
            {
                if (0 < product1.Count)
                {
                    Codigo = product1[0].Barcode;
                }
                else
                {
                    do
                    {
                        for (int i = 1; i <= 20; i++)
                        {
                            codigo = rnd.Next(1000000, 10000001);
                        }
                        Codigo = Convert.ToString(codigo);
                        var product2 = _context.TProduct.Where(p => p.Barcode.Equals(Codigo)).ToList();
                        count = product2.Count();
                    } while (count > 0);
                }
            }
            else
            {
                var product2 = _context.TProduct.Where(p => p.Barcode.Equals(codigosBarra)).ToList();
                if (0 < product2.Count)
                {
                    if (0 < product1.Count)
                    {
                        var data1 = product1[0].Description.Replace(" ", "").ToLower();
                        var data2 = product2[0].Description.Replace(" ", "").ToLower();
                        if (data1.Equals(data2) && product1[0].Price.Equals(product2[0].Price))
                        {
                            Codigo = codigosBarra;
                        }
                        else
                        {
                            Codigo = null;
                        }
                    }
                    else
                    {
                        Codigo = codigosBarra;
                    }
                }
                else
                {
                    Codigo = codigosBarra;
                }
            }
            if (Codigo != null)
            {

                var dir = $"{_environment.ContentRootPath}/wwwroot/codes";
                if (!(Directory.Exists(dir)))
                {
                    Directory.CreateDirectory(dir);
                }
                barcode.IncludeLabel = true;
                var image = barcode.Encode(TYPE.CODE128, Codigo, Color.Black, Color.White, 196, 65);
                using (MemoryStream ms = new MemoryStream())
                {
                    using (Bitmap bitMap = new Bitmap(image))
                    {
                        bitMap.Save(ms, ImageFormat.Png);
                        bitMap.Save(dir + "\\" + Codigo + ".png", ImageFormat.Png);
                        BarcodeImage = "data:image/png;base64," + Convert.ToBase64String(ms.ToArray());
                    }
                }
            }
                return Codigo;
        }
    }
}
