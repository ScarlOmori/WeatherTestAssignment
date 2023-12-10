using Microsoft.AspNetCore.Mvc;

namespace TestTask.Controllers
{
    /// <summary>
    /// Контроллер главной страницы.
    /// </summary>
    public class HomeController : Controller
    {
        /// <summary>
        /// Отображает главную страницу.
        /// </summary>
        /// <param name="isError">Флаг, есть ли ошибка.</param>
        /// <returns>Отображение главной страницы.</returns>
        public IActionResult Index(bool isError = false)
        {
            if (isError)
            {
                ViewBag.Message = "В процессе загрузки архива произошла ошибка.\nПроверьте архив на соответствие необходимой структуре документа.";
            }

            return View();
        }
    }
}