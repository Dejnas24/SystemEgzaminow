using SystemEgzaminow.Core.Enums;

namespace SystemEgzaminow.Core.DTO
{
    public class EndTestSessionDto
    {
        public int WynikTestuId { get; set; }

        public int ZdobytePunkty { get; set; }

        public int MaksymalnePunkty { get; set; }

        public decimal Procent { get; set; }

        public bool CzyZaliczony { get; set; }
        public string? InformacjaKoncowa { get; set; }

        public bool CzyZakonczonyAutomatycznie { get; set; }
        public bool CzyProbny { get; set; }

        public decimal Ocena { get; set; }

        public bool CzyPokazacWynikPoZakonczeniu { get; set; }

        public SposobWyswietlaniaWynikuEnum SposobWyswietlaniaWyniku { get; set; }
        public SposobOcenianiaEnum SposobOceniania { get; set; }
        public List<ResultQuestionDto> Pytania { get; set; } = new();
    }
}