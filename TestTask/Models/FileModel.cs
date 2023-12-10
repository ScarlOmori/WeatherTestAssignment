namespace TestTask.Models
{
    /// <summary>
    /// Модель загружаемого файла.
    /// </summary>
    public class FileModel
    {
        /// <summary>
        /// Идентифекатор.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Наименование файла.
        /// </summary>
        public string? Name { get; set; }

        /// <summary>
        /// Путь к файлу.
        /// </summary>
        public string? Path { get; set; }

        /// <summary>
        /// Прочитан и разобран ли файл.
        /// </summary>
        public bool IsReaded { get; set; }

    }
}
