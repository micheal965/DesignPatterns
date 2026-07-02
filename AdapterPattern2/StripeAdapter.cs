using System;
using System.Collections.Generic;
using System.Text;

namespace AdapterPattern2
{
    public class StripeAdapter : IPaymentProcessor
    {
        private readonly StripeAPI _stripeAPI;
        private string _lastTransactionId;
        public StripeAdapter()
        {
            _stripeAPI = new StripeAPI();
        }

        public bool ProcessPayment(decimal amount, string cardNumber)
        {
            // Convert dollars to cents
            int amountInCents = (int)(amount * 100);

            // Stripe uses tokens instead of card numbers
            string token = $"tok_{cardNumber.Substring(cardNumber.Length - 4)}";

            _lastTransactionId = _stripeAPI.Charge(amountInCents, token);

            return !string.IsNullOrEmpty(_lastTransactionId);
        }

        public bool RefundPayment(string transactionId)
        {
            return _stripeAPI.CreateRefund(transactionId);
        }
    }
}
