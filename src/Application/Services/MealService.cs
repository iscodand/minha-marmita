using Application.Contracts.Services;
using Application.Dtos.Meal;
using Application.Dtos.Order;
using Application.Parameters;
using Application.Wrappers;
using Domain.Entities;
using Infrastructure.Data.Repositories;

namespace Application.Services
{
    public class MealService : IMealService
    {
        private readonly IMealRepository _mealRepository;

        public MealService(IMealRepository mealRepository)
        {
            _mealRepository = mealRepository;
        }

        public async Task<Response<GetMealDto>> GetMealByIdAsync(int mealId, int userCompanyId)
        {
            Meal meal = await _mealRepository.DetailMealAsync(mealId);

            if (meal == null)
            {
                return new Response<GetMealDto>()
                {
                    Message = "Sabor não encontrado",
                    Succeeded = false
                };
            }

            if (meal.CompanyId != userCompanyId)
            {
                return new Response<GetMealDto>()
                {
                    Message = "Esse sabor não pertence a sua empresa",
                    Succeeded = false
                };
            }

            var mappedMeal = GetMealDto.Map(meal);

            return new Response<GetMealDto>()
            {
                Message = "Sabor encontrado com sucesso",
                Succeeded = true,
                Data = mappedMeal
            };
        }

        public async Task<Response<IEnumerable<GetMealDto>>> SearchByMealAsync(string name)
        {
            IEnumerable<Meal> meals = await _mealRepository.SearchByMealAsync(name);
            var mappedMeals = GetMealDto.Map(meals);

            return new Response<IEnumerable<GetMealDto>>(
                message: "Sabores recuperados com sucesso.",
                data: mappedMeals,
                status: 200
            );
        }

        public async Task<Response<DetailMealDto>> DetailMealAsync(int mealId, int userCompanyId)
        {
            Meal meal = await _mealRepository.DetailMealAsync(mealId);

            if (meal == null)
            {
                return new Response<DetailMealDto>()
                {
                    Message = "Sabor não encontrado",
                    Succeeded = false
                };
            }

            if (meal.CompanyId != userCompanyId)
            {
                return new Response<DetailMealDto>()
                {
                    Message = "Esse sabor não pertence a sua empresa",
                    Succeeded = false
                };
            }

            List<GetOrderDto> getOrderDtoCollection = new();
            foreach (Order order in meal.Orders)
            {
                GetOrderDto getOrderDto = new()
                {
                    Id = order.Id,
                    Description = order.Description,
                    Price = order.Price,
                    IsPaid = order.IsPaid,
                    PaidAt = order.PaidAt,
                    PaymentType = order.PaymentType.Description,
                    Meal = order.Meal.Description,
                    Customer = order.Customer.Name,
                    CreatedAt = order.CreatedAt
                };

                getOrderDtoCollection.Add(getOrderDto);
            }

            DetailMealDto detailMealDto = new()
            {
                Id = meal.Id,
                Description = meal.Description,
                Accompaniments = meal.Accompaniments,
                Orders = getOrderDtoCollection,
                CreatedBy = meal.User.Name
            };

            return new Response<DetailMealDto>()
            {
                Message = "Sabor encontrado com sucesso",
                Succeeded = true,
                Data = detailMealDto
            };
        }

        public async Task<Response<CreateMealDto>> CreateMealAsync(CreateMealDto createMealDto)
        {
            bool mealExists = await _mealRepository.MealExistsByDescriptionAsync(createMealDto.Description, createMealDto.CompanyId);
            if (mealExists)
            {
                return new Response<CreateMealDto>()
                {
                    Message = "Esse sabor já está cadastrado",
                    Succeeded = false
                };
            }

            Meal meal = Meal.Create(
                createMealDto.Description,
                createMealDto.Accompaniments,
                createMealDto.CompanyId,
                createMealDto.UserId
            );

            await _mealRepository.CreateAsync(meal);

            return new Response<CreateMealDto>()
            {
                Message = "Sabor salvo com sucesso.",
                Succeeded = true
            };
        }

