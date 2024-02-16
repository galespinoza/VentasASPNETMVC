using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SalesSystem.Library
{
    public class LTicket
    {
        private StringBuilder lineas = new StringBuilder();
        private int maxCaracter = 40, stop;

        public String LineasGuion()
        {
            string linea = "";
            for (int i = 0; i < maxCaracter; i++)
            {
                linea += "-";
            }
            return lineas.AppendLine(linea).ToString();
        }
        public String LineAsteriscos()
        {
            string asterisco = "";
            for (int i = 0; i < maxCaracter; i++)
            {
                asterisco += "*";
            }
            return lineas.AppendLine(asterisco).ToString();
        }
        public String Lineaigual()
        {
            string igual = "";
            for (int i = 0; i < maxCaracter; i++)
            {
                igual += "=";
            }
            return lineas.AppendLine(igual).ToString();
        }
        public void EncabezadoVenta(String columnas)
        {
            lineas.AppendLine(columnas);
        }
        public void TextoIzquierda(String texto)
        {
            if (texto.Length > maxCaracter)
            {
                int caracterActual = 0;
                for (int i = texto.Length; i > maxCaracter; i -= maxCaracter)
                {
                    lineas.AppendLine(texto.Substring(caracterActual, maxCaracter));
                    caracterActual += maxCaracter;
                }
                lineas.AppendLine(texto.Substring(caracterActual, texto.Length - caracterActual));
            }
            else
            {
                lineas.AppendLine(texto);
            }
        }
        public void TextoDerecho(String texto)
        {
            if (texto.Length > maxCaracter)
            {
                int caracterActual = 0;
                for (int i = texto.Length; i > maxCaracter; i -= maxCaracter)
                {
                    lineas.AppendLine(texto.Substring(caracterActual, maxCaracter));
                    caracterActual += maxCaracter;
                }
                String espacios = "";
                for (int i = 0; i < (maxCaracter - texto.Substring(caracterActual,
                    texto.Length - caracterActual).Length); i++)
                {
                    espacios += " ";
                }
                lineas.AppendLine(espacios + texto.Substring(caracterActual, texto.Length - caracterActual));
            }
            else
            {
                String espacios = "";
                for (int i = 0; i < (maxCaracter - texto.Length); i++)
                {
                    espacios += " ";
                }
                lineas.AppendLine(espacios + texto);
            }
        }
        public void TextoCentro(String texto)
        {
            if (texto.Length > maxCaracter)
            {
                int caracterActual = 0;
                for (int i = texto.Length; i > maxCaracter; i -= maxCaracter)
                {
                    lineas.AppendLine(texto.Substring(caracterActual, maxCaracter));
                    caracterActual += maxCaracter;
                }
                String espacios = "";
                int centrar = (maxCaracter - texto.Substring(caracterActual,
                    texto.Length - caracterActual).Length) / 2;
                for (int i = 0; i < centrar; i++)
                {
                    espacios += " ";
                }
                lineas.AppendLine(espacios + texto.Substring(caracterActual, texto.Length - caracterActual));
            }
            else
            {
                String espacios = "";
                int centrar = (maxCaracter - texto.Length) / 2;
                for (int i = 0; i < centrar; i++)
                {
                    espacios += " ";
                }
                lineas.AppendLine(espacios + texto);
            }
        }
        public void TextoExtremo(String izquierdo, String derecho)
        {
            String der, izq, completo = "", espacio = "";
            if (izquierdo.Length > 18)
            {
                stop = izquierdo.Length - 18;
                izq = izquierdo.Remove(18, stop);
            }
            else
            {
                izq = izquierdo;
            }
            completo = izq;
            if (derecho.Length > 20)
            {
                stop = derecho.Length - 20;
                der = derecho.Remove(20, stop);
            }
            else
            {
                der = derecho;
            }
            int numEspacios = maxCaracter - (izq.Length + der.Length);
            for (int i = 0; i < numEspacios; i++)
            {
                espacio += " ";
            }
            completo += espacio + derecho;
            lineas.AppendLine(completo);
        }
        public void AgregarTotales(String texto, Decimal total, String money)
        {
            String resumen, completo = "", espacio = "", valor;
            if (texto.Length > 25)
            {
                stop = texto.Length - 25;
                resumen = texto.Remove(25, stop);
            }
            else
            {
                resumen = texto;
            }
            completo = resumen;
            valor = money + String.Format("{0:#,###,###,##0.00####}", total);
            int numEspacios = maxCaracter - (resumen.Length + valor.Length);
            for (int i = 0; i < numEspacios; i++)
            {
                espacio += " ";
            }
            completo += espacio + valor;
            lineas.AppendLine(completo);
        }
        public void AgregarArticulo(string articulo, String cant, String precio)
        {
            String elemento = "", espacios = "";
            bool bandera = false;
            int numEspacios = 10;
            if (articulo.Length > 20)
            {
                //colocar la cantida a la derecha
                espacios = "";
                for (int i = 0; i < (numEspacios - cant.Length); i++)
                {
                    espacios += " ";
                }
                elemento += cant + espacios;
                //colocar el precio a la derecha
                espacios = "";
                for (int i = 0; i < (numEspacios - precio.Length); i++)
                {
                    espacios += " ";
                }
                elemento += precio + espacios;
                int caracterActual = 0;
                for (int i = articulo.Length; i > 20; i -= 20)
                {
                    if (bandera)
                    {
                        lineas.AppendLine(articulo.Remove(0, caracterActual));

                    }
                    else
                    {
                        lineas.AppendLine(articulo.Substring(caracterActual, 20) + elemento);
                        bandera = true;
                    }
                    caracterActual += 20;
                }
                lineas.AppendLine(articulo.Remove(0, caracterActual));
            }
            else
            {
                for (int i = 0; i < (20 - articulo.Length); i++)
                {
                    espacios += " ";  // Agrega espacios para poner el valor de cantidad
                }
                elemento = articulo + espacios;
                //colocar la cantidad a la drecha
                espacios = "";
                for (int i = 0; i < (numEspacios - cant.Length); i++)
                {
                    espacios += " ";   // Agrega espacios para poner el valor de cantidad
                }
                elemento += cant + espacios;
                //colocamos el precio a la derecha
                espacios = "";
                for (int i = 0; i < (numEspacios - cant.Length); i++)
                {
                    espacios += " "; // Agrega espacios para poner el valor de cantidad
                }
                elemento += precio + espacios;
                lineas.AppendLine(elemento);
            }
        }
        public void CortaTicket()
        {
            lineas.AppendLine("\x1B" + "m");// caracteres de corte
            lineas.AppendLine("\x1B" + "d" + "\x09");  // avanza 9 renglones               
        }
        public void AbreCajon()
        {
            lineas.AppendLine("\x1B" + "p" + "\x00" + "\x0F" + "\x96");  // caracteres de apertura cajon
        }
        public void ImprimirTicket(String impresora)
        {
            RawPrinterHelper.SendStringToPrinter(impresora, lineas.ToString());
            lineas.Clear();
        }
    }
}
