namespace DesignOfPatterns.Creational;

/* Назначение: абстрактная фабрика предоставляет интерфейс для создания семейства 
 * взаимосвязанных или родственных объектов (dependent or related objects), не
 * специфицируя их конкретных классов.

 * Другими словами: абстрактная фабрика представляет собой стратегию создания
 * семейства взаимосвязанных или родственных объектов. */

internal sealed class AbstractFactory : LaunchablePattern
{
    protected override Task Main()
    {
        ICarFactory factory;

        void createParts(string factoryName)
        {
            factory.CreateEngine();
            factory.CreateWheels();
            factory.CreateBody();

            Console.WriteLine($"{factoryName} created: engine, wheels, body");
        }

        factory = new MersedesCarFactory();

        Console.WriteLine($"Factory: {factory}");
        createParts(factory.ToString()!);

        factory = new BmwCarFactory();

        Console.WriteLine($"Factory: {factory}");
        createParts(factory.ToString()!);

        return Task.CompletedTask;
    }

    // interfaces
    interface ICarFactory
    {
        ICarBody CreateBody();
        ICarEngine CreateEngine();
        ICarWheels CreateWheels();
    }

    interface ICarBody { }
    interface ICarEngine { }
    interface ICarWheels { }

    // implementations
    class MersedesCarFactory : ICarFactory
    {
        public ICarBody CreateBody()
            => new MersedesBody();

        public ICarEngine CreateEngine()
            => new MersedesEngine();

        public ICarWheels CreateWheels()
            => new MersedesWheels();

        public override string ToString()
            => nameof(MersedesCarFactory);
    }

    record MersedesBody : ICarBody;
    record MersedesEngine : ICarEngine;
    record MersedesWheels : ICarWheels;

    class BmwCarFactory : ICarFactory
    {
        public ICarBody CreateBody()
            => new BmwBody();

        public ICarEngine CreateEngine()
            => new BmwEngine();

        public ICarWheels CreateWheels()
            => new BmwWheels();

        public override string ToString()
          => nameof(BmwCarFactory);
    }

    record BmwBody : ICarBody;
    record BmwEngine : ICarEngine;
    record BmwWheels : ICarWheels;
}
