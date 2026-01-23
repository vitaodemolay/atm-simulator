using atm_executor.consoleTemplate.abstractions;
using atm_executor.domain.contracts;

namespace atm_executor.consoleTemplate.screens
{
    /// <summary>
    /// Tela para exibição de informações como estado dos slots e mensagens
    /// </summary>
    public class DisplayScreen : BaseScreen
    {
        private string _content;
        private bool _hasContinuePrompt;
        private Dictionary<int, string> _menuOptions;

        public DisplayScreen(string title = "", string content = "", bool hasContinuePrompt = false) 
            : base(title)
        {
            _content = content;
            _hasContinuePrompt = hasContinuePrompt;
            _menuOptions = new Dictionary<int, string>();
        }

        /// <summary>
        /// Define o conteúdo a ser exibido
        /// </summary>
        public void SetContent(string content)
        {
            _content = content;
        }

        /// <summary>
        /// Define a mensagem principal da tela
        /// </summary>
        public void SetMessage(string message)
        {
            Message = message;
        }

        /// <summary>
        /// Define se deve exibir prompt para continuar
        /// </summary>
        public void SetHasContinuePrompt(bool hasContinuePrompt)
        {
            _hasContinuePrompt = hasContinuePrompt;
        }

        /// <summary>
        /// Adiciona uma opção ao menu (para casos onde exibe informações com opções)
        /// </summary>
        public void AddMenuOption(int number, string description)
        {
            _menuOptions[number] = description;
        }

        /// <summary>
        /// Limpa as opções de menu
        /// </summary>
        public void ClearMenuOptions()
        {
            _menuOptions.Clear();
        }

        /// <summary>
        /// Exibe o estado dos slots da máquina ATM
        /// </summary>
        public void DisplaySlots(IEnumerable<IMoneySlotView> slots)
        {
            StartRender();
            RenderHeader();

            if (!string.IsNullOrWhiteSpace(Message))
            {
                Console.WriteLine(Message);
                RenderEmptyLine();
            }

            Console.WriteLine("Estado Atual dos Slots:");
            RenderEmptyLine();

            int slotNumber = 1;
            foreach (var slot in slots)
            {
                Console.WriteLine($"{slotNumber} - {slot}");
                slotNumber++;
            }

            RenderEmptyLine();

            // Exibe as opções de menu se houver
            if (_menuOptions.Count > 0)
            {
                var orderedOptions = _menuOptions.OrderBy(kvp => kvp.Key).ToList();
                foreach (var option in orderedOptions)
                {
                    Console.WriteLine($"{option.Key} - {option.Value}");
                }
                RenderEmptyLine();
            }

            RenderFooter();
        }

        /// <summary>
        /// Renderiza a tela de exibição genérica
        /// </summary>
        public override void Render()
        {
            StartRender();
            RenderHeader();

            if (!string.IsNullOrWhiteSpace(_content))
            {
                Console.WriteLine(_content);
                RenderEmptyLine();
            }

            if (!string.IsNullOrWhiteSpace(Message))
            {
                Console.WriteLine(Message);
                RenderEmptyLine();
            }

            // Exibe as opções de menu se houver
            if (_menuOptions.Count > 0)
            {
                var orderedOptions = _menuOptions.OrderBy(kvp => kvp.Key).ToList();
                foreach (var option in orderedOptions)
                {
                    Console.WriteLine($"{option.Key} - {option.Value}");
                }
                RenderEmptyLine();
            }

            RenderFooter();
        }

        /// <summary>
        /// Mostra uma mensagem simples
        /// </summary>
        public void ShowMessage(string message)
        {
            StartRender();
            RenderHeader();
            Console.WriteLine(message);
            RenderEmptyLine();

            if (_hasContinuePrompt)
            {
                WaitForContinue();
            }

            RenderFooter();
        }

        /// <summary>
        /// Aguarda ENTER para continuar
        /// </summary>
        public void WaitForUserInput()
        {
            WaitForContinue();
        }

        /// <summary>
        /// Obtém a opção selecionada pelo usuário (se tiver menu options)
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
                        return option;
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
        /// Obtém a opção selecionada para slot (1-based, retorna -1 para voltar)
        /// </summary>
        public int GetSelectedSlotOption(int slotCount)
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
                    if (option > 0 && option <= slotCount)
                    {
                        return option; // Retorna 1-based
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
    }
}
