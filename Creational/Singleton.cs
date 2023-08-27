namespace DesignOfPatterns.Creational;

/* Паттерн Singleton - помогает ограничить кол-во создаваемых экземпляров класса до одного.
 * Другими словами - гарантирует, что у класса только один экземпляр. */

internal class Singleton : LaunchablePattern
{
    protected override Task Main()
    {
        var instance = GlobalSettings.Instance;
        var instance2 = GlobalSettings.Instance;

        Console.WriteLine($"hash1 = {instance.GetHashCode()}");
        Console.WriteLine($"hash2 = {instance2.GetHashCode()}");

        return Task.CompletedTask;
    }

    class GlobalSettings
    {
        public int Volume { get; set; }
        public float Sensitivity { get; set; }
        public byte Mode { get; set; }

        private static volatile GlobalSettings? _instance;

        public static GlobalSettings Instance
        {
            get
            {
                lock (syncRoot)
                {
                    if (_instance is null)
                        return _instance = new GlobalSettings();

                    return _instance;
                }
            }
        }

        private static object syncRoot = new();

        private GlobalSettings() { }
    }
}
