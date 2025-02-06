using System.Text.Json.Serialization;

namespace Application.Dtos.Customer
{
    public class CreateCustomerDto
    {
        public string Name { get; set; }
        public string Phone { get; set; }

        [JsonIgnore]
        public string UserId { get; set; }

        [JsonIgnore]
        public int CompanyId { get; set; }
    }
}