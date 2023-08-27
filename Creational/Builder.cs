using System.Text;

namespace DesignOfPatterns.Creational;

/* Паттерн Builder – помогает организовать пошаговое построение сложного объекта-продукта так, что
 * клиенту не требуется понимать последовательность шагов и внутреннее устройство строящегося объектапродукта,
 * при этом в результате одного и того же процесса конструирования могут получаться объектыпродукты
 * с различным представлением(внутренним устройством). */

internal class Builder : LaunchablePattern
{
    protected override Task Main()
    {
        INutritionPlanBuilder builder = new WeightGainNutritionPlanBuilder(10);

        var plan = builder
            .AddProtein(new MaxlerGainer())
            .AddСarbohydrates(new Bread())
            .AddProtein(new Curd())
            .AddСarbohydrates(new Pasta())
            .Build();

        Console.WriteLine(plan);

        return Task.CompletedTask;
    }

    // interfaces
    interface INutritionPlanBuilder
    {
        INutritionPlanBuilder AddProtein(IProteinFood protein);
        INutritionPlanBuilder AddСarbohydrates(IСarbohydratesFood сarbohydrates);
        INutritionPlanBuilder AddFats(IFatsFood fats);
        NutritionPlan Build();
    }

    interface IFood
    {
        int Callories { get; }
        string Name { get; }
    }

    interface IProteinFood : IFood { }
    interface IСarbohydratesFood : IFood { }
    interface IFatsFood : IFood { }

    sealed class NutritionPlan
    {
        public readonly List<IFood> Foods = new();

        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.AppendLine($"Total callories: {Foods.Sum(x => x.Callories)}");

            foreach (var item in Foods)
            {
                sb.AppendLine($"{item.Name} : {item.GetType().Name} - {item.Callories} callories");
            }

            return sb.ToString();
        }
    }

    // implementations

    #region Food implementations
    class Curd : IProteinFood
    {
        public int Callories => 10;
        public string Name => "Творог";
    }

    class MaxlerGainer : IProteinFood, IСarbohydratesFood, IFatsFood
    {
        public int Callories => 400;
        public string Name => "Гейнер от Maxler";
    }

    class Bread : IСarbohydratesFood
    {
        public int Callories => 100;
        public string Name => "Хлеб";
    }

    class Pasta : IСarbohydratesFood
    {
        public int Callories => 200;
        public string Name => "Макароны Шебекинские";
    }

    #endregion

    class WeightGainNutritionPlanBuilder : INutritionPlanBuilder
    {
        private readonly NutritionPlan _plan = new();
        private readonly int _currentWeight = 0;

        private const int CALLORIES_PER_1KG = 45;

        public WeightGainNutritionPlanBuilder(int currentWeight)
        {
            _currentWeight = currentWeight;
        }

        public INutritionPlanBuilder AddFats(IFatsFood fats)
        {
            _plan.Foods.Add(fats);

            return this;
        }

        public INutritionPlanBuilder AddProtein(IProteinFood protein)
        {
            _plan.Foods.Add(protein);

            return this;
        }

        public INutritionPlanBuilder AddСarbohydrates(IСarbohydratesFood сarbohydrates)
        {
            _plan.Foods.Add(сarbohydrates);

            return this;
        }

        public NutritionPlan Build()
        {
            if (_plan.Foods.Sum(x => x.Callories) < _currentWeight * CALLORIES_PER_1KG)
                throw new Exception("Need more calories to gain weight. Add more food!");

            return _plan;
        }
    }
}
