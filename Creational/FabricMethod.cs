namespace DesignOfPatterns.Creational;

/* Когда при обсуждении дизайна упоминается фабрика, то в подавляющем большинстве случаев
 * имеется в виду одна из разновидностей паттерна «Фабричный метод».

 * Назначение: определяет интерфейс для создания объекта, но оставляет подклассам
 * решение о том, какой класс инстанцировать.Фабричный метод позволяет классу
 * делегировать инстанцирование подклассам. */

internal class FabricMethod : LaunchablePattern
{
    protected override Task Main()
    {
        Console.Write("Select factory (Default or Boss): ");
        var userInput = Console.ReadLine() ?? "Default";

        IEnemyFactory factory = userInput switch
        {
            "Default" => new DefaultEnemyFactory(),
            "Boss" => new BossEnemyFactory(),
            _ => new DefaultEnemyFactory(),
        };

        var enemy = factory.CreateEnemy();
        enemy.Atack(new("pig"));

        return Task.CompletedTask;
    }

    // intefaces
    interface IEnemyFactory
    {
         IEnemy CreateEnemy();
    }

    interface IEnemy
    {
        int MaxHp { get; }
        int CurrentHp { get; }
        int Damage { get; }

        void SetHp(int hp);
        void Atack(Target target);
    }

    sealed record Target(string name);

    // implementations
    sealed class DefaultEnemyFactory : IEnemyFactory
    {
        public IEnemy CreateEnemy()
            => new DefaultEnemy();
    }

    sealed class BossEnemyFactory : IEnemyFactory
    {
        public IEnemy CreateEnemy()
            => new BossEnemy();
    }

    class DefaultEnemy : IEnemy
    {
        public int MaxHp => 100;

        public int CurrentHp { get; private set; }

        public int Damage => 10;

        public void Atack(Target target)
            => Console.WriteLine($"Default enemy atacking {target.name}");

        public void SetHp(int hp) => CurrentHp = hp;
    }

    class BossEnemy : IEnemy
    {
        public int MaxHp => 150;

        public int CurrentHp { get; private set; }

        public int Damage => 30;

        public void Atack(Target target)
            => Console.WriteLine($"Boss enemy is attacking {target.name} so aggressively!");

        public void SetHp(int hp)
                => CurrentHp = hp;
    }
}
