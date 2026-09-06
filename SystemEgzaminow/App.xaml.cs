using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using QuestPDF.Infrastructure;
using System.IO;
using System.Text.Json;
using System.Windows;
using SystemEgzaminow.Data.Context;

namespace SystemEgzaminow
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public App()
        {
            QuestPDF.Settings.License = LicenseType.Community;
        }

        public static ServiceProvider ServiceProvider { get; private set; }

        protected override void OnStartup(StartupEventArgs e)
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

            var services = new ServiceCollection();

            services.AddDbContext<AppDbContext>(options =>
                options.UseSqlServer(
                   connectionString));

            ServiceProvider = services.BuildServiceProvider();

            base.OnStartup(e);
        }
    }
}