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
    [Route("api/TeamResult")]
    public class TeamResultController : Controller
    {
        IRepository _repository;

        public TeamResultController(IRepository repository)
        {
            _repository = repository;
        }

        [HttpGet("{scorecardId}")]
        public TeamResultsViewModel GetTeamResults(int scorecardId)
        {
            return _repository.CalculateTeamResults(scorecardId, false);
        }

        [HttpPost("{scorecardId}")]
        public void SaveTeamResults(int scorecardId)
        {
            _repository.CalculateTeamResults(scorecardId, true);
            _repository.CalculatePlayerResults(scorecardId, true);
            _repository.FinalizeMatch(scorecardId);
        }
    }
}