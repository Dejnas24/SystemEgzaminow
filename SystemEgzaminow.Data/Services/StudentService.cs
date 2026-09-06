using Microsoft.EntityFrameworkCore;
using SystemEgzaminow.Core.DTO;
using SystemEgzaminow.Core.Enums;
using SystemEgzaminow.Data.Context;

namespace SystemEgzaminow.Data.Services
{
    public class StudentService
    {
        private readonly AppDbContext _context;

        public StudentService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<StudentAssignedTestDto>> GetAssignedTestsAsync(int uczenId)
        {
            var tests = await _context.PrzypisaneTesty
        .AsNoTracking()
        .Where(pt => pt.UczenId == uczenId)
        .Select(pt => new StudentAssignedTestDto
        {
            PrzypisanyTestId = pt.Id,
            TestId = pt.TestId,

            Tytul = pt.Test.Tytul,
            TypTestu = pt.Test.TypTestu.NazwaTypu,

            DataDostepnosci = pt.DataDostepnosci,
            DataWygasniecia = pt.DataWygasniecia,

            LiczbaProb = pt.LiczbaProb,
            InformacjaStartowa = pt.InformacjaStartowa,
            InformacjaKoncowa = pt.InformacjaKoncowa,
            WykorzystaneProby = pt.WynikiTestow.Count(),

            PozostaleProby =
    pt.LiczbaProb > pt.WynikiTestow.Count()
        ? pt.LiczbaProb - pt.WynikiTestow.Count()
        : 0,

            CzyPokazacWynikPoZakonczeniu =
                pt.CzyPokazacWynikPoZakonczeniu,

            SposobWyswietlaniaWyniku =
                pt.SposobWyswietlaniaWyniku,

            RokSzkolny = pt.Klasa != null
         ? pt.Klasa.RokSzkolny
         : null,

            StatusTekst = pt.Status.ToString()
        }).OrderBy(pt => pt.DataDostepnosci)
        .ToListAsync();

            var teraz = DateTime.Now;
            foreach (var test in tests)
            {
                // 1. Informacja o terminie
                test.InformacjaTerminowa =
                    GetTermInfo(
                        test.DataDostepnosci,
                        test.DataWygasniecia);

                // 2. Czy można rozwiązać
                test.CzyMoznaRozwiazac =
        test.StatusTekst == StatusPrzypisanegoTestu.Oczekuje.ToString()
        && teraz >= test.DataDostepnosci
        && teraz <= test.DataWygasniecia
        && test.PozostaleProby > 0;

                // 3. Czy można zobaczyć wynik
                test.CzyMoznaPokazacWynik =
        test.WykorzystaneProby > 0 &&
        (
            test.CzyPokazacWynikPoZakonczeniu ||
            teraz > test.DataWygasniecia
        );
            }

            return tests;
        }

        public async Task<List<TestTypeFilterDto>> GetAvailableTestTypesAsync(int uczenId)
        {
            return await _context.PrzypisaneTesty
                .AsNoTracking()
                .Where(pt => pt.UczenId == uczenId)
                .Select(pt => new TestTypeFilterDto
                {
                    Id = pt.Test.IdTypuTestu,
                    Nazwa = pt.Test.TypTestu.NazwaTypu
                })
                .Distinct()
                .OrderBy(t => t.Nazwa)
                .ToListAsync();
        }

        public async Task<List<string>> GetAvailableSchoolYearsAsync(int uczenId)
        {
            return await _context.PrzypisaneTesty
                .AsNoTracking()
                .Where(pt =>
                    pt.UczenId == uczenId &&
                    pt.KlasaId != null)
                .Select(pt => pt.Klasa!.RokSzkolny)
                .Where(rok => !string.IsNullOrEmpty(rok))
                .Distinct()
                .OrderByDescending(rok => rok)
                .ToListAsync();
        }

        private string GetTermInfo(DateTime dataDostepnosci, DateTime dataWygasniecia)
        {
            DateTime teraz = DateTime.Now;

            if (teraz < dataDostepnosci)
            {
                TimeSpan pozostalo = dataDostepnosci - teraz;

                if (pozostalo.TotalDays >= 1)
                {
                    return $"Za {(int)pozostalo.TotalDays} dni";
                }

                return $"Za {pozostalo.Hours} godz. {pozostalo.Minutes} min";
            }

            if (teraz <= dataWygasniecia)
            {
                TimeSpan pozostalo = dataWygasniecia - teraz;

                if (pozostalo.TotalDays >= 1)
                {
                    return $"Pozostało {(int)pozostalo.TotalDays} dni";
                }

                return $"Pozostało {pozostalo.Hours} godz. {pozostalo.Minutes} min";
            }

            return "Termin minął";
        }

