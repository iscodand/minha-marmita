namespace Application.Dtos.User
{
    public class UpdateUserDto
    {
        public string Username { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public int CompanyId { get; set; }

        public static Domain.Entities.User Map(Domain.Entities.User user, UpdateUserDto request)
        {
            user.Name = request.Name;
            user.NormalizedName = request.Name.Trim().ToUpper();
            user.Email = request.Email;
            user.NormalizedEmail = request.Email.Trim().ToUpper();
            user.UserName = request.Username;
            user.NormalizedUserName = request.Username.Trim().ToUpper();
            user.PhoneNumber = request.PhoneNumber;

            return user;
        }
    }
}