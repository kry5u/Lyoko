using Microsoft.AspNetCore.Mvc;
using Lyoko.Models;
using Lyoko.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using Microsoft.CodeAnalysis.CSharp.Scripting;
using Microsoft.CodeAnalysis.Scripting;

namespace Lyoko.Controllers
{
    [Authorize]
    public class GameController : Controller
    {
        private readonly ApplicationDbContext _context;

        public GameController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Play(int levelId)
        {
            var level = await _context.Levels.FindAsync(levelId);
            if (level == null) return NotFound();

            ViewData["Title"] = level.Name;
            return View(level);
        }

        [HttpPost]
        public async Task<IActionResult> SubmitScore(int levelId, int input)
        {
            var level = await _context.Levels.FindAsync(levelId);
            if (level == null) return NotFound();

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var existingReplay = await _context.Replays
            .Include(r => r.User)
            .FirstOrDefaultAsync(r => r.LevelId == levelId && r.UserId == userId);


            bool isValid = await VerifyInput(level.Data, input);
            if (!isValid)
            {
                return RedirectToAction("Redirect", new
                {
                    message = "Niewłaściwy wynik.",
                    returnUrl = Url.Action("Play", new { levelId })
                });
            }

            int score = GenerateScore(input, level.Data);

            if (existingReplay != null && existingReplay.Score >= score)
            {
                return RedirectToAction("Redirect", new
                {
                    message = "Wynik jest mniejszy niż twój obecny wynik.",
                    returnUrl = Url.Action("Index", "Level")
                });
            }

            if (existingReplay != null)
            {
                _context.Replays.Remove(existingReplay);
            }

            var replay = new Replay
            {
                LevelId = levelId,
                UserId = userId,
                Input = input,
                Score = score
            };

            
            _context.Replays.Add(replay);
            await _context.SaveChangesAsync();

            return RedirectToAction("Redirect", new
            {
                message = "Wynik został przesłany.",
                returnUrl = Url.Action("Index", "Level")
            });

        }

        public async Task<IActionResult> Replay(int replayId)
        {
            var replay = await _context.Replays.Include(r => r.User).Include(r => r.Level).FirstOrDefaultAsync(r => r.Id == replayId);
            if (replay == null) return NotFound();

            ViewData["Title"] = "Powtórka dla poziomu: " + replay.Level.Name;
            return View(replay);
        }

        private async Task<bool> VerifyInput(string levelData, int input)
        {
            try
            {
                string expression = levelData.Replace("x", input.ToString());
                bool result = await CSharpScript.EvaluateAsync<bool>(
                expression,
                ScriptOptions.Default
            );
                return result;
            }
            catch
            {
                return false;
            }
        }

        private int GenerateScore(int input, string levelData)
        {

            int score = 4576 * input;

            return score;
        }

        public IActionResult Redirect(string message, string returnUrl)
        {
            var redirectViewModel = new Models.RedirectViewModel
            {
                Message = message,
                ReturnUrl = returnUrl
            };
            ViewData["Title"] = "Przekierowanie";
            return View("MessageRedirect", redirectViewModel);
        }

    }
}
