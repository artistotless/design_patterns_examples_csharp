using DesignOfPatterns;
using DesignOfPatterns.Creational;
using DesignOfPatterns.Behavioral;

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
    new Visitor()
};

foreach (var pattern in patterns)
    pattern.Start();

Console.ReadLine();
