using SystemEgzaminow.Core.Enums;

namespace SystemEgzaminow.Core.Models
{
    public class LogLogowania
    {
        public int Id { get; set; }

        public int? UzytkownikId { get; set; }
        public Uzytkownik? Uzytkownik { get; set; }

        public DateTime DataLogowania { get; set; }
        public DateTime? DataWylogowania { get; set; }

        public bool CzySukces { get; set; }

        public SesjaEnum Sesja { get; set; }

        public string? AdresIp { get; set; }
    }
}