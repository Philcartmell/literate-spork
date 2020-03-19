namespace Notifications.Core.Models
{
    public interface IRenewalItem
    {
        int Id { get; }
        string Title { get; }
        string FirstName { get; }
        string Surname { get; }
        string ProductName { get; }
        decimal PayoutAmount { get; }
        decimal AnnualPremium { get; }
    }
}