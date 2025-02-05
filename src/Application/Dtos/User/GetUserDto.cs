namespace Application.Dtos.User
{
    public class GetUserDto
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string Role { get; set; }
        public string PhoneNumber { get; set; }
        public IList<string> Roles { get; set; }

        public static GetUserDto Map(Domain.Entities.User user, IList<string> roles)
        {
            return new()
            {
                Name = user.Name,
                Username = user.UserName,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber,
                Roles = roles
            };
        }

        public static IEnumerable<GetUserDto> Map(IEnumerable<Domain.Entities.User> users)
        {
            return users.Select(x => new GetUserDto
            {
                Name = x.Name,
                Username = x.UserName,
                Email = x.Email,
                Roles = new List<string>() { "" }
            });
        }
    }
}