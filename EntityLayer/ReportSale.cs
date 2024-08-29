using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityLayer
{
    public class ReportSale
    {
        public string dateSale { get;set; }
        public string client { get;set; }
        public string nameProduct { get; set; }
        public decimal priceProduct { get; set; }
        public int orderProduct { get; set; }
        public decimal totalProduct { get; set; }
        public string idTransaction { get; set; }
    }
}
