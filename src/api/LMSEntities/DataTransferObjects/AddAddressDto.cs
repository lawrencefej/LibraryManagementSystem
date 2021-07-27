using System.ComponentModel.DataAnnotations;

namespace LMSEntities.DataTransferObjects
{
    public class AddAddressDto
    {
        [Required]
        public string Street { get; set; }

        [Required]
        public string City { get; set; }

        [Required]
        public int StateId { get; set; }

        public StateDto State { get; set; }

        [Required]
        [StringLength(5, MinimumLength = 5)]
        public string Zipcode { get; set; }

    }
}
