using MongoApi.Models;
using MongoApi.Services;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace MongoApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PlayerController : ControllerBase
    {
        private readonly PlayerService _playerService;

        public PlayerController(PlayerService playerService)
        {
            _playerService = playerService;
        }

        [HttpGet]
        public ActionResult<List<Player>> Get() =>
            _playerService.Get();

        [HttpGet("{id:length(24)}", Name = "GetPlayer")]
        public ActionResult<Player> Get(string id)
        {
            var player = _playerService.Get(id);

            if (player == null)
            {
                return NotFound();
            }

            return player;
        }

        [HttpPost]
        public ActionResult<Player> Create(Player player)
        {
            _playerService.Create(player);

            return CreatedAtRoute("GetPlayer", new { id = player.Id.ToString() }, player);
        }

        [HttpPut("{id:length(24)}")]
        public IActionResult Update(string id, Player updatedPlayer)
        {
            var player = _playerService.Get(id);

            if (player == null)
            {
                return NotFound();
            }

            _playerService.Update(id, updatedPlayer);

            return NoContent();
        }

        [HttpDelete("{id:length(24)}")]
        public IActionResult Delete(string id)
        {
            var player = _playerService.Get(id);

            if (player == null)
            {
                return NotFound();
            }

            _playerService.Remove(player.Id);

            return NoContent();
        }
    }
}