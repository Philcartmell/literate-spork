using Notifications.Core.Extensions;
using System;

namespace Notifications.Core.Models
{
    /// <summary>
    /// Renewal item decorarted with calculations for monthly breakdown
    /// </summary>
    public class RenewalItemWithMonthlyBreakdown : RenewalItemDecorator
    {
        private readonly decimal _creditChargeInterestRate;

        public decimal CreditCharge { get; }
        public decimal TotalPremiumDirectDebit { get; }
        public decimal AverageMonthlyPremium { get; }
        public decimal SubsequentMonthlyPaymentAmount { get; }
        public decimal FirstMonthlyPaymentAmount { get; }

        public RenewalItemWithMonthlyBreakdown(IRenewalItem renewalItem, decimal creditChargeInterestRate)
            : base(renewalItem)
        {

            if (creditChargeInterestRate < 0)
                throw new ArgumentOutOfRangeException(nameof(creditChargeInterestRate));

            _creditChargeInterestRate = creditChargeInterestRate;

            CreditCharge = RenewalItem.AnnualPremium * _creditChargeInterestRate;
            TotalPremiumDirectDebit = RenewalItem.AnnualPremium + CreditCharge;
            AverageMonthlyPremium = TotalPremiumDirectDebit / 12;

            if (AverageMonthlyPremium.CountDecimalPlaces() > 2)
            {
                var roundedDown = Math.Floor(AverageMonthlyPremium * 100) / 100;
                var fraction = (AverageMonthlyPremium - roundedDown).GetFraction();

                SubsequentMonthlyPaymentAmount = AverageMonthlyPremium - fraction;
                FirstMonthlyPaymentAmount = roundedDown + fraction * 12;
            }
            else
            {
                SubsequentMonthlyPaymentAmount = AverageMonthlyPremium;
                FirstMonthlyPaymentAmount = AverageMonthlyPremium;
            }
        }
    }
}
