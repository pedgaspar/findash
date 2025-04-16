using FluentValidation;

namespace Findash.Users.DTOs;

public class CreateUserRequest
{
    public  string? FirstName { get; set; }
    public  string? LastName { get; set; }
    public  string? UserName { get; set; }
    public  string? Email { get; set; }
    public  DateOnly? BirthDate { get; set; }
    public Gender? Gender { get; set; }
}

public class CreateUserRequestValidator : AbstractValidator<CreateUserRequest>
{
    public CreateUserRequestValidator()
    {
        RuleFor(x => x.FirstName).NotEmpty();
        RuleFor(x => x.LastName).NotEmpty();
        RuleFor(x => x.UserName).NotEmpty();
        RuleFor(x => x.Email).NotEmpty();
        RuleFor(x => x.BirthDate).NotEmpty();
        RuleFor(x => x.Gender).NotEmpty();
    }
}