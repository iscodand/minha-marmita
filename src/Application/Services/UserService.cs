using Application.Dtos.User;
using Application.Wrappers;
using Domain.Entities;
using Application.Contracts.Repositories;
using Microsoft.AspNetCore.Identity;
using Application.Contracts.Services;
using Application.Dtos.Order;
using Application.Parameters;

namespace Application.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IRoleRepository _roleRepository;
        private readonly IUserRoleRepository _userRoleRepository;
        private readonly UserManager<User> _userManager;

        public UserService(IUserRepository userRepository,
                           IRoleRepository roleRepository,
                           IUserRoleRepository userRoleRepository,
                           UserManager<User> userManager)
        {
            _userRepository = userRepository;
            _roleRepository = roleRepository;
            _userRoleRepository = userRoleRepository;
            _userManager = userManager;
        }

        public async Task<Response<IEnumerable<GetUserDto>>> GetUsersByCompanyAsync(int companyId)
        {
            ICollection<User> users = await _userRepository.GetUsersByCompanyAsync(companyId);
            var mappedUsers = GetUserDto.Map(users);

            return new()
            {
                Message = "Usuários encontrados com sucesso.",
                Succeeded = true,
                Data = mappedUsers
            };
        }

        public async Task<PagedResponse<IEnumerable<GetUserDto>>> GetUsersByCompanyPagedAsync(int companyId, RequestParameter parameter)
        {
            var users = await _userRepository.GetUsersByCompanyPagedAsync(companyId, parameter.PageNumber, parameter.PageSize);
            var mappedUsers = GetUserDto.Map(users.users);
            return new(mappedUsers, parameter.PageNumber, parameter.PageSize, users.count);
        }

        public async Task<Response<GetUserDto>> GetUserByUsernameAsync(string username)
        {
            User user = await _userRepository.GetUserByUsernameAsync(username);
            if (user is null)
            {
                return new()
                {
                    Message = "Usuário não encontrado.",
                    Succeeded = false
                };
            }

            IList<string> roles = await _userManager.GetRolesAsync(user);
            GetUserDto getUserDto = GetUserDto.Map(user, roles);

            return new()
            {
                Data = getUserDto,
                Message = "Usuário recuperado com sucesso.",
                Succeeded = true
            };
        }

        public async Task<Response<IEnumerable<GetRolesDto>>> GetRolesAsync()
        {
            IEnumerable<Role> roles = await _roleRepository.GetAllAsync();
            IEnumerable<GetRolesDto> mappedRoles = GetRolesDto.Map(roles);

            return new()
            {
                Message = "Cargos encontrados com sucesso",
                Succeeded = true,
                Data = mappedRoles
            };
        }

        public async Task<Response<DetailUserDto>> DetailUserAsync(string username, int companyId)
        {
            // TODO => implementar paginação
            User user = await _userRepository.GetWithOrdersAsync(username);
            if (user == null)
            {
                return new Response<DetailUserDto>()
                {
                    Message = "Usuário não encontrado.",
                    Succeeded = false
                };
            }

            if (user.CompanyId != companyId)
            {
                return new Response<DetailUserDto>()
                {
                    Message = "Esse usuário não faz parte da sua empresa.",
                    Succeeded = false
                };
            }

            IdentityUserRole<string> userRole = await _userRoleRepository.GetUserRoleAsync(user.Id);
            Role role = await _roleRepository.GetRoleByIdAsync(userRole.RoleId);

            DetailUserDto detailUserDto = DetailUserDto.Map(user, role.Description);

            return new Response<DetailUserDto>()
            {
                Message = "Usuário encontrado com sucesso.",
                Succeeded = true,
                Data = detailUserDto
            };
        }

        public async Task<Response<UpdateUserDto>> UpdateUserAsync(UpdateUserDto request)
        {
            User user = await _userRepository.GetUserByUsernameAsync(request.Username);
            if (user is null)
            {
                return new Response<UpdateUserDto>()
                {
                    Message = "Usuário não encontrado. Verifique e tente novamente.",
                    Succeeded = false
                };
            }

            if (user.CompanyId != request.CompanyId)
            {
                return new Response<UpdateUserDto>()
                {
                    Message = "Usuário não encontrado. Verifique e tente novamente.",
                    Succeeded = false
                };
            }

            bool usernameAlreadyInUse = await _userRepository.UserExistsByUsernameAsync(request.Username, user.Id);
            if (usernameAlreadyInUse)
            {
                return new Response<UpdateUserDto>()
                {
                    Message = "Esse nome de usuário já está sendo utilizado. Verifique e tente novamente.",
                    Succeeded = false
                };
            }

            bool emailAlreadyInUse = await _userRepository.UserExistsByEmailAsync(request.Email, user.Id);
            if (emailAlreadyInUse)
            {
                return new Response<UpdateUserDto>()
                {
                    Message = "Esse e-mail já está sendo utilizado. Verifique e tente novamente.",
                    Succeeded = false
                };
            }
;
            user = UpdateUserDto.Map(user, request);
            await _userRepository.UpdateAsync(user);

            return new()
            {
                Message = "Usuário atualizado com sucesso",
                Succeeded = true
            };
        }

        public async Task<Response<GetUserDto>> ActivateUserAsync(string username, int companyId)
        {
            User user = await _userRepository.GetUserByUsernameAsync(username);

            if (user == null)
            {
                return new Response<GetUserDto>()
                {
                    Message = "Usuário não encontrado.",
                    Succeeded = false
                };
            }

            if (user.CompanyId != companyId)
            {
                return new Response<GetUserDto>()
                {
                    Message = "Esse usuário não faz parte da sua empresa.",
                    Succeeded = false
                };
            }

            if (user.IsActive)
            {
                return new Response<GetUserDto>()
                {
                    Message = "O usuário já está ativo.",
                    Succeeded = false
                };
            }

            user.Activate();
            await _userRepository.UpdateAsync(user);

            return new Response<GetUserDto>()
            {
                Message = "Usuário foi ativo com sucesso.",
                Succeeded = true
            };
        }

        public async Task<Response<GetUserDto>> DeactivateUserAsync(string username, int companyId)
        {
            User user = await _userRepository.GetUserByUsernameAsync(username);
            if (user == null)
            {
                return new Response<GetUserDto>()
                {
                    Message = "Usuário não encontrado.",
                    Succeeded = false
                };
            }

            if (user.CompanyId != companyId)
            {
                return new Response<GetUserDto>()
                {
                    Message = "Esse usuário não faz parte da sua empresa.",
                    Succeeded = false
                };
            }

            if (!user.IsActive)
            {
                return new Response<GetUserDto>()
                {
                    Message = "O usuário já está inativo.",
                    Succeeded = false
                };
            }

            user.Deactivate();
            await _userRepository.UpdateAsync(user);

            return new Response<GetUserDto>()
            {
                Message = "Usuário foi desativado com sucesso.",
                Succeeded = true
            };
        }
    }
}