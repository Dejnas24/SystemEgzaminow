using Microsoft.EntityFrameworkCore;
using SystemEgzaminow.Core.Drafts;
using SystemEgzaminow.Core.DTO;
using SystemEgzaminow.Core.Models;
using SystemEgzaminow.Core.Session;
using SystemEgzaminow.Data.Context;

namespace SystemEgzaminow.Data.Services
{
    public class TestService
    {
        private readonly AppDbContext _context;

        public TestService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<AssignedQuestionDto> GetAssignedQuestionByIdAsync(
    int questionId)
        {
            var question = await _context.Pytania
                .AsNoTracking()
                .Where(p => p.Id == questionId)
                .Select(p => new AssignedQuestionDto
                {
                    Id = p.Id,
                    TrescPytania = p.TrescPytania,
                    TypPytania = p.TypPytania,
                    LiczbaPunktow = p.LiczbaPunktow,
                    SciezkaZdjecia = p.SciezkaZdjecia,

                    AutorPelnaNazwa =
                        p.Autor.Imie + " " + p.Autor.Nazwisko,

                    Odpowiedzi = p.Odpowiedzi
    .OrderBy(o => o.Id)
    .Select(o => new AssignedAnswerDto
    {
        Id = o.Id,
        TrescOdpowiedzi = o.TrescOdpowiedzi,
        CzyPoprawna = o.CzyPoprawna,
        LiczbaPunktow = o.LiczbaPunktow
    })
    .ToList(),

                    OdpowiedzWzorcowa = p.Odpowiedzi
    .OrderBy(o => o.Id)
    .Select(o => o.TrescOdpowiedzi)
    .FirstOrDefault() ?? string.Empty
                })
                .FirstOrDefaultAsync();

            if (question == null)
            {
                throw new Exception(
                    "Nie znaleziono wybranego pytania.");
            }

            return question;
        }

        public async Task<int> SaveTestAsync(CreateTestDto dto)
        {
            if (dto == null)
                throw new ArgumentNullException(nameof(dto));

            if (dto.Pytania.Count == 0)
                throw new Exception("Test musi zawierać pytania.");

            var testTypeExists = await _context.TypyTestow
                .AnyAsync(t => t.Id == dto.TypTestuId);

            if (!testTypeExists)
                throw new Exception("Wybrany typ testu nie istnieje.");

            var questionIds = dto.Pytania
                .Select(p => p.PytanieId)
                .Distinct()
                .ToList();

            if (questionIds.Count != dto.Pytania.Count)
                throw new Exception("Ten sam test nie może zawierać powtórzonych pytań.");

            int existingQuestionsCount = await _context.Pytania
                .CountAsync(p => questionIds.Contains(p.Id));

            if (existingQuestionsCount != questionIds.Count)
            {
                throw new Exception(
                    "Co najmniej jedno przypisane pytanie nie istnieje w bazie.");
            }
            if (LoggedUser.Id <= 0)
            {
                throw new Exception(
                    "Brak poprawnie zalogowanego użytkownika.");
            }
            bool authorExists = await _context.Uzytkownicy
    .AnyAsync(u => u.Id == LoggedUser.Id);

            if (!authorExists)
            {
                throw new Exception(
                    "Zalogowany użytkownik nie istnieje w bazie.");
            }
            if (LoggedUser.RolaId != 1 &&
    LoggedUser.RolaId != 2)
            {
                throw new Exception(
                    "Tylko administrator lub nauczyciel może utworzyć test.");
            }

            var test = new Test
            {
                Tytul = dto.Tytul,
                Opis = dto.Opis,
                IdTypuTestu = dto.TypTestuId,
                ProgZdania = dto.ProgZdania,
                DataUtworzenia = DateTime.Now,
                CzasTrwaniaMinuty = dto.CzasTrwaniaMinuty,
                LosujKolejnoscPytan = dto.LosujKolejnoscPytan,
                LosujKolejnoscOdpowiedzi = dto.LosujKolejnoscOdpowiedzi,
                IdAutora = LoggedUser.Id,
                TestPytania = dto.Pytania
                    .OrderBy(p => p.Kolejnosc)
                    .Select(p => new TestPytanie
                    {
                        PytanieId = p.PytanieId,
                        NumerKolejnosci = p.Kolejnosc
                    })
                    .ToList()
            };

            _context.Testy.Add(test);

            await _context.SaveChangesAsync();

            return test.Id;
        }

        public List<TestListItem> GetTestsForCurrentUser()
        {
            var query = _context.Testy
                .Include(p => p.Autor)
                .Include(p => p.TypTestu)
                .AsQueryable();

            if (LoggedUser.RolaId == 2)
            {
                query = query.Where(p => p.IdAutora == LoggedUser.Id);
            }

            return query
                .OrderByDescending(p => p.Id)
                .Select(p => new TestListItem
                {
                    Id = p.Id,
                    Tytul = p.Tytul,
                    IdTyp = p.IdTypuTestu,
                    Typ = p.TypTestu.NazwaTypu,
                    LiczbaPytan = p.TestPytania.Count,
                    CzasTrwaniaMinuty = p.CzasTrwaniaMinuty,
                    Prog = p.ProgZdania,
                    IdAutora = p.IdAutora,
                    Autor = p.Autor.Imie + " " + p.Autor.Nazwisko,
                    DataUtworzenia = p.DataUtworzenia,
                    CzyZarchiwizowany = p.CzyZarchiwizowany
                })
                .ToList();
        }

