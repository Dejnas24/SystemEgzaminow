namespace SystemEgzaminow.Core.DTO
{
    public class TestListItem
    {
        public int Id { get; set; }
        public string Tytul { get; set; } = string.Empty;
        public int IdTyp { get; set; }
        public string Typ { get; set; } = string.Empty;
        public int LiczbaPytan { get; set; }
        public int CzasTrwaniaMinuty { get; set; }
        public int Prog { get; set; }
        public int IdAutora { get; set; }
        public string Autor { get; set; } = string.Empty;
        public DateTime DataUtworzenia { get; set; }
        public bool CzyZarchiwizowany { get; set; }

        public string ArchiveButtonText =>
            CzyZarchiwizowany ? "Przywróć" : "Archiwizuj";

        public bool CzyMoznaEdytowac =>
    !CzyZarchiwizowany;

        public bool CzyMoznaRozwiazacProbnie =>
            !CzyZarchiwizowany;

        public bool CzyMoznaUsunac =>
    CzyZarchiwizowany;
    }
}