using Findash.Users;

namespace Findash.Domain.Accounts;

public class UserAccount
{
    public Guid UserId { get; set; }
    public User User { get; set; } = null!;
    public Guid AccountId { get; set; }
    public Account Account { get; set; } = null!;
}