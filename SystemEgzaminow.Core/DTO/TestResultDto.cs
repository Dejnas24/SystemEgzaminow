
namespace SystemEgzaminow.Core.DTO
{
    public class TestResultDto
    {
        public int Id { get; set; }

        public string Imie { get; set; } = string.Empty;
        public string Nazwisko { get; set; } = string.Empty;

        public DateTime? DataGodzinaRozpoczecia { get; set; }
        public DateTime? DataGodzinaZakonczenia { get; set; }

        public decimal LiczbaPunktow { get; set; }
        public decimal Procent { get; set; }
        public decimal? Ocena { get; set; }
        public bool CzyZdal { get; set; }

        public bool CzyMoznaEdytowac { get; set; }
        public bool CzyMoznaZakonczyc { get; set; }
        public bool CzyMoznaOpublikowac { get; set; }
        public bool CzyMoznaWyswietlic { get; set; }
        public bool CzyMoznaArchiwizowac { get; set; }

        

    }

    }

