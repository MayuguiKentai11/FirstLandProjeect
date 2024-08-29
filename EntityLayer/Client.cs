namespace EntityLayer
{
    public class Client
    {
        public int ID { get; set; }
        public string name { get; set; }
        public string email { get; set; }
        public string password { get; set; }
        public bool resetPassword { get; set; }
        
        public string lastPassword { get; set; }
    }
}
