using System;
using System.Collections.Generic;
using System.Text;

namespace AdapterPattern2
{
    public interface IPaymentProcessor
    {
        bool ProcessPayment(decimal amount, string cardNumber);
        bool RefundPayment(string transactionId);
    }
}
