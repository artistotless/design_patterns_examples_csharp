namespace DesignOfPatterns;

internal abstract class LaunchablePattern
{
    public virtual void Start()
    {
        var exampleName = this.GetType().Name;

        Console.WriteLine($"------ {exampleName} ------ begin");

        Main().GetAwaiter().GetResult();

        Stop();
    }

    protected virtual void Stop()
    {
        var exampleName = this.GetType().Name;

        Console.WriteLine($"------ {exampleName} ------ end \n");
    }

    protected abstract Task Main();
}
