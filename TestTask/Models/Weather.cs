using NPOI.SS.UserModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TestTask.Models
{
    /// <summary>
    /// Модель погоды.
    /// </summary>
    public class Weather
    {
        /// <summary>
        /// Идентифекатор.
        /// </summary>
        [Key]
        public Guid Id { get; set; }

        /// <summary>
        /// Дата и время.
        /// </summary>
        public DateTime DateTime { get; set; }

        /// <summary>
        /// Температура.
        /// </summary>
        public double Temperature { get; set; }

        /// <summary>
        /// Влажность.
        /// </summary>
        public double Humidity { get; set; }

        /// <summary>
        /// Точка росы.
        /// </summary>
        public double DewPoint { get; set; }

        /// <summary>
        /// Атмосферное давление.
        /// </summary>
        public int AtmosphPressure { get; set; }

        /// <summary>
        /// Направление ветра.
        /// </summary>
        public string? WindDirection { get; set; }

        /// <summary>
        /// Скорость ветра.
        /// </summary>
        public int WindSpeed { get; set; }

        /// <summary>
        /// Облачность (в %).
        /// </summary>
        public int CloudinessPercent { get; set; }

        /// <summary>
        /// Минимальная граница облачности.
        /// </summary>
        public int MinimalScopeCloudiness { get; set; }

        /// <summary>
        /// Горизонтальная видимость.
        /// </summary>
        public int HorizontalVision { get; set; }

        /// <summary>
        /// Природные явления.
        /// </summary>
        public string? WeatherCondition { get; set; }
        
    }
}
