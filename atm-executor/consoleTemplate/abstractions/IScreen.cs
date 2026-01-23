namespace atm_executor.consoleTemplate.abstractions
{
    /// <summary>
    /// Interface que define o contrato para todas as telas do console
    /// </summary>
    public interface IScreen
    {
        /// <summary>
        /// Renderiza a tela no console
        /// </summary>
        void Render();

        /// <summary>
        /// Limpa a tela do console
        /// </summary>
        void Clear();
    }
}
