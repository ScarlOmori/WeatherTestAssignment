using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TestTask.Models;
using TestTask.Services;

namespace TestTask.Controllers
{
    /// <summary>
    /// Выполняет функционал по загрузке и отображению погоды.
    /// </summary>
    public class WeatherController : Controller
    {
        /// <summary>
        /// Контекст базы данных.
        /// </summary>
        private readonly ApplicationContext _context;

        /// <summary>
        /// Среда приложения.
        /// </summary>
        private readonly IWebHostEnvironment _appEnvironment;

        /// <summary>
        /// Предоставляет функционал для выполнения загрузки погоды.
        /// </summary>
        private readonly WeatherService _weatherService;

        /// <summary>
        /// Конструктор с получением необходимых экземпляров через DI.
        /// </summary>
        /// <param name="context">Экземпляр контекста базы данных.</param>
        /// <param name="appEnvironment">Экземпляр среды приложения.</param>
        /// <param name="weatherService">Экземпляр сервиса с функционалом для загрузки погоды.</param>
        public WeatherController(ApplicationContext context, IWebHostEnvironment appEnvironment, WeatherService weatherService)
        {
            _context = context;
            _appEnvironment = appEnvironment;
            _weatherService = weatherService;
        }

        /// <summary>
        /// Отображает страницу загрузки архива погоды.
        /// </summary>
        /// <returns>Страница загрузки архива погоды.</returns>
        public IActionResult Upload()
        {
            return View(_context.Files.ToList());
        }

