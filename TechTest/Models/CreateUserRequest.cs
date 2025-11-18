

using System.Text.Json.Serialization;
using System.ComponentModel.DataAnnotations;

namespace TechTest.Models
{
    public class CreateUserRequest
    {
        [JsonPropertyName("name")]
        [Required(ErrorMessage = "Name is required.")]
        public string Name { get; set; }

        [JsonPropertyName("address")]
        [Required(ErrorMessage = "Address is required.")]
        public UserAddress Address { get; set; }

        [JsonPropertyName("phoneNumber")]
        [Required(ErrorMessage = "Phone number is required.")]
        [RegularExpression(@"^\+[1-9]\d{1,14}$", ErrorMessage = "Invalid Phone number")]
        public string PhoneNumber { get; set; }

        [JsonPropertyName("email")]
        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Invalid email address.")]
        public string Email { get; set; }
    }

    public class UserAddress
    {
        [JsonPropertyName("line1")]
        [Required(ErrorMessage = "Address line1 is required.")] 
        public string Line1 { get; set; }

        [JsonPropertyName("line2")]
        public string? Line2 { get; set; }

        [JsonPropertyName("line3")]        
        public string? Line3 { get; set; }

        [JsonPropertyName("town")]
        [Required(ErrorMessage = "town is required.")]
        public string Town { get; set; }

        [JsonPropertyName("county")]
        [Required(ErrorMessage = "county is required.")]
        public string County { get; set; }

        [JsonPropertyName("postcode")]
        [Required(ErrorMessage = "Postcode is required.")]
        public string Postcode { get; set; }
    }
}
