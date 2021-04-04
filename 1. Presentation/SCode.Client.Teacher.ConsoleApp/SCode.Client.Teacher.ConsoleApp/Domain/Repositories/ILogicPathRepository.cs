namespace SCode.Client.Teacher.ConsoleApp.Domain.Repositories
{
    /// <summary>
    /// Almacena las rutas de los archivos con su Id lógico
    /// </summary>
    public interface ILogicPathRepository
    {
        /// <summary>
        /// Devuelve la ruta del Id solicitado
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Ruta o nulo</returns>
        string Get(int id);

        /// <summary>
        /// Añade una ruta si no existe
        /// y devuelve el Id creado
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        int Add(string path);

        /// <summary>
        /// Actualiza un valor dado su id
        /// </summary>
        /// <param name="id"></param>
        /// <param name="path"></param>
        void Update(int id, string path);
    }
}