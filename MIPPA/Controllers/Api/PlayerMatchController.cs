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
    [Route("api/PlayerMatch")]
    public class PlayerMatchController : Controller
    {
        IRepository _repository;

        public PlayerMatchController(IRepository repository)
        {
            _repository = repository;
        }

        [HttpPut()]
        public ScoreValidationResult UpdatePlayerScores([FromBody] PlayerMatchViewModel viewModel)
        {
            int homePlayerScore = 0;
            int awayPlayerScore = 0;
            int scorecardState = 0;

            _repository.UpdatePlayerScores(viewModel, out homePlayerScore, out awayPlayerScore, out scorecardState);

            return new ScoreValidationResult { homeScore = homePlayerScore, awayScore = awayPlayerScore, scorecardState = scorecardState };
        }
    }

    public class ScoreValidationResult
    {
        public int homeScore { get; set; }
        public int awayScore { get; set; }
        public int scorecardState { get; set; }
    }
}