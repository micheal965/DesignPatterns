namespace AdapterPattern2
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("╔════════════════════════════════════════════════════════╗");
            Console.WriteLine("║         ADAPTER PATTERN: PAYMENT PROCESSING            ║");
            Console.WriteLine("╚════════════════════════════════════════════════════════╝");
            Console.WriteLine();

            // Client code works with IPaymentProcessor interface
            ProcessOrder(new StripeAdapter(), 99.99m, "4532-1234-5678-9010");
            Console.WriteLine();

            ProcessOrder(new PaypalAdapter(), 149.99m, "5105-1051-0510-5100");
            Console.WriteLine();

            Console.WriteLine("✅ Different payment gateways adapted to same interface!");
            Console.WriteLine("Press any key to exit...");
            Console.ReadKey();
        }
        static void ProcessOrder(IPaymentProcessor paymentProcessor, decimal amount, string card)
        {
            Console.WriteLine($"Processing payment of ${amount}...");

            bool success = paymentProcessor.ProcessPayment(amount, card);

            if (success)
                Console.WriteLine("✅ Payment successful!");
            else
                Console.WriteLine("❌ Payment failed!");
        }
    }
}
