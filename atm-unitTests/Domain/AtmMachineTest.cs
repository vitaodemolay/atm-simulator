using atm_executor.domain;
using Xunit;

namespace atm_unitTests.Domain
{
    public class AtmMachineTest
    {
        [Fact]
        public void Should_create_atm_machine()
        {
            var atm = new AtmMachine();

            Assert.NotNull(atm);
            Assert.Equal(Money.Zero, atm.GetTotalCashAvailable());
            Assert.Equal(new List<double> { 100.0, 50.0, 20.0 }, atm.GetSupportedDenominations());
        }

        [Fact]
        public void Should_load_cash_into_slots()
        {
            var atm = new AtmMachine();
            atm.LoadCash(100.0, 10);
            atm.LoadCash(50.0, 20);
            atm.LoadCash(20.0, 30);

            Assert.Equal(new Money(1000.0 + 1000.0 + 600.0), atm.GetTotalCashAvailable());
        }

        [Theory]
        [InlineData(100.0, 1)]
        [InlineData(50.0, 1)]
        [InlineData(20.0, 1)]
        public void Should_return_valid_withdrawal_options(double amount, int expectedOptions)
        {
            var atm = new AtmMachine();
            atm.LoadCash(100.0, 1);
            atm.LoadCash(50.0, 1);
            atm.LoadCash(20.0, 1);

            var options = atm.ValidWithdrawalOptions(new Money(amount));

            Assert.Equal(expectedOptions, options.Count());
            Assert.Equal(new Money(amount), options.First().TotalAmount);
            Assert.Equal(expectedOptions, options.First().Combination.Count);
        }

        [Theory]
        [InlineData(100.0, 3)]
        [InlineData(120.0, 3)]
        [InlineData(180.0, 2)]
        [InlineData(280.0, 1)]
        public void Should_return_valid_withdrawal_options_for_multiple_combinations(double amount, int expectedOptions)
        {
            var atm = new AtmMachine();
            atm.LoadCash(100.0, 1);
            atm.LoadCash(50.0, 3);
            atm.LoadCash(20.0, 6);
            var expectedAmount = new Money(amount);

            var options = atm.ValidWithdrawalOptions(new Money(amount));

            Assert.Equal(expectedOptions, options.Count());
            Assert.Equal(expectedAmount, options.First().TotalAmount);
            Assert.True(expectedAmount == options.First().Combination.Sum(kv => kv.Key * kv.Value));
            Assert.Equal(expectedAmount, options.Last().TotalAmount);
            Assert.True(expectedAmount == options.Last().Combination.Sum(kv => kv.Key * kv.Value));
        }

        [Fact]
        public void Should_return_no_withdrawal_options_when_amount_is_less_than_minimum_denomination()
        {
            var atm = new AtmMachine();
            atm.LoadCash(100.0, 1);
            atm.LoadCash(50.0, 1);
            atm.LoadCash(20.0, 1);

            var options = atm.ValidWithdrawalOptions(new Money(10.0));

            Assert.Empty(options);
        }

        [Fact]
        public void Should_return_no_withdrawal_options_when_no_has_combinations_available()
        {
            var atm = new AtmMachine();
            atm.LoadCash(100.0, 1);
            atm.LoadCash(50.0, 1);
            atm.LoadCash(20.0, 1);

            var options = atm.ValidWithdrawalOptions(new Money(110.0));

            Assert.Empty(options);
        }

        [Fact]
        public void Should_withdraw_money()
        {
            var atm = new AtmMachine();
            atm.LoadCash(100.0, 1);
            atm.LoadCash(50.0, 1);
            atm.LoadCash(20.0, 1);

            var options = atm.ValidWithdrawalOptions(new Money(170.0));
            atm.Withdraw(options.First());

            Assert.Single(options);
            Assert.Equal(Money.Zero, atm.GetTotalCashAvailable());
        }

        [Fact]
        public void Should_not_withdraw_money_when_combination_is_invalid()
        {
            var atm = new AtmMachine();
            atm.LoadCash(100.0, 1);
            atm.LoadCash(50.0, 1);
            atm.LoadCash(20.0, 1);

            var option = new WithdrawalOption()
            {
                Combination = new Dictionary<Money, int>
                {
                    { 100.0, 2 } // Invalid: only 1 bill of 100 is available
                },
                TotalAmount = new Money(200.0)
            };

            Assert.Throws<InvalidOperationException>(() => atm.Withdraw(option));
        }
    }
}