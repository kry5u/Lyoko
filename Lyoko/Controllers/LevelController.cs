using Microsoft.AspNetCore.Mvc;
using Lyoko.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace ProgramowanieZaawansowane.Controllers
{
    [Authorize]
    public class LevelController : Controller
    {
        private readonly ApplicationDbContext _context;

        public LevelController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            ViewData["Title"] = "Przeglądaj poziomy";
            var levels = await _context.Levels.ToListAsync();
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var scores = await _context.Replays
            .Where(r => r.UserId == userId)
            .GroupBy(r => r.LevelId)
            .Select(g => g.FirstOrDefault())
            .ToListAsync();

            var levelWithScores = levels.Select(level => new
            {
                Level = level,
                PlayerScore = scores.FirstOrDefault(s => s.LevelId == level.Id)?.Score
            });

            return View(levelWithScores);
        }

        public async Task<IActionResult> GetRanking(int id)
        {

            var replays = await _context.Replays
                .Include(r => r.User)
                .Where(r => r.LevelId == id)
                .OrderByDescending(r => r.Score)
                .Take(20)
                .ToListAsync();

            return PartialView("_Ranking", replays);
        }

        public IActionResult Play(int levelId)
        {
            return RedirectToAction("Play", "Game", new { levelId });
        }

        public IActionResult Replay(int replayId)
        {
            return RedirectToAction("Replay", "Game", new { replayId });
        }
    }
}
