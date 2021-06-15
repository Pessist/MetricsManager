using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Threading.Tasks;
using MetricsAgent.DAL.Interfaces;
using MetricsAgent.DAL.Models;

namespace MetricsAgent.DAL.Repository
{
    public class CpuMetricsRepository : ICpuMetricsRepository
    {
        private const string ConnectionString = "Data Source=metrics.db;Version=3;Pooling=true;Max Pool Size=100;";
        // инжектируем соединение с базой данных в наш репозиторий через конструктор
        public void Create(CpuMetric item)
        {
            using var connection = new SQLiteConnection(ConnectionString);
            connection.Open();
            // создаем команду
            using var cmd = new SQLiteCommand(connection);
            // прописываем в команду SQL запрос на вставку данных
            cmd.CommandText = "INSERT INTO cpumetrics(value, time) VALUES(@value, @time)";

            // добавляем параметры в запрос из нашего объекта
            cmd.Parameters.AddWithValue("@value", item.Value);

            // в таблице будем хранить время в секундах, потому преобразуем перед записью в секунды
            // через свойство
            cmd.Parameters.AddWithValue("@time", item.Time.ToUnixTimeSeconds());
            // подготовка команды к выполнению
            cmd.Prepare();

            // выполнение команды
            cmd.ExecuteNonQuery();

        }

        public IList<CpuMetric> GetTimeByPeriod(DateTimeOffset fromTime, DateTimeOffset toTime)
        {
            var fromSeconds = fromTime.ToUnixTimeSeconds();
            var toSeconds = toTime.ToUnixTimeSeconds();

            using var connection = new SQLiteConnection(ConnectionString);
            connection.Open();
            using var cmd = new SQLiteCommand(connection);
            cmd.CommandText = "SELECT * FROM cpumetrics WHERE (time > @fromTime) and (time < @toTime)";
            cmd.Parameters.AddWithValue("@fromTime", fromSeconds);
            cmd.Parameters.AddWithValue("@toTime", toSeconds);
            cmd.Prepare();

            connection.Open();

            var returnList = new List<CpuMetric>();
            using var reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                returnList.Add(new CpuMetric()
                {
                    Id = reader.GetInt32(0),
                    Value = reader.GetInt32(1),
                    Time = DateTimeOffset.FromUnixTimeSeconds(reader.GetInt32(2))                    
                });
            }
            connection.Close();

            return returnList;
        }
    }
}
