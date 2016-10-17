using Microsoft.AspNetCore.Mvc;
using Mippa.Models;
using System.Collections.Generic;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace Mippa.Controllers.Api
{
    [Route("api/[controller]")]
    public class TeamController : Controller
    {
        // TODO: What the hell goes here?
        public IRepository _repository { get; set; }

        public TeamController(IRepository repo)
        {
            _repository = repo;
        }

        /// <summary>
        /// Returns a list of all Teams for a given Session
        /// </summary>
        /// <returns></returns>
        [HttpGet("{sessionId}")]
        public IEnumerable<Team> GetAll(int sessionId)
        {
            return _repository.GetAllTeamsFromSession(sessionId);
        }

        /// <summary>
        /// Returns a list of all Teams for a given Session
        /// </summary>
        /// <returns></returns>
        public IEnumerable<Team> GetAll()
        {
            return _repository.GetAllTeams();
        }

        /// <summary>
        /// Obtain a specific Team by name from a given Session
        /// </summary>
        /// <param name="sessionId"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        [HttpGet("{sessionId}/{name}", Name = "GetTeam")]
        public IActionResult GetByNameFromSessionId(int sessionId, string name)
        {
            var team = _repository.GetTeamFromSessionByName(name, sessionId);

            if (team == null)
            {
                return NotFound();
            }
            return new ObjectResult(team);
        }

        /// <summary>
        /// Creates a new Team under a given Session
        /// </summary>
        /// <param name="team"></param>
        /// <param name="sessionId"></param>
        /// <returns></returns>
        [HttpPost("{sessionId}")]
        public IActionResult Create([FromBody] Team team, int sessionId)
        {
            if (team == null || team.SessionId != sessionId)
            {
                return BadRequest();
            }
            _repository.AddTeamToSession(team, sessionId);
            return CreatedAtRoute("GetTeam", new { name = team.Name, sessionId = sessionId }, team);
        }

        /// <summary>
        /// Update Team properties
        /// </summary>
        /// <param name="teamId"></param>
        /// <param name="team"></param>
        /// <returns></returns>
        [HttpPut("{teamId}")]
        public IActionResult Update(int teamId, [FromBody] Team team)
        {
            if (team == null || team.TeamId != teamId)
            {
                return BadRequest();
            }

            var teamToUpdate = _repository.GetTeamById(teamId);

            if (teamToUpdate == null)
            {
                return NotFound();
            }

            _repository.UpdateTeam(team);
            return new NoContentResult();
        }

        /// <summary>
        /// Delete a Team from the database
        /// </summary>
        /// <param name="id"></param>
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
            _repository.RemoveTeam(id);
        }
    }
}
