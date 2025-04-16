namespace Findash.Domain.Accounts;

public class UserAccount : BaseEntity
{
    public int EmployeeId { get; set; }
    public Employee Employee { get; set; } = null!;
    public int BenefitId { get; set; }
    public Benefit Benefit { get; set; } = null!;
    public decimal? CostToEmployee { get; set; }
}