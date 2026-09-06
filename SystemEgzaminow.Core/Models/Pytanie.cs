using SystemEgzaminow.Core.Enums;

namespace SystemEgzaminow.Core.Models
{
    public class Pytanie
    {
        public int Id { get; set; }
        public string TrescPytania { get; set; } = string.Empty;
        public TypPytaniaEnum TypPytania { get; set; }
        public int LiczbaPunktow { get; set; }
        public int IdAutora { get; set; }
        public string? SciezkaZdjecia { get; set; }
        public Uzytkownik Autor { get; set; } = null!;
        public bool CzyZarchiwizowane { get; set; } = false;

        public List<TestPytanie> TestPytania { get; set; } = new();
        public List<Odpowiedz> Odpowiedzi { get; set; } = new();
    }
}