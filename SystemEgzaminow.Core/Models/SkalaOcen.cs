namespace SystemEgzaminow.Core.Models
{
    public class SkalaOcen
    {
        public int Id { get; set; }
        public int IdTypuTestu { get; set; }
        public int? IdUzytkownika { get; set; }

        public string Nazwa { get; set; } = string.Empty;
        public bool CzyAktywna { get; set; } = true;
        public DateTime DataUtworzenia { get; set; } = DateTime.Now;

        public TypTestu TypTestu { get; set; } = null!;
        public Uzytkownik? Uzytkownik { get; set; }

        public List<ProgOceny> ProgiOcen { get; set; } = new();
    }
}