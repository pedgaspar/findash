namespace Findash.Users.DTOs;

public class GetUsersResponse
{
    public required Guid Id { get; set; }
    public required string FirstName { get; set; }
    public required string LastName { get; set; }
    public required string UserName { get; set; }
    public required string Email { get; set; }
    public required DateOnly BirthDate { get; set; }
    public Gender Gender { get; set; }
}