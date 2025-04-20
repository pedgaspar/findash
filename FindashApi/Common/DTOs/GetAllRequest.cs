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