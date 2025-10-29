using Lection1024.Contexts;
using Lection1024.DTOs;
using Lection1024.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Lection1024.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class GamesController(GamesDbContext context) : ControllerBase
    {
        private readonly GamesDbContext _context = context;

        #region specific get

        [HttpGet("categories")] //?categories=cat1,cat2,...
        public async Task<ActionResult<IEnumerable<Game>>> GetGamesByCategories(string categories)
        {
            var values = categories.Split(',', StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries);
            return await _context.Games
                .Include(g => g.Category) //optional
                .Where(g => values.Contains(g.Category.Name))
                .ToListAsync();
        }

        [HttpGet("category")] //category=category
        public async Task<ActionResult<IEnumerable<Game>>> GetGamesByCategory(string name)
        {
            //tipo svyaz M:M
            var category = await _context.Categories.Include(c => c.Games).FirstOrDefaultAsync(c => c.Name == name);
            return category is null ? NotFound() : category.Games.ToList();
        }

        [HttpGet("price")] //?price=priceMin-priceMax
        public async Task<ActionResult<IEnumerable<Game>>> GetGamesByPrice(string price)
        {
            var values = price.Split('-');

            if (values.Length != 2)
                return BadRequest();

            int minPrice, maxPrice;
            if (!Int32.TryParse(values[0], out minPrice) || !Int32.TryParse(values[1], out maxPrice))
                return BadRequest();

            return await _context.Games.Where(g => g.Price >= minPrice && g.Price <= maxPrice).ToListAsync();
        }

        [HttpGet("filter")] //filters?filters=price:_,title:_
        public async Task<ActionResult<IEnumerable<Game>>> GetGamesByFilters(string filters)
        {
            var values = filters.Split(',', StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries);
            Dictionary<string,string> pairs = new();
            foreach (var v in values)
            {
                var pair = v.Split(':');
                pairs[pair[0]] = pair[1];
            }

            //.. tipa deistviya

            return await _context.Games.ToListAsync();
        }


        //[HttpGet("columns")] //columns?col1,col2,cal3,...
        //public async Task<ActionResult<IEnumerable<Game>>> GetColumns(string columns)
        //{

        //    return await _context.Database.SqlQuery($"").ToListAsync();
        //}


        #endregion

        // GET: api/Games
        [HttpGet("info")]
        public async Task<ActionResult<IEnumerable<GameDto>>> GetGamesInfo()
        {
            return await _context.Games.Select(g => g.ToDto())
                .ToListAsync();
        }
        // GET: api/Games
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Game>>> GetGames()
        {
            return await _context.Games.ToListAsync();
        }

        // GET: api/Games/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Game>> GetGame(int id)
        {
            var game = await _context.Games.FindAsync(id);

            if (game == null)
            {
                return NotFound();
            }

            return game;
        }

        // PUT: api/Games/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutGame(int id, Game game)
        {
            if (id != game.GameId)
            {
                return BadRequest();
            }

            _context.Entry(game).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!GameExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Games
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Game>> PostGame(Game game)
        {
            _context.Games.Add(game);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetGame", new { id = game.GameId }, game);
        }

        // DELETE: api/Games/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteGame(int id)
        {
            var game = await _context.Games.FindAsync(id);
            if (game == null)
            {
                return NotFound();
            }

            _context.Games.Remove(game);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool GameExists(int id)
        {
            return _context.Games.Any(e => e.GameId == id);
        }
    }
}
