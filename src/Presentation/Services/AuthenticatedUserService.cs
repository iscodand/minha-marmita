using Application.DTOs.Authentication;
using Domain.Entities;
using Application.Contracts.Repositories;
using Application.Contracts.Services;

namespace Presentation.Services
{
    public class AuthenticatedUserService : IAuthenticatedUserService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IUserRepository _userRepository;

        public AuthenticatedUserService(IHttpContextAccessor httpContextAccessor, IUserRepository userRepository)
        {
            _httpContextAccessor = httpContextAccessor;
            _userRepository = userRepository;
        }

        public async Task<GetAuthenticatedUserDto> GetAuthenticatedUserAsync()
        {
            string username = _httpContextAccessor.HttpContext.User.Identity.Name;
            User authenticatedUser = await _userRepository.GetUserByUsernameAsync(username);

            if (authenticatedUser is null)
            {
                return null;
            }

            GetAuthenticatedUserDto getUserDto = new()
            {
                Id = authenticatedUser.Id,
                Name = authenticatedUser.Name,
                Email = authenticatedUser.Email,
                Username = authenticatedUser.UserName,
                PhoneNumber = authenticatedUser.PhoneNumber,
                Role = authenticatedUser.UserRoles.Select(x => x.Role.Name).FirstOrDefault(),
                CompanyName = authenticatedUser.UserCompany.Name,
                CompanyId = authenticatedUser.CompanyId
            };

            return getUserDto;
        }
    }
}