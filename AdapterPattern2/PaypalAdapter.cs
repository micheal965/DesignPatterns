using System;
using System.Collections.Generic;
using System.Text;

namespace AdapterPattern2
{
    public class PaypalAdapter : IPaymentProcessor
    {
        private readonly PaypalSDK _paypalSDK;
        private string _lastTransactionId;

        public PaypalAdapter()
        {
            _paypalSDK = new PaypalSDK();
        }

        public bool ProcessPayment(decimal amount, string cardNumber)
        {
            // PayPal needs email, simulate extracting from card
            string email = $"customer_{cardNumber.Substring(cardNumber.Length - 4)}@email.com";

            _lastTransactionId = _paypalSDK.SendPayment((double)amount, email);

            return !string.IsNullOrEmpty(_lastTransactionId);
        }

        public bool RefundPayment(string transactionId)
        {
            return _paypalSDK.RefundOrder(transactionId);
        }
    }
}
