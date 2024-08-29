using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityLayer
{
    public  class DetailSale
    {
        public DetailSale() { }
        public int Id { get; set; }
        public int IdSale { get; set; }
        public Product IdProduct { get; set; }
        public int orderProduct { get; set; }
        public decimal totalProduct { get; set; }
        public string IdTransaction { get; set; }
    }
}
