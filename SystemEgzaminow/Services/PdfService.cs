using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using SystemEgzaminow.Core.DTO;
using SystemEgzaminow.Core.Enums;

namespace SystemEgzaminow.WPF.Services
{
    public class PdfService
    {
        public void GenerateStudentTestResultPdf(StudentTestResultDto result, string filePath)
        {
            Document.Create(container =>
            {
                container.Page(page =>
                {
                    page.Size(PageSizes.A4);
                    page.Margin(35);

                    page.DefaultTextStyle(x =>
                        x.FontSize(11));

                    page.Content().Column(column =>
                    {
                        column.Spacing(15);

                        // Nagłówek
                        column.Item()
                            .AlignCenter()
                            .Text("Wyniki testu")
                            .FontSize(24)
                            .Bold();

                        // Tytuł i typ
                        column.Item()
                            .AlignCenter()
                            .Column(info =>
                            {
                                info.Spacing(5);

                                info.Item().Text(text =>
                                {
                                    text.Span("Tytuł testu: ").Bold();
                                    text.Span(result.TytulTestu);
                                });

                                info.Item().Text(text =>
                                {
                                    text.Span("Typ testu: ").Bold();
                                    text.Span(result.TypTestu);
                                });
                            });

                        // Główny panel
                        column.Item()
                            .Border(1)
                            .BorderColor(Colors.Grey.Lighten2)
                            .Padding(20)
                            .Row(row =>
                            {
                                // LEWA KOLUMNA
                                row.RelativeItem()
                                    .PaddingRight(20)
                                    .Column(left =>
                                    {
                                        left.Spacing(10);

                                        left.Item()
                                            .Text("Dane ucznia i wynik")
                                            .FontSize(16)
                                            .Bold();

                                        AddField(
                                            left,
                                            "Uczeń:",
                                            result.ImieNazwiskoUcznia);
                                        switch (result.SposobOceniania)
                                        {
                                            case SposobOcenianiaEnum.Punkty:
                                                AddField(
                                                    left,
                                                    "Punkty zdobyte:",
                                                    $"{result.ZdobytePunkty} / {result.MaksymalnePunkty}");
                                                break;

                                            case SposobOcenianiaEnum.Procent:
                                                AddField(
                                                    left,
                                                    "Procent:",
                                                    $"{result.Procent:0.##}%");
                                                break;

                                            case SposobOcenianiaEnum.PunktyIProcent:
                                                AddField(
                                                    left,
                                                    "Punkty zdobyte:",
                                                    $"{result.ZdobytePunkty} / {result.MaksymalnePunkty}");

                                                AddField(
                                                    left,
                                                    "Procent:",
                                                    $"{result.Procent:0.##}%");
                                                break;

                                            case SposobOcenianiaEnum.Ocena:
                                                AddField(
                                                    left,
                                                    "Ocena:",
                                                    result.Ocena > 0
                                                        ? result.Ocena.ToString("0.##")
                                                        : "-");
                                                break;

                                            case SposobOcenianiaEnum.TylkoZaliczenie:
                                                AddField(
                                                    left,
                                                    "Czy zaliczony:",
                                                    result.CzyZaliczony ? "Tak" : "Nie");
                                                break;
                                        }

                                        AddField(
                                            left,
                                            "Próba:",
                                            $"{result.NumerProby} z {result.LiczbaProb}");
                                    });

                                // Separator
                                row.ConstantItem(1)
                                    .Background(Colors.Grey.Lighten2);

                                // PRAWA KOLUMNA
                                row.RelativeItem()
                                    .PaddingLeft(20)
                                    .Column(right =>
                                    {
                                        right.Spacing(10);

                                        right.Item()
                                            .Text("Dane testu")
                                            .FontSize(16)
                                            .Bold();

                                        AddField(
                                            right,
                                            "Klasa:",
                                            result.Klasa ?? "-");

                                        AddField(
                                            right,
                                            "Rok szkolny:",
                                            result.RokSzkolny ?? "-");

                                        AddField(
                                            right,
                                            "Data i godzina rozwiązania:",
                                            result.DataZakonczenia
                                                .ToString("dd.MM.yyyy HH:mm"));

                                        AddField(
                                            right,
                                            "Maksymalna liczba punktów:",
                                            result.MaksymalnePunkty.ToString());

                                        AddField(
                                            right,
                                            "Minimalny próg procentowy:",
                                            $"{result.MinimalnyProgProcentowy}%");

                                        AddField(
                                            right,
                                            "Minimalna liczba punktów:",
                                            $"{result.MinimalnaLiczbaPunktow} / {result.MaksymalnePunkty}");
                                    });
                            });
                    });

                    page.Footer()
      .AlignCenter()
      .Text(text =>
      {
          text.DefaultTextStyle(
              TextStyle.Default
                  .FontSize(9)
                  .FontColor(Colors.Grey.Medium));

          text.Span("System Egzaminów • ");
          text.CurrentPageNumber();
          text.Span(" / ");
          text.TotalPages();
      });
                });
            })
     .GeneratePdf(filePath);
        }

        private static void AddField(
    ColumnDescriptor column,
    string label,
    string value)
        {
            column.Item().Column(field =>
            {
                field.Spacing(2);

                field.Item()
                    .Text(label)
                    .Bold();

                field.Item()
                    .Text(value);
            });
        }
    }
}