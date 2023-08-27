using System.Collections.Concurrent;
using System.Reactive.Subjects;

namespace DesignOfPatterns.Behavioral;

/* Назначение: определяет зависимость типа «один ко многим» между объектами
 * таким образом, что при изменении состояния одного объекта все зависящие от него
 * оповещаются об этом и автоматически обновляются.
 * Другими словами: наблюдатель уведомляет все заинтересованные стороны о произошедшем событии 
 * или об изменении своего состояния. */

internal class Observer : LaunchablePattern
{
    protected override async Task Main()
    {
        var bank = new BankAccount();

        var monitor = new AsyncMonitor<Transaction>()
            .Subscribe(bank.NewTransactions);

        var generalMonitor = new AsyncMonitor<BankBaseEntity>()
            .Subscribe(bank.NewTransactions)
            .Subscribe(bank.NewCustomers);

        _ = Task.Run(async () =>
        {
            // Observable публикует события
            foreach (var item in Enumerable.Range(1, 5))
            {
                await Task.Delay(100);
                bank.CreateTransaction(new(Random.Shared.Next()));
                await Task.Delay(100);
                bank.CreateNewCustomer(new());
            }
        });

        var tasks = new Task[]{ Task.Run(async () =>
        {
            // Читаем асинхронно то, что нам послал BankAccount (async stream pattern)
            await foreach (var item in monitor)
            {
                await Console.Out.WriteLineAsync($"New transaction: {item.sum} USD");
            }
        }),

        Task.Run(async () =>
        {
            await foreach (var item in generalMonitor)
            {
                await Console.Out.WriteLineAsync($"New entity [{item.GetType().Name}]: {item.id}");
            }
        })};

        await Task.WhenAll(tasks);
    }

    record BankBaseEntity(Guid id);
    record Transaction(int sum) : BankBaseEntity(Guid.NewGuid());
    record Customer() : BankBaseEntity(Guid.NewGuid());

    class BankAccount
    {
        public Subject<Transaction> NewTransactions = new();
        public Subject<Customer> NewCustomers = new();

        public void CreateTransaction(Transaction transaction)
            => NewTransactions.OnNext(transaction);

        public void CreateNewCustomer(Customer customer)
           => NewCustomers.OnNext(customer);
    }

    // Реализация наблюдателя с использованием async stream pattern.
    // Поступаемые от BankAccount потоки данных можно читать асинхронно в async foreach
    class AsyncMonitor<T> : IObserver<T>, IAsyncEnumerator<T>, IAsyncEnumerable<T>
    {
        private readonly ConcurrentQueue<T> _observableObjects = new();

        private TaskCompletionSource _waitingSubjectTask;
        private const int TIMEOUT_WAITING_SUBJECTS_SECONDS = 3;

        public AsyncMonitor<T> Subscribe(IObservable<T> subject)
        {
            subject.Subscribe(this);

            return this;
        }

        public T Current => Dequeue();

        private T Dequeue()
        {
            _ = _observableObjects.TryDequeue(out T subject);

            return subject!;
        }
        public ValueTask DisposeAsync()
            => ValueTask.CompletedTask;

        public IAsyncEnumerator<T> GetAsyncEnumerator(CancellationToken cancellationToken = default)
            => this;

        public async ValueTask<bool> MoveNextAsync()
        {
            _waitingSubjectTask = new TaskCompletionSource();

            try
            {
                await _waitingSubjectTask.Task.WaitAsync(
                    TimeSpan.FromSeconds(TIMEOUT_WAITING_SUBJECTS_SECONDS));
            }
            catch (TimeoutException)
            {
                return false;
            }

            return true;
        }

        public void OnCompleted()
        {
            Console.WriteLine(nameof(OnCompleted));
        }

        public void OnError(Exception error)
        {
            Console.WriteLine(nameof(OnError));
        }

        public void OnNext(T value)
        {
            _observableObjects.Enqueue(value);
            _waitingSubjectTask.TrySetResult();
        }

        public void Reset()
           => _observableObjects.Clear();
    }
}
