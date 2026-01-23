using atm_executor.consoleTemplate.abstractions;

namespace atm_executor.consoleTemplate.screens
{
    /// <summary>
    /// Tela para exibição de resultado de operações (sucesso ou falha)
    /// </summary>
    public class ConfirmationScreen : BaseScreen
    {
        private bool _isSuccess;
        private string _successMessage;
        private string _failureMessage;
        private string _details;

        public ConfirmationScreen(string title = "") 
            : base(title)
        {
            _isSuccess = true;
            _successMessage = "Operação realizada com sucesso!";
            _failureMessage = "Operação falhou!";
            _details = "";
        }

        /// <summary>
        /// Define a mensagem de sucesso
        /// </summary>
        public void SetSuccessMessage(string message)
        {
            _successMessage = message;
        }

        /// <summary>
        /// Define a mensagem de falha
        /// </summary>
        public void SetFailureMessage(string message)
        {
            _failureMessage = message;
        }

        /// <summary>
        /// Define detalhes adicionais a serem exibidos
        /// </summary>
        public void SetDetails(string details)
        {
            _details = details;
        }

        /// <summary>
        /// Renderiza a tela de confirmação
        /// </summary>
        public override void Render()
        {
            StartRender();
            RenderHeader();
        }

        /// <summary>
        /// Exibe o resultado de uma operação (sucesso ou falha)
        /// </summary>
        public void ShowResult(bool success, string details = "")
        {
            _isSuccess = success;
            _details = details;

            StartRender();
            RenderHeader();

            if (_isSuccess)
            {
                Console.WriteLine(_successMessage);
            }
            else
            {
                Console.WriteLine(_failureMessage);
            }

            if (!string.IsNullOrWhiteSpace(_details))
            {
                RenderEmptyLine();
                Console.WriteLine(_details);
            }

            RenderEmptyLine();
            WaitForContinue();
            RenderFooter();
        }

        /// <summary>
        /// Exibe resultado de saque bem-sucedido com detalhes da operação
        /// </summary>
        public void ShowWithdrawalSuccess(double amount, string withdrawalDetails)
        {
            _isSuccess = true;
            StartRender();
            RenderHeader();

            Console.WriteLine("Saque Realizado com Sucesso!");
            RenderEmptyLine();
            Console.WriteLine($"Valor Sacado: R$ {amount:F2}");
            RenderEmptyLine();
            Console.WriteLine("Opção Selecionada:");
            Console.WriteLine(withdrawalDetails);
            RenderEmptyLine();

            WaitForContinue();
            RenderFooter();
        }

        /// <summary>
        /// Exibe resultado de reabastecimento bem-sucedido
        /// </summary>
        public void ShowRefillSuccess(double denominationValue, int quantity)
        {
            _isSuccess = true;
            StartRender();
            RenderHeader();

            Console.WriteLine("Reabastecimento Realizado com Sucesso!");
            RenderEmptyLine();
            Console.WriteLine($"Slot: R$ {denominationValue:F2}");
            Console.WriteLine($"Quantidade Adicionada: {quantity} notas");
            RenderEmptyLine();

            WaitForContinue();
            RenderFooter();
        }

        /// <summary>
        /// Exibe mensagem de falha com opção para retry
        /// </summary>
        public void ShowErrorWithRetryOption(string errorMessage)
        {
            _isSuccess = false;
            StartRender();
            RenderHeader();

            Console.WriteLine(errorMessage);
            RenderEmptyLine();

            RenderFooter();
        }

        /// <summary>
        /// Retorna se a última operação foi bem-sucedida
        /// </summary>
        public bool IsSuccess()
        {
            return _isSuccess;
        }
    }
}
