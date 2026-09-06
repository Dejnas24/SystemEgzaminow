using Microsoft.EntityFrameworkCore;
using SystemEgzaminow.Core.DTO;
using SystemEgzaminow.Core.Enums;
using SystemEgzaminow.Core.Models;
using SystemEgzaminow.Data.Context;

namespace SystemEgzaminow.Data.Services
{
    public class TestSessionService
    {
        private readonly AppDbContext _context;

        public TestSessionService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<StartTestSessionDto?> GetStartTestSessionDtoAsync(int testId, bool czyProbny, int? przypisanyTestId = null)
        {
            var dto = await _context.Testy
         .AsNoTracking()
         .Where(t => t.Id == testId)
         .Select(t => new StartTestSessionDto
         {
             TestId = t.Id,

             Tytul = t.Tytul,
             CzasTrwaniaMinuty = t.CzasTrwaniaMinuty,
             ProgZaliczenia = t.ProgZdania,

             MaksymalnaLiczbaPunktow = t.TestPytania
                 .Select(tp => (int?)tp.Pytanie.LiczbaPunktow)
                 .Sum() ?? 0,

             CzyProbny = czyProbny
         })
         .FirstOrDefaultAsync();

            if (dto == null)
                return null;

            if (czyProbny)
            {
                dto.PrzypisanyTestId = null;
                dto.InformacjaOdNauczyciela = null;
            }
            else
            {
                if (!przypisanyTestId.HasValue)
                {
                    throw new InvalidOperationException(
                        "Dla testu ucznia wymagane jest PrzypisanyTestId.");
                }

                var przypisanie = await _context.PrzypisaneTesty
                    .AsNoTracking()
                    .Where(pt =>
                        pt.Id == przypisanyTestId.Value &&
                        pt.TestId == testId)
                    .Select(pt => new
                    {
                        pt.Id,
                        pt.InformacjaStartowa
                    })
                    .FirstOrDefaultAsync();

                if (przypisanie == null)
                {
                    throw new InvalidOperationException(
                        "Nie znaleziono przypisania testu.");
                }

                dto.PrzypisanyTestId = przypisanie.Id;
                dto.InformacjaOdNauczyciela =
                    przypisanie.InformacjaStartowa;
            }

            return dto;
        }

        public async Task<TestSessionDto?> GetTestSessionDtoAsync(int testId, bool czyProbny, int? przypisanyTestId = null)
        {
            TestSessionDto? dto = await _context.Testy
                .AsNoTracking()
                .Where(t => t.Id == testId)
                .Select(t => new TestSessionDto
                {
                    TestId = t.Id,
                    PrzypisanyTestId = przypisanyTestId,
                    CzyProbny = czyProbny,
                    Tytul = t.Tytul,
                    CzasTrwaniaMinuty = t.CzasTrwaniaMinuty,
                    LosujKolejnoscPytan = t.LosujKolejnoscPytan,
                    LosujKolejnoscOdpowiedzi = t.LosujKolejnoscOdpowiedzi,
                    Pytania = t.TestPytania
                        .Select(tp => new TestSessionQuestionDto
                        {
                            PytanieId = tp.Pytanie.Id,
                            TrescPytania = tp.Pytanie.TrescPytania,
                            TypPytania = tp.Pytanie.TypPytania,
                            LiczbaPunktow = tp.Pytanie.LiczbaPunktow,
                            Odpowiedzi = tp.Pytanie.Odpowiedzi
                                .Select(o => new TestSessionAnswerDto
                                {
                                    OdpowiedzId = o.Id,
                                    Tresc = o.TrescOdpowiedzi
                                })
                                .ToList()
                        })
                        .ToList()
                })
                .FirstOrDefaultAsync();
            return dto;
        }

