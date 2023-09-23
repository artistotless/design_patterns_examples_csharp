using DesignOfPatterns;
using DesignOfPatterns.Behavioral;
using DesignOfPatterns.Creational;
using DesignOfPatterns.Structural;

List<LaunchablePattern> patterns = new()
{
    new AbstractFactory(),
    new Builder(),
    new FabricMethod(),

    new Strategy(),
    new TemplateMethod(),
    new Mediator(),
    new Observer(),
    new Iterator(),
    new Visitor(),
    new Command(),
    new StatePattern(),
    new Memento(),
    new CoR(),

    new Adapter(),
};

foreach (var pattern in patterns)
    pattern.Start();

Console.ReadLine();
