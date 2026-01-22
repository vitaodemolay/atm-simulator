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
        public static bool operator ==(Money a, Money b) => a.amount == b.amount;
        public static bool operator !=(Money a, Money b) => a.amount != b.amount;

        public static bool operator ==(Money a, double b) => a.amount == b;
        public static bool operator !=(Money a, double b) => a.amount != b;
        public static bool operator ==(double a, Money b) => a == b.amount;
        public static bool operator !=(double a, Money b) => a != b.amount;
        public override bool Equals(object? obj) => obj is Money m && this == m;
        public override int GetHashCode() => amount.GetHashCode();
        public static implicit operator double(Money m) => m.amount;
        public static implicit operator Money(double v) => new Money(v);
        public static Money Zero => new Money(0);


        public override string ToString() => $"{currency} {amount:F2}";
    }
}