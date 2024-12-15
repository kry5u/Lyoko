using Lyoko.Models;

namespace Lyoko.Data
{
    public static class DbSeeder
    {
        public static async Task SeedLevels(ApplicationDbContext context)
        {
            if (context.Levels.Any())
                return;


            var levels = new List<Level>
        {
            new Level { Name = "the basics", Data = "x < 213 && x % 13 == 0" },
            new Level { Name = "a hint of whats to come", Data = "x < 717 && x % 5 == 3 && (400 / x) % 23 == 0" },
            new Level { Name = "wild language", Data = "x < 2564 && x % 45 == 0 && (x*x) % 6 == 0" },
            new Level { Name = "end sinister", Data = "x < 9876 && (14320 / x) > 65 && (x*x*x) % 7 == 0" },
            new Level { Name = "the tenth circle of hell", Data = "x < 65874 && (x / 17 - 213) % 17 == 0 && (x + 73) % 11 == 0 && x % 19 == 0" },
        };

            await context.Levels.AddRangeAsync(levels);
            await context.SaveChangesAsync();
        }
    }
}

