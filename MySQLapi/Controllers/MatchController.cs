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
        private readonly PlayerContext _playerContext;

        public MatchController(MatchContext context,PlayerContext playerContext)
        {
            _context = context;
            _playerContext = playerContext;
        }

        // GET: api/Matches
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Match>>> GetTopTenMatches(MatchDuration duration)
        {
            _context.HowManyGamesPlayed();
            _context.AverageScore();
            // var myMatch = await _context.match.FromSqlRaw(
            //     "SELECT * FROM `match`WHERE `match`.`date`>= CAST({0} AS DATE) AND `match`.`date` <= CAST({1} AS DATE) " +
            //     "ORDER BY `match`.`score` DESC LIMIT 10;", duration.FromDate, duration.ToDate).ToListAsync();
            var myMatch = await _context.match.Where(x => x.date >= duration.FromDate & x.date <= duration.ToDate)
                .OrderByDescending(x => x.score).Take(10).ToListAsync();
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
            // var myMatch = await _context.match.FromSqlRaw("SELECT * FROM `match` WHERE `match`.`player_id` = {0};", id).ToListAsync();
            var myMatch = await _context.match.Where(x => x.player_id == id).ToListAsync();
            if (myMatch == null)
            {
                return NotFound();
            }

            return myMatch;
        }

        // PUT: api/Match/5
        [HttpPut("{id}")]
        public async Task<ActionResult<IEnumerable<TopTenMatch>>> Put_GetTopTenMatches(int id, MatchDuration duration)
        {
            var myMatches = await _context.match.Where(x => x.date >= duration.FromDate & x.date <= duration.ToDate)
                            .OrderByDescending(x => x.score).Take(10).ToListAsync();
            if (myMatches == null)
            {
                return NotFound();
            }

            // Create new list with player nick names
            List<TopTenMatch> returnMatches = new List<TopTenMatch>();
            foreach (var match in myMatches)
            {
                TopTenMatch newMatch = new TopTenMatch();
                var player = await _playerContext.player.FindAsync(match.player_id);
                newMatch.level_number = match.level_number;
                newMatch.score = match.score;
                newMatch.date = match.date;
                newMatch.player_nickname = player.nickname;
                returnMatches.Add(newMatch);
            }
            return returnMatches;
        }

        // POST: api/Match
        [HttpPost]
        public async Task<ActionResult<Match>> Postmatch(Match match)
        {
            Match newMatch = new Match();
            newMatch.level_number = match.level_number;
            newMatch.score = match.score;
            newMatch.date = match.date;
            newMatch.player_id = match.player_id;
            _context.match.Add(newMatch);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // DELETE: api/Match/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<IEnumerable<Match>>> DeleteAllPlayerMatch(int id)
        {
            // var myMatches = await _context.match.FromSqlRaw("SELECT * FROM `match` WHERE `match`.`player_id` = {0};", id).ToListAsync();
            var myMatches = await _context.match.Where(x => x.player_id == id).ToListAsync();
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
