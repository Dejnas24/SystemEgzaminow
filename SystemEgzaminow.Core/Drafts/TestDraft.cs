namespace SystemEgzaminow.Core.Drafts
{
    public class TestDraft
    {
        public int? TestId { get; set; }
        public string Tytul { get; set; } = string.Empty;
        public string? Opis { get; set; }
        public int ProgZdania { get; set; }
        public int CzasTrwaniaMinuty { get; set; }
        public int? IdTypuTestu { get; set; }
        public bool LosujKolejnoscPytan { get; set; }
        public bool LosujKolejnoscOdpowiedzi { get; set; }

        public List<PrzypisanePytaniaDraft> PrzypisanePytania { get; set; } = new();
    }
}