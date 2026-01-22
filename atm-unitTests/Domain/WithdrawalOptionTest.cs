using atm_executor.domain;
using Xunit;

namespace atm_unitTests.Domain
{
    public class WithdrawalOptionTest
    {
        [Fact]
        public void Should_create_withdrawal_option()
        {
            var option = new WithdrawalOption();

            Assert.NotNull(option);
            Assert.Empty(option.Combination);
            Assert.Equal(Money.Zero, option.TotalAmount);
        }

        [Fact]
        public void Should_format_withdrawal_option_as_string()
        {
            var option = new WithdrawalOption();
            option.Combination[new Money(100.0)] = 2;
            option.Combination[new Money(50.0)] = 1;
            option.TotalAmount = new Money(250.0);

            Assert.Equal("R$ 250,00 ( 2x R$ 100,00 + 1x R$ 50,00 )", option.ToString());
        }
    }
}