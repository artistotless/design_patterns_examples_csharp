using System.Numerics;

namespace DesignOfPatterns.Behavioral;

/* Паттерн State – позволяет объекту изменять свое поведение в зависимости от своего состояния.
 * Поведение объекта изменяется на столько, что создается впечатление, что изменился класс объекта.
 * Паттерн «Состояние» предполагает выделение базового класса или интерфейса 
 * для всех допустимых операций и наследника для каждого возможного состояния */

internal class StatePattern : LaunchablePattern
{
    protected override Task Main()
    {
        var bot = new NPC();
        var player = new Player("artistotless");

        bot.Dialog.Greet(player);
        bot.Dialog.Greet(player);
        bot.Dialog.Greet(player);
        bot.Dialog.Greet(player);

        bot.Attacking.Attack(player);
        bot.Attacking.SetWeapon(new Weapon());
        bot.Attacking.Attack(player);
        bot.Movement.Move();
        bot.Movement.Endurance = 10;
        bot.Movement.Move();


        return Task.CompletedTask;
    }

    record Player(string name);
    record Weapon();

    class NPC
    {
        public Vector3 Position { get; set; }

        public MovementBehavior Movement { get; set; }
        public DialogBehavior Dialog { get; set; }
        public AttackBehavior Attacking { get; set; }

        public NPC()
        {
            Movement = new(this);
            Dialog = new();
            Attacking = new();
        }
    }

    interface Context<TState>
    {
        TState CurrentState { get; set; }
    }

    abstract class State<TState, TContext> where TContext : Context<TState>
    {
        protected private TContext context;
        protected State(TContext context)
            => this.context = context;

        protected void ChangeState(TState newState)
            => context.CurrentState = newState;
    }

    #region Movement
    class MovementBehavior : Context<MovementState>
    {
        private readonly NPC _npc;

        public MovementBehavior(NPC npc)
        {
            _npc = npc;
            Endurance = 5;
            CurrentState = new Idle(this);
        }

        public MovementState CurrentState { get; set; }
        public int Endurance { get; set; }

        public void ChangePosition(Vector3 position)
            => _npc.Position = position;

        public void Move()
            => CurrentState.Move();
    }

    abstract class MovementState : State<MovementState, MovementBehavior>
    {
        public MovementState(MovementBehavior context) : base(context) { }

        public abstract void Move();
    }

    class Idle : MovementState
    {
        public Idle(MovementBehavior context) : base(context) { }

        public override void Move()
        {
            if (context.Endurance > 0)
            {
                ChangeState(new Walk(context));
                context.Move();
                return;
            }

            Console.WriteLine("I have no endurance. Give me few seconds");

            Task.Delay(2000);

            context.Endurance = 10;
        }
    }

    class Walk : MovementState
    {
        public Walk(MovementBehavior context) : base(context) { }

        public override void Move()
        {
            if (context.Endurance > 5)
            {
                ChangeState(new Run(context));
                context.Move();
                return;
            }

            Console.WriteLine("I am walking...");

            while (context.Endurance-- > 0)
                Thread.Sleep(500);

            context.ChangePosition(new Vector3(10, 0, 2));

            if (context.Endurance <= 0)
            {
                ChangeState(new Idle(context));
                context.Move();
            }
        }
    }

    class Run : MovementState
    {
        public Run(MovementBehavior context) : base(context) { }

        public override void Move()
        {
            Console.WriteLine("I am running...");

            while (context.Endurance-- > 0)
                Thread.Sleep(500);

            context.ChangePosition(new Vector3(50, 0, 17));

            if (context.Endurance <= 0)
            {
                ChangeState(new Idle(context));
                context.Move();
            }
        }
    }
    #endregion

    #region Attacking
    class AttackBehavior : Context<AttackState>
    {
        public Weapon Weapon { get; private set; }
        public AttackState CurrentState { get; set; }

        public AttackBehavior()
        {
            CurrentState = new NoAtack(this);
        }

        public void SetWeapon(Weapon weapon)
            => Weapon = weapon;

        public void Attack(Player player)
            => CurrentState.Attack(player);
    }

    abstract class AttackState : State<AttackState, AttackBehavior>
    {
        public AttackState(AttackBehavior context) : base(context) { }

        public abstract void Attack(Player player);
    }

    class NoAtack : AttackState
    {
        public NoAtack(AttackBehavior context) : base(context) { }

        public override void Attack(Player player)
        {
            if (context.Weapon is not null)
            {
                ChangeState(new MeeleeAttack(context));
                context.Attack(player);
                return;
            }

            Console.WriteLine($"I cant attack {player.name}. I have no weapon!");
        }
    }

    class MeeleeAttack : AttackState
    {
        public MeeleeAttack(AttackBehavior context) : base(context) { }

        public override void Attack(Player player)
        {
            if (context.Weapon is null)
            {
                ChangeState(new NoAtack(context));
                context.Attack(player);
                return;
            }

            Console.WriteLine($"bam bam bam! I punched {player.name}");
        }
    }
    #endregion

    #region Dialog
    class DialogBehavior : Context<ConversationState>
    {
        public ConversationState CurrentState { get; set; }

        public DialogBehavior()
        {
            CurrentState = new Neutral(this);
        }

        public void Greet(Player player)
            => CurrentState.Greet(player);
    }

    abstract class ConversationState : State<ConversationState, DialogBehavior>
    {
        protected ConversationState(DialogBehavior context) : base(context) { }
        public abstract void Greet(Player player);
    }

    class Neutral : ConversationState
    {
        protected Dictionary<string, int> playersConversations = new();

        public Neutral(DialogBehavior context) : base(context) { }

        public override void Greet(Player player)
        {
            if (playersConversations.ContainsKey(player.name))
                playersConversations[player.name]++;
            else
                playersConversations.Add(player.name, 1);

            if (playersConversations[player.name] > 2)
                ChangeState(new Happy(context));

            Console.WriteLine($"Hellow, {player.name}!");
        }
    }

    class Happy : ConversationState
    {
        public Happy(DialogBehavior context) : base(context) { }

        public override void Greet(Player player)
        {
            Console.WriteLine($"I am glad to see you, {player.name}!");
        }
    }
    #endregion
}
