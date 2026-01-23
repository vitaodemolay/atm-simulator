using atm_executor.consoleTemplate;
using atm_executor.consoleTemplate.screens;
using atm_executor.domain;

namespace atm_executor
{
    /// <summary>
    /// Exemplo de como usar o ScreenNavigator e as telas de console
    /// Este arquivo demonstra o padrão de uso para implementar os fluxos de Cliente e Agente
    /// </summary>
    public class ScreenExample
    {
        private ScreenNavigator _navigator;
        private AtmMachine _atmMachine;

        public ScreenExample()
        {
            _navigator = new ScreenNavigator();
            _atmMachine = new AtmMachine();
            
            // Setup inicial da máquina com algumas notas
            _atmMachine.LoadCash(new Money(100), 10);
            _atmMachine.LoadCash(new Money(50), 20);
            _atmMachine.LoadCash(new Money(20), 30);
        }

        /// <summary>
        /// Exemplo de fluxo para Cliente realizar Saque
        /// </summary>
        public void RunClientWithdrawalFlow()
        {
            // 1. Menu Principal
            var mainMenu = _navigator.CreateMainMenu();
            mainMenu.Render();
            int mainChoice = mainMenu.GetSelectedOption();

            if (mainChoice == 1) // Cliente
            {
                // 2. Menu Cliente
                var clientMenu = _navigator.CreateClientMenu();
                clientMenu.Render();
                int clientChoice = clientMenu.GetSelectedOption();

                if (clientChoice == 1) // Realizar Saque
                {
                    // Loop de saque do cliente
                    bool continueShopping = true;
                    while (continueShopping)
                    {
                        // 3. Entrada de valor de saque
                        var amountScreen = _navigator.CreateWithdrawalAmountScreen();
                        amountScreen.Render();
                        Money withdrawalAmount = amountScreen.GetNumericInput();

                        // 4. Validar opções de saque
                        var validOptions = _atmMachine.ValidWithdrawalOptions(withdrawalAmount);

                        if (validOptions.Count() == 0)
                        {
                            // Sem opções disponíveis
                            var errorScreen = _navigator.CreateWithdrawalErrorScreen(
                                $"Desculpe, não é possível realizar saque de R$ {withdrawalAmount:F2}.\n" +
                                "A máquina não possui combinação de notas disponível para este valor."
                            );
                            errorScreen.Render();
                            int errorChoice = errorScreen.GetSelectedOption();

                            if (errorChoice == 0)
                            {
                                continueShopping = false; // Voltar ao menu
                            }
                            // Se escolher 1, continua no loop para tentar outro valor
                        }
                        else
                        {
                            // 5. Exibir opções de saque
                            var optionScreen = _navigator.CreateWithdrawalOptionScreen();
                            optionScreen.SetOptions(validOptions);
                            optionScreen.Render();
                            int selectedIndex = optionScreen.GetSelectedOption();

                            if (selectedIndex == -1)
                            {
                                continueShopping = false; // Voltar ao menu
                            }
                            else
                            {
                                // 6. Executar saque
                                var selectedOption = optionScreen.GetSelectedWithdrawalOption(selectedIndex);
                                _atmMachine.Withdraw(selectedOption);

                                // 7. Confirmar saque
                                var confirmScreen = _navigator.CreateWithdrawalConfirmationScreen();
                                confirmScreen.ShowWithdrawalSuccess(
                                    withdrawalAmount.Amount,
                                    selectedOption.ToString()
                                );

                                // Volta ao menu de cliente para nova operação
                                // continueShopping permanece true
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Exemplo de fluxo para Agente realizar Reabastecimento
        /// </summary>
        public void RunAgentRefillFlow()
        {
            // 1. Menu Principal
            var mainMenu = _navigator.CreateMainMenu();
            mainMenu.Render();
            int mainChoice = mainMenu.GetSelectedOption();

            if (mainChoice == 2) // Agente
            {
                // 2. Menu Agente
                var agentMenu = _navigator.CreateAgentMenu();
                agentMenu.Render();
                int agentChoice = agentMenu.GetSelectedOption();

                if (agentChoice == 1) // Realizar Reabastecimento
                {
                    // Loop de reabastecimento
                    bool continueRefilling = true;
                    while (continueRefilling)
                    {
                        // 3. Exibir estado dos slots
                        var slotScreen = _navigator.CreateSlotStatusScreen();
                        slotScreen.DisplaySlots(_atmMachine.GetMoneySlots());
                        int selectedSlot = slotScreen.GetSelectedSlotOption(3);

                        if (selectedSlot == -1)
                        {
                            continueRefilling = false; // Voltar ao menu
                        }
                        else
                        {
                            // 4. Obter denominação do slot selecionado
                            var slots = _atmMachine.GetMoneySlots().ToList();
                            var selectedMoneySlot = slots[selectedSlot - 1]; // Converter para 0-based

                            // 5. Entrada de quantidade de notas
                            var quantityScreen = _navigator.CreateNotesQuantityScreen(selectedMoneySlot.Value.Amount);
                            quantityScreen.Render();
                            int quantity = quantityScreen.GetIntegerInput();

                            // 6. Realizar reabastecimento
                            _atmMachine.LoadCash(selectedMoneySlot.Value, quantity);

                            // 7. Confirmar reabastecimento
                            var confirmScreen = _navigator.CreateRefillConfirmationScreen();
                            confirmScreen.ShowRefillSuccess(selectedMoneySlot.Value.Amount, quantity);

                            // Continua no loop para reabasteceu outro slot
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Método simples para demonstrar uso de MenuScreen
        /// </summary>
        public void SimpleMenuExample()
        {
            var menu = new MenuScreen("== EXEMPLO DE MENU ==");
            menu.AddOption(1, "Opção 1");
            menu.AddOption(2, "Opção 2");
            menu.AddOption(3, "Opção 3");
            menu.AddOption(0, "Sair");

            menu.Render();
            int choice = menu.GetSelectedOption();
            
            Console.WriteLine($"Você escolheu: {choice}");
        }

        /// <summary>
        /// Método simples para demonstrar uso de NumericInputScreen
        /// </summary>
        public void SimpleNumericInputExample()
        {
            var inputScreen = new NumericInputScreen(
                "== ENTRADA DE VALOR ==",
                "Digite um valor entre 10 e 1000: "
            );
            inputScreen.SetMinValue(10);
            inputScreen.SetMaxValue(1000);
            
            inputScreen.Render();
            Money value = inputScreen.GetNumericInput();
            
            Console.WriteLine($"Valor inserido: R$ {value}");
        }

        /// <summary>
        /// Método simples para demonstrar uso de DisplayScreen
        /// </summary>
        public void SimpleDisplayExample()
        {
            var displayScreen = new DisplayScreen(
                "== INFORMAÇÕES ==",
                "Esta é uma tela de exibição de informações"
            );
            displayScreen.SetHasContinuePrompt(true);
            
            displayScreen.Render();
            displayScreen.WaitForUserInput();
        }
    }
}
