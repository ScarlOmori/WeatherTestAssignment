namespace TestTask.Models
{
    /// <summary>
    /// Модель для страницы с таблицей.
    /// </summary>
    public class IndexViewModel
    {
        /// <summary>
        /// Погодные явления к отображению на странице.
        /// </summary>
        public IEnumerable<Weather> Weathers { get; set; }

        /// <summary>
        /// Описание текущей страницы в таблице.
        /// </summary>
        public TablePagingViewModel PageViewModel { get; set; }
    }
}
