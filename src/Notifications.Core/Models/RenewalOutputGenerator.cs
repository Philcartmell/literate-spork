using System;
using System.Threading.Tasks;

namespace Notifications.Core.Models
{
    /// <summary>
    /// Interpolates a RenewalItem with provided template
    /// </summary>
    public class RenewalOutputGenerator : IRenewalOutputGenerator
    {
        public readonly string DateFormat;
        public readonly string Template;

        public RenewalOutputGenerator(string dateFormat, string template)
        {
            if (String.IsNullOrEmpty(dateFormat))
                throw new ArgumentNullException(nameof(dateFormat));

            if (String.IsNullOrEmpty(template))
                throw new ArgumentNullException(nameof(template));

            DateFormat = dateFormat;
            Template = template;
        }

        public Task<string> GenerateAsync(DateTime letterTimestamp, IRenewalItem renewalItem)
        {
            if (renewalItem == null)
                throw new ArgumentNullException(nameof(renewalItem));

            // use a copy of template, retaining original value
            // allowing method to be ran in parellel if necessary.
            var t = Template;

            t = t.Replace("((letter_date))", letterTimestamp.ToString(DateFormat));
            t = t.Replace("((title))", renewalItem.Title);
            t = t.Replace("((first_name))", renewalItem.FirstName);
            t = t.Replace("((surname))", renewalItem.Surname);
            t = t.Replace("((product_name))", renewalItem.ProductName);
            t = t.Replace("((payout_amount))", Math.Round(renewalItem.PayoutAmount, 2).ToString("f2"));
            t = t.Replace("((annual_premium))", Math.Round(renewalItem.AnnualPremium, 2).ToString("f2"));
           
            if (renewalItem is RenewalItemWithMonthlyBreakdown)
            {
                var monthlyBreakdown = renewalItem as RenewalItemWithMonthlyBreakdown;
                t = t.Replace("((credit_charge))", Math.Round(monthlyBreakdown.CreditCharge, 2).ToString("f2"));
                t = t.Replace("((first_payment))", Math.Round(monthlyBreakdown.FirstMonthlyPaymentAmount, 2).ToString("f2"));
                t = t.Replace("((subsequent_payment))", Math.Round(monthlyBreakdown.SubsequentMonthlyPaymentAmount, 2).ToString("f2"));
                t = t.Replace("((total_premium_dd))", Math.Round(monthlyBreakdown.TotalPremiumDirectDebit, 2).ToString("f2"));
            }

            return Task.FromResult(t);

        }
    }
}
