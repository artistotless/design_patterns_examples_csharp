namespace DesignOfPatterns.Behavioral;

// Cтратегия инкапсулирует определенное поведение с возможностью его подмены.

internal class Strategy : LaunchablePattern
{
    protected override Task Main()
    {
        Console.Write("Select sending strategy (Email or Sms): ");
        var userInput = Console.ReadLine() ?? "Email";

        ISendStrategy strategy = userInput switch
        {
            "Email" => new EmailSender(),
            "Sms" => new SmsSender(),
            _ => new EmailSender(),
        };

        Console.Write("Write some message: ");
        var message = Console.ReadLine() ?? "testMessage";

        new Publisher(strategy).Send(message);

        return Task.CompletedTask;
    } 

    class Publisher
    {
        private readonly ISendStrategy _strategy;

        public Publisher(ISendStrategy strategy)
        {
            _strategy = strategy;
        }

        public void Send(string message)
            => _strategy.Send(message);

        public void Send<TSender>(string message) where TSender : ISendStrategy, new()
            => new TSender().Send(message);

        public void Send(string message, Action<string> strategy)
            => strategy(message);
    }

    // interfaces
    interface ISendStrategy
    {
        void Send(string message);
    }

    // implementations
    class EmailSender : ISendStrategy
    {
        public void Send(string message)
            => Console.WriteLine($"Sending a mesage '{message}' using email");
    }

    class SmsSender : ISendStrategy
    {
        public void Send(string message)
            => Console.WriteLine($"Sending a mesage '{message}' using sms");
    }
}