namespace SystemEgzaminow.Core.DTO
{
    public class ResultAnswerDto
    {
        public string TrescOdpowiedzi { get; set; } = string.Empty;

        public bool CzyPoprawna { get; set; }
        public bool CzyWybranaPrzezUcznia { get; set; }

        public string? TrescOdpowiedziUcznia { get; set; }

        public int Kolejnosc { get; set; }

        public decimal LiczbaPunktow { get; set; }

        public string? KomentarzNauczyciela { get; set; }
    }
}