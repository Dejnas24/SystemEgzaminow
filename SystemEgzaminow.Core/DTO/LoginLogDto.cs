using SystemEgzaminow.Core.Enums;

namespace SystemEgzaminow.Core.DTO
{
    public class LoginLogDto
    {
        public int Id { get; set; }

        public int? UzytkownikId { get; set; }

        public string Login { get; set; } = string.Empty;

        public string ImieNazwisko { get; set; } = string.Empty;

        public string Rola { get; set; } = string.Empty;

        public DateTime DataLogowania { get; set; }

        public DateTime? DataWylogowania { get; set; }

        public bool CzySukces { get; set; }

        public string CzySukcesTekst =>
    CzySukces ? "Tak" : "Nie";

        public string SesjaTekst => Sesja switch
        {
            SesjaEnum.Zalogowany => "Zalogowany",
            SesjaEnum.Wylogowany => "Wylogowany",
            SesjaEnum.NieudanaProba => "Nieudana próba",
            _ => "-"
        };

        public SesjaEnum Sesja { get; set; }

        public string? AdresIp { get; set; }
    }
}