using atm_executor.domain.contracts;

namespace atm_executor.domain
{
    public class AtmMachine
    {
        private readonly Dictionary<Money, MoneySlot> moneySlots;
        private readonly List<double> supportedDenominations = new List<double>
        {
            100.0,
            50.0,
            20.0,
        };

        public AtmMachine()
        {
            moneySlots = new Dictionary<Money, MoneySlot>();
            foreach (var denomination in supportedDenominations)
            {
                moneySlots[denomination] = new MoneySlot(denomination);
            }
        }

        public Money GetTotalCashAvailable()
        {
            Money total = Money.Zero;
            foreach (var slot in moneySlots.Values)
            {
                total += slot.TotalAmount();
            }
            return total;
        }

        public IEnumerable<double> GetSupportedDenominations() => supportedDenominations;
        public IEnumerable<IMoneySlotView> GetMoneySlots() => moneySlots.Values;

        public void LoadCash(Money slotValue, int quantity)
        {
            if (moneySlots.ContainsKey(slotValue))
            {
                moneySlots[slotValue].Setup(quantity);
            }
            else
            {
                throw new ArgumentException("Invalid slot value.");
            }
        }

        public IEnumerable<WithdrawalOption> ValidWithdrawalOptions(Money amount)
        {
            var validCombinations = new List<WithdrawalOption>();

            // Validação 1: Se valor solicitado é menor que o menor denominador disponível
            if (amount < supportedDenominations.Min())
            {
                return validCombinations;
            }

            // Usar backtracking para encontrar até 3 combinações
            var sortedDenominations = supportedDenominations.OrderByDescending(d => d).ToList();
            FindCombinations(
                sortedDenominations,
                amount.Amount,
                0,
                new Dictionary<Money, int>(),
                validCombinations
            );

            return validCombinations;
        }

        public void Withdraw(WithdrawalOption option)
        {
            // Verificar se a combinação é válida
            foreach (var kvp in option.Combination)
            {
                var denomination = kvp.Key;
                var quantity = kvp.Value;

                if (!moneySlots.ContainsKey(denomination) || !moneySlots[denomination].CanDispense(quantity))
                {
                    throw new InvalidOperationException("Invalid withdrawal option.");
                }
            }

            // Dispensar o dinheiro
            foreach (var kvp in option.Combination)
            {
                var denomination = kvp.Key;
                var quantity = kvp.Value;
                moneySlots[denomination].Dispense(quantity);
            }
        }

        private void FindCombinations(
            List<double> sortedDenominations,
            double remainingAmount,
            int denominationIndex,
            Dictionary<Money, int> currentCombination,
            List<WithdrawalOption> validCombinations)
        {
            // Parar se já encontrou 3 opções
            if (validCombinations.Count >= 3)
                return;

            // Caso base: valor exato atingido
            if (Math.Abs(remainingAmount) < 0.01) // usar tolerância para ponto flutuante
            {
                var option = new WithdrawalOption
                {
                    Combination = new Dictionary<Money, int>(currentCombination),
                    TotalAmount = new Money(currentCombination.Sum(kvp => kvp.Key.Amount * kvp.Value))
                };
                validCombinations.Add(option);
                return;
            }

            // Caso base: ultrapassou o valor ou chegou ao final das denominações
            if (remainingAmount < 0 || denominationIndex >= sortedDenominations.Count)
                return;

            var currentDenomination = sortedDenominations[denominationIndex];
            var moneyDenomination = new Money(currentDenomination);

            // Verificar disponibilidade de notas deste denominador
            if (!moneySlots.ContainsKey(moneyDenomination))
            {
                // Passar para próximo denominador
                FindCombinations(
                    sortedDenominations,
                    remainingAmount,
                    denominationIndex + 1,
                    currentCombination,
                    validCombinations
                );
                return;
            }

            var availableQuantity = moneySlots[moneyDenomination].Quantity;

            // Tentar usar 0, 1, 2, ..., quantidadeDisponível notas deste denominador
            for (int quantity = 0; quantity <= availableQuantity; quantity++)
            {
                if (validCombinations.Count >= 3)
                    return;

                var cost = currentDenomination * quantity;
                var newRemaining = remainingAmount - cost;

                if (newRemaining < -0.01)
                    break; // Não adianta tentar mais notas deste denominador

                // Adicionar esta quantidade à combinação atual
                if (quantity > 0)
                {
                    currentCombination[moneyDenomination] = quantity;
                }

                // Recursivamente tentar preencher o restante com denominadores menores
                FindCombinations(
                    sortedDenominations,
                    newRemaining,
                    denominationIndex + 1,
                    currentCombination,
                    validCombinations
                );

                // Remover da combinação para tentar próxima quantidade
                if (quantity > 0)
                {
                    currentCombination.Remove(moneyDenomination);
                }
            }
        }
    }
}