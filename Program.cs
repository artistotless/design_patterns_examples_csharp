using DesignOfPatterns;
using DesignOfPatterns.Creational;
using DesignOfPatterns.Behavioral;
using System.Collections;
using System.Runtime.CompilerServices;

List<LaunchablePattern> patterns = new()
{
    //new AbstractFactory(),
    //new Builder(),
    //new FabricMethod(),

    //new Strategy(),
    //new TemplateMethod(),
    //new Mediator(),
    //new Observer(),
    //new Iterator(),
    //new Visitor(),
    //new Command(),
    //new StatePattern(),
    //new Memento(),
    new CoR()
};

foreach (var pattern in patterns)
    pattern.Start();

Console.ReadLine();
