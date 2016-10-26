using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Mippa.Models;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace MIPPA.Controllers.Api
{
    [Route("api/[controller]")]
    public class ResetRequestController : Controller
    {
        private IRepository _repository;

        public ResetRequestController(IRepository repo)
        {
            _repository = repo;
        }

        // GET: api/values
        [HttpGet("{scorecardId?}")]
        public IActionResult Get(int? scorecardId = null)
        {
            if (scorecardId.HasValue)
            {
                _repository.ResetScorecard(scorecardId.Value);
            }

            return new JsonResult("Success");
        }

        // GET api/values/5
        [HttpPut("{scorecardId}")]
        public IActionResult RequestReset(int scorecardId, [FromBody] ResetRequest request)
        {
            _repository.RequestReset(scorecardId, request);

            return new JsonResult("Success");
        }
    }
}
