namespace SystemEgzaminow.Core.Models
{
    public class Test
    {
        public int Id { get; set; }
        public string Tytul { get; set; } = string.Empty;
        public string? Opis { get; set; }
        public int ProgZdania { get; set; }
        public DateTime DataUtworzenia { get; set; }
        public int CzasTrwaniaMinuty { get; set; }

        public int IdAutora { get; set; }
        public Uzytkownik Autor { get; set; } = null!;
        public int IdTypuTestu { get; set; }
        public TypTestu TypTestu { get; set; } = null!;
        public bool LosujKolejnoscPytan { get; set; }
        public bool LosujKolejnoscOdpowiedzi { get; set; }
        public bool CzyZarchiwizowany { get; set; } = false;

        public List<TestPytanie> TestPytania { get; set; } = new();
        public List<PrzypisanyTest> PrzypisaneTesty { get; set; } = new();
    }
}