# Arquitetura de Templates de Console - ATM Simulator

## Estrutura Criada

A seguinte arquitetura foi implementada para facilitar a criação de telas de console para o simulador de ATM:

```
atm-executor/
├── consoleTemplate/
│   ├── abstractions/
│   │   ├── IScreen.cs           ← Interface base para todas as telas
│   │   └── BaseScreen.cs        ← Classe abstrata com funcionalidades comuns
│   ├── screens/
│   │   ├── MenuScreen.cs        ← Tela de menu com opções numeradas
│   │   ├── NumericInputScreen.cs ← Tela para entrada numérica (valores e quantidades)
│   │   ├── OptionSelectionScreen.cs ← Tela de seleção de opções de saque
│   │   ├── DisplayScreen.cs     ← Tela genérica de exibição de informações
│   │   └── ConfirmationScreen.cs ← Tela de confirmação de operações
│   └── ScreenNavigator.cs       ← Gerenciador de navegação entre telas
```

## Componentes Implementados

### 1. **IScreen** (Interface)
- Define o contrato para todas as telas
- Métodos: `Render()`, `Clear()`

### 2. **BaseScreen** (Classe Abstrata)
- Fornece funcionalidades comuns a todas as telas:
  - Renderização de bordas e separadores
  - Centralização de texto
  - Aguardar ENTER do usuário
  - Limpeza de tela
- Propriedades: `Title`, `Message`, `ClearScreenBefore`

### 3. **MenuScreen**
- Exibe menu com opções numeradas
- Valida entrada do usuário
- Métodos:
  - `AddOption(int number, string description)`
  - `SetOptions(Dictionary<int, string> options)`
  - `GetSelectedOption()` - com validação automática

### 4. **NumericInputScreen**
- Captura entrada numérica com validação
- Suporta limites (mín/máx)
- Métodos:
  - `GetNumericInput()` - retorna Money
  - `GetIntegerInput()` - retorna int (para quantidade de notas)
  - `SetMinValue()`, `SetMaxValue()`

### 5. **OptionSelectionScreen**
- Exibe opções de saque com combinações de notas
- Métodos:
  - `SetOptions(IEnumerable<WithdrawalOption> options)`
  - `GetSelectedOption()` - retorna índice (0-based) ou -1 para voltar
  - `GetSelectedWithdrawalOption(int index)`
  - `HasOptions()` - verifica disponibilidade

### 6. **DisplayScreen**
- Exibe informações gerais e estado dos slots
- Métodos:
  - `DisplaySlots(IEnumerable<IMoneySlotView> slots)` - mostra estado ATM
  - `ShowMessage(string message)` - exibe mensagem simples
  - `GetSelectedOption()` - captura seleção com validação
  - `GetSelectedSlotOption(int slotCount)` - captura seleção de slot

### 7. **ConfirmationScreen**
- Exibe resultado de operações (sucesso/falha)
- Métodos:
  - `ShowResult(bool success, string details)` - resultado genérico
  - `ShowWithdrawalSuccess(double amount, string withdrawalDetails)`
  - `ShowRefillSuccess(double denominationValue, int quantity)`
  - `ShowErrorWithRetryOption(string errorMessage)`

### 8. **ScreenNavigator**
- Gerencia navegação entre telas
- Mantém histórico de telas para voltar
- Métodos factory para criar telas pré-configuradas:
  - `CreateMainMenu()` - menu principal
  - `CreateClientMenu()` - menu de cliente
  - `CreateAgentMenu()` - menu de agente
  - `CreateWithdrawalAmountScreen()` - entrada de valor
  - `CreateNotesQuantityScreen(double denominationValue)`
  - `CreateWithdrawalOptionScreen()`
  - `CreateSlotStatusScreen()`
  - `CreateWithdrawalErrorScreen(string errorMessage)`
  - `CreateWithdrawalConfirmationScreen()`
  - `CreateRefillConfirmationScreen()`

## Características

✅ **Separação de Responsabilidades**: Toda lógica de console isolada em `consoleTemplate`  
✅ **Reutilizabilidade**: Classes genéricas para múltiplos contextos  
✅ **Validação Automática**: Cada tela valida sua própria entrada  
✅ **Mensagens Dinâmicas**: Compatível com objetos do domain  
✅ **Navegação Controlada**: Sistema de histórico para voltar  
✅ **Extensível**: Novas telas podem ser criadas herdando de `BaseScreen`  
✅ **Type-safe**: Usa tipos do domain (`Money`, `WithdrawalOption`, etc)

## Como Usar

### Exemplo: Menu Principal
```csharp
var navigator = new ScreenNavigator();
var mainMenu = navigator.CreateMainMenu();
mainMenu.Render();
int choice = mainMenu.GetSelectedOption();
```

### Exemplo: Entrada de Valor de Saque
```csharp
var amountScreen = navigator.CreateWithdrawalAmountScreen();
amountScreen.Render();
Money amount = amountScreen.GetNumericInput();
```

### Exemplo: Exibição de Slots
```csharp
var slotsScreen = navigator.CreateSlotStatusScreen();
slotsScreen.DisplaySlots(atmMachine.GetMoneySlots());
int selectedSlot = slotsScreen.GetSelectedSlotOption(3);
```

### Exemplo: Confirmação de Saque
```csharp
var confirmScreen = navigator.CreateWithdrawalConfirmationScreen();
confirmScreen.ShowWithdrawalSuccess(200.0, "2x R$ 100.00");
```

## Próximas Etapas

Agora você pode integrar estas classes ao seu `Program.cs` para implementar os fluxos de:
1. **Cliente - Saque**: Menu Principal → Menu Cliente → Valor → Opções → Confirmação
2. **Agente - Reabastecimento**: Menu Principal → Menu Agente → Slots → Quantidade → Confirmação

Toda a interação com console já está abstraída! 🎉
