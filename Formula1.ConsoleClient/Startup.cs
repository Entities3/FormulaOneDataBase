namespace Formula1.ConsoleClient
{
    using Core;

    public class Startup
    {
        static void Main(string[] args)
        {
            Engine engine = new Engine(null, null, null, null);
            engine.Start();
        }
    }
}

