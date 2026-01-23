# Guia Prático de Uso do Template de Console

## 📋 Resumo Rápido

A arquitetura `consoleTemplate` abstrai toda a lógica de entrada/saída do console, permitindo que você trabalhe com objetos de tela ao invés de chamar `Console.WriteLine()` diretamente.

## 🎯 Estrutura de Arquivos Criados

```
consoleTemplate/
├── abstractions/
│   ├── IScreen.cs           # Interface base
│   └── BaseScreen.cs        # Classe abstrata com métodos comuns
├── screens/
│   ├── MenuScreen.cs        # Menu com opções numeradas
│   ├── NumericInputScreen.cs # Entrada de números (valores e quantidades)
│   ├── OptionSelectionScreen.cs # Seleção de opções de saque
│   ├── DisplayScreen.cs     # Exibição de informações genéricas
│   └── ConfirmationScreen.cs # Confirmação de operações
└── ScreenNavigator.cs       # Gerenciador de navegação
```

## 🔧 Como Usar

### 1. **MenuScreen** - Exibir Menu com Opções

```csharp
var menu = new MenuScreen("== MENU PRINCIPAL ==");
menu.AddOption(1, "Opção A");
menu.AddOption(2, "Opção B");
menu.AddOption(0, "Sair");

menu.Render();
int choice = menu.GetSelectedOption(); // Retorna a opção selecionada com validação automática
```

### 2. **NumericInputScreen** - Capturar Valor Monetário

```csharp
var screen = new NumericInputScreen(
    title: "== VALOR DE SAQUE ==",
    prompt: "Digite o valor: ",
    allowNegative: false
);
screen.SetMinValue(0.01);
screen.SetMaxValue(5000);

screen.Render();
Money amount = screen.GetNumericInput(); // Retorna Money com validação
```

### 3. **NumericInputScreen** - Capturar Quantidade de Notas

```csharp
var screen = new NumericInputScreen(
    title: "== REABASTECIMENTO ==",
    prompt: "Quantas notas? ",
    allowNegative: false
);
screen.SetMinValue(1);

screen.Render();
int quantity = screen.GetIntegerInput(); // Retorna int com validação
```

### 4. **OptionSelectionScreen** - Seleção de Opções de Saque

```csharp
var optionScreen = new OptionSelectionScreen("== OPÇÕES DE SAQUE ==");

// Obter opções válidas da máquina
var validOptions = atmMachine.ValidWithdrawalOptions(withdrawalAmount);
optionScreen.SetOptions(validOptions);

optionScreen.Render();
int selectedIndex = optionScreen.GetSelectedOption(); // Retorna índice (0-based) ou -1 para voltar

if (selectedIndex != -1)
{
    var selected = optionScreen.GetSelectedWithdrawalOption(selectedIndex);
    atmMachine.Withdraw(selected);
}
```

### 5. **DisplayScreen** - Exibir Estado dos Slots

```csharp
var slotScreen = new DisplayScreen("== ESTADO DOS SLOTS ==");
slotScreen.SetMessage("Selecione um slot para reabastecer:");

slotScreen.DisplaySlots(atmMachine.GetMoneySlots());
int selectedSlot = slotScreen.GetSelectedSlotOption(3); // Retorna slot (1-based) ou -1 para voltar
```

### 6. **ConfirmationScreen** - Confirmar Operações

```csharp
var confirmScreen = new ConfirmationScreen("== CONFIRMAÇÃO ==");

// Para saque
confirmScreen.ShowWithdrawalSuccess(
    amount: 200.0,
    withdrawalDetails: "2x R$ 100.00"
);

// Para reabastecimento
confirmScreen.ShowRefillSuccess(
    denominationValue: 100.0,
    quantity: 5
);

// Para erro
confirmScreen.ShowErrorWithRetryOption(
    errorMessage: "Desculpe, não há opções disponíveis para este valor."
);
```

### 7. **ScreenNavigator** - Factory para Criar Telas

```csharp
var navigator = new ScreenNavigator();

// Criar telas pré-configuradas
var mainMenu = navigator.CreateMainMenu();
var clientMenu = navigator.CreateClientMenu();
var agentMenu = navigator.CreateAgentMenu();
var amountScreen = navigator.CreateWithdrawalAmountScreen();
var slotScreen = navigator.CreateSlotStatusScreen();
var confirmScreen = navigator.CreateWithdrawalConfirmationScreen();
```

## 📊 Fluxo Típico - Cliente Realizando Saque

