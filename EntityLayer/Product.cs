using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityLayer
{
    public class Product
    {
        public int IdProduct { get; set; }
        public string nameProduct { get; set; }
        public string descriptionProduct { get; set; }
        public Category idCategory { get; set; }
        public decimal priceProduct { get; set; }
        public string priceProductText { get; set; }
        public int stockProduct { get; set; } 
        public string routeImage { get; set; }
        public string nameImage { get; set; }
        public bool activeProduct { get; set; }
        public string base64Image { get;set; }
        public string extensionImageProduct { get; set; }
    }
}
