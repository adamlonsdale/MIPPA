﻿using Microsoft.AspNetCore.Mvc;
using Mippa.Models;
using MIPPA.ViewModels;
using System.Collections.Generic;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace Mippa.Controllers.Api
{
    [Route("api/[controller]")]
    public class SessionController : Controller
    {
        // TODO: What the hell goes here?
        public IRepository _repository { get; set; }

        public SessionController(IRepository repo)
        {
            _repository = repo;
        }

        /// <summary>
        /// Returns a list of all Sessions under a given Manager
        /// </summary>
        /// <returns>IEnumerable of Manager</returns>
        [HttpGet("{managerId}")]
        public IEnumerable<ManageSessionViewModel> GetAll(int managerId)
        {
            return _repository.GetAllSessions(managerId);
        }

        /// <summary>
        /// Creates a new Session under a league Manager
        /// </summary>
        /// <param name="session">Client-side Session</param>
        /// <param name="managerId"></param>
        /// <returns></returns>
        [HttpPost("{managerId}")]
        public IActionResult Create([FromBody] Session session, int managerId)
        {
            if (session == null || session.ManagerId != managerId)
            {
                return BadRequest();
            }
            _repository.AddSession(session, managerId);
            return CreatedAtRoute("GetSession", new { managerId = managerId, sessionId = session.SessionId }, session);
        }

        /// <summary>
        /// Update Session properties
        /// </summary>
        /// <param name="sessionId">Primary key of the Session</param>
        /// <param name="session">Client-side session</param>
        /// <returns></returns>
        [HttpPut("{sessionId}")]
        public IActionResult Update(int sessionId, [FromBody] Session session)
        {
            if (session == null || session.SessionId != sessionId)
            {
                return BadRequest();
            }

            var sessionToUpdate = _repository.GetSession(sessionId);

            if (sessionToUpdate == null)
            {
                return NotFound();
            }

            _repository.UpdateSession(session);
            return new NoContentResult();
        }

        /// <summary>
        /// Delete a Session from the database
        /// </summary>
        /// <param name="sessionId"></param>
        [HttpDelete("{sessionId}")]
        public void Delete(int sessionId)
        {
            _repository.RemoveSession(sessionId);
        }
    }
}
