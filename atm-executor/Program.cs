
using atm_executor.consoleTemplate;
using atm_executor.domain;

namespace atm_executor
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var executor = new AtmExecutor();
            executor.Run();
        }
    }

    public class AtmExecutor
    {
        public void Run()
        {
            var atmMachine = new AtmMachine();
            var navigator = new ScreenNavigator();

            MainMenuFlow(atmMachine, navigator);
        }

        private void MainMenuFlow(AtmMachine atmMachine, ScreenNavigator navigator)
        {
            bool exitRequested = false;

            while (!exitRequested)
            {
                var mainMenuScreen = navigator.CreateMainMenu();
                mainMenuScreen.Render();

                int choice = mainMenuScreen.GetSelectedOption();

                switch (choice)
                {
                    case 0:
                        // exit
                        exitRequested = true;
                        break;
                    case 1:
                        // Flow de cliente
                        WithdrawalMainMenuFlow(atmMachine, navigator);
                        break;
                    case 2:
                        // Flow de reabastecimento
                        LoadMainMenuFlow(atmMachine, navigator);
                        break;
                    default:
                        // Opção inválida
                        break;
                }
            }
        }

        private void LoadMainMenuFlow(AtmMachine atmMachine, ScreenNavigator navigator)
        {
            bool continueRefilling = true;

            while (continueRefilling)
            {
                // 1. Menu de reabastecimento
                var refillMenu = navigator.CreateAgentMenu();
                refillMenu.Render();
                int refillChoice = refillMenu.GetSelectedOption();

                if (refillChoice == 0)
                {
                    continueRefilling = false; // Voltar ao menu principal
                    continue;
                }

                if (refillChoice == 1)
                {
                    // 2. Seleção de denominação para reabastecimento
                    DisplaySlotsScreen(atmMachine, navigator);
                }

            }
        }

        private void DisplaySlotsScreen(AtmMachine atmMachine, ScreenNavigator navigator)
        {
            var displayScreen = navigator.CreateSlotStatusScreen();
            var slots = atmMachine.GetMoneySlots();
            displayScreen.AddMenuOption(0, "Voltar ao Menu Anterior");
            displayScreen.DisplaySlots(slots);
            int selectedSlot = displayScreen.GetSelectedSlotOption(slots.Count());
            if (selectedSlot == -1)
            {
                return; // Voltar ao menu anterior
            }
            var selectedMoneySlot = slots.ElementAt(selectedSlot - 1); // Converter para 0-based
            // 3. Entrada de quantidade de notas
            var quantityScreen = navigator.CreateNotesQuantityScreen(selectedMoneySlot.Value.Amount);
            quantityScreen.Render();
            int quantity = quantityScreen.GetIntegerInput();
            // 4. Realizar reabastecimento
            atmMachine.LoadCash(selectedMoneySlot.Value, quantity);
            // 5. Confirmar reabastecimento
            var confirmScreen = navigator.CreateRefillConfirmationScreen();
            confirmScreen.ShowRefillSuccess(selectedMoneySlot.Value.Amount, quantity);
        }

        private void WithdrawalMainMenuFlow(AtmMachine atmMachine, ScreenNavigator navigator)
        {
            var continueWithdrawal = true;

            while (continueWithdrawal)
            {
                // 1. Menu de cliente
                var clientMenu = navigator.CreateClientMenu();
                clientMenu.Render();
                int clientChoice = clientMenu.GetSelectedOption();

                if (clientChoice == 0)
                {
                    continueWithdrawal = false; // Voltar ao menu principal
                    continue;
                }

                if (clientChoice == 1)
                {
                    // 2. Tela de saque
                    DisplayWithdrawalScreen(atmMachine, navigator);
                }
            }
        }

        private void DisplayWithdrawalScreen(AtmMachine atmMachine, ScreenNavigator navigator)
        {
            if(!atmMachine.CanWithdraw())
            {
                var errorScreen = navigator.CreateWithdrawalErrorScreen("A máquina não possui fundos para realizar saques no momento.");
                errorScreen.Render();
                errorScreen.GetSelectedOption(); // Apenas para aguardar o usuário
                return;
            }
            
            var minValue = atmMachine.GetMoneySlots().Where(s => s.Quantity > 0).Min(s => s.Value.Amount);
            var continueWithdrawal = true;
            while (continueWithdrawal)
            {
                // 1. Tela de entrada do valor do saque
                var withdrawalAmountScreen = navigator.CreateWithdrawalAmountScreen();
                withdrawalAmountScreen.SetMinValue(minValue);
                withdrawalAmountScreen.Render();
                var amountToWithdraw = withdrawalAmountScreen.GetNumericInput();

                var options = atmMachine.ValidWithdrawalOptions(amountToWithdraw);
                if (options.Count() == 0)
                {
                    var errorScreen = navigator.CreateWithdrawalErrorScreen("Não há opções disponíveis para o valor solicitado.");
                    errorScreen.Render();
                    if (errorScreen.GetSelectedOption() == 0)
                    {
                        return; // Voltar ao menu de cliente
                    }
                    continue;
                }

                // 2. Tela de seleção da opção de saque
                var optionSelectionScreen = navigator.CreateWithdrawalOptionScreen();
                optionSelectionScreen.SetOptions(options);
                optionSelectionScreen.Render();
                int selectedOptionIndex = optionSelectionScreen.GetSelectedOption();
                if(selectedOptionIndex == -1)
                {
                    return; // Voltar ao menu de cliente
                }
                var selectedOption = options.ElementAt(selectedOptionIndex); // Converter para 0-based
                // 3. Realizar saque
                atmMachine.Withdraw(selectedOption);
                // 4. Confirmar saque
                var confirmationScreen = navigator.CreateWithdrawalConfirmationScreen();
                confirmationScreen.ShowWithdrawalSuccess(amountToWithdraw.Amount, selectedOption.ToString());
                return;
            }

        }
    }
}