namespace SingletonPattern
{
    internal class Program
    {
        static void Main(string[] args)
        {
            while (true)
            {
                Console.WriteLine("Enter base currency:");
                string baseCurrency = Console.ReadLine();

                Console.WriteLine("Enter target currency:");
                string targetCurrency = Console.ReadLine();

                Console.WriteLine("Enter amount:");
                decimal amount = decimal.Parse(Console.ReadLine());


                decimal convertedAmount = CurrencyConverter.Instance.Convert(baseCurrency, targetCurrency, amount);

                Console.WriteLine($"{amount} {baseCurrency} = {convertedAmount} {targetCurrency}");
                Console.WriteLine("-----------------------------------");
            }
        }
    }
}
