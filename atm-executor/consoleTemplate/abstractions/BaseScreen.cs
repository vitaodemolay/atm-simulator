namespace atm_executor.consoleTemplate.abstractions
{
    /// <summary>
    /// Classe abstrata base para todas as telas do console
    /// Fornece métodos auxiliares para renderização comum
    /// </summary>
    public abstract class BaseScreen : IScreen
    {
        protected string Title { get; set; }
        protected string Message { get; set; }
        protected bool ClearScreenBefore { get; set; }

        private const int ConsoleWidth = 60;
        private const char BorderChar = '=';

        protected BaseScreen(string title = "", string message = "", bool clearScreenBefore = true)
        {
            Title = title;
            Message = message;
            ClearScreenBefore = clearScreenBefore;
        }

        /// <summary>
        /// Implementação abstrata que cada tela concreta deve fornecer
        /// </summary>
        public abstract void Render();

        /// <summary>
        /// Limpa a tela do console
        /// </summary>
        public virtual void Clear()
        {
            Console.Clear();
        }

        /// <summary>
        /// Renderiza o cabeçalho com título e bordas
        /// </summary>
        protected void RenderHeader()
        {
            RenderBorder();
            if (!string.IsNullOrWhiteSpace(Title))
            {
                Console.WriteLine(CenterText(Title));
                RenderBorder();
            }
        }

        /// <summary>
        /// Renderiza uma linha de separação com caracteres
        /// </summary>
        protected void RenderBorder()
        {
            Console.WriteLine(new string(BorderChar, ConsoleWidth));
        }

        /// <summary>
        /// Renderiza uma linha em branco
        /// </summary>
        protected void RenderEmptyLine()
        {
            Console.WriteLine();
        }

        /// <summary>
        /// Renderiza o rodapé
        /// </summary>
        protected void RenderFooter()
        {
            RenderBorder();
        }

        /// <summary>
        /// Centraliza um texto na largura do console
        /// </summary>
        protected string CenterText(string text)
        {
            int spaces = Math.Max(0, (ConsoleWidth - text.Length) / 2);
            return new string(' ', spaces) + text;
        }

        /// <summary>
        /// Renderiza uma mensagem e aguarda o usuário pressionar ENTER
        /// </summary>
        protected void WaitForContinue()
        {
            Console.WriteLine("\nPressione ENTER para continuar...");
            Console.ReadLine();
        }

        /// <summary>
        /// Inicia a renderização da tela (limpa se necessário)
        /// </summary>
        protected void StartRender()
        {
            if (ClearScreenBefore)
                Clear();
        }
    }
}
