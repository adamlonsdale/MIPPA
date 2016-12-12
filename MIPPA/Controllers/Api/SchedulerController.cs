using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MIPPA.ViewModels.Scheduler;
using Mippa.Models;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace MIPPA.Controllers.Api
{
    [Route("api/[controller]")]
    public class SchedulerController : Controller
    {
        private IRepository _repository;

        public SchedulerController(IRepository repository)
        {
            _repository = repository;
        }

        // GET: api/values
        [HttpGet("{sessionId}/{scheduleIndex}")]
        public WeekViewModel Get(int sessionId, int scheduleIndex)
        {
            return _repository.GetWeekViewModel(sessionId, scheduleIndex);
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/values
        [HttpPost("{sessionId}/{scheduleIndex}")]
        public IActionResult Post(int sessionId, int scheduleIndex, [FromBody]WeekViewModel viewModel)
        {
            _repository.PostMatchups(sessionId, scheduleIndex, viewModel);

            return new JsonResult("Success");
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
