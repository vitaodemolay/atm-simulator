using atm_executor.domain;
using Xunit;

namespace atm_unitTests.Domain
{
    public class MoneyTest
    {
        [Fact]
        public void Should_create_money_instance()
        {
            var money = new Money(10.0);
            Assert.NotNull(money);
            Assert.Equal(10.0, money.Amount);
        }

        [Fact]
        public void Should_create_by_implicit_conversion()
        {
            Money money = 25.5;
            Assert.NotNull(money);
            Assert.Equal(25.5, money.Amount);
        }

        [Fact]
        public void Should_convert_to_double_implicitly()
        {
            var money = new Money(30.0);
            double value = money;
            Assert.Equal(30.0, value);
        }

        [Fact]
        public void Should_add_two_money_instances()
        {
            var money1 = new Money(15.0);
            var money2 = new Money(25.0);
            var result = money1 + money2;
            Assert.Equal(40.0, result.Amount);
        }

        [Fact]
        public void Should_subtract_two_money_instances()
        {
            var money1 = new Money(50.0);
            var money2 = new Money(20.0);
            var result = money1 - money2;
            Assert.Equal(30.0, result.Amount);
        }

        [Fact]
        public void Should_format_money_as_string()
        {
            var money = new Money(123.456);
            Assert.Equal("R$ 123,46", money.ToString());
        }
    }
}