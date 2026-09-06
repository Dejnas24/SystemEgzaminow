namespace SystemEgzaminow.Core.Models
{
    public class Klasa
    {
        public int Id { get; set; }

        public string NazwaKlasy { get; set; } = string.Empty;

        public string RokSzkolny { get; set; } = string.Empty;

        public List<Uzytkownik> Uczniowie { get; set; } = new();

        public List<PrzypisanyTest> PrzypisaneTesty { get; set; } = new();
    }
}