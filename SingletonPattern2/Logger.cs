namespace SingletonPattern2
{
    public class Logger
    {
        private static Logger _instance = new Logger();
        public static Logger Instance
        {
            get
            {
                return _instance;
            }
        }
        public void Log(string message) => Console.WriteLine("Log " + message);
    }
}
