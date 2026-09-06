namespace SystemEgzaminow.Core.Models
{
    public class Uzytkownik
    {
        public int Id { get; set; }

        public string Login { get; set; }

        public string Haslo { get; set; }

        public string Imie { get; set; }

        public string Nazwisko { get; set; }

        public int RolaId { get; set; }

        public Rola Rola { get; set; } = null!;
        public bool CzyMoznaEdytowac => Id > 3;

        public int? KlasaId { get; set; }
        public Klasa? Klasa { get; set; }

        public List<Test> Testy { get; set; } = new();
        public List<Pytanie> Pytania { get; set; } = new();
        public List<WynikTestu> WynikiTestow { get; set; } = new();
        public List<PrzypisanyTest> PrzypisaneTestyUcznia { get; set; } = new();
        public List<PrzypisanyTest> PrzypisaneTestyNauczyciela { get; set; } = new();
        public List<LogLogowania> LogiLogowan { get; set; } = new();
        public List<LogRozwiazywaniaTestu> LogiRozwiazywaniaTestow { get; set; } = new();
        public List<SkalaOcen> SkaleOcen { get; set; } = new();
    }
}