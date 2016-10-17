using Microsoft.AspNetCore.Mvc;
using Mippa.Models;
using System.Collections.Generic;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace Mippa.Controllers.Api
{
    [Route("api/[controller]")]
    public class LeagueManagerController : Controller
    {
        // TODO: What the hell goes here?
        public IRepository _repository { get; set; }

        public LeagueManagerController(IRepository repo)
        {
            _repository = repo;
        }

        /// <summary>
        /// Returns a list of all Managers who manage leagues
        /// </summary>
        /// <returns>IEnumerable of Manager</returns>
        [HttpGet]
        public IEnumerable<Manager> GetAll()
        {
            return _repository.GetAllManagers();
        }

        /// <summary>
        /// Obtain a specific Manager by ManagerId
        /// </summary>
        /// <param name="managerId">The primary key of the Manager</param>
        /// <returns>ObjectResult of Manager</returns>
        [HttpGet("{managerId}", Name = "GetManager")]
        public IActionResult GetById(int managerId)
        {
            var manager = _repository.GetManager(managerId);

            if (manager == null)
            {
                return NotFound();
            }
            return new ObjectResult(manager);
        }

        /// <summary>
        /// Creates a new Manager who can manage a league
        /// </summary>
        /// <param name="manager">Client-side Manager</param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult Create([FromBody] Manager manager)
        {
            if (manager == null)
            {
                return BadRequest();
            }
            _repository.AddManager(manager);
            return CreatedAtRoute("GetManager", new { managerId = manager.ManagerId }, manager);
        }

        /// <summary>
        /// Update Manager properties
        /// </summary>
        /// <param name="managerId">Primary key of the Manager</param>
        /// <param name="manager">Client-side manager</param>
        /// <returns></returns>
        [HttpPut("{managerId}")]
        public IActionResult Update(int managerId, [FromBody] Manager manager)
        {
            if (manager == null || manager.ManagerId != managerId)
            {
                return BadRequest();
            }

            var managerToUpdate = _repository.GetManager(managerId);

            if (managerToUpdate == null)
            {
                return NotFound();
            }

            _repository.UpdateManager(manager);
            return new NoContentResult();
        }

        /// <summary>
        /// Delete a Manager from the database
        /// </summary>
        /// <param name="managerId"></param>
        [HttpDelete("{managerId}")]
        public void Delete(int managerId)
        {
            _repository.RemoveManager(managerId);
        }
    }
}
