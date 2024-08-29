using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityLayer
{
    public class ShoppingProduct
    {
        public string DateRegisterSale { get; set; }

        public string RouteImageProduct { get; set; }

        public string NameProduct { get;set; }

        public int OrderProduct { get; set; }

        public decimal PriceProduct { get; set; }

        public decimal TotalProduct { get; set; }

        public string IdTransaction { get; set; }

        public Product IdProduct { get; set; }

        public string NameImageProduct { get; set; }
    }
}
