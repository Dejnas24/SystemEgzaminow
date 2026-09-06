namespace SystemEgzaminow.Core.DTO
{
    public class GradeScaleDto
    {
        public int? Id { get; set; }

        public int IdTypuTestu { get; set; }
        public string NazwaTypuTestu { get; set; } = string.Empty;

        public int? IdUzytkownika { get; set; }

        public string Nazwa { get; set; } = string.Empty;

        public bool CzyAktywna { get; set; }

        public bool CzyDomyslna => IdUzytkownika == null;

        public List<GradeThresholdDto> Progi { get; set; } = new();
    }
}