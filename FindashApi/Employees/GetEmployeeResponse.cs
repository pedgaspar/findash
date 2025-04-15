using System.ComponentModel.DataAnnotations;
using Findash.Abstractions;
using FluentValidation;

namespace Findash.Employees;

public class CreateEmployeeRequest
{
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string? SocialSecurityNumber { get; set; }

    public string? Address1 { get; set; }
    public string? Address2 { get; set; }
    public string? City { get; set; }
    public string? State { get; set; }
    public string? ZipCode { get; set; }
    public string? PhoneNumber { get; set; }
    public string? Email { get; set; }
}

public class CreateEmployeeRequestValidator : AbstractValidator<CreateEmployeeRequest>
{
    public CreateEmployeeRequestValidator()
    {
        RuleFor(x => x.FirstName).NotEmpty().WithMessage("First name is required");
        RuleFor(x => x.LastName).NotEmpty();
    }
}

public class GetAllEmployeesRequest
{
    public int? Page { get; set; }
    public int? RecordsPerPage { get; set; }
    
    public string? FirstNameContains { get; set; }
    public string? LastNameContains { get; set; }
}

public class GetAllEmployeesRequestValidator : AbstractValidator<GetAllEmployeesRequest>
{
    public GetAllEmployeesRequestValidator()
    {
        RuleFor(r => r.Page).GreaterThanOrEqualTo(1).WithMessage("Page number must be set to a positive non-zero integer.");
        RuleFor(r => r.RecordsPerPage)
            .GreaterThanOrEqualTo(1).WithMessage("You must return at least one record.")
            .LessThanOrEqualTo(100).WithMessage("You cannot return more than 100 records.");
    }
}

public class GetEmployeeResponse
{
    public required string FirstName { get; set; }
    public required string LastName { get; set; }

    public string? Address1 { get; set; }
    public string? Address2 { get; set; }
    public string? City { get; set; }
    public string? State { get; set; }
    public string? ZipCode { get; set; }
    public string? PhoneNumber { get; set; }
    public string? Email { get; set; }
    public required List<GetEmployeeResponseEmployeeBenefit> Benefits { get; set; }
}

public class GetEmployeeResponseEmployeeBenefit
{
    public int Id { get; set; }
    public int EmployeeId { get; set; }
    public BenefitType BenefitType { get; set; }
    public decimal Cost { get; set; }
}

public class UpdateEmployeeRequest
{
    public string? Address1 { get; set; }
    public string? Address2 { get; set; }
    public string? City { get; set; }
    public string? State { get; set; }
    public string? ZipCode { get; set; }
    public string? PhoneNumber { get; set; }
    public string? Email { get; set; }
}

public class UpdateEmployeeRequestValidator : AbstractValidator<UpdateEmployeeRequest>
{
    private readonly HttpContext _httpContext;
    private readonly IRepository<Employee> _repository;

    public UpdateEmployeeRequestValidator(IHttpContextAccessor httpContextAccessor, IRepository<Employee> repository)
    {
        this._httpContext = httpContextAccessor.HttpContext!;
        this._repository = repository;

        RuleFor(x => x.Address1).MustAsync(NotBeEmptyIfItIsSetOnEmployeeAlreadyAsync).WithMessage("Address1 must not be empty.");
    }

    private async Task<bool> NotBeEmptyIfItIsSetOnEmployeeAlreadyAsync(string? address, CancellationToken token)
    {
        await Task.CompletedTask;   //again, we'll not make this async for now!

        var id = Convert.ToInt32(_httpContext.Request.RouteValues["id"]);
        var employee = _repository.GetById(id);

        if (employee!.Address1 != null && string.IsNullOrWhiteSpace(address))
        {
            return false;
        }

        return true;
    }
}