        public async Task<EndTestSessionDto> FinishTestSessionAsync(TestSessionDto sessionDto, bool czyProbny, bool czyZakonczonyAutomatycznie)
        {
            if (sessionDto == null)
                throw new ArgumentNullException(nameof(sessionDto));

            if (sessionDto.Pytania.Count == 0)
                throw new InvalidOperationException(
                    "Nie można zakończyć testu bez pytań.");

            var test = await _context.Testy
    .AsNoTracking()
    .Where(t => t.Id == sessionDto.TestId)
    .Select(t => new
    {
        t.Id,
        t.ProgZdania,
        t.IdTypuTestu,

        Pytania = t.TestPytania
            .Select(tp => new
            {
                PytanieId = tp.Pytanie.Id,
                tp.Pytanie.TrescPytania,
                tp.Pytanie.TypPytania,
                tp.Pytanie.LiczbaPunktow,
                tp.Pytanie.IdAutora,

                Odpowiedzi = tp.Pytanie.Odpowiedzi
                    .Select(o => new
                    {
                        OdpowiedzId = o.Id,
                        o.TrescOdpowiedzi,
                        o.CzyPoprawna
                    })
                    .ToList()
            })
            .ToList()
    })
    .FirstOrDefaultAsync();

            if (test == null)
            {
                throw new InvalidOperationException(
                    "Nie znaleziono testu w bazie danych.");
            }

            // 1. obliczymy punkty
            int zdobytePunkty = 0;
            int maksymalnePunkty = 0;

            var punktyZaPytania = new Dictionary<int, int>();

            foreach (var pytanie in test.Pytania)
            {
                maksymalnePunkty += pytanie.LiczbaPunktow;

                var dtoPytanie = sessionDto.Pytania
                    .First(p => p.PytanieId == pytanie.PytanieId);

                // tutaj za chwilę policzymy,
                // czy odpowiedź jest poprawna

                bool czyPoprawne = false;

                switch (pytanie.TypPytania)
                {
                    case TypPytaniaEnum.JednokrotnyWybor:
                        {
                            int? wybranaOdpowiedzId = dtoPytanie.Odpowiedzi
                                .Where(o => o.CzyWybrana)
                                .Select(o => (int?)o.OdpowiedzId)
                                .FirstOrDefault();

                            int? poprawnaOdpowiedzId = pytanie.Odpowiedzi
                                .Where(o => o.CzyPoprawna)
                                .Select(o => (int?)o.OdpowiedzId)
                                .FirstOrDefault();

                            czyPoprawne =
                                wybranaOdpowiedzId.HasValue &&
                                wybranaOdpowiedzId == poprawnaOdpowiedzId;

                            break;
                        }

                    case TypPytaniaEnum.WielokrotnyWybor:
                        {
                            var wybraneOdpowiedziIds = dtoPytanie.Odpowiedzi
                                .Where(o => o.CzyWybrana)
                                .Select(o => o.OdpowiedzId)
                                .OrderBy(id => id)
                                .ToList();

                            var poprawneOdpowiedziIds = pytanie.Odpowiedzi
                                .Where(o => o.CzyPoprawna)
                                .Select(o => o.OdpowiedzId)
                                .OrderBy(id => id)
                                .ToList();

                            czyPoprawne =
                                wybraneOdpowiedziIds.SequenceEqual(
                                    poprawneOdpowiedziIds);

                            break;
                        }

                    case TypPytaniaEnum.Otwarte:
                        {
                            // W wersji 1.0 pytania otwarte są oceniane automatycznie.
                            string odpowiedzUcznia = NormalizeAnswer(dtoPytanie.TrescOdpowiedziUcznia);

                            string odpowiedzWzorcowa = NormalizeAnswer(pytanie.Odpowiedzi
                                        .FirstOrDefault(o => o.CzyPoprawna)?
                                        .TrescOdpowiedzi);

                            czyPoprawne =
                                !string.IsNullOrWhiteSpace(odpowiedzUcznia) &&
                                odpowiedzUcznia == odpowiedzWzorcowa;

                            break;
                        }
                }

                int punktyZaPytanie =
    czyPoprawne
        ? pytanie.LiczbaPunktow
        : 0;

                punktyZaPytania[pytanie.PytanieId] = punktyZaPytanie;
                zdobytePunkty += punktyZaPytanie;
            }
            //Procent
            decimal procent = maksymalnePunkty == 0
    ? 0
    : Math.Round((decimal)zdobytePunkty / maksymalnePunkty * 100, 2);

            bool czyZaliczony = procent >= test.ProgZdania;

            int? nauczycielId = null;
            bool czyPokazacWynikPoZakonczeniu = false;
            PrzypisanyTest? przypisanyTest = null;
            // Aktualizacja statusu przypisanego testu
            if (!czyProbny && sessionDto.PrzypisanyTestId.HasValue)
            {
                przypisanyTest = await _context.PrzypisaneTesty
                   .FirstOrDefaultAsync(pt =>
                       pt.Id == sessionDto.PrzypisanyTestId.Value);

                if (przypisanyTest == null)
                    throw new InvalidOperationException(
                        "Nie znaleziono przypisanego testu.");

                nauczycielId = przypisanyTest.NauczycielId;

                czyPokazacWynikPoZakonczeniu = przypisanyTest.CzyPokazacWynikPoZakonczeniu;
                int poprzednieProby = await _context.WynikiTestow
                    .CountAsync(w =>
                        w.PrzypisanyTestId == przypisanyTest.Id);

                int wykorzystaneProby = poprzednieProby + 1;

                if (czyZaliczony)
                    przypisanyTest.Status = StatusPrzypisanegoTestu.Ukonczony;
                else if (wykorzystaneProby >= przypisanyTest.LiczbaProb)
                    przypisanyTest.Status = StatusPrzypisanegoTestu.Ukonczony;
                else if (DateTime.Now > przypisanyTest.DataWygasniecia)
                    przypisanyTest.Status = StatusPrzypisanegoTestu.Wygasl;
                else
                    przypisanyTest.Status = StatusPrzypisanegoTestu.Oczekuje;
            }

            var gradeScaleService = new GradeScaleService(_context);

            decimal ocena = await gradeScaleService.GetGradeAsync(test.IdTypuTestu, nauczycielId, procent) ?? 0;
            // 2. utworzymy WynikTestu
            DateTime dataZakonczenia = DateTime.Now;

            var wynikTestu = new WynikTestu
            {
                UzytkownikId = sessionDto.UserId,
                PrzypisanyTestId = sessionDto.PrzypisanyTestId,

                LiczbaPunktow = zdobytePunkty,
                MaksymalnaLiczbaPunktow = maksymalnePunkty,
                Procent = procent,

                // Skala ocen będzie później.
                Ocena = ocena,

                CzyZdane = czyZaliczony,
                CzyProbny = czyProbny,

                DataRozpoczecia = sessionDto.DataRozpoczecia,
                DataZakonczenia = dataZakonczenia,

                CzasRozwiazywaniaMinuty = (int)Math.Ceiling((dataZakonczenia - sessionDto.DataRozpoczecia).TotalMinutes)
            };
            // 3. utworzymy RozwiazanePytania
            foreach (var pytanie in test.Pytania)
            {
                var dtoPytanie =
                    sessionDto.Pytania
                    .First(p => p.PytanieId == pytanie.PytanieId);

                var rozwiazanePytanie = new RozwiazanePytanie
                {
                    IdPytaniaZrodlowego = pytanie.PytanieId,
                    TrescPytania = pytanie.TrescPytania,
                    TypPytania = pytanie.TypPytania,
                    LiczbaPunktowMax = pytanie.LiczbaPunktow,
                    LiczbaPunktowZdobytych = punktyZaPytania[pytanie.PytanieId],
                    IdAutoraPytania = pytanie.IdAutora,
                    Kolejnosc = sessionDto.Pytania.IndexOf(dtoPytanie) + 1
                };

                wynikTestu.RozwiazanePytania
                    .Add(rozwiazanePytanie);
                // 4. utworzymy RozwiazaneOdpowiedzi

                int liczbaPoprawnychOdpowiedzi =
    pytanie.Odpowiedzi.Count(o => o.CzyPoprawna);

                decimal punktyZaJednaPoprawnaOdpowiedz =
                    liczbaPoprawnychOdpowiedzi == 0
                        ? 0
                        : Math.Round((decimal)pytanie.LiczbaPunktow / liczbaPoprawnychOdpowiedzi, 2);

                foreach (var odpowiedz in pytanie.Odpowiedzi)
                {
                    var dtoOdpowiedz =
                        dtoPytanie.Odpowiedzi
                        .First(o => o.OdpowiedzId == odpowiedz.OdpowiedzId);

                    var rozwiazanaOdpowiedz = new RozwiazanaOdpowiedz
                    {
                        IdOdpowiedziZrodlowej = odpowiedz.OdpowiedzId,
                        TrescOdpowiedzi = odpowiedz.TrescOdpowiedzi,
                        CzyPoprawna = odpowiedz.CzyPoprawna,

                        CzyWybranaPrzezUcznia =
                            dtoOdpowiedz.CzyWybrana,

                        TrescOdpowiedziUcznia =
                            dtoPytanie.TrescOdpowiedziUcznia,

                        Kolejnosc = dtoPytanie.Odpowiedzi.IndexOf(dtoOdpowiedz) + 1,

                        LiczbaPunktow = odpowiedz.CzyPoprawna ? Math.Round(punktyZaJednaPoprawnaOdpowiedz, 2) : 0,
                        //w przyszłości zbuduje się moduł sprawdzania oppowiedzi otwartych i wtedy nauczyciel będzie mógł dodać komentarz do odpowiedzi ucznia
                        KomentarzNauczyciela = null
                    };

                    rozwiazanePytanie
                        .RozwiazaneOdpowiedzi
                        .Add(rozwiazanaOdpowiedz);
                }
            }

            // 5. zapiszemy wszystko jedną transakcją.
            _context.WynikiTestow.Add(wynikTestu);
            await _context.SaveChangesAsync();

            string? informacjaKoncowa = przypisanyTest?.InformacjaKoncowa;

            var pytaniaDto = wynikTestu.RozwiazanePytania
      .OrderBy(p => p.Kolejnosc)
      .Select(p => new ResultQuestionDto
      {
          TrescPytania = p.TrescPytania,
          TypPytania = p.TypPytania,
          LiczbaPunktowMax = p.LiczbaPunktowMax,
          LiczbaPunktowZdobytych = p.LiczbaPunktowZdobytych,
          Kolejnosc = p.Kolejnosc,

          Odpowiedzi = p.RozwiazaneOdpowiedzi
    .Where(o => o.CzyWybranaPrzezUcznia || o.CzyPoprawna)
    .OrderBy(o => o.Kolejnosc)
              .Select(o => new ResultAnswerDto
              {
                  TrescOdpowiedzi = o.TrescOdpowiedzi,
                  CzyPoprawna = o.CzyPoprawna,
                  CzyWybranaPrzezUcznia = o.CzyWybranaPrzezUcznia,
                  TrescOdpowiedziUcznia = o.TrescOdpowiedziUcznia,
                  Kolejnosc = o.Kolejnosc,
                  LiczbaPunktow = o.LiczbaPunktow,
                  KomentarzNauczyciela = o.KomentarzNauczyciela
              })
              .ToList()
      })
      .ToList();
            return new EndTestSessionDto
            {
                WynikTestuId = wynikTestu.Id,
                ZdobytePunkty = zdobytePunkty,
                MaksymalnePunkty = maksymalnePunkty,
                Procent = procent,
                Ocena = ocena,
                CzyZaliczony = czyZaliczony,
                CzyZakonczonyAutomatycznie = czyZakonczonyAutomatycznie,
                InformacjaKoncowa = informacjaKoncowa,
                CzyProbny = sessionDto.CzyProbny,
                CzyPokazacWynikPoZakonczeniu = czyPokazacWynikPoZakonczeniu,
                SposobWyswietlaniaWyniku = przypisanyTest?.SposobWyswietlaniaWyniku ?? SposobWyswietlaniaWynikuEnum.TylkoPotwierdzenie,
                SposobOceniania = przypisanyTest?.SposobOceniania ?? SposobOcenianiaEnum.Punkty,
                Pytania = pytaniaDto
            };
        }

        private static string NormalizeAnswer(string? text)
        {
            if (string.IsNullOrWhiteSpace(text))
                return string.Empty;

            return text
                .Trim()
                .ToLowerInvariant();
        }

        public async Task MarkAssignedTestAsStartedAsync(int przypisanyTestId)
        {
            var przypisanyTest = await _context.PrzypisaneTesty
                .FirstOrDefaultAsync(pt => pt.Id == przypisanyTestId);

            if (przypisanyTest == null)
            {
                throw new InvalidOperationException(
                    "Nie znaleziono przypisanego testu.");
            }

            if (przypisanyTest.Status == StatusPrzypisanegoTestu.Anulowany ||
                przypisanyTest.Status == StatusPrzypisanegoTestu.Ukonczony ||
                przypisanyTest.Status == StatusPrzypisanegoTestu.Wygasl)
            {
                throw new InvalidOperationException(
                    "Tego testu nie można już rozpocząć.");
            }

            przypisanyTest.Status =
                StatusPrzypisanegoTestu.Rozpoczety;

            await _context.SaveChangesAsync();
        }
    }
}