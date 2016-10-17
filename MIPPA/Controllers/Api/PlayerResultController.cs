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
    [Route("api/PlayerResult")]
    public class PlayerResultController : Controller
    {
        IRepository _repository;

        public PlayerResultController(IRepository repository)
        {
            _repository = repository;
        }

        [HttpGet("{scorecardId}")]
        public PlayerResultsViewModel GetPlayerResults(int scorecardId)
        {
            return _repository.GetPlayerResults(scorecardId);
        }
    }
}