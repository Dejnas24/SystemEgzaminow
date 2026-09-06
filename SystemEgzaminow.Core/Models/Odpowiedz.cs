namespace SystemEgzaminow.Core.Models
{
    public class Odpowiedz
    {
        public int Id { get; set; }

        public string TrescOdpowiedzi { get; set; } = string.Empty;

        public bool CzyPoprawna { get; set; }
        public int? LiczbaPunktow { get; set; }

        public int PytanieId { get; set; }

        public Pytanie Pytanie { get; set; } = null!;
    }
}