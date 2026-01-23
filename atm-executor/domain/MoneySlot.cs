using atm_executor.domain.contracts;

namespace atm_executor.domain
{
    public class MoneySlot : IMoneySlotView
    {
        private int _quantity;

        public Money Value { get; private set; }
        public int Quantity => _quantity;
        public Money TotalAmount() => new Money(Value.Amount * _quantity);

        public MoneySlot(Money slotValue) => (Value, _quantity) = (slotValue, default);
        public void Setup (int quantity) => _quantity = quantity;

        public bool CanDispense(int quantity) => quantity <= _quantity;

        public void Dispense(int quantity)
        {
            if (quantity > _quantity)
            {
                throw new InvalidOperationException("Not enough money in the slot to dispense the requested quantity.");
            }

            _quantity -= quantity;
        }
    }
}