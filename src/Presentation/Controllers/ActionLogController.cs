using Application.Contracts.Services;
using Application.Parameters;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Presentation.Controllers
{
    [Route("auditoria")]
    [Authorize(Roles = "SuperAdmin")]
    public class ApplicationLogController : Controller
    {
        private readonly ILoggerService _loggerService;

        public ApplicationLogController(ILoggerService loggerService)
        {
            _loggerService = loggerService;
        }

        [HttpGet]
        public async Task<IActionResult> Logs(int pageNumber = 1)
        {
            RequestParameter parameter = new()
            {
                PageNumber = pageNumber,
                PageSize = 10
            };

            var result = await _loggerService.GetLogsPagedAsync(parameter);

            return View(result);
        }
    }
}
