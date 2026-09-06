using SystemEgzaminow.Core.Enums;

namespace SystemEgzaminow.Core.Models
{
    public class PrzypisanyTest
    {
        public int Id { get; set; }

        public int TestId { get; set; }
        public Test Test { get; set; } = null!;

        public int UczenId { get; set; }
        public Uzytkownik Uczen { get; set; } = null!;

        public int NauczycielId { get; set; }
        public Uzytkownik Nauczyciel { get; set; } = null!;

        public DateTime DataPrzypisania { get; set; }

        public DateTime DataDostepnosci { get; set; }

        public DateTime DataWygasniecia { get; set; }

        public StatusPrzypisanegoTestu Status { get; set; }
        public int LiczbaProb { get; set; } = 1;
        public SposobOcenianiaEnum SposobOceniania { get; set; }

        public string? InformacjaStartowa { get; set; }

        public string? InformacjaKoncowa { get; set; }

        public SposobWyswietlaniaWynikuEnum SposobWyswietlaniaWyniku { get; set; } = SposobWyswietlaniaWynikuEnum.PunktyIProcent;

        public bool CzyPokazacWynikPoZakonczeniu { get; set; }
        public int? KlasaId { get; set; }
        public Klasa? Klasa { get; set; }

        public List<LogRozwiazywaniaTestu> LogiRozwiazywaniaTestow { get; set; } = new();
        public List<WynikTestu> WynikiTestow { get; set; } = new();
    }
}