
namespace SystemEgzaminow.Core.DTO
{
    public class TestResultsContextDto
    {
        public bool CzyMoznaEdytowacKlase { get; set; }
        public bool CzyMoznaEksportowacKlase { get; set; }
        public bool CzyMoznaZakonczycTestKlasy { get; set; }
        public bool CzyMoznaOpublikowacTestKlasy { get; set; }
        public bool CzyMoznaArchiwizowacKlase { get; set; }

        public List<TestResultDto> Wyniki { get; set; } = new();
    }
}
