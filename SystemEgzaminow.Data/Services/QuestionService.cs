using Microsoft.EntityFrameworkCore;
using SystemEgzaminow.Core.Drafts;
using SystemEgzaminow.Core.DTO;
using SystemEgzaminow.Core.Enums;
using SystemEgzaminow.Core.Models;
using SystemEgzaminow.Core.Session;
using SystemEgzaminow.Data.Context;

namespace SystemEgzaminow.Data.Services
{
    public class QuestionService
    {
        private readonly AppDbContext _context;

        public QuestionService(AppDbContext context)
        {
            _context = context;
        }

        public int SaveSingleChoiceQuestion(QuestionDraft draft)
        {
            if (string.IsNullOrWhiteSpace(draft.TrescPytania))
                throw new Exception("Treść pytania nie może być pusta.");

            if (draft.LiczbaPunktow <= 0)
                throw new Exception("Liczba punktów musi być większa od 0.");

            if (draft.Odpowiedzi.Count < 2)
                throw new Exception("Pytanie jednokrotnego wyboru musi mieć co najmniej dwie odpowiedzi.");

            int liczbaPoprawnych = draft.Odpowiedzi.Count(o => o.CzyPoprawna);

            if (liczbaPoprawnych != 1)
                throw new Exception("Pytanie jednokrotnego wyboru musi mieć dokładnie jedną poprawną odpowiedź.");

            var pytanie = new Pytanie
            {
                TrescPytania = draft.TrescPytania,
                TypPytania = TypPytaniaEnum.JednokrotnyWybor,
                LiczbaPunktow = draft.LiczbaPunktow,
                IdAutora = draft.IdAutora,
                SciezkaZdjecia = draft.SciezkaZdjecia
            };

            _context.Pytania.Add(pytanie);
            _context.SaveChanges();

            foreach (var odpDraft in draft.Odpowiedzi)
            {
                var odpowiedz = new Odpowiedz
                {
                    TrescOdpowiedzi = odpDraft.TrescOdpowiedzi,
                    CzyPoprawna = odpDraft.CzyPoprawna,
                    LiczbaPunktow = odpDraft.LiczbaPunktow,
                    PytanieId = pytanie.Id
                };

                _context.Odpowiedzi.Add(odpowiedz);
            }

            _context.SaveChanges();
            return pytanie.Id;
        }

        public int SaveMultipleChoiceQuestion(QuestionDraft draft)
        {
            if (string.IsNullOrWhiteSpace(draft.TrescPytania))
                throw new Exception("Treść pytania nie może być pusta.");

            if (draft.LiczbaPunktow <= 0)
                throw new Exception("Liczba punktów musi być większa od 0.");

            if (draft.Odpowiedzi.Count < 2)
                throw new Exception("Pytanie wielokrotnego wyboru musi mieć co najmniej dwie odpowiedzi.");

            int liczbaPoprawnych = draft.Odpowiedzi.Count(o => o.CzyPoprawna);

            if (liczbaPoprawnych < 1)
                throw new Exception("Pytanie wielokrotnego wyboru musi mieć co najmniej jedną poprawną odpowiedź.");

            int sumaPunktow = draft.Odpowiedzi
                .Where(o => o.CzyPoprawna)
                .Sum(o => o.LiczbaPunktow);

            if (sumaPunktow != draft.LiczbaPunktow)
                throw new Exception("Suma punktów za poprawne odpowiedzi musi być równa liczbie punktów pytania.");

            var pytanie = new Pytanie
            {
                TrescPytania = draft.TrescPytania,
                TypPytania = TypPytaniaEnum.WielokrotnyWybor,
                LiczbaPunktow = draft.LiczbaPunktow,
                IdAutora = draft.IdAutora,
                SciezkaZdjecia = draft.SciezkaZdjecia
            };

            _context.Pytania.Add(pytanie);
            _context.SaveChanges();

            foreach (var odpDraft in draft.Odpowiedzi)
            {
                var odpowiedz = new Odpowiedz
                {
                    TrescOdpowiedzi = odpDraft.TrescOdpowiedzi,
                    CzyPoprawna = odpDraft.CzyPoprawna,
                    LiczbaPunktow = odpDraft.LiczbaPunktow,
                    PytanieId = pytanie.Id
                };

                _context.Odpowiedzi.Add(odpowiedz);
            }

            _context.SaveChanges();

            return pytanie.Id;
        }

        public int SaveOpenQuestion(QuestionDraft draft)
        {
            if (string.IsNullOrWhiteSpace(draft.TrescPytania))
                throw new Exception("Treść pytania nie może być pusta.");

            if (draft.LiczbaPunktow <= 0)
                throw new Exception("Liczba punktów musi być większa od 0.");

            if (draft.Odpowiedzi.Count != 1)
                throw new Exception("Pytanie otwarte musi mieć jedną odpowiedź wzorcową.");

            var pytanie = new Pytanie
            {
                TrescPytania = draft.TrescPytania,
                TypPytania = TypPytaniaEnum.Otwarte,
                LiczbaPunktow = draft.LiczbaPunktow,
                IdAutora = draft.IdAutora,
                SciezkaZdjecia = draft.SciezkaZdjecia
            };

            _context.Pytania.Add(pytanie);
            _context.SaveChanges();

            var odpDraft = draft.Odpowiedzi[0];

            var odpowiedz = new Odpowiedz
            {
                TrescOdpowiedzi = odpDraft.TrescOdpowiedzi,
                CzyPoprawna = true,
                LiczbaPunktow = draft.LiczbaPunktow,
                PytanieId = pytanie.Id
            };

            _context.Odpowiedzi.Add(odpowiedz);
            _context.SaveChanges();

            return pytanie.Id;
        }

