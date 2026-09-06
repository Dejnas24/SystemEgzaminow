using SystemEgzaminow.Core.Enums;

namespace SystemEgzaminow.Core.DTO
{
    public class StudentTestResultDto
    {
        public int WynikTestuId { get; set; }
        public int PrzypisanyTestId { get; set; }

        public string TytulTestu { get; set; } = string.Empty;
        public string TypTestu { get; set; } = string.Empty;

        public string ImieNazwiskoUcznia { get; set; } = string.Empty;

        public string? Klasa { get; set; }
        public string? RokSzkolny { get; set; }

        public DateTime DataZakonczenia { get; set; }

        public bool CzyZaliczony { get; set; }

        public int ZdobytePunkty { get; set; }
        public int MaksymalnePunkty { get; set; }

        public decimal Procent { get; set; }
        public decimal Ocena { get; set; }

        public int MinimalnyProgProcentowy { get; set; }
        public int MinimalnaLiczbaPunktow { get; set; }

        public int NumerProby { get; set; }
        public int LiczbaProb { get; set; }
        public SposobOcenianiaEnum SposobOceniania { get; set; }
    }
}