using System;
using System.Collections.Generic;
using System.Text;

namespace AdapterPattern2
{
    // ADAPTEE - Third-party PayPal SDK
    public class PaypalSDK
    {
        public string SendPayment(double amount, string email)
        {
            Console.WriteLine($"PayPal: Sending ${amount} to {email}");
            return $"paypal_tx_{Guid.NewGuid().ToString().Substring(0, 8)}";
        }

        public bool RefundOrder(string orderId)
        {
            Console.WriteLine($"PayPal: Refunding order {orderId}");
            return true;
        }
    }
}
