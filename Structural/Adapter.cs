namespace DesignOfPatterns.Structural;

/* Адаптер — это структурный паттерн проектирования, 
 * который позволяет объектам с несовместимыми интерфейсами работать вместе. */

internal class Adapter : LaunchablePattern
{
    protected override Task Main()
    {
        var products = new List<Product>()
        {
            new("Acer E5 551G",23_000),
            new("Macbook Pro", 140_000),
            new("Steelseries Headphone", 4500)
        };

        var cart = new Cart(products);
        double totalPrice = 0;

        PriceCalculator calculator = new DefaultCalculator();
        totalPrice = calculator.CalculatePrice(cart);
        Console.WriteLine($"[DefaultCalculator] Total price: {totalPrice}");

        calculator = new ThirdPartyCalculatorAdapter(new ThirdPartyCalculator());
        totalPrice = calculator.CalculatePrice(cart);
        Console.WriteLine($"[ThirdPartyCalculatorAdapter] Total price: {totalPrice}");

        return Task.CompletedTask;
    }

    record Product(string name, double price);
    record Cart(List<Product> products);

    abstract class PriceCalculator
    {
        public abstract double CalculatePrice(Cart cart);
    }

    sealed class ThirdPartyCalculator
    {
        private const float _DISCOUNT = 0.5f;

        public float Calculate(IEnumerable<float> prices)
        {
            // hard calculations here...

            return ApplyDiscount(prices.Sum());
        }

        private float ApplyDiscount(float totalPrice)
            => totalPrice * _DISCOUNT;
    }

    class DefaultCalculator : PriceCalculator
    {
        public override double CalculatePrice(Cart cart)
            => cart.products.Sum(p => p.price);
    }

    class ThirdPartyCalculatorAdapter : PriceCalculator
    {
        private readonly ThirdPartyCalculator _calculator;

        public ThirdPartyCalculatorAdapter(ThirdPartyCalculator calculator)
        {
            _calculator = calculator;
        }

        public override double CalculatePrice(Cart cart)
        {
            var argument = cart.products.Select(x => (float)x.price);
            var result = _calculator.Calculate(argument);

            return (double)result;
        }
    }
}
