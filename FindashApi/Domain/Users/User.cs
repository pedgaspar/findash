using System.ComponentModel.DataAnnotations;
using Findash.Domain.Accounts;

namespace Findash.Users;

public class User : BaseEntity
{
    public required string FirstName { get; set; }
    public required string LastName { get; set; }
    public required string UserName { get; set; }
    public required string Email { get; set; }
    public required DateOnly BirthDate { get; set; }
    public Gender Gender { get; set; }
    public List<UserAccount> Accounts { get; set; } = [];
}

public enum Gender
{
    Male,
    Female,
    Other
}