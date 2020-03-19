using System;

namespace Notifications.Core.Models
{
    /// <summary>
    /// Base RenewalItem decorator
    /// </summary>
    public abstract class RenewalItemDecorator : IRenewalItem
    {
        protected readonly IRenewalItem RenewalItem;
        public int Id { get => RenewalItem.Id; }
        public string Title { get => RenewalItem.Title; }
        public string FirstName { get => RenewalItem.FirstName; }
        public string Surname { get => RenewalItem.Surname; }
        public string ProductName { get => RenewalItem.ProductName; }
        public decimal PayoutAmount { get => RenewalItem.PayoutAmount; }
        public decimal AnnualPremium { get => RenewalItem.AnnualPremium; }

        public RenewalItemDecorator(IRenewalItem renewalItem)
        {
            if (renewalItem == null)
                throw new ArgumentNullException(nameof(renewalItem));

            RenewalItem = renewalItem;
        }
    }
}
