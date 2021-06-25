namespace LMSEntities.Models
{
    public class Address
    {
        public int Id { get; set; }
        public string Street { get; set; }
        public string City { get; set; }
        public State State { get; set; }
        public int StateId { get; set; }
        public string Zipcode { get; set; }
    }
}
