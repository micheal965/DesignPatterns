using System;
using System.Collections.Generic;
using System.Text;

namespace AdapterPattern2
{
    // ADAPTEE - Third-party Stripe API
    public class StripeAPI
    {
        public string Charge(int amountInCents, string token)
        {
            Console.WriteLine($"Stripe: Charging ${amountInCents / 100.0} using token {token}");
            return $"stripe_tx_{Guid.NewGuid().ToString().Substring(0, 8)}";
        }

        public bool CreateRefund(string chargeId)
        {
            Console.WriteLine($"Stripe: Refunding charge {chargeId}");
            return true;
        }
    }
}
