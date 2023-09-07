namespace DesignOfPatterns.Behavioral;

/* Назначение: инкапсулирует запрос как объект, 
 * позволяя тем самым задавать параметры клиентов для обработки соответствующих запросов, 
 * ставить запросы в очередь или протоколировать их, а также поддерживать отмену операций. 
 * Паттерн «Команда» позволяет спрятать действие в объекте и отвязать источник
 * этого действия от места его исполнения*/

internal class Command : LaunchablePattern
{
    protected override async Task Main()
    {
        var boardContext = new BoardContext();
        var invoker = new CommandInvoker<BoardContext>(boardContext);

        invoker.RunNext(new MoveCommand(new(1, 1), new(2, 2)));

        await Task.Delay(1000);

        invoker.Undo(); 
    }

    interface ICommand<TContext>
    {
        void Execute(TContext context);
        void Undo();
    }

    record struct Point(int x, int y);
    class Figure
    {
        public Point Position { get; set; }

        public Figure(Point position)
        {
            Position = position;
        }
    }

    sealed class CommandInvoker<TContext>
    {
        private readonly Stack<ICommand<TContext>> _commands = new();
        private readonly TContext _context;

        public CommandInvoker(TContext context)
         => _context = context;

        public void RunNext(ICommand<TContext> command)
        {
            _commands.Push(command);
            command.Execute(_context);
        }

        public void Undo()
            => _commands.Pop().Undo();
    }

    class BoardContext
    {
        public Figure[] Figures { get; set; }

        public BoardContext()
        {
            Figures = new Figure[]
            {
                new Figure(new Point(1,1)),
                new Figure(new Point(1,3)),
                new Figure(new Point(1,5)),
                new Figure(new Point(1,7)),
            };
        }
    }

    class MoveCommand : ICommand<BoardContext>
    {
        private Point _startPose;
        private Point _endPose;

        private BoardContext _ctx;

        public MoveCommand(Point startPose, Point endPose)
        {
            _startPose = startPose;
            _endPose = endPose;
        }

        public void Execute(BoardContext context)
        {
            Console.WriteLine($"{this.GetType().Name} get executed");
            Console.WriteLine($"Figure moved from {_startPose} to {_endPose}");

            // some operation with BoardContext...
            context.Figures[0].Position = _endPose;

            // caching context for Undo action..
            _ctx = context;
        }

        public void Undo()
        {
            Console.WriteLine($"{this.GetType().Name} get canceled");
            Console.WriteLine($"Figure moved from {_endPose} to {_startPose}");

            // some operation with BoardContext...
            _ctx.Figures[0].Position = _startPose;
        }
    }
}