        public void RestoreTest(int testId)
        {
            var test = _context.Testy
                .FirstOrDefault(p => p.Id == testId);

            if (test == null)
                throw new Exception("Nie znaleziono testu.");

            test.CzyZarchiwizowany = false;

            _context.SaveChanges();
        }

        public void ArchiveTest(int testId)
        {
            var test = _context.Testy
                .FirstOrDefault(p => p.Id == testId);

            if (test == null)
                throw new Exception("Nie znaleziono testu.");

            test.CzyZarchiwizowany = true;

            _context.SaveChanges();
        }

        public void DeleteTest(int testId)
        {
            var test = _context.Testy
        .Include(t => t.TestPytania)
        .FirstOrDefault(t => t.Id == testId);

            if (test is null)
                throw new Exception("Nie znaleziono testu.");

            _context.TestPytania.RemoveRange(test.TestPytania);
            _context.Testy.Remove(test);

            _context.SaveChanges();
        }

        public async Task<TestDraft> GetTestDraftByIdAsync(int testId)
        {
            var test = await _context.Testy
                .Include(t => t.TestPytania)
                .ThenInclude(tp => tp.Pytanie)
                .ThenInclude(p => p.Odpowiedzi)
                .FirstOrDefaultAsync(t => t.Id == testId);
            if (test == null)
                throw new Exception("Nie znaleziono testu.");
            var testDraft = new TestDraft
            {
                TestId = test.Id,
                Tytul = test.Tytul,
                Opis = test.Opis,
                ProgZdania = test.ProgZdania,
                CzasTrwaniaMinuty = test.CzasTrwaniaMinuty,
                IdTypuTestu = test.IdTypuTestu,
                LosujKolejnoscPytan = test.LosujKolejnoscPytan,
                LosujKolejnoscOdpowiedzi = test.LosujKolejnoscOdpowiedzi,
                PrzypisanePytania = test.TestPytania
                    .OrderBy(tp => tp.NumerKolejnosci)
                    .Select(tp => new PrzypisanePytaniaDraft
                    {
                        PytanieId = tp.PytanieId,
                        TrescPytania = tp.Pytanie.TrescPytania,
                        TypPytania = tp.Pytanie.TypPytania,
                        LiczbaPunktow = tp.Pytanie.LiczbaPunktow,
                        SciezkaZdjecia = tp.Pytanie.SciezkaZdjecia,
                        Kolejnosc = tp.NumerKolejnosci
                    })
                    .ToList()
            };
            return testDraft;
        }

        public async Task<int> UpdateTestAsync(int testId, CreateTestDto dto)
        {
            if (dto == null)
                throw new ArgumentNullException(nameof(dto));

            if (dto.Pytania.Count == 0)
                throw new Exception("Test musi zawierać pytania.");

            bool testTypeExists = await _context.TypyTestow
    .AnyAsync(t => t.Id == dto.TypTestuId);

            if (!testTypeExists)
                throw new Exception("Wybrany typ testu nie istnieje.");

            var questionIds = dto.Pytania
    .Select(p => p.PytanieId)
    .Distinct()
    .ToList();

            if (questionIds.Count != dto.Pytania.Count)
            {
                throw new Exception(
                    "Ten sam test nie może zawierać powtórzonych pytań.");
            }

            int existingQuestionsCount = await _context.Pytania
    .CountAsync(p => questionIds.Contains(p.Id));

            if (existingQuestionsCount != questionIds.Count)
            {
                throw new Exception(
                    "Co najmniej jedno przypisane pytanie nie istnieje w bazie.");
            }

            if (LoggedUser.Id <= 0)
            {
                throw new Exception(
                    "Brak poprawnie zalogowanego użytkownika.");
            }

            if (LoggedUser.RolaId != 1 &&
                LoggedUser.RolaId != 2)
            {
                throw new Exception(
                    "Tylko administrator lub nauczyciel może edytować test.");
            }

            var test = await _context.Testy.FirstOrDefaultAsync(t => t.Id == testId);
            if (test == null)
                throw new Exception("Nie znaleziono testu.");

            if (LoggedUser.RolaId == 2 && test.IdAutora != LoggedUser.Id)
            {
                throw new Exception("Nauczyciel może edytować tylko własne testy.");
            }

            // Update test properties
            test.Tytul = dto.Tytul;
            test.Opis = dto.Opis;
            test.ProgZdania = dto.ProgZdania;
            test.CzasTrwaniaMinuty = dto.CzasTrwaniaMinuty;
            test.IdTypuTestu = dto.TypTestuId;
            test.LosujKolejnoscPytan = dto.LosujKolejnoscPytan;
            test.LosujKolejnoscOdpowiedzi = dto.LosujKolejnoscOdpowiedzi;

            // Update assigned questions
            var existingTestQuestions = await _context.TestPytania.Where(tp => tp.TestId == testId).ToListAsync();
            _context.TestPytania.RemoveRange(existingTestQuestions);

            foreach (var question in dto.Pytania)
            {
                var testQuestion = new TestPytanie
                {
                    TestId = testId,
                    PytanieId = question.PytanieId,
                    NumerKolejnosci = question.Kolejnosc
                };
                _context.TestPytania.Add(testQuestion);
            }

            await _context.SaveChangesAsync();
            return testId;
        }
    }
}