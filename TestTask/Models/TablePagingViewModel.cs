namespace TestTask.Models
{
    /// <summary>
    /// Модель описывающая страницу в таблице.
    /// </summary>
    public class TablePagingViewModel
    {
        /// <summary>
        /// Номер текущей страницы.
        /// </summary>
        public int PageNumber { get; private set; }

        /// <summary>
        /// Всего страниц.
        /// </summary>
        public int TotalPages { get; private set; }

        /// <summary>
        /// Задание количества страниц и текущей страницы.
        /// </summary>
        /// <param name="count">Всего записей.</param>
        /// <param name="pageNumber">Текущая страница таблицы.</param>
        /// <param name="pageSize">Количество элементов на странице таблицы.</param>
        public TablePagingViewModel(int count, int pageNumber, int pageSize)
        {
            PageNumber = pageNumber;
            TotalPages = (int)Math.Ceiling(count / (double)pageSize);
        }

        /// <summary>
        /// Имеется ли предыдущая страница таблицы.
        /// </summary>
        public bool HasPreviousPage
        {
            get { return (PageNumber > 1); }
        }

        /// <summary>
        /// Имеется ли следующая страница таблицы.
        /// </summary>
        public bool HasNextPage
        {
            get { return (PageNumber < TotalPages); }
        }
    }
}
