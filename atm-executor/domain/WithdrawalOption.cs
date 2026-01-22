namespace atm_executor.domain
{
    public class WithdrawalOption
    {
        public Dictionary<Money, int> Combination { get; internal set; }
        public Money TotalAmount { get; internal set; }

        public WithdrawalOption()
        {
            Combination = new Dictionary<Money, int>();
            TotalAmount = Money.Zero;
        }

        public override string ToString()
        {
            if (Combination.Count == 0)
                return string.Empty;

            var parts = Combination
                .OrderByDescending(kvp => kvp.Key.Amount)
                .Select(kvp => $"{kvp.Value}x {kvp.Key}")
                .ToList();

            return $"{TotalAmount} ( {string.Join(" + ", parts)} )";
        }
    }
}
