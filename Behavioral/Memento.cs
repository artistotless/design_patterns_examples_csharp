using System.Drawing;

namespace DesignOfPatterns.Behavioral;

/* Снимок — это поведенческий паттерн проектирования, 
 * который позволяет сохранять и восстанавливать прошлые состояния объектов,
 * не раскрывая подробностей их реализации.*/

internal class Memento : LaunchablePattern
{
    protected override Task Main()
    {
        var settings = MouseSettings.Instance;

        var defaultPreset = settings.SavePreset("default");
        Console.WriteLine("Saved 'default' preset ...");

        settings.SetRgb(false);
        settings.SetSensetivity(1.78f);
        settings.SetDpi(400);
        Console.WriteLine("Modified MouseSettings state ...");

        var customPreset = settings.SavePreset("my_preset");
        Console.WriteLine("Saved 'my_preset' preset...");

        defaultPreset.Restore();
        Console.WriteLine("Restored 'default' preset ...");

        customPreset.Restore();
        Console.WriteLine("Restored 'my_preset' preset ...");

        return Task.CompletedTask;
    }

    interface ISnapshot
    {
        string Name { get; set; }
        DateTime Created { get; set; }
        void Restore();
    }

    // singleton
    class MouseSettings
    {
        private float _sensitivity;
        private Color _rgbColor;
        private bool _rgbEnabled;
        private int _dpi;

        private static volatile MouseSettings? _instance;
        private static object syncRoot = new();

        private MouseSettings()
        {
            _sensitivity = 1;
            _rgbColor = Color.AliceBlue;
            _rgbEnabled = true;
            _dpi = 800;
        }

        public static MouseSettings Instance
        {
            get
            {
                lock (syncRoot)
                {
                    if (_instance is null)
                        return _instance = new MouseSettings();

                    return _instance;
                }
            }
        }

        public ISnapshot SavePreset(string name)
            => new MouseSettingsPreset(this, name);

        public void SetSensetivity(float value)
            => _sensitivity = value;

        public void SetRgbColor(Color value)
            => _rgbColor = value;

        public void SetRgb(bool enabled)
             => _rgbEnabled = enabled;

        public void SetDpi(int dpi)
           => _dpi = dpi;


        class MouseSettingsPreset : ISnapshot
        {
            public string Name { get; set; }
            public DateTime Created { get; set; }

            private MouseSettings _settings;

            private float _sensitivity;
            private Color _rgbColor;
            private bool _rgbEnabled;
            private int _dpi;

            public MouseSettingsPreset(MouseSettings settings, string name)
            {
                _settings = settings;
                _sensitivity = settings._sensitivity;
                _rgbColor = settings._rgbColor;
                _rgbEnabled = settings._rgbEnabled;
                _dpi = settings._dpi;

                Name = name;
                Created = DateTime.Now;
            }

            public void Restore()
            {
                _settings._sensitivity = _sensitivity;
                _settings._rgbColor = _rgbColor;
                _settings._rgbEnabled = _rgbEnabled;
                _settings._dpi = _dpi;
            }
        }
    }
}
