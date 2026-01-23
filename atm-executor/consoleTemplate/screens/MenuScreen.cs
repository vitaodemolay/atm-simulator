using atm_executor.consoleTemplate.abstractions;

namespace atm_executor.consoleTemplate.screens
{
    /// <summary>
    /// Tela de menu que exibe opções numeradas para o usuário selecionar
    /// </summary>
    public class MenuScreen : BaseScreen
    {
        private readonly Dictionary<int, string> _menuOptions;
        private int _selectedOption;

        public MenuScreen(string title = "", string message = "") 
            : base(title, message)
        {
            _menuOptions = new Dictionary<int, string>();
            _selectedOption = -1;
        }

        /// <summary>
        /// Adiciona uma opção ao menu
        /// </summary>
        public void AddOption(int number, string description)
        {
            _menuOptions[number] = description;
        }

        /// <summary>
        /// Limpa todas as opções do menu
        /// </summary>
        public void ClearOptions()
        {
            _menuOptions.Clear();
        }

        /// <summary>
        /// Define as opções do menu a partir de um dicionário
        /// </summary>
        public void SetOptions(Dictionary<int, string> options)
        {
            _menuOptions.Clear();
            foreach (var kvp in options)
            {
                _menuOptions[kvp.Key] = kvp.Value;
            }
        }

        /// <summary>
        /// Renderiza o menu no console
        /// </summary>
        public override void Render()
        {
            StartRender();
            RenderHeader();

            if (!string.IsNullOrWhiteSpace(Message))
            {
                Console.WriteLine(Message);
                RenderEmptyLine();
            }

            // Renderiza as opções do menu
            var orderedOptions = _menuOptions.OrderBy(kvp => kvp.Key).ToList();
            foreach (var option in orderedOptions)
            {
                Console.WriteLine($"{option.Key} - {option.Value}");
            }

            RenderEmptyLine();
            RenderFooter();
        }

        /// <summary>
        /// Obtém a opção selecionada pelo usuário com validação
        /// </summary>
        public int GetSelectedOption()
        {
            while (true)
            {
                Console.Write("Selecione uma opção: ");
                
                if (int.TryParse(Console.ReadLine(), out int option))
                {
                    if (_menuOptions.ContainsKey(option))
                    {
                        _selectedOption = option;
                        return _selectedOption;
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
        /// Retorna a última opção selecionada
        /// </summary>
        public int GetLastSelectedOption()
        {
            return _selectedOption;
        }
    }
}
