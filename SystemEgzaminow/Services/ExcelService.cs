using ClosedXML.Excel;
using SystemEgzaminow.Core.DTO;
using SystemEgzaminow.Core.Models;

namespace SystemEgzaminow.WPF.Services
{
    public class ExcelService
    {
        public void ExportUsers(List<Uzytkownik> users, string filePath)
        {
            using var workbook = new XLWorkbook();

            var worksheet =
                workbook.Worksheets.Add("Użytkownicy");

            // Nagłówki
            worksheet.Cell(1, 1).Value = "Id";
            worksheet.Cell(1, 2).Value = "Login";
            worksheet.Cell(1, 3).Value = "Imię";
            worksheet.Cell(1, 4).Value = "Nazwisko";
            worksheet.Cell(1, 5).Value = "Rola";
            worksheet.Cell(1, 6).Value = "Klasa";
            worksheet.Cell(1, 7).Value = "Rok szkolny";

            int row = 2;

            foreach (var user in users)
            {
                worksheet.Cell(row, 1).Value = user.Id;
                worksheet.Cell(row, 2).Value = user.Login;
                worksheet.Cell(row, 3).Value = user.Imie;
                worksheet.Cell(row, 4).Value = user.Nazwisko;
                worksheet.Cell(row, 5).Value =
                    user.Rola?.Nazwa ?? "";

                worksheet.Cell(row, 6).Value =
                    user.Klasa?.NazwaKlasy ?? "";

                worksheet.Cell(row, 7).Value =
                    user.Klasa?.RokSzkolny ?? "";

                row++;
            }

            var header = worksheet.Range(1, 1, 1, 7);

            header.Style.Font.Bold = true;

            worksheet.Columns()
                .AdjustToContents();

            workbook.SaveAs(filePath);
        }

        public void ExportLoginLogs(List<LoginLogDto> logs, string filePath)
        {
            using var workbook = new XLWorkbook();

            var worksheet = workbook.Worksheets.Add("Logi logowania");

            worksheet.Cell(1, 1).Value = "Id";
            worksheet.Cell(1, 2).Value = "Login";
            worksheet.Cell(1, 3).Value = "Użytkownik";
            worksheet.Cell(1, 4).Value = "Rola";
            worksheet.Cell(1, 5).Value = "Data logowania";
            worksheet.Cell(1, 6).Value = "Data wylogowania";
            worksheet.Cell(1, 7).Value = "Sukces";
            worksheet.Cell(1, 8).Value = "Sesja";
            worksheet.Cell(1, 9).Value = "Adres IP";

            int row = 2;

            foreach (var log in logs)
            {
                worksheet.Cell(row, 1).Value = log.Id;
                worksheet.Cell(row, 2).Value = log.Login;
                worksheet.Cell(row, 3).Value = log.ImieNazwisko;
                worksheet.Cell(row, 4).Value = log.Rola;

                worksheet.Cell(row, 5).Value =
                    log.DataLogowania;

                if (log.DataWylogowania.HasValue)
                {
                    worksheet.Cell(row, 6).Value =
                        log.DataWylogowania.Value;
                }

                worksheet.Cell(row, 7).Value =
                    log.CzySukcesTekst;

                worksheet.Cell(row, 8).Value =
                    log.SesjaTekst;

                worksheet.Cell(row, 9).Value =
                    log.AdresIp ?? "";

                row++;
            }

            // Format dat
            worksheet.Column(5).Style.DateFormat.Format =
                "dd.MM.yyyy HH:mm:ss";

            worksheet.Column(6).Style.DateFormat.Format =
                "dd.MM.yyyy HH:mm:ss";

            // Nagłówek
            var header =
                worksheet.Range(1, 1, 1, 9);

            header.Style.Font.Bold = true;

            // Automatyczna szerokość
            worksheet.Columns().AdjustToContents();

            // Zamrożenie nagłówka
            worksheet.SheetView.FreezeRows(1);

            workbook.SaveAs(filePath);
        }
    }
}