```csharp
public void RunClientWithdrawal()
{
    var navigator = new ScreenNavigator();
    
    // 1. Menu principal
    var mainMenu = navigator.CreateMainMenu();
    mainMenu.Render();
    if (mainMenu.GetSelectedOption() != 1) return;
    
    // 2. Menu do cliente
    var clientMenu = navigator.CreateClientMenu();
    clientMenu.Render();
    if (clientMenu.GetSelectedOption() != 1) return;
    
    // 3. Entrada de valor (LOOP)
    bool continueShopping = true;
    while (continueShopping)
    {
        var amountScreen = navigator.CreateWithdrawalAmountScreen();
        amountScreen.Render();
        Money amount = amountScreen.GetNumericInput();
        
        // 4. Validar opções
        var options = atmMachine.ValidWithdrawalOptions(amount);
        
        if (options.Count() == 0)
        {
            // Sem opções
            var errorScreen = navigator.CreateWithdrawalErrorScreen(
                $"Sem opções para R$ {amount:F2}"
            );
            errorScreen.Render();
            if (errorScreen.GetSelectedOption() == 0)
                continueShopping = false;
        }
        else
        {
            // 5. Seleção de opção
            var optionScreen = navigator.CreateWithdrawalOptionScreen();
            optionScreen.SetOptions(options);
            optionScreen.Render();
            int idx = optionScreen.GetSelectedOption();
            
            if (idx == -1)
            {
                continueShopping = false;
            }
            else
            {
                // 6. Executar saque
                var selected = optionScreen.GetSelectedWithdrawalOption(idx);
                atmMachine.Withdraw(selected);
                
                // 7. Confirmação
                var confirmScreen = navigator.CreateWithdrawalConfirmationScreen();
                confirmScreen.ShowWithdrawalSuccess(amount.Amount, selected.ToString());
                // Volta ao loop (continueShopping = true)
            }
        }
    }
}
```

## 📊 Fluxo Típico - Agente Realizando Reabastecimento

```csharp
public void RunAgentRefill()
{
    var navigator = new ScreenNavigator();
    
    // 1. Menu principal
    var mainMenu = navigator.CreateMainMenu();
    mainMenu.Render();
    if (mainMenu.GetSelectedOption() != 2) return;
    
    // 2. Menu do agente
    var agentMenu = navigator.CreateAgentMenu();
    agentMenu.Render();
    if (agentMenu.GetSelectedOption() != 1) return;
    
    // 3. Loop de reabastecimento
    bool continueRefilling = true;
    while (continueRefilling)
    {
        // Estado dos slots
        var slotScreen = navigator.CreateSlotStatusScreen();
        slotScreen.DisplaySlots(atmMachine.GetMoneySlots());
        int selectedSlot = slotScreen.GetSelectedSlotOption(3);
        
        if (selectedSlot == -1)
        {
            continueRefilling = false;
        }
        else
        {
            // Obter slot selecionado
            var slots = atmMachine.GetMoneySlots().ToList();
            var slot = slots[selectedSlot - 1];
            
            // Entrada de quantidade
            var qtyScreen = navigator.CreateNotesQuantityScreen(slot.Value.Amount);
            qtyScreen.Render();
            int qty = qtyScreen.GetIntegerInput();
            
            // Executar reabastecimento
            atmMachine.LoadCash(slot.Value, qty);
            
            // Confirmação
            var confirmScreen = navigator.CreateRefillConfirmationScreen();
            confirmScreen.ShowRefillSuccess(slot.Value.Amount, qty);
            // Volta ao loop (continueRefilling = true)
        }
    }
}
```

## ✨ Características Importantes

| Classe | Responsabilidade |
|--------|------------------|
| `IScreen` | Define contrato mínimo |
| `BaseScreen` | Fornece métodos comuns (bordas, centralização, etc) |
| `MenuScreen` | Menu com validação de opções |
| `NumericInputScreen` | Entrada de números com validação de ranges |
| `OptionSelectionScreen` | Seleção entre WithdrawalOptions |
| `DisplayScreen` | Exibição genérica com menu opcional |
| `ConfirmationScreen` | Resultado de operações |
| `ScreenNavigator` | Factory + navegação entre telas |

## 🔄 Fluxo de Validação

Cada tela valida sua entrada automaticamente:

- ✅ `MenuScreen`: Verifica se opção existe
- ✅ `NumericInputScreen`: Valida tipo, intervalo, negativos
- ✅ `OptionSelectionScreen`: Valida índice
- ✅ `DisplayScreen`: Valida opção de menu se houver

Isso significa que métodos como `GetSelectedOption()` **sempre** retornam um valor válido!

## 🎨 Customização de Telas

Todas as telas herdam de `BaseScreen` e podem ser customizadas:

```csharp
var customScreen = new MenuScreen("== MINHA TELA ==");
customScreen.AddOption(1, "Opção 1");

// Customizar titulo e mensagem
customScreen.Message = "Escolha uma opção:";

// Não limpar tela antes de renderizar
customScreen.ClearScreenBefore = false;

customScreen.Render();
```

## 📝 Próximos Passos

1. Use `ScreenExample.cs` como referência
2. Implemente os fluxos no seu `Program.cs`
3. Execute e teste cada fluxo (Cliente e Agente)
4. Adicione novas telas personalizadas conforme necessário

**Tudo está pronto para usar! 🚀**
