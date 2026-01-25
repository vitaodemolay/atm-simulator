#!/usr/bin/env bash
# 🎯 QUICK START - Como começar a usar o Template de Console
# 
# Este arquivo documenta os primeiros passos para integrar os templates
# no seu Program.cs e começar a usá-los

# ============================================================================
# PASSO 1: Entender a Estrutura
# ============================================================================
# 
# O consoleTemplate foi organizado assim:
#
# consoleTemplate/
# ├── abstractions/
# │   ├── IScreen.cs          ← Interface base
# │   └── BaseScreen.cs       ← Classe abstrata com helpers
# ├── screens/
# │   ├── MenuScreen.cs       ← Menus
# │   ├── NumericInputScreen.cs ← Entrada de números
# │   ├── OptionSelectionScreen.cs ← Seleção de opções
# │   ├── DisplayScreen.cs    ← Exibição
# │   └── ConfirmationScreen.cs ← Confirmação
# └── ScreenNavigator.cs      ← Factory + navegação

# ============================================================================
# PASSO 2: Imports Necessários no seu Program.cs
# ============================================================================
#
# using atm_executor.consoleTemplate;
# using atm_executor.consoleTemplate.screens;
# using atm_executor.domain;

# ============================================================================
# PASSO 3: Criar Instâncias
# ============================================================================
#
# var atmMachine = new AtmMachine();
# var navigator = new ScreenNavigator();

# ============================================================================
# PASSO 4: Fluxo de Cliente (Saque)
# ============================================================================
#
# while (true) {
#     // Menu principal
#     var mainMenu = navigator.CreateMainMenu();
#     mainMenu.Render();
#     int choice = mainMenu.GetSelectedOption();
#
#     if (choice == 1) { // Cliente
#         // Menu do cliente
#         var clientMenu = navigator.CreateClientMenu();
#         clientMenu.Render();
#         int clientChoice = clientMenu.GetSelectedOption();
#
#         if (clientChoice == 1) { // Realizar saque
#             bool continueShopping = true;
#             while (continueShopping) {
#                 // Entrada de valor
#                 var amountScreen = navigator.CreateWithdrawalAmountScreen();
#                 amountScreen.Render();
#                 Money amount = amountScreen.GetNumericInput();
#
#                 // Validar opções
#                 var options = atmMachine.ValidWithdrawalOptions(amount);
#
#                 if (options.Count() == 0) {
#                     // Sem opções - exibir erro
#                     var errorScreen = navigator.CreateWithdrawalErrorScreen(
#                         $"Desculpe, sem opções para R$ {amount:F2}"
#                     );
#                     errorScreen.Render();
#                     int errorChoice = errorScreen.GetSelectedOption();
#                     continueShopping = (errorChoice == 1);
#                 } else {
#                     // Com opções - exibir seleção
#                     var optionScreen = navigator.CreateWithdrawalOptionScreen();
#                     optionScreen.SetOptions(options);
#                     optionScreen.Render();
#                     int idx = optionScreen.GetSelectedOption();
#
#                     if (idx == -1) {
#                         continueShopping = false;
#                     } else {
#                         // Executar saque
#                         var selected = optionScreen.GetSelectedWithdrawalOption(idx);
#                         atmMachine.Withdraw(selected);
#
#                         // Confirmação
#                         var confirmScreen = navigator.CreateWithdrawalConfirmationScreen();
#                         confirmScreen.ShowWithdrawalSuccess(
#                             amount.Amount, 
#                             selected.ToString()
#                         );
#                         // continueShopping = true (continua o loop)
#                     }
#                 }
#             }
#         }
#     }
# }

