using atm_executor.consoleTemplate.abstractions;
using atm_executor.domain;

namespace atm_executor.consoleTemplate.screens
{
    /// <summary>
    /// Tela para seleção de opções de saque
    /// Exibe as opções de combinação de notas e permite o usuário escolher uma
    /// </summary>
    public class OptionSelectionScreen : BaseScreen
    {
        private List<WithdrawalOption> _options;
        private string _errorMessage;

        public OptionSelectionScreen(string title = "") 
            : base(title)
        {
            _options = new List<WithdrawalOption>();
            _errorMessage = "";
        }

        /// <summary>
        /// Define as opções de saque disponíveis
        /// </summary>
        public void SetOptions(IEnumerable<WithdrawalOption> options)
        {
            _options = options.ToList();
        }

        /// <summary>
        /// Define uma mensagem de erro a ser exibida
        /// </summary>
        public void SetErrorMessage(string errorMessage)
        {
            _errorMessage = errorMessage;
        }

        /// <summary>
        /// Define a mensagem principal da tela
        /// </summary>
        public void SetMessage(string message)
        {
            Message = message;
        }

        /// <summary>
        /// Renderiza a tela de seleção de opções
        /// </summary>
        public override void Render()
        {
            StartRender();
            RenderHeader();

            if (!string.IsNullOrWhiteSpace(_errorMessage))
            {
                Console.WriteLine(_errorMessage);
                RenderEmptyLine();
            }

            if (!string.IsNullOrWhiteSpace(Message))
            {
                Console.WriteLine(Message);
                RenderEmptyLine();
            }

            // Renderiza as opções
            for (int i = 0; i < _options.Count; i++)
            {
                Console.WriteLine($"{i + 1} - {_options[i]}");
            }

            Console.WriteLine("0 - Voltar ao Menu Anterior");

            RenderEmptyLine();
            RenderFooter();
        }

        /// <summary>
        /// Obtém a opção selecionada pelo usuário (retorna índice 0-based, ou -1 para voltar)
        /// </summary>
        public int GetSelectedOption()
        {
            while (true)
            {
                Console.Write("Selecione uma opção: ");

                if (int.TryParse(Console.ReadLine(), out int option))
                {
                    // Opção 0: voltar
                    if (option == 0)
                    {
                        return -1;
                    }

                    // Valida se a opção está dentro do intervalo
                    if (option > 0 && option <= _options.Count)
                    {
                        return option - 1; // Retorna índice 0-based
                    }
                    else
                    {
                        Console.WriteLine("Opção inválida. Tente novamente.\n");
                    }
                }
                else
                {
                    Console.WriteLine("Entrada inválida. Por favor, digite um número.\n");
                }
            }
        }

        /// <summary>
        /// Retorna a opção selecionada como objeto WithdrawalOption
        /// </summary>
        public WithdrawalOption GetSelectedWithdrawalOption(int index)
        {
            if (index >= 0 && index < _options.Count)
            {
                return _options[index];
            }
            throw new IndexOutOfRangeException("Índice de opção inválido");
        }

        /// <summary>
        /// Retorna todas as opções disponíveis
        /// </summary>
        public List<WithdrawalOption> GetAllOptions()
        {
            return _options;
        }

        /// <summary>
        /// Verifica se há opções disponíveis
        /// </summary>
        public bool HasOptions()
        {
            return _options.Count > 0;
        }
    }
}
