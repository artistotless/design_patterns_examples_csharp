namespace DesignOfPatterns.Behavioral;

/* Назначение: описывает операцию, выполняемую с каждым объектом из некоторой
 * иерархии классов. Паттерн «Посетитель» позволяет определить новую операцию,
 * не изменяя классов этих объектов. */

internal class Visitor : LaunchablePattern
{
    protected override Task Main()
    {
        ArmedSoldier armedSoldier = new ConcreateArmedSoldier();
        SmartSoldier smartSoldier = new ConcreateSmartSoldier();

        armedSoldier.Accept(new HackPentagonCommand());
        armedSoldier.Accept(new GreetCommand());
        smartSoldier.Accept(new HackPentagonCommand());
        armedSoldier.Accept(new ShootTargetCommand(new { enemy = "Imran Zahaev" }));

        return Task.CompletedTask;
    }

    interface ISoldier
    {
        void Accept(BaseSoldiersCommand visitor);
    }

    // Классы солдат. Предполагается, что эти классы - неизменяемое кол-во
    abstract record StrongSoldier() : ISoldier
    {
        public void Accept(BaseSoldiersCommand visitor)
            => visitor.Visit(this);
    }

    abstract record SmartSoldier() : ISoldier
    {
        public void Accept(BaseSoldiersCommand visitor)
            => visitor.Visit(this);
    }

    abstract record ArmedSoldier() : ISoldier
    {
        public void Accept(BaseSoldiersCommand visitor)
            => visitor.Visit(this);
    }

    sealed record ConcreateArmedSoldier() : ArmedSoldier;
    sealed record ConcreateSmartSoldier() : SmartSoldier;


    // Посетитель представляет собой некое действие, совершаемое над иеархией солдат
    abstract class BaseSoldiersCommand
    {
        public virtual void Visit(ISoldier soldier) { }
        public virtual void Visit(StrongSoldier soldier)
            => Visit((ISoldier)soldier);
        public virtual void Visit(SmartSoldier soldier)
            => Visit((ISoldier)soldier);
        public virtual void Visit(ArmedSoldier soldier)
            => Visit((ISoldier)soldier);
    }

    sealed class ShootTargetCommand : BaseSoldiersCommand
    {
        private readonly object _target; // для примера

        public ShootTargetCommand(object target)
           => _target = target;

        public override void Visit(ArmedSoldier soldier)
            => Console.WriteLine("armed soldier shot a target");
    }

    sealed class HackPentagonCommand : BaseSoldiersCommand
    {
        public override void Visit(SmartSoldier soldier)
            => Console.WriteLine("smart soldier hacked a pentagon");

        public override void Visit(ArmedSoldier soldier)
            => Console.WriteLine("Warning! armed soldier cannot hack a pentagon");
    }

    sealed class GreetCommand : BaseSoldiersCommand
    {
        public override void Visit(ISoldier soldier)
           => Console.WriteLine("Greetings!");
    }
}
