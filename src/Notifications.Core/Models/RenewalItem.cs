using System;

namespace Notifications.Core.Models
{
    /// <summary>
    /// Renewal item data model
    /// </summary>
    public struct RenewalItem : IRenewalItem
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string FirstName { get; set; }
        public string Surname { get; set; }
        public string ProductName { get; set; }
        public decimal PayoutAmount { get; set; }
        public decimal AnnualPremium { get; set; }

        /// <summary>
        /// Instantiates a renewal item
        /// </summary>
        /// <param name="id">Id</param>
        /// <param name="title">Title</param>
        /// <param name="firstName">First name</param>
        /// <param name="lastName">Last name</param>
        /// <param name="productName">Product name</param>
        /// <param name="payoutAmount">Payout amount</param>
        /// <param name="annualPremium">Annual premium</param>
        public RenewalItem(int id, string title, string firstName, string lastName, string productName, decimal payoutAmount, decimal annualPremium)
        {
            if (id < 0)
                throw new ArgumentOutOfRangeException(nameof(id));

            if (String.IsNullOrEmpty(title))
                throw new ArgumentNullException(nameof(title));

            if (String.IsNullOrEmpty(firstName))
                throw new ArgumentNullException(nameof(firstName));

            if (String.IsNullOrEmpty(lastName))
                throw new ArgumentNullException(nameof(lastName));

            if (String.IsNullOrEmpty(productName))
                throw new ArgumentNullException(nameof(productName));

            if (payoutAmount <= 0m)
                throw new ArgumentOutOfRangeException(nameof(payoutAmount));

            if (annualPremium <= 0m)
                throw new ArgumentOutOfRangeException(nameof(annualPremium));

            Id = id;
            Title = title;
            FirstName = firstName;
            Surname = lastName;
            ProductName = productName;
            PayoutAmount = payoutAmount;
            AnnualPremium = annualPremium;

        }
    }
}
