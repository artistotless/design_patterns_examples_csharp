namespace DesignOfPatterns.Behavioral;

/* Назначение: шаблонный метод определяет основу алгоритма и позволяет подклассам переопределять
 * некоторые шаги алгоритма, не изменяя его структуры в целом.

 * Другими словами: шаблонный метод — это каркас, в который наследники могут
 * подставить реализации недостающих элементов. */

internal class TemplateMethod : LaunchablePattern
{
    protected override Task Main()
    {
        ReportGenerator reporter = new();

        reporter.GenerateAsync().GetAwaiter().GetResult();

        return Task.CompletedTask;
    }

    record Report(object data, DateTime timestamp, string author);

    class ReportGenerator
    {
        public async Task<Report> GenerateAsync()
        {
            var response = await FetchDataFromServiceAsync();
            var report = ProcessData(response);

            return report;
        }

        // step 1
        protected virtual async Task<ServiceResponse> FetchDataFromServiceAsync()
            => await new SomeService().FetchData();

        // step 2
        protected virtual Report ProcessData(ServiceResponse response)
            => new Report(response.data, response.timestamp, response.author);
    }

    sealed class TestReportGenerator : ReportGenerator
    {
        protected override Task<ServiceResponse> FetchDataFromServiceAsync()
            => Task.FromResult(new ServiceResponse(1, new byte[] { 0, 0 }, DateTime.UtcNow, "user"));
    }

    #region Helpers
    record ServiceResponse(int id, byte[] data, DateTime timestamp, string author);
    class SomeService
    {
        public async Task<ServiceResponse> FetchData()
        {
            await Task.Delay(500);
            Console.WriteLine("Connecting to the service...");
            await Task.Delay(500);
            Console.WriteLine("Fetching data...");
            await Task.Delay(500);
            Console.WriteLine("Done...");
            await Task.Delay(500);

            byte[] buffer = new byte[10];
            Random.Shared.NextBytes(buffer);

            return new ServiceResponse(
                id: Random.Shared.Next(),
                data: buffer,
                timestamp: DateTime.UtcNow,
                author: "admin");
        }
    }
    #endregion
}
