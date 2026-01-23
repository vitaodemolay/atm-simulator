using atm_executor.domain;
using Xunit;

namespace atm_unitTests.Domain
{
    public class MoneySlotTest
    {
        [Fact]
        public void Should_create_money_slot()
        {
            var slotValue = new Money(20.0);
            var moneySlot = new MoneySlot(slotValue);

            Assert.NotNull(moneySlot);
            Assert.Equal(slotValue, moneySlot.Value);
            Assert.Equal(0, moneySlot.Quantity);
            Assert.Equal(Money.Zero, moneySlot.TotalAmount());
        }

        [Fact]
        public void Should_setup_quantity()
        {
            var slotValue = new Money(50.0);
            var moneySlot = new MoneySlot(slotValue);
            moneySlot.Setup(10);

            Assert.Equal(10, moneySlot.Quantity);
            Assert.Equal(slotValue, moneySlot.Value);
            Assert.True(500.0 == moneySlot.TotalAmount());
        }

        [Fact]
        public void Should_dispense_money()
        {
            var slotValue = new Money(100.0);
            var moneySlot = new MoneySlot(slotValue);
            moneySlot.Setup(5);

            moneySlot.Dispense(2);

            Assert.Equal(3, moneySlot.Quantity);
            Assert.Equal(slotValue, moneySlot.Value);
            Assert.True(300.0 == moneySlot.TotalAmount());
        }

        [Fact]
        public void Should_not_dispense_more_than_available()
        {
            var slotValue = new Money(20.0);
            var moneySlot = new MoneySlot(slotValue);
            moneySlot.Setup(3);

            Assert.Throws<InvalidOperationException>(() => moneySlot.Dispense(5));
        }

        [Fact]
        public void Should_check_if_can_dispense()
        {
            var slotValue = new Money(50.0);
            var moneySlot = new MoneySlot(slotValue);
            moneySlot.Setup(4);

            Assert.True(moneySlot.CanDispense(3));
            Assert.False(moneySlot.CanDispense(5));
        }

        [Fact]
        public void Should_return_string_representation()
        {
            var slotValue = new Money(100.0);
            var moneySlot = new MoneySlot(slotValue);
            moneySlot.Setup(5);

            Assert.Equal("Slot R$ 100,00 - Quantity: 5 - Total: R$ 500,00", moneySlot.ToString());
        }
    }
}