using Microsoft.EntityFrameworkCore;
using System.IO;
using System.Text.Json;
using SystemEgzaminow.Data.Context;

namespace SystemEgzaminow.WPF
{
    public static class DbContextHelper
    {
        public static AppDbContext CreateDbContext()
        {
            string path = Path.Combine(
            AppContext.BaseDirectory,
            "databaseSettings.json");

            if (!File.Exists(path))
            {
                throw new FileNotFoundException(
                    "Nie znaleziono pliku databaseSettings.json.",
                    path);
            }

            string json = File.ReadAllText(path);

            using JsonDocument document = JsonDocument.Parse(json);

            string? connectionString = document.RootElement
                .GetProperty("ConnectionString")
                .GetString();

            if (string.IsNullOrWhiteSpace(connectionString))
            {
                throw new InvalidOperationException(
                    "Brak ConnectionString w databaseSettings.json.");
            }

            var optionsBuilder = new DbContextOptionsBuilder<AppDbContext>();
            optionsBuilder.UseSqlServer(connectionString);

            return new AppDbContext(optionsBuilder.Options);
        }
    }
}