using Microsoft.AspNetCore.Mvc;
using Mippa.Models;
using System.Collections.Generic;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace Mippa.Controllers.Api
{
    [Route("api/[controller]")]
    public class PlayerController : Controller
    {
        // TODO: What the hell goes here?
        public IRepository _repository { get; set; }

        public PlayerController(IRepository repo)
        {
            _repository = repo;
        }

        /// <summary>
        /// Returns a list of all Players in league
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IEnumerable<Player> GetAll([FromQuery] string term)
        {
            if (string.IsNullOrWhiteSpace(term))
            {
                return _repository.GetAllPlayers();
            }

            return _repository.GetPlayersFromQuery(term);
        }

        /// <summary>
        /// Obtain a specific Player by PlayerId
        /// </summary>
        /// <param name="playerId"></param>
        /// <returns></returns>
        [HttpGet("{playerId}", Name = "GetPlayer")]
        public IActionResult GetById(int playerId)
        {
            var player = _repository.GetPlayer(playerId);

            if (player == null)
            {
                return NotFound();
            }
            return new ObjectResult(player);
        }

        /// <summary>
        /// Creates a new Player
        /// </summary>
        /// <param name="player"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult Create([FromBody] Player player)
        {
            if (player == null)
            {
                return BadRequest();
            }
            _repository.AddPlayer(player);
            return CreatedAtRoute("GetPlayer", new { playerId = player.PlayerId }, player);
        }

        /// <summary>
        /// Update Player properties
        /// </summary>
        /// <param name="playerId"></param>
        /// <param name="player"></param>
        /// <returns></returns>
        [HttpPut("{playerId}")]
        public IActionResult Update(int playerId, [FromBody] Player player)
        {
            if (player == null || player.PlayerId != playerId)
            {
                return BadRequest();
            }

            var playerToUpdate = _repository.GetPlayer(playerId);

            if (playerToUpdate == null)
            {
                return NotFound();
            }

            _repository.UpdatePlayer(player);
            return new NoContentResult();
        }

        /// <summary>
        /// Delete a Player from the database
        /// </summary>
        /// <param name="playerId"></param>
        [HttpDelete("{playerId}")]
        public void Delete(int playerId)
        {
            _repository.RemovePlayer(playerId);
        }
    }
}
