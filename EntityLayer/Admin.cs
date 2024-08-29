using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityLayer
{
    public class Admin
    {
        public Admin() { }
        public int Id { get; set; }
        public string nameAdmin { get; set; }
        public string lastNameAdmin { get; set; }
        public string emailAdmin { get; set; }
        public string passwordAdmin { get; set; }
        public bool resetPasswordAdmin { get; set; }
        public bool activeAdminAccount { get; set; }

    }
}
