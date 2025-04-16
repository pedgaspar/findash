using System.ComponentModel.DataAnnotations;

namespace Findash.Users;

public class User : BaseEntity
{
    public required string FirstName { get; set; }
    public required string LastName { get; set; }
    public required string UserName { get; set; }
    public required string Email { get; set; }
    public required DateOnly BirthDate { get; set; }
    public Gender Gender { get; set; }
}

public enum Gender
{
    Male,
    Female,
    Other
}