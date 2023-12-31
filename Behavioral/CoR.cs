﻿namespace DesignOfPatterns.Behavioral;

/* Паттерн Chain of Responsibility - позволяет избежать привязки 
 * объекта-отправителя запроса к объекту-получателю запроса, 
 * при этом давая шанс обработать этот запрос нескольким объектам. 

 * Паттерн Chain of Responsibility – связывает в цепочку объекты-получатели запроса
 * и передает запрос вдоль этой цепочки, пока один из объектов, 
 * составляющих эту цепочку не обработает передаваемый запрос */

internal class CoR : LaunchablePattern
{
    protected override Task Main()
    {
        var middleware1 = new Authorization();
        var middleware2 = new NameFiltration();
        var middleware3 = new EndpointResolver();

        middleware1.SetNext(middleware2).SetNext(middleware3);

        middleware1.Handle(new("Conor", Role.user), new("login-page"));
        middleware1.Handle(new("Conor", Role.admin), new("admin-page"));
        middleware1.Handle(new("Donald", Role.admin), new("admin-page"));
        middleware1.Handle(new("Gary", Role.admin), new("admin-page"));
        middleware1.Handle(new("Conor", Role.admin), new("admin-panel"));

        return Task.CompletedTask;
    }

    enum Role { admin, user }
    record User(string name, Role role);
    record Request(string resource);

    abstract class Middleware
    {
        protected Middleware Next { get; set; }

        public Middleware SetNext(Middleware middleware)
        {
            Next = middleware;

            return Next;
        }

        protected virtual void BeforeHandle()
            => Console.WriteLine($"{GetType().Name} start");

        protected virtual void AfterHandle()
            => Console.WriteLine($"{GetType().Name} end");

        public void Handle(User user, Request request)
        {
            BeforeHandle();
            Run(user, request);
            AfterHandle();
        }

        protected abstract void Run(User user, Request request);

        protected void PassToNext(User user, Request request)
        {
            if (Next is not null)
                Next.Handle(user, request);
        }
    }

    class NameFiltration : Middleware
    {
        private readonly List<string> _blackListNames;

        public NameFiltration()
        {
            _blackListNames = new()
            {
                "Donald", "John", "Joseph", "Gary"
            };
        }

        protected override void Run(User user, Request request)
        {
            if (_blackListNames.Contains(user.name))
                Console.WriteLine($"The '{user.name}' name is in blacklist");
            else
                PassToNext(user, request);
        }
    }

    class Authorization : Middleware
    {
        protected override void Run(User user, Request request)
        {
            if (user.role == Role.admin)
                PassToNext(user, request);
            else
                Console.WriteLine("user role is not 'admin'");
        }
    }

    class EndpointResolver : Middleware
    {
        protected override void Run(User user, Request request)
        {
            if (request.resource is null ||
                request.resource != "admin-panel")
                Console.WriteLine("404 NotFound");
            else
                PassToNext(user, request);
        }
    }
}
