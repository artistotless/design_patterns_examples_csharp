using System.Collections;

namespace DesignOfPatterns.Behavioral;

// Назначение: представляет доступ ко всем элементам составного объекта, не раскрывая его внутреннего представления.

internal class Iterator : LaunchablePattern
{
    protected override Task Main()
    {
        var park = new AutoPark
        {
            new("bmw", 100),
            new("mersedes", 120),
            new("jeep", 87)
        };

        Console.WriteLine("Cars in the autopark: ");

        foreach (var car in park)
            Console.WriteLine($"{car.name} | {car.maxSpeed} (max speed)");

        return Task.CompletedTask;
    }

    record Car(string name, int maxSpeed);

    class AutoPark : IEnumerable<Car>
    {
        private List<Car> _cars = new();

        public void Add(Car car)
            => _cars.Add(car);

        public void Remove(string name)
            => _cars.RemoveAll(x => x.name == name);

        public IEnumerator<Car> GetEnumerator()
            => new Enumerator(this);

        IEnumerator IEnumerable.GetEnumerator()
            => new Enumerator(this);

        struct Enumerator : IEnumerator<Car>
        {
            private AutoPark _park;
            private int _cursor = -1;

            public Enumerator(AutoPark park)
                => _park = park;

            public Car Current => _park._cars[_cursor];

            object IEnumerator.Current => Current;

            public void Dispose()
            {
                _park = null;
                _cursor = -1;
            }

            public bool MoveNext()
            {
                return _park._cars.Count > ++_cursor;
            }

            public void Reset()
            {
                _cursor = -1;
            }
        }
    }


}