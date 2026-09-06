namespace SystemEgzaminow.Core.DTO
{
    public class StartTestSessionDto
    {
        public int TestId { get; set; }

        public int? PrzypisanyTestId { get; set; }

        public string Tytul { get; set; } = string.Empty;

        public int CzasTrwaniaMinuty { get; set; }

        public int MaksymalnaLiczbaPunktow { get; set; }

        public int ProgZaliczenia { get; set; }

        public string? InformacjaOdNauczyciela { get; set; }

        public bool CzyProbny { get; set; }
    }
}