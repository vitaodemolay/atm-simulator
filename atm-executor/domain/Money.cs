namespace atm_executor.domain
{
    public class Money
    {
        private readonly double amount;
        private readonly string currency = "R$";

        public Money(double amount) => this.amount = amount;
        public double Amount => amount;
        public static Money operator +(Money a, Money b) => new Money(a.amount + b.amount);
        public static Money operator -(Money a, Money b) => new Money(a.amount - b.amount);
        public static implicit operator double(Money m) => m.amount;
        public static implicit operator Money(double v) => new Money(v);


        public override string ToString() => $"{currency} {amount:F2}";
    }
}