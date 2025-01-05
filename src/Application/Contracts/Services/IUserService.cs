using Application.Dtos.User;
using Application.Parameters;
using Application.Wrappers;

namespace Application.Contracts.Services
{
    public interface IUserService
    {
        public Task<Response<IEnumerable<GetUserDto>>> GetUsersByCompanyAsync(int companyId);
        public Task<PagedResponse<IEnumerable<GetUserDto>>> GetUsersByCompanyPagedAsync(int companyId, RequestParameter parameter);
        public Task<Response<GetUserDto>> GetUserByUsernameAsync(string username);
        public Task<Response<DetailUserDto>> DetailUserAsync(string username, int companyId);
        public Task<Response<IEnumerable<GetRolesDto>>> GetRolesAsync();
        public Task<Response<UpdateUserDto>> UpdateUserAsync(UpdateUserDto updateUserDto);
        public Task<Response<GetUserDto>> ActivateUserAsync(string username, int companyId);
        public Task<Response<GetUserDto>> DeactivateUserAsync(string username, int companyId);
    }
}