using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace MySQLapi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MatchController : ControllerBase
    {
        private readonly MatchContext _context;

        public MatchController(MatchContext context)
        {
            _context = context;
        }

        // GET: api/Matches
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Match>>> GetTopTenMatches(MatchDuration duration)
        {
            var myMatch = await _context.match.FromSqlRaw(
                "SELECT * FROM `match`WHERE `match`.`date`>= CAST({0} AS DATE) AND `match`.`date` <= CAST({1} AS DATE) "+
                "ORDER BY `match`.`score` DESC LIMIT 10;", duration.FromDate, duration.ToDate).ToListAsync();
            if (myMatch == null)
            {
                return NotFound();
            }

            return myMatch;
        }

        // GET: api/Match/5
        [HttpGet("{id}")]
        public async Task<ActionResult<IEnumerable<Match>>> GetAllMatchesFromPlayer(int id)
        {
            var myMatch = await _context.match.FromSqlRaw("SELECT * FROM `match` WHERE `match`.`player_id` = {0};",id).ToListAsync();
            if (myMatch == null)
            {
                return NotFound();
            }

            return myMatch;
        }

        // PUT: api/Match/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutMatch(int id, Match match)
        {
            if (id != match.match_id)
            {
                return BadRequest();
            }

            _context.Entry(match).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!MatchExists(id))
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

        // POST: api/Match
        [HttpPost]
        public async Task<ActionResult<Match>> Postmatch(Match match)
        {
            _context.match.Add(match);
            await _context.SaveChangesAsync();

            return CreatedAtAction("Getmatch", new { id = match.match_id }, match);
        }

        // DELETE: api/Match/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<IEnumerable<Match>>> DeleteAllPlayerMatch(int id)
        {
            var myMatches = await _context.match.FromSqlRaw("SELECT * FROM `match` WHERE `match`.`player_id` = {0};",id).ToListAsync();
            if (myMatches == null)
            {
                return NotFound();
            }

            _context.match.RemoveRange(myMatches);
            await _context.SaveChangesAsync();

            return myMatches;
        }

        private bool MatchExists(int id)
        {
            return _context.match.Any(e => e.match_id == id);
        }
    }
}
