using NPOI.SS.UserModel;
using System.Text.RegularExpressions;
using TestTask.Models;

namespace TestTask.Services
{
    /// <summary>
    /// Предоставляет функционал для выполнения загрузки погоды.
    /// </summary>
    public class WeatherService
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
        /// Конструктор с получением необходимых экземпляров через DI.
        /// </summary>
        /// <param name="context">Экземпляр контекста базы данных.</param>
        /// <param name="appEnvironment">Экземпляр среды приложения.</param>
        public WeatherService(ApplicationContext context, IWebHostEnvironment appEnvironment)
        {
            _context = context;
            _appEnvironment = appEnvironment;
        }

        /// <summary>
        /// Загружает данные из архива погоды в базу данных.
        /// </summary>
        /// <returns>True - в случае удачной загрузки, в противном случае false</returns>
        public async Task<bool> GetExcelDataToDb()
        {
            foreach (var file in _context.Files.ToList())
            {
                if (!file.IsReaded)
                {
                    using (var transaction = _context.Database.BeginTransaction())
                    {
                        try
                        {
                            var filePath = _appEnvironment.WebRootPath + file.Path;
                            var fs = new FileStream(filePath, FileMode.Open, FileAccess.Read);
                            var weather = new Weather();
                            var workbook = WorkbookFactory.Create(fs);
                            var _doubleCellStyle = workbook.CreateCellStyle();

                            _doubleCellStyle.DataFormat = workbook.CreateDataFormat().GetFormat("#,##0.###");

                            if (filePath.IndexOf(".xlsx") > 0)
                            {
                                for (var i = 0; i <= 11; i++)
                                {
                                    ISheet sheet = workbook.GetSheetAt(i);
                                    DataFormatter formatter = new DataFormatter();
                                    int rowCount = sheet.LastRowNum;

                                    for (var k = 1; k <= rowCount; k++)
                                    {
                                        IRow currentRow = sheet.GetRow(k);

                                        var cellValue = currentRow.GetCell(0).StringCellValue.Trim();
                                        var match = Regex.Match(cellValue, @"\d\d[.]\d\d[.]\d\d\d\d");

                                        if (match.Success)
                                        {
                                            for (var j = 0; j <= currentRow.Cells.Count; j++)
                                            {
                                                if (j == 2 || j == 3 || j == 4)
                                                {
                                                    currentRow.GetCell(j).CellStyle = _doubleCellStyle;
                                                }
                                            }

                                            weather.Id = Guid.NewGuid();

                                            DateTime.TryParse(currentRow.GetCell(0).StringCellValue, out DateTime dateCell);
                                            var timeCell = currentRow.GetCell(1).StringCellValue.Split(':');

                                            weather.DateTime = new DateTime(dateCell.Year,
                                                dateCell.Month,
                                                dateCell.Day,
                                                Convert.ToInt32(timeCell[0]),
                                                Convert.ToInt32(timeCell[1]),
                                                00);

                                            weather.Temperature = ValidateNumericCell(formatter, currentRow, 2);
                                            weather.Humidity = ValidateNumericCell(formatter, currentRow, 3);
                                            weather.DewPoint = ValidateNumericCell(formatter, currentRow, 4);
                                            weather.AtmosphPressure = (int)ValidateNumericCell(formatter, currentRow, 5);
                                            weather.WindDirection = formatter.FormatCellValue(currentRow.GetCell(6));
                                            weather.WindSpeed = (int)ValidateNumericCell(formatter, currentRow, 7);
                                            weather.CloudinessPercent = (int)ValidateNumericCell(formatter, currentRow, 8);
                                            weather.MinimalScopeCloudiness = (int)ValidateNumericCell(formatter, currentRow, 9);
                                            weather.HorizontalVision = (int)ValidateNumericCell(formatter, currentRow, 10);
                                            weather.WeatherCondition = formatter.FormatCellValue(currentRow.GetCell(11));

                                            _context.ChangeTracker.AutoDetectChangesEnabled = false;

                                            await _context.Weathers.AddAsync(weather);
                                            await _context.SaveChangesAsync();
                                        }
                                    }
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            await transaction.RollbackAsync();
                            _context.Files.Remove(file);
                            await _context.SaveChangesAsync();
                            Console.Out.WriteLine(ex.Message);

                            return false;
                        }
                        finally
                        {
                            _context.ChangeTracker.AutoDetectChangesEnabled = true;
                        }

                        await transaction.CommitAsync();
                    }
                    file.IsReaded = true;
                }
                _context.SaveChanges();
            }

            return true;

            //Валидация строки состоящей из чисел в архиве погоды.
            static double ValidateNumericCell(DataFormatter formatter, IRow currentRow, int i)
            {
                return currentRow.GetCell(i).CellType == CellType.String ? 0.0 : Convert.ToDouble(formatter.FormatCellValue(currentRow.GetCell(i)));
            }
        }
    }
}
