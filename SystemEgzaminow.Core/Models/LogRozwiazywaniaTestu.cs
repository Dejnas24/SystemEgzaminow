namespace SystemEgzaminow.Core.Models
{
    public class LogRozwiazywaniaTestu
    {
        public int Id { get; set; }

        public int PrzypisanyTestId { get; set; }
        public PrzypisanyTest PrzypisanyTest { get; set; } = null!;

        public int UzytkownikId { get; set; }
        public Uzytkownik Uzytkownik { get; set; } = null!;

        public DateTime DataZdarzenia { get; set; }

        public string TypZdarzenia { get; set; } = string.Empty;
    }
}