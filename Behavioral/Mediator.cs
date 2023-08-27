namespace DesignOfPatterns.Behavioral;

/* Назначение: определяет объект, инкапсулирующий способ взаимодействия множества объектов.
 * Другими словами: посредник — это клей, связывающий несколько независимых
 * классов между собой. Он избавляет классы от необходимости ссылаться друг н
 * друга, позволяя тем самым их независимо изменять и анализировать.

 * Существует явная и неявная реализация посредника.
 * При явной реализации - независимые классы не знают друг о друге, но знают о посреднике.
 * При неявной реализаци - независимые классы не знают друг о друге и не знают о посреднике. */

internal class Mediator : LaunchablePattern
{
    protected override Task Main()
    {
        var mediator = new VideoService();
        mediator.DoWork("avatar2.mp4");

        return Task.CompletedTask;
    }

    /// <summary>
    /// Класс VideoService выступает в роли посредника: он связывает воедино несколько
    /// низкоуровневых классов для обеспечения нового высокоуровневого поведения.
    /// Такой подход обеспечивает гибкость в развитии системы, хотя и не вводит полиморфного поведения.
    /// Посредник в этом случае выступает барьером, который гасит изменения в одной части системы, 
    /// не давая им распространяться на другие части!
    /// Любые изменения в классе VideoUploader приведут к модификации VideoService, 
    /// но не потребуют изменений VideoDownloader и VideoConverter.
    /// </summary>
    class VideoService
    {
        // композиция
        private readonly VideoUploader _uploader = new();
        private readonly VideoDownloader _downloader = new();
        private readonly VideoConverter _converter = new();

        public void DoWork(string filename)
        {
            var video = _downloader.Download(filename);

            _converter.Process(video);
            _uploader.Upload(video);
        }
    }

    // 3 независимых сервиса. Друг о друге они ничего не знают.

    class VideoUploader
    {
        public void Upload(byte[] video)
        {
            Console.WriteLine($"Uploading video ...");
        }
    }

    class VideoConverter
    {
        public byte[] Process(byte[] video)
        {
            Console.WriteLine($"Converting video ...");

            return video;
        }
    }

    class VideoDownloader
    {
        public byte[] Download(string filename)
        {
            Console.WriteLine($"Downloading [{filename}] ...");

            byte[] result = new byte[20];
            Random.Shared.NextBytes(result);

            return result;
        }
    }
}
