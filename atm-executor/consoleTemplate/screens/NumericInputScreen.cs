using atm_executor.consoleTemplate.abstractions;
using atm_executor.domain;

namespace atm_executor.consoleTemplate.screens
{
    /// <summary>
    /// Tela para captura de entrada numérica com validação
    /// Utiliza a classe Money do domain para trabalhar com valores monetários
    /// </summary>
    public class NumericInputScreen : BaseScreen
    {
        private string _prompt;
        private bool _allowNegative;
        private double? _minValue;
        private double? _maxValue;

        public NumericInputScreen(string title = "", string prompt = "Digite um valor: ", 
                                  bool allowNegative = false) 
            : base(title, "")
        {
            _prompt = prompt;
            _allowNegative = allowNegative;
            _minValue = null;
            _maxValue = null;
        }

        /// <summary>
        /// Define o valor mínimo permitido
        /// </summary>
        public void SetMinValue(double minValue)
        {
            _minValue = minValue;
        }

        /// <summary>
        /// Define o valor máximo permitido
        /// </summary>
        public void SetMaxValue(double maxValue)
        {
            _maxValue = maxValue;
        }

        /// <summary>
        /// Define a mensagem de prompt
        /// </summary>
        public void SetPrompt(string prompt)
        {
            _prompt = prompt;
        }

        /// <summary>
        /// Renderiza a tela de entrada numérica
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

            RenderFooter();
        }

        /// <summary>
        /// Captura e valida entrada numérica do usuário
        /// Retorna um objeto Money com o valor inserido
        /// </summary>
        public Money GetNumericInput()
        {
            Render();

            while (true)
            {
                Console.Write(_prompt);

                if (double.TryParse(Console.ReadLine(), out double value))
                {
                    // Valida se é negativo quando não permitido
                    if (value < 0 && !_allowNegative)
                    {
                        Console.WriteLine("Erro: Valores negativos não são permitidos. Tente novamente.\n");
                        continue;
                    }

                    // Valida valor mínimo
                    if (_minValue.HasValue && value < _minValue.Value)
                    {
                        Console.WriteLine($"Erro: O valor deve ser maior ou igual a {_minValue:F2}. Tente novamente.\n");
                        continue;
                    }

                    // Valida valor máximo
                    if (_maxValue.HasValue && value > _maxValue.Value)
                    {
                        Console.WriteLine($"Erro: O valor deve ser menor ou igual a {_maxValue:F2}. Tente novamente.\n");
                        continue;
                    }

                    return new Money(value);
                }
                else
                {
                    Console.WriteLine("Entrada inválida. Por favor, digite um número válido.\n");
                }
            }
        }

        /// <summary>
        /// Captura entrada numérica inteira (para quantidade de notas)
        /// </summary>
        public int GetIntegerInput()
        {
            Render();

            while (true)
            {
                Console.Write(_prompt);

                if (int.TryParse(Console.ReadLine(), out int value))
                {
                    // Valida se é negativo
                    if (value < 0)
                    {
                        Console.WriteLine("Erro: Valores negativos não são permitidos. Tente novamente.\n");
                        continue;
                    }

                    // Valida valor mínimo
                    if (_minValue.HasValue && value < _minValue.Value)
                    {
                        Console.WriteLine($"Erro: O valor deve ser maior ou igual a {(int)_minValue.Value}. Tente novamente.\n");
                        continue;
                    }

                    // Valida valor máximo
                    if (_maxValue.HasValue && value > _maxValue.Value)
                    {
                        Console.WriteLine($"Erro: O valor deve ser menor ou igual a {(int)_maxValue.Value}. Tente novamente.\n");
                        continue;
                    }

                    return value;
                }
                else
                {
                    Console.WriteLine("Entrada inválida. Por favor, digite um número inteiro válido.\n");
                }
            }
        }
    }
}
