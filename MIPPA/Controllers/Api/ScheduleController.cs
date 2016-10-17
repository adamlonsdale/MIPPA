using Microsoft.AspNetCore.Mvc;
using Mippa.Models;
using Mippa.Utilities;
using System.Collections.Generic;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace Mippa.Controllers.Api
{
    [Route("api/[controller]")]
    public class ScheduleController : Controller
    {
        // TODO: What the hell goes here?
        public IRepository _repository { get; set; }

        public ScheduleController(IRepository repo)
        {
            _repository = repo;
        }

        /// <summary>
        /// Returns a list of all Schedules in league
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IEnumerable<Schedule> GetAll()
        {
            return _repository.GetAllSchedules();
        }

        /// <summary>
        /// Obtain all schedules for a given session
        /// </summary>
        /// <param name="sessionId"></param>
        /// <returns></returns>
        [HttpGet("{sessionId}")]
        public IEnumerable<Schedule> GetAllFromSession(int sessionId)
        {
            return _repository.GetAllSchedulesFromSession(sessionId);
        }

        /// <summary>
        /// Obtain a specific Schedule by ScheduleId
        /// </summary>
        /// <param name="scheduleId"></param>
        /// <returns></returns>
        //[HttpGet("{scheduleId}", Name = "GetSchedule")]
        //public IActionResult GetById(int scheduleId)
        //{
        //    var schedule = _repository.GetScheduleById(scheduleId);

        //    if (schedule == null)
        //    {
        //        return NotFound();
        //    }
        //    return new ObjectResult(schedule);
        //}

        /// <summary>
        /// Obtain a specific schedule based on ScheduleId for a given Session
        /// </summary>
        /// <param name="scheduleId"></param>
        /// <param name="sessionId"></param>
        /// <returns></returns>
        [HttpGet("{scheduleId}/{sessionId}", Name = "GetScheduleFromSession")]
        public IActionResult GetByIdFromSession(int scheduleId, int sessionId)
        {
            var schedule = _repository.GetScheduleByIdFromSession(scheduleId, sessionId);

            if (schedule == null)
            {
                return NotFound();
            }
            return new ObjectResult(schedule);
        }

        /// <summary>
        /// Creates a new Schedule in a Session
        /// </summary>
        /// <param name="schedule"></param>
        /// <param name="sessionId"></param>
        /// <returns></returns>
        [HttpPost("{sessionId}")]
        public IEnumerable<Schedule> Create([FromBody] Scheduler scheduler, int sessionId)
        {
            if (scheduler == null || scheduler.SessionId != sessionId)
            {
                return null;
            }
            Common.ScheduleSession(_repository, scheduler, 1);

            return GetAllFromSession(sessionId);
        }

        /// <summary>
        /// Update Schedule properties
        /// </summary>
        /// <param name="scheduleId"></param>
        /// <param name="schedule"></param>
        /// <returns></returns>
        [HttpPut("{playerId}")]
        public IActionResult Update(int scheduleId, [FromBody] Schedule schedule)
        {
            if (schedule == null || schedule.ScheduleId != scheduleId)
            {
                return BadRequest();
            }

            var scheduleToUpdate = _repository.GetScheduleById(scheduleId);

            if (scheduleToUpdate == null)
            {
                return NotFound();
            }

            //_repository.UpdateSchedule(schedule);
            return new NoContentResult();
        }

        /// <summary>
        /// Delete a Schedule from the database
        /// </summary>
        /// <param name="scheduleId"></param>
        [HttpDelete("{scheduleId}")]
        public void Delete(int scheduleId)
        {
            _repository.RemoveSchedule(scheduleId);
        }
    }
}
