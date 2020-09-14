using MongoApi.Models;
using MongoApi.Services;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace MongoApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MatchController : ControllerBase
    {
        private readonly MatchService _matchService;

        public MatchController(MatchService matchService)
        {
            _matchService = matchService;
        }

        [HttpGet]
        public ActionResult<List<Match>> GetTopTenMatches(MatchDuration duration) 
        {
            _matchService.HowManyGamesPlayed();
            _matchService.AverageScore();
            return _matchService.GetTopTenMatches(duration);
        }

        [HttpGet("{id:length(24)}", Name = "GetMatch")]
        public ActionResult<List<Match>> GetAllMatchesFromPlayer(string id)
        {
            var match = _matchService.GetAllMatchesFromPlayer(id);

            if (match == null)
            {
                return NotFound();
            }

            return match;
        }

        [HttpPost("{playerId:length(24)}")]
        public ActionResult<Player> CreateMatch(string playerId, Match match)
        {
            return _matchService.CreateMatch(playerId,match);
        }

        // [HttpPut("{id:length(24)}")]
        // public IActionResult Update(string id, Match math)
        // {
        //     // var player = _playerService.Get(id);

        //     // if (player == null)
        //     // {
        //     //     return NotFound();
        //     // }

        //     // _matchService.CreateMatch(id, updatedPlayer);

        //     return NoContent();
        // }

        // [HttpDelete("{id:length(24)}")]
        // public IActionResult Delete(string id)
        // {
        //     // var match = _matchService.Get(id);

        //     // if (player == null)
        //     // {
        //     //     return NotFound();
        //     // }

        //     // _playerService.Remove(player.Id);

        //     return NoContent();
        // }
    }
}