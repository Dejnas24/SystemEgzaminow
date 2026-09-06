namespace SystemEgzaminow.Core.Models
{
    public class WynikTestu
    {
        public int Id { get; set; }

        public int? UzytkownikId { get; set; }
        public Uzytkownik Uzytkownik { get; set; } = null!;

        public int? PrzypisanyTestId { get; set; }
        public PrzypisanyTest PrzypisanyTest { get; set; } = null!;

        public int LiczbaPunktow { get; set; }

        public int MaksymalnaLiczbaPunktow { get; set; }

        public decimal Procent { get; set; }

        public decimal Ocena { get; set; }

        public bool CzyZdane { get; set; }

        //Administrator lub nauczyciel rozwiązuje test próbny
        public bool? CzyProbny { get; set; }

        public DateTime? DataRozpoczecia { get; set; }

        public DateTime? DataZakonczenia { get; set; }

        public int? CzasRozwiazywaniaMinuty { get; set; }

        public List<RozwiazanePytanie> RozwiazanePytania { get; set; } = new();
    }
}