using Microsoft.AspNetCore.Mvc;
using Mippa.Models;
using System.Collections.Generic;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace Mippa.Controllers.Api
{
    [Route("api/[controller]")]
    public class TeamRosterController : Controller
    {
        // TODO: What the hell goes here?
        public IRepository _repository { get; set; }

        public TeamRosterController(IRepository repo)
        {
            _repository = repo;
        }

        /// <summary>
        /// Returns a list of all Players assigned for a given Team
        /// </summary>
        /// <returns></returns>
        [HttpGet("{teamId}")]
        public IEnumerable<Player> GetAll(int teamId)
        {
            return _repository.GetAllPlayersFromTeam(teamId);
        }

        /// <summary>
        /// Obtain a specific Player on a Team
        /// </summary>
        /// <param name="playerId">The primary key of the Manager</param>
        /// <param name="teamId"></param>
        /// <returns>ObjectResult of Manager</returns>
        [HttpGet("{playerId}/{teamId}", Name = "GetPlayerFromTeam")]
        public IActionResult GetById(int playerId, int teamId)
        {
            var player = _repository.GetPlayerFromTeam(playerId, teamId);

            if (player == null)
            {
                return NotFound();
            }
            return new ObjectResult(player);
        }

        /// <summary>
        /// Creates a new Player under a given Team
        /// </summary>
        /// <param name="player"></param>
        /// <param name="teamId"></param>
        /// <returns></returns>
        [HttpPost("{teamId}")]
        public IActionResult Create([FromBody] Player player, int teamId)
        {
            if (player == null)
            {
                return BadRequest();
            }
            _repository.AddPlayerToTeam(player, teamId);
            return CreatedAtRoute("GetPlayerFromTeam", new { teamId = teamId, playerId = player.PlayerId }, player);
        }

        /// <summary>
        /// Delete a Player from a team in the database
        /// </summary>
        /// <param name="playerId"></param>
        /// <param name="teamId"></param>
        [HttpDelete("{playerId}/{teamId}")]
        public void Delete(int playerId, int teamId)
        {
            _repository.RemovePlayerFromTeam(playerId, teamId);
        }
    }
}
