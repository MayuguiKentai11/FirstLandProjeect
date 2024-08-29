using System.Collections.Generic;

namespace EntityLayer
{
    public  class Sale
    {
        public int Id { get; set; }
        
        public int IdClient { get; set; } // int
        
        public decimal totalSaleCost { get; set; }
        
        public string contactSale { get; set; }
        
        public string IdDistrict { get; set; }
        
        public string telephoneClient { get; set; }
        
        public string addressClient { get; set; }
        
        public string IdTransaction { get; set; }
        
        public List<DetailSale> details { get; set; }

        public decimal IGV { get; set; }

        public decimal subTotal { get; set; }

        public string email { get; set; }
    }
}
