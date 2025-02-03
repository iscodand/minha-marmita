namespace Application.DTOs.Authentication
{
    public class GetAuthenticatedUserDto
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string Role { get; set; }
        public int CompanyId { get; set; }
        public string CompanyName { get; set; }
    }
}