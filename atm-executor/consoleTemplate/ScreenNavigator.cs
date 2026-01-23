using atm_executor.consoleTemplate.abstractions;
using atm_executor.consoleTemplate.screens;

namespace atm_executor.consoleTemplate
{
    /// <summary>
    /// Classe responsável pela navegação entre telas
    /// Gerencia a tela atual e fornece métodos para navegação
    /// </summary>
    public class ScreenNavigator
    {
        private IScreen? _currentScreen;
        private Stack<IScreen> _screenHistory;

        public ScreenNavigator()
        {
            _screenHistory = new Stack<IScreen>();
        }

        /// <summary>
        /// Obtém a tela atualmente ativa
        /// </summary>
        public IScreen? GetCurrentScreen()
        {
            return _currentScreen;
        }

        /// <summary>
        /// Navega para uma nova tela
        /// </summary>
        public void NavigateTo(IScreen screen)
        {
            if (_currentScreen != null)
            {
                _screenHistory.Push(_currentScreen);
            }
            _currentScreen = screen;
        }

        /// <summary>
        /// Volta para a tela anterior no histórico
        /// </summary>
        public bool GoBack()
        {
            if (_screenHistory.Count > 0)
            {
                _currentScreen = _screenHistory.Pop();
                return true;
            }
            return false;
        }

        /// <summary>
        /// Limpa o histórico de navegação
        /// </summary>
        public void ClearHistory()
        {
            _screenHistory.Clear();
        }

        /// <summary>
        /// Retorna a profundidade do histórico
        /// </summary>
        public int GetHistoryDepth()
        {
            return _screenHistory.Count;
        }

        /// <summary>
        /// Cria e retorna o menu principal
        /// </summary>
        public MenuScreen CreateMainMenu()
        {
            var mainMenu = new MenuScreen("== MENU PRINCIPAL ==");
            mainMenu.AddOption(1, "Cliente - Realizar Saque");
            mainMenu.AddOption(2, "Agente - Reabastecimento");
            mainMenu.AddOption(0, "Sair");
            return mainMenu;
        }

        /// <summary>
        /// Cria e retorna o menu de cliente
        /// </summary>
        public MenuScreen CreateClientMenu()
        {
            var clientMenu = new MenuScreen("== MENU CLIENTE ==");
            clientMenu.AddOption(1, "Realizar Saque");
            clientMenu.AddOption(0, "Voltar ao Menu Principal");
            return clientMenu;
        }

        /// <summary>
        /// Cria e retorna o menu de agente
        /// </summary>
        public MenuScreen CreateAgentMenu()
        {
            var agentMenu = new MenuScreen("== MENU AGENTE ==");
            agentMenu.AddOption(1, "Realizar Reabastecimento");
            agentMenu.AddOption(0, "Voltar ao Menu Principal");
            return agentMenu;
        }

        /// <summary>
        /// Cria tela para entrada de valor de saque
        /// </summary>
        public NumericInputScreen CreateWithdrawalAmountScreen()
        {
            var screen = new NumericInputScreen(
                "== VALOR DE SAQUE ==",
                "Digite o valor que deseja sacar: ",
                allowNegative: false
            );
            screen.SetMinValue(0.01);
            return screen;
        }

        /// <summary>
        /// Cria tela para entrada de quantidade de notas
        /// </summary>
        public NumericInputScreen CreateNotesQuantityScreen(double denominationValue)
        {
            var screen = new NumericInputScreen(
                "== REABASTECIMENTO ==",
                $"Quantas notas de R$ {denominationValue:F2} deseja adicionar? ",
                allowNegative: false
            );
            screen.SetMinValue(1);
            return screen;
        }

        /// <summary>
        /// Cria tela para seleção de opções de saque
        /// </summary>
        public OptionSelectionScreen CreateWithdrawalOptionScreen()
        {
            var screen = new OptionSelectionScreen("== SELEÇÃO DE OPÇÃO ==");
            screen.SetMessage("Escolha uma das opções abaixo:");
            return screen;
        }

        /// <summary>
        /// Cria tela para exibição do estado dos slots
        /// </summary>
        public DisplayScreen CreateSlotStatusScreen()
        {
            var screen = new DisplayScreen("== ESTADO DOS SLOTS ==");
            screen.SetMessage("Selecione um slot para reabastecer:");
            return screen;
        }

        /// <summary>
        /// Cria tela para mensagem de erro de saque
        /// </summary>
        public DisplayScreen CreateWithdrawalErrorScreen(string errorMessage)
        {
            var screen = new DisplayScreen("== ERRO - SAQUE INDISPONÍVEL ==");
            screen.SetContent(errorMessage);
            screen.AddMenuOption(1, "Tentar com outro valor");
            screen.AddMenuOption(0, "Voltar ao Menu Anterior");
            return screen;
        }

        /// <summary>
        /// Cria tela de confirmação de saque
        /// </summary>
        public ConfirmationScreen CreateWithdrawalConfirmationScreen()
        {
            var screen = new ConfirmationScreen("== CONFIRMAÇÃO DE SAQUE ==");
            screen.SetSuccessMessage("Saque Realizado com Sucesso!");
            return screen;
        }

        /// <summary>
        /// Cria tela de confirmação de reabastecimento
        /// </summary>
        public ConfirmationScreen CreateRefillConfirmationScreen()
        {
            var screen = new ConfirmationScreen("== CONFIRMAÇÃO DE REABASTECIMENTO ==");
            screen.SetSuccessMessage("Reabastecimento Realizado com Sucesso!");
            return screen;
        }
    }
}
