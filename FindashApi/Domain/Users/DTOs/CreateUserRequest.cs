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
    private readonly HttpContext _httpContext;
    private readonly AppDbContext _appDbContext;
    public CreateUserRequestValidator(IHttpContextAccessor httpContextAccessor, AppDbContext appDbContext)
    {
        _httpContext = httpContextAccessor.HttpContext!;
        _appDbContext = appDbContext;

        RuleFor(x => x.FirstName).NotEmpty();
        RuleFor(x => x.LastName).NotEmpty();
        RuleFor(x => x.UserName).NotEmpty().Must(UniqueUserName).WithMessage("Username is already taken.");
        RuleFor(x => x.Email).NotEmpty().Must(UniqueEmail).WithMessage("Email is already taken.");
        RuleFor(x => x.BirthDate).NotEmpty().Must(BeEighteen).WithMessage("Age must be at least 18.");
        RuleFor(x => x.Gender).NotEmpty();
    }

    private bool UniqueUserName(string? username)
    {
        //var id = Convert.ToInt32(_httpContext.Request.RouteValues["id"]);
        var registeredUser = _appDbContext.Users.Any(w => w.UserName == username);
        return !registeredUser;
    } 
    
    private bool UniqueEmail(string? email)
    {
        var registeredUser = _appDbContext.Users.Any(w => w.Email == email);
        return !registeredUser;
    }

    private bool BeEighteen(DateOnly? birthDate)
    {
        var today = DateOnly.FromDateTime(DateTime.Today);
        var cutoff = today.AddYears(-18);
        return birthDate <= cutoff;
    }
}
