using SystemEgzaminow.Core.Enums;

namespace SystemEgzaminow.Core.DTO
{
    public class AssignTestDto
    {
        public int TestId { get; set; }

        public List<int> UczenIds { get; set; } = new();

        public DateTime DataDostepnosci { get; set; }

        public DateTime DataWygasniecia { get; set; }

        public int LiczbaProb { get; set; } = 1;

        public StatusPrzypisanegoTestu Status { get; set; }

        public SposobOcenianiaEnum SposobOceniania { get; set; }

        public SposobWyswietlaniaWynikuEnum SposobWyswietlaniaWyniku { get; set; }

        public bool CzyPokazacWynikPoZakonczeniu { get; set; }

        public string? InformacjaStartowa { get; set; }

        public string? InformacjaKoncowa { get; set; }
    }
}