        public async Task<Response<IEnumerable<GetMealDto>>> GetMealsByCompanyAsync(int companyId)
        {
            if (companyId <= 0)
            {
                return new()
                {
                    Message = "Empresa não encontrada. Verifique e tente novamente",
                    Succeeded = false
                };
            }

            IEnumerable<Meal> meals = await _mealRepository.GetMealsByCompanyAsync(companyId);
            IEnumerable<GetMealDto> getMealDtoCollection = GetMealDto.Map(meals);

            return new()
            {
                Message = "Sabores encontrados com sucesso",
                Succeeded = true,
                Data = getMealDtoCollection
            };
        }

        public async Task<Response<IEnumerable<GetMealDto>>> GetMealsByDateRangeAsync(int userCompanyId, DateTime initialDate, DateTime finalDate)
        {
            ICollection<Meal> meals = await _mealRepository.GetMealsByDateRangeAsync(userCompanyId, initialDate.Date, finalDate.Date);
            IEnumerable<GetMealDto> getMealDtoCollection = GetMealDto.Map(meals);

            return new()
            {
                Message = "Sabores encontrados com sucesso",
                Succeeded = true,
                Data = getMealDtoCollection
            };
        }

        public async Task<Response<GetMealDto>> UpdateMealAsync(UpdateMealDto updateMealDto)
        {
            if (updateMealDto == null)
            {
                return new Response<GetMealDto>()
                {
                    Message = "Sabor não pode ser nulo",
                    Succeeded = false
                };
            }

            Meal meal = await _mealRepository.GetByIdAsync(updateMealDto.Id);

            if (meal == null)
            {
                return new Response<GetMealDto>()
                {
                    Message = "Sabor não encontrado",
                    Succeeded = false
                };
            }

            bool mealExists = await _mealRepository.MealExistsByDescriptionAsync(updateMealDto.Description, updateMealDto.UserCompanyId);

            if (mealExists)
            {
                return new Response<GetMealDto>()
                {
                    Message = "Um sabor já foi cadastrado com essa Descrição. Verifique e tente novamente",
                    Succeeded = false
                };
            }

            if (meal.CompanyId != updateMealDto.UserCompanyId)
            {
                return new Response<GetMealDto>()
                {
                    Message = "Esse sabor não pertence a sua empresa",
                    Succeeded = false
                };
            }

            meal.Update(updateMealDto.Description, updateMealDto.Accompaniments);

            await _mealRepository.SaveAsync();

            return new Response<GetMealDto>()
            {
                Message = "Sabor atualizado com sucesso",
                Succeeded = true
            };
        }

        public async Task<Response<GetMealDto>> DeleteMealAsync(DeleteMealDto deleteMealDto)
        {
            if (deleteMealDto == null)
            {
                return new Response<GetMealDto>()
                {
                    Message = "Sabor não pode ser nulo",
                    Succeeded = false
                };
            }

            Meal meal = await _mealRepository.DetailMealAsync(deleteMealDto.Id);

            if (meal == null)
            {
                return new Response<GetMealDto>()
                {
                    Message = "Sabor não encontrado",
                    Succeeded = false
                };
            }

            if (meal.Orders.Count > 0)
            {
                return new Response<GetMealDto>()
                {
                    Message = $"Você não pode deletar esse sabor, pois ele está vinculado a {meal.Orders.Count} pedido(s)",
                    Succeeded = false
                };
            }

            if (meal.CompanyId != deleteMealDto.UserCompanyId)
            {
                return new Response<GetMealDto>()
                {
                    Message = "Esse sabor não pertence a sua empresa",
                    Succeeded = false
                };
            }

            await _mealRepository.DeleteAsync(meal);

            return new Response<GetMealDto>()
            {
                Message = "Sabor deletado com sucesso",
                Succeeded = true
            };
        }

        public async Task<PagedResponse<IEnumerable<GetMealDto>>> GetByCompanyPagedAsync(int companyId, RequestParameter parameter)
        {
            var meals = await _mealRepository.GetByCompanyPagedAsync(companyId, parameter.PageNumber, parameter.PageSize);
            IEnumerable<GetMealDto> mappedMeals = GetMealDto.Map(meals.meals);

            return new(
                data: mappedMeals,
                pageNumber: parameter.PageNumber,
                pageSize: parameter.PageSize,
                totalItems: meals.count
            );
        }
    }
}