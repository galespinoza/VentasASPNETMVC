using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SalesSystem.Areas.Users.Models
{
    public class TUsers
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string LastName { get; set; }
        public string NID { get; set; }
        public string Email { get; set; }
        public string IdUser { get; set; }
        public byte[] Image { get; set; }
    }
}