        public List<QuestionListItem> GetQuestionsForCurrentUser()
        {
            var query = _context.Pytania
                .Include(p => p.Autor)
                .AsQueryable();

            if (LoggedUser.RolaId == 2)
            {
                query = query.Where(p => p.IdAutora == LoggedUser.Id);
            }

            return query
                .OrderByDescending(p => p.Id)
                .Select(p => new QuestionListItem
                {
                    Id = p.Id,
                    TrescPytania = p.TrescPytania,
                    TypPytania = p.TypPytania,
                    LiczbaPunktow = p.LiczbaPunktow,
                    IdAutora = p.IdAutora,
                    AutorPelnaNazwa = p.Autor.Imie + " " + p.Autor.Nazwisko,
                    SciezkaZdjecia = p.SciezkaZdjecia,

                    // Na razie false, dopóki nie dodamy statusu do modelu.
                    CzyZarchiwizowane = p.CzyZarchiwizowane
                })
                .ToList();
        }

        public void UpdateMultipleChoiceAnswers(
    int questionId,
    List<OdpowiedzDraft> answerDrafts)
        {
            var questionExists = _context.Pytania
                .Any(p => p.Id == questionId);

            if (!questionExists)
                throw new Exception("Nie znaleziono pytania.");

            if (answerDrafts.Count < 2)
                throw new Exception(
                    "Pytanie wielokrotnego wyboru musi mieć co najmniej dwie odpowiedzi.");

            if (!answerDrafts.Any(a => a.CzyPoprawna))
                throw new Exception(
                    "Co najmniej jedna odpowiedź musi być poprawna.");

            var existingAnswers = _context.Odpowiedzi
                .Where(o => o.PytanieId == questionId)
                .ToList();

            _context.Odpowiedzi.RemoveRange(existingAnswers);

            foreach (var draft in answerDrafts)
            {
                _context.Odpowiedzi.Add(new Odpowiedz
                {
                    PytanieId = questionId,
                    TrescOdpowiedzi = draft.TrescOdpowiedzi,
                    CzyPoprawna = draft.CzyPoprawna,
                    LiczbaPunktow = draft.LiczbaPunktow
                });
            }

            _context.SaveChanges();
        }

        public void UpdateSingleChoiceAnswers(
    int questionId,
    List<OdpowiedzDraft> answerDrafts)
        {
            bool questionExists = _context.Pytania
                .Any(p => p.Id == questionId);

            if (!questionExists)
                throw new Exception("Nie znaleziono pytania.");

            if (answerDrafts.Count < 2)
            {
                throw new Exception(
                    "Pytanie jednokrotnego wyboru musi mieć co najmniej dwie odpowiedzi.");
            }

            if (answerDrafts.Count(a => a.CzyPoprawna) != 1)
            {
                throw new Exception(
                    "Pytanie jednokrotnego wyboru musi mieć dokładnie jedną poprawną odpowiedź.");
            }

            var existingAnswers = _context.Odpowiedzi
                .Where(o => o.PytanieId == questionId)
                .ToList();

            _context.Odpowiedzi.RemoveRange(existingAnswers);

            foreach (var draft in answerDrafts)
            {
                _context.Odpowiedzi.Add(new Odpowiedz
                {
                    PytanieId = questionId,
                    TrescOdpowiedzi = draft.TrescOdpowiedzi,
                    CzyPoprawna = draft.CzyPoprawna,
                    LiczbaPunktow = draft.LiczbaPunktow
                });
            }

            _context.SaveChanges();
        }

        public void UpdateOpenAnswer(
   int questionId,
   string answerText,
   int points)
        {
            if (string.IsNullOrWhiteSpace(answerText))
            {
                throw new Exception(
                    "Odpowiedź wzorcowa nie może być pusta.");
            }

            var answer = _context.Odpowiedzi
                .FirstOrDefault(o => o.PytanieId == questionId);

            if (answer == null)
            {
                throw new Exception(
                    "Nie znaleziono odpowiedzi wzorcowej.");
            }

            answer.TrescOdpowiedzi = answerText.Trim();
            answer.CzyPoprawna = true;
            answer.LiczbaPunktow = points;

            _context.SaveChanges();
        }

        public void RestoreQuestion(int questionId)
        {
            var question = _context.Pytania
                .FirstOrDefault(p => p.Id == questionId);

            if (question == null)
                throw new Exception("Nie znaleziono pytania.");

            question.CzyZarchiwizowane = false;

            _context.SaveChanges();
        }

        public void ArchiveQuestion(int questionId)
        {
            var question = _context.Pytania
                .FirstOrDefault(p => p.Id == questionId);

            if (question == null)
                throw new Exception("Nie znaleziono pytania.");

            question.CzyZarchiwizowane = true;

            _context.SaveChanges();
        }

        public void DeleteQuestion(int questionId)
        {
            var question = _context.Pytania
                .Include(p => p.Odpowiedzi)
                .FirstOrDefault(p => p.Id == questionId);

            if (question == null)
                throw new Exception("Nie znaleziono pytania.");

            bool isAssignedToTest = _context.TestPytania
                .Any(tp => tp.PytanieId == questionId);

            if (isAssignedToTest)
            {
                throw new Exception(
                    "Nie można usunąć pytania, ponieważ jest przypisane do co najmniej jednego testu.");
            }

            _context.Odpowiedzi.RemoveRange(question.Odpowiedzi);
            _context.Pytania.Remove(question);

            _context.SaveChanges();
        }
    }
}