using FluentValidation;

namespace Findash.Common.DTOs;

public class GetAllRequest
{
    public int? Page { get; set; }
    public int? RecordsPerPage { get; set; }
    public string? Contains { get; set; }
}

public class GetAllRequestValidator : AbstractValidator<GetAllRequest>
{
    public GetAllRequestValidator()
    {
        RuleFor(r => r.Page).GreaterThanOrEqualTo(1).WithMessage("Page number must be set to a positive non-zero integer.");
        RuleFor(r => r.RecordsPerPage)
            .GreaterThanOrEqualTo(1).WithMessage("You must return at least one record.")
            .LessThanOrEqualTo(100).WithMessage("You cannot return more than 100 records.");
    }
}

// public class UpdateEmployeeRequestValidator : AbstractValidator<UpdateEmployeeRequest>
// {
//     private readonly HttpContext _httpContext;
//     private readonly AppDbContext _appDbContext;
//
//     public UpdateEmployeeRequestValidator(IHttpContextAccessor httpContextAccessor, AppDbContext appDbContext)
//     {
//         this._httpContext = httpContextAccessor.HttpContext!;
//         this._appDbContext = appDbContext;
//
//         RuleFor(x => x.Address1).MustAsync(NotBeEmptyIfItIsSetOnEmployeeAlreadyAsync).WithMessage("Address1 must not be empty.");
//     }
//
//     private async Task<bool> NotBeEmptyIfItIsSetOnEmployeeAlreadyAsync(string? address, CancellationToken token)
//     {
//         var id = Convert.ToInt32(_httpContext.Request.RouteValues["id"]);
//         Employee employee = null!;//await _appDbContext.Employees.FindAsync(id);
//         await Task.CompletedTask;
//         if (employee!.Address1 != null && string.IsNullOrWhiteSpace(address))
//         {
//             return false;
//         }
//
//         return true;
//     }
// }