        /// <summary>
        /// Отображает страницу просмотра архивов погоды.
        /// </summary>
        /// <param name="sortOrder">Столбец сортировки таблицы погоды.</param>
        /// <returns>Страница просмотра архива погоды.</returns>
        [HttpGet]
        public async Task<IActionResult> Watch(int? yearSearch, int? monthSearch, int tablePage = 1, SortState sortOrder = SortState.TemperatureDesc)
        {
            var weathers = _context.Weathers.AsNoTracking();

            ViewBag.CurrentSort = sortOrder;
            ViewBag.CurrentYearSearch = yearSearch;
            ViewBag.CurrentMonthSearch = monthSearch;

            ViewData["TemperatureSort"] = sortOrder == SortState.TemperatureAsc ? SortState.TemperatureDesc : SortState.TemperatureAsc;
            ViewData["HumiditySort"] = sortOrder == SortState.HumidityAsc ? SortState.HumidityDesc : SortState.HumidityAsc;
            ViewData["DateSort"] = sortOrder == SortState.DateAsc ? SortState.DateDesc : SortState.DateAsc;
            ViewData["TimeSort"] = sortOrder == SortState.TimeAsc ? SortState.TimeDesc : SortState.TimeAsc;
            ViewData["DewPointSort"] = sortOrder == SortState.DewPointAsc ? SortState.DewPointDesc : SortState.DewPointAsc;
            ViewData["AtmospherePressureSort"] = sortOrder == SortState.AtmospherePressureAsc ? SortState.AtmospherePressureDesc : SortState.AtmospherePressureAsc;
            ViewData["WindSpeedSort"] = sortOrder == SortState.WindSpeedAsc ? SortState.WindSpeedDesc : SortState.WindSpeedAsc;
            ViewData["CloudinessSort"] = sortOrder == SortState.CloudinessAsc ? SortState.CloudinessDesc : SortState.CloudinessAsc;
            ViewData["MinimalScopeCloudinessSort"] = sortOrder == SortState.MinimalScopeCloudinessAsc ? SortState.MinimalScopeCloudinessDesc : SortState.MinimalScopeCloudinessAsc;
            ViewData["HorizontalVisionSort"] = sortOrder == SortState.HorizontalVisionAsc ? SortState.HorizontalVisionDesc : SortState.HorizontalVisionAsc;

            if (yearSearch != null)
            {
                weathers = weathers
                .Where(w => w.DateTime.Year == yearSearch).AsNoTracking();
            }

            if (monthSearch != null)
            {
                weathers = weathers
                .Where(w => w.DateTime.Month == monthSearch).AsNoTracking();
            }

            weathers = sortOrder switch
            {
                SortState.TemperatureAsc => weathers.OrderBy(w => Convert.ToDouble(w.Temperature)),
                SortState.TemperatureDesc => weathers.OrderByDescending(w => Convert.ToDouble(w.Temperature)),
                SortState.HumidityDesc => weathers.OrderByDescending(w => Convert.ToInt32(w.Humidity)),
                SortState.HumidityAsc => weathers.OrderByDescending(w => Convert.ToInt32(w.Humidity)),
                SortState.DateAsc => weathers.OrderBy(w => w.DateTime.Date),
                SortState.DateDesc => weathers.OrderByDescending(w => w.DateTime.Date),
                SortState.TimeAsc => weathers.OrderBy(w => w.DateTime.TimeOfDay),
                SortState.TimeDesc => weathers.OrderByDescending(w => w.DateTime.TimeOfDay),
                SortState.DewPointAsc => weathers.OrderBy(w => Convert.ToDouble(w.DewPoint)),
                SortState.DewPointDesc => weathers.OrderByDescending(w => Convert.ToDouble(w.DewPoint)),
                SortState.AtmospherePressureAsc => weathers.OrderBy(w => Convert.ToInt32(w.AtmosphPressure)),
                SortState.AtmospherePressureDesc => weathers.OrderByDescending(w => Convert.ToInt32(w.AtmosphPressure)),
                SortState.WindSpeedAsc => weathers.OrderBy(w => Convert.ToInt32(w.WindSpeed)),
                SortState.WindSpeedDesc => weathers.OrderByDescending(w => Convert.ToInt32(w.WindSpeed)),
                SortState.CloudinessAsc => weathers.OrderBy(w => Convert.ToInt32(w.CloudinessPercent)),
                SortState.CloudinessDesc => weathers.OrderByDescending(w => Convert.ToInt32(w.CloudinessPercent)),
                SortState.MinimalScopeCloudinessAsc => weathers.OrderBy(w => Convert.ToInt32(w.MinimalScopeCloudiness)),
                SortState.MinimalScopeCloudinessDesc => weathers.OrderByDescending(w => Convert.ToInt32(w.MinimalScopeCloudiness)),
                SortState.HorizontalVisionAsc => weathers.OrderBy(w => Convert.ToInt32(w.HorizontalVision)),
                SortState.HorizontalVisionDesc => weathers.OrderByDescending(w => Convert.ToInt32(w.HorizontalVision)),
                _ => weathers.OrderBy(w => w.DateTime.Date)
            };

            const int pageSize = 18;

            var count = await weathers.CountAsync();
            var items = await weathers.Skip((tablePage - 1) * pageSize).Take(pageSize).ToListAsync();

            var pageViewModel = new TablePagingViewModel(count, tablePage, pageSize);
            var viewModel = new IndexViewModel
            {
                PageViewModel = pageViewModel,
                Weathers = items
            };

            return View(viewModel);
        }

        /// <summary>
        /// Загрузка файла на сервер и создание соответствующих записей в базе данных.
        /// </summary>
        /// <param name="uploads">Коллекция файлов.</param>
        /// <returns>В случае удачной загрузки возвращает страницу загрузки архивов,
        /// в случае получения ошибок в процессе загрузки открывает главную страницу с ошибкой.</returns>
        [HttpPost]
        public async Task<IActionResult> AddFile(IFormFileCollection uploads)
        {
            foreach (var uploadedFile in uploads)
            {
                var path = "/Files/" + uploadedFile.FileName;

                using (var fileStream = new FileStream(_appEnvironment.WebRootPath + path, FileMode.Create))
                {
                    await uploadedFile.CopyToAsync(fileStream);
                }

                var file = new FileModel { Name = uploadedFile.FileName, Path = path };

                _context.Files.Add(file);
            }
            _context.SaveChanges();

            if (await _weatherService.GetExcelDataToDb())
            {
                return RedirectToAction("Upload");
            }
            else
            {
                return RedirectToAction("Index", "Home", new { isError = true });
            }
        }
    }
}
