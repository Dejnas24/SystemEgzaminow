using SystemEgzaminow.Core.Enums;

namespace SystemEgzaminow.Core.DTO
{
    public class StudentAssignedTestDto
    {
        public int PrzypisanyTestId { get; set; }
        public int TestId { get; set; }

        public string Tytul { get; set; } = string.Empty;
        public string TypTestu { get; set; } = string.Empty;

        public DateTime DataDostepnosci { get; set; }
        public DateTime DataWygasniecia { get; set; }

        public string InformacjaTerminowa { get; set; } = string.Empty;

        public string? InformacjaStartowa { get; set; }
        public string? InformacjaKoncowa { get; set; }

        public int LiczbaProb { get; set; }
        public int WykorzystaneProby { get; set; }
        public int PozostaleProby { get; set; }

        public string StatusTekst { get; set; } = string.Empty;

        public bool CzyMoznaRozwiazac { get; set; }
        public bool CzyMoznaPokazacWynik { get; set; }

        public bool CzyPokazacWynikPoZakonczeniu { get; set; }
        public string? RokSzkolny { get; set; }

        public SposobWyswietlaniaWynikuEnum SposobWyswietlaniaWyniku { get; set; }
    }
}