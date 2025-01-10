using Application.Dtos.Data;
using Application.DTOs.Company.Response;

namespace Presentation.ViewModels.Company
{
    public class CompanyViewModel
    {
        public DetailCompanyDTO Company { get; set; }
        public GetDataDto Data { get; set; }
    }
}