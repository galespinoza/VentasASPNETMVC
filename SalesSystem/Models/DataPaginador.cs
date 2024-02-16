using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SalesSystem.Models
{
    public class DataPaginador<T>
    {
        public List<T> List { get; set; }
        public string Pagi_info { get; set; }
        public string Pagi_navegacion { get; set; }
        public T Input { get; set; }
        public string Search { get; set; }
    }
}
