using System.ComponentModel.DataAnnotations;

namespace AGDATAApi.Models
{
    public class Location
    {
        [Required(ErrorMessage = "Id can not be empty.")]
        public string Id { get; set; }

        [Required(ErrorMessage = "Name can not be empty.")]
        public string Name { get; set; }

        [Required(ErrorMessage = "StreetName can not be empty.")]
        public string StreetName { get; set; }

        [Required(ErrorMessage = "City can not be empty.")]
        public string City { get; set; }

        [Required(ErrorMessage = "Province can not be empty.")]
        public string Province { get; set; }

        [Required(ErrorMessage ="Postal Code can not be empty.")]
        public string PostalCode { get; set; }
    }
}
