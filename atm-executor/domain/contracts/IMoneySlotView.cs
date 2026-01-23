
namespace atm_executor.domain.contracts
{
    public interface IMoneySlotView
    {
        public Money Value { get; }
        public int Quantity { get; }
    }
}