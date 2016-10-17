using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Mippa.ViewModels;
using Mippa.Models;

namespace Mippa.Controllers.Api
{
    [Produces("application/json")]
    [Route("api/Scorecard")]
    public class ScorecardController : Controller
    {
        IRepository _repository;

        public ScorecardController(IRepository repository)
        {
            _repository = repository;
        }

        [HttpGet("{scorecardId}")]
        public ScorecardViewModel GetInformation(int scorecardId)
        {
            return _repository.GetScorecardInformation(scorecardId);
        }

        [HttpPost]
        public IActionResult PostInformation([FromBody] ScorecardViewModel viewModel)
        {
            _repository.CreateMatchesForScorecard(viewModel);

            return new JsonResult("Success");
        }
    }
}