        public async Task UpdateAssignedTestStatusesAsync(int uczenId)
        {
            var teraz = DateTime.Now;

            var przypisaneTesty = await _context.PrzypisaneTesty
                .Include(pt => pt.WynikiTestow)
                .Where(pt => pt.UczenId == uczenId)
                .ToListAsync();

            bool czyZmieniono = false;

            foreach (var pt in przypisaneTesty)
            {
                // Statusy końcowe - automat ich już nie zmienia
                if (pt.Status == StatusPrzypisanegoTestu.Anulowany ||
                    pt.Status == StatusPrzypisanegoTestu.Ukonczony)
                {
                    continue;
                }

                bool czyZdany =
                    pt.WynikiTestow.Any(w => w.CzyZdane);

                int wykorzystaneProby =
                    pt.WynikiTestow.Count;

                // Uczeń zdał - dalsze próby nie są potrzebne
                if (czyZdany)
                {
                    pt.Status = StatusPrzypisanegoTestu.Ukonczony;
                    czyZmieniono = true;
                }
                // Nie zdał, ale wykorzystał wszystkie próby
                else if (wykorzystaneProby >= pt.LiczbaProb)
                {
                    pt.Status = StatusPrzypisanegoTestu.Ukonczony;
                    czyZmieniono = true;
                }
                // Ma jeszcze próby, ale minął termin
                else if (teraz > pt.DataWygasniecia)
                {
                    pt.Status = StatusPrzypisanegoTestu.Wygasl;
                    czyZmieniono = true;
                }
            }

            if (czyZmieniono)
            {
                await _context.SaveChangesAsync();
            }
        }

        public async Task<StudentTestResultDto?> GetLatestTestResultAsync(int przypisanyTestId)
        {
            var wynik = await _context.WynikiTestow
                .AsNoTracking()
                .Where(w =>
                    w.PrzypisanyTestId == przypisanyTestId &&
                    w.CzyProbny == false)
                .OrderByDescending(w => w.DataZakonczenia)
                .FirstOrDefaultAsync();

            if (wynik == null)
                return null;

            var przypisanyTest = await _context.PrzypisaneTesty
                .AsNoTracking()
                .Include(pt => pt.Test)
                    .ThenInclude(t => t.TypTestu)
                .Include(pt => pt.Uczen)
                .Include(pt => pt.Klasa)
                .FirstOrDefaultAsync(pt => pt.Id == przypisanyTestId);

            if (przypisanyTest == null)
                return null;

            int numerProby = await _context.WynikiTestow
                .AsNoTracking()
                .CountAsync(w =>
                    w.PrzypisanyTestId == przypisanyTestId &&
                    w.CzyProbny == false &&
                    w.DataZakonczenia <= wynik.DataZakonczenia);

            int minimalnaLiczbaPunktow =
                (int)Math.Ceiling(
                    wynik.MaksymalnaLiczbaPunktow *
                    przypisanyTest.Test.ProgZdania / 100m);

            return new StudentTestResultDto
            {
                WynikTestuId = wynik.Id,
                PrzypisanyTestId = przypisanyTest.Id,

                TytulTestu = przypisanyTest.Test.Tytul,
                TypTestu = przypisanyTest.Test.TypTestu.NazwaTypu,

                ImieNazwiskoUcznia =
                    przypisanyTest.Uczen.Imie + " " +
                    przypisanyTest.Uczen.Nazwisko,

                Klasa = przypisanyTest.Klasa?.NazwaKlasy,
                RokSzkolny = przypisanyTest.Klasa?.RokSzkolny,

                DataZakonczenia =
                    wynik.DataZakonczenia ?? DateTime.MinValue,

                CzyZaliczony = wynik.CzyZdane,

                ZdobytePunkty = wynik.LiczbaPunktow,
                MaksymalnePunkty = wynik.MaksymalnaLiczbaPunktow,

                Procent = wynik.Procent,
                Ocena = wynik.Ocena,

                MinimalnyProgProcentowy =
                    przypisanyTest.Test.ProgZdania,

                MinimalnaLiczbaPunktow =
                    minimalnaLiczbaPunktow,

                NumerProby = numerProby,
                LiczbaProb = przypisanyTest.LiczbaProb,
                SposobOceniania = przypisanyTest.SposobOceniania,
            };
        }
    }
}