namespace SingletonPattern
{
    public class CurrencyConverter
    {
        private static object _lock = new object();
        private static CurrencyConverter? _instance;
        private IEnumerable<ExchangeRate> _exchangeRates;
        private CurrencyConverter()
        {
            LoadExchangeRates();
        }
        public static CurrencyConverter Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (_lock)
                    {
                        if (_instance == null)
                            _instance = new();
                    }
                }
                return _instance;
            }
        }
        private void LoadExchangeRates()
        {
            Thread.Sleep(3000);

            _exchangeRates = new[]
            {
                new ExchangeRate("USD","SAR",3.75m),
                new ExchangeRate("USD","EGP",30.60m),
                new ExchangeRate("SAR","EGP",8.16m),
            };
        }

        public decimal Convert(string baseCurrency, string targetCurrency, decimal amount)
        {
            var exchangeRate = _exchangeRates.FirstOrDefault(rate => rate.BaseCurrency == baseCurrency
                                                        && rate.TargetCurrency == targetCurrency);
            return amount * exchangeRate!.Rate;
        }
    }
}