# ============================================================================
# PASSO 5: Fluxo de Agente (Reabastecimento)
# ============================================================================
#
# if (choice == 2) { // Agente
#     var agentMenu = navigator.CreateAgentMenu();
#     agentMenu.Render();
#     int agentChoice = agentMenu.GetSelectedOption();
#
#     if (agentChoice == 1) {
#         bool continueRefilling = true;
#         while (continueRefilling) {
#             // Estado dos slots
#             var slotScreen = navigator.CreateSlotStatusScreen();
#             slotScreen.DisplaySlots(atmMachine.GetMoneySlots());
#             int selectedSlot = slotScreen.GetSelectedSlotOption(3);
#
#             if (selectedSlot == -1) {
#                 continueRefilling = false;
#             } else {
#                 // Pegar slot
#                 var slots = atmMachine.GetMoneySlots().ToList();
#                 var slot = slots[selectedSlot - 1];
#
#                 // Entrada de quantidade
#                 var qtyScreen = navigator.CreateNotesQuantityScreen(slot.Value.Amount);
#                 qtyScreen.Render();
#                 int qty = qtyScreen.GetIntegerInput();
#
#                 // Reabastecimento
#                 atmMachine.LoadCash(slot.Value, qty);
#
#                 // Confirmação
#                 var confirmScreen = navigator.CreateRefillConfirmationScreen();
#                 confirmScreen.ShowRefillSuccess(slot.Value.Amount, qty);
#                 // Loop continua
#             }
#         }
#     }
# }

# ============================================================================
# PASSO 6: Compilar e Testar
# ============================================================================
# 
# cd /Users/vitor/Documents/Developer/Projetos_dotnet/atm-simulator
# dotnet build
# dotnet run --project atm-executor/atm-executor.csproj

# ============================================================================
# REFERÊNCIAS RÁPIDAS
# ============================================================================
#
# MenuScreen:
#   - Render()
#   - AddOption(int, string)
#   - GetSelectedOption() -> int
#
# NumericInputScreen:
#   - Render()
#   - GetNumericInput() -> Money
#   - GetIntegerInput() -> int
#   - SetMinValue(double)
#   - SetMaxValue(double)
#
# OptionSelectionScreen:
#   - SetOptions(IEnumerable<WithdrawalOption>)
#   - Render()
#   - GetSelectedOption() -> int (índice) ou -1 (voltar)
#   - GetSelectedWithdrawalOption(int) -> WithdrawalOption
#
# DisplayScreen:
#   - DisplaySlots(IEnumerable<IMoneySlotView>)
#   - Render()
#   - GetSelectedSlotOption(int) -> int (slot) ou -1 (voltar)
#   - GetSelectedOption() -> int
#
# ConfirmationScreen:
#   - ShowWithdrawalSuccess(double, string)
#   - ShowRefillSuccess(double, int)
#   - ShowErrorWithRetryOption(string)
#
# ScreenNavigator:
#   - CreateMainMenu()
#   - CreateClientMenu()
#   - CreateAgentMenu()
#   - CreateWithdrawalAmountScreen()
#   - CreateNotesQuantityScreen(double)
#   - CreateWithdrawalOptionScreen()
#   - CreateSlotStatusScreen()
#   - CreateWithdrawalErrorScreen(string)
#   - CreateWithdrawalConfirmationScreen()
#   - CreateRefillConfirmationScreen()

# ============================================================================
# EXEMPLO MÍNIMO (Teste Rápido)
# ============================================================================
#
# static void Main(string[] args)
# {
#     var navigator = new ScreenNavigator();
#     
#     // Menu principal
#     var menu = navigator.CreateMainMenu();
#     menu.Render();
#     int choice = menu.GetSelectedOption();
#     
#     Console.WriteLine($"Você escolheu: {choice}");
# }

# ============================================================================
# DOCUMENTAÇÃO ADICIONAL
# ============================================================================
#
# Ver arquivos:
# - CONSOLE_TEMPLATE_GUIDE.md   (Documentação técnica)
# - PRACTICAL_GUIDE.md          (Exemplos práticos)
# - ScreenExample.cs            (Código completo)
# - README_TEMPLATES.md         (Visão geral)

# ============================================================================
# PRÓXIMOS PASSOS
# ============================================================================
#
# 1. ✅ Estrutura criada e compilada
# 2. ⬜ Integrar no Program.cs
# 3. ⬜ Testar fluxo de cliente
# 4. ⬜ Testar fluxo de agente
# 5. ⬜ Refinar conforme necessário

echo "✅ Arquitetura de templates está pronta!"
echo "📚 Veja PRACTICAL_GUIDE.md para exemplos detalhados"
echo "🚀 Comece pelo passo 3 acima"
