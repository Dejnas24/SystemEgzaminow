using Microsoft.EntityFrameworkCore;
using SystemEgzaminow.Core.DTO;
using SystemEgzaminow.Core.Models;
using SystemEgzaminow.Core.Session;
using SystemEgzaminow.Data.Context;

namespace SystemEgzaminow.Data.Services
{
    public class AssignmentService
    {
        private readonly AppDbContext _context;

        public AssignmentService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<AssignTestListItemDto>> GetTestsAsync(int? typTestuId = null)
        {
            var query = _context.Testy
                .AsNoTracking()
                .Where(t => !t.CzyZarchiwizowany);

            if (LoggedUser.RolaId == 2)
            {
                query = query.Where(t => t.IdAutora == LoggedUser.Id);
            }

            if (typTestuId.HasValue)
            {
                query = query.Where(t => t.IdTypuTestu == typTestuId.Value);
            }

            return await query
                .Select(t => new AssignTestListItemDto
                {
                    TestId = t.Id,
                    Tytul = t.Tytul,
                    TypTestu = t.TypTestu.NazwaTypu,
                    LiczbaPytan = t.TestPytania.Count,
                    CzasTrwaniaMinuty = t.CzasTrwaniaMinuty
                })
                .OrderBy(t => t.Tytul)
                .ToListAsync();
        }

        public async Task<List<AssignmentRecipientDto>> GetStudentsAsync()
        {
            return await _context.Uzytkownicy
                .AsNoTracking()
                .Where(u => u.RolaId == 3)
                .Select(u => new AssignmentRecipientDto
                {
                    Id = u.Id,
                    Nazwa = u.Imie + " " + u.Nazwisko,
                    Login = u.Login,

                    KlasaGrupa = u.Klasa != null
                        ? u.Klasa.NazwaKlasy + " / " + u.Klasa.RokSzkolny
                        : "Brak klasy"
                })
                .OrderBy(u => u.Nazwa)
                .ToListAsync();
        }

        public async Task<List<AssignmentRecipientDto>> GetClassesAsync()
        {
            return await _context.Klasy
                .AsNoTracking()
                .Where(k => k.Uczniowie.Any())
                .Select(k => new AssignmentRecipientDto
                {
                    Id = k.Id,
                    Nazwa = k.NazwaKlasy,
                    Login = string.Empty,
                    KlasaGrupa = k.RokSzkolny
                })
                .OrderByDescending(k => k.KlasaGrupa)
                .ThenBy(k => k.Nazwa)
                .ToListAsync();
        }

        public async Task AssignTestAsync(AssignTestDto dto, int nauczycielId)
        {
            if (dto == null)
                throw new ArgumentNullException(nameof(dto));

            if (dto.UczenIds.Count == 0)
                throw new InvalidOperationException(
                    "Nie wybrano żadnego ucznia.");

            if (dto.DataWygasniecia <= dto.DataDostepnosci)
                throw new InvalidOperationException(
                    "Data wygaśnięcia musi być późniejsza niż data dostępności.");

            if (dto.LiczbaProb < 1)
                throw new InvalidOperationException(
                    "Liczba prób musi być większa od zera.");

            var uczenIds = dto.UczenIds
                .Distinct()
                .ToList();

            var uczniowie = await _context.Uzytkownicy
    .AsNoTracking()
    .Where(u =>
        uczenIds.Contains(u.Id) &&
        u.RolaId == 3)
    .Select(u => new
    {
        u.Id,
        u.KlasaId
    })
    .ToListAsync();

            if (uczniowie.Count != uczenIds.Count)
                throw new InvalidOperationException(
                    "Co najmniej jeden wybrany uczeń nie istnieje lub nie ma roli ucznia.");

            foreach (var uczen in uczniowie)
            {
                var przypisanyTest = new PrzypisanyTest
                {
                    TestId = dto.TestId,
                    UczenId = uczen.Id,

                    KlasaId = uczen.KlasaId,

                    NauczycielId = nauczycielId,

                    DataPrzypisania = DateTime.Now,
                    DataDostepnosci = dto.DataDostepnosci,
                    DataWygasniecia = dto.DataWygasniecia,

                    Status = dto.Status,
                    LiczbaProb = dto.LiczbaProb,

                    SposobOceniania = dto.SposobOceniania,

                    SposobWyswietlaniaWyniku =
                        dto.SposobWyswietlaniaWyniku,

                    CzyPokazacWynikPoZakonczeniu =
                        dto.CzyPokazacWynikPoZakonczeniu,

                    InformacjaStartowa =
                        dto.InformacjaStartowa,

                    InformacjaKoncowa =
                        dto.InformacjaKoncowa
                };

                _context.PrzypisaneTesty.Add(przypisanyTest);
            }

            await _context.SaveChangesAsync();
        }

        public async Task<List<int>> GetStudentIdsForClassesAsync(
    IEnumerable<int> klasaIds)
        {
            var ids = klasaIds
                .Distinct()
                .ToList();

            return await _context.Uzytkownicy
                .AsNoTracking()
                .Where(u =>
                    u.KlasaId.HasValue &&
                    ids.Contains(u.KlasaId.Value) &&
                    u.RolaId == 3)
                .Select(u => u.Id)
                .Distinct()
                .ToListAsync();
        }

        public async Task<List<AssignTestListItemDto>> GetByClassAndTypeAsync(int classId, int testTypeId)
        {
            return await _context.PrzypisaneTesty
      .AsNoTracking()
      .Where(pt => pt.KlasaId == classId &&
                   pt.Test.IdTypuTestu == testTypeId)
      .Select(pt => new
      {
          pt.TestId,
          pt.Test.Tytul,
          TypTestu = pt.Test.TypTestu.NazwaTypu
      })
      .Distinct()
      .OrderBy(t => t.Tytul)
      .Select(t => new AssignTestListItemDto
      {
          TestId = t.TestId,
          Tytul = t.Tytul,
          TypTestu = t.TypTestu
      })
      .ToListAsync();

        }
    }
}