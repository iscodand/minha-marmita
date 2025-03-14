using Application.Contracts.Services;
using Application.DTOs.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Text.Json;

namespace Presentation.Attributes
{

    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class)]
    public class LogActionAttribute : Attribute, IAsyncActionFilter
    {
        private readonly ILoggerService _logger;
        private readonly IAuthenticatedUserService _authenticatedUserService;

        public LogActionAttribute(ILoggerService logger, IAuthenticatedUserService authenticatedUserService)
        {
            _logger = logger;
            _authenticatedUserService = authenticatedUserService;
        }

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var request = context.ActionArguments;
            var executedContext = await next();
            object response = null;
            GetAuthenticatedUserDto user = await _authenticatedUserService.GetAuthenticatedUserAsync();

            if (executedContext.Result is ObjectResult objectResult)
            {
                response = objectResult.Value;
            }

            _logger.LogInfo(new()
            {
                Action = context.ActionDescriptor.DisplayName,
                Message = "Executando ação do controlador.",
                Request = JsonSerializer.Serialize(request),
                Response = JsonSerializer.Serialize(response),
                UserId = user.Id
            }
            );
        }
    }
}