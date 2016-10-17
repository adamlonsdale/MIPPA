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
    [Route("api/Handicap")]
    public class HandicapController : Controller
    {
        IRepository _repository;

        public HandicapController(IRepository repository)
        {
            _repository = repository;
        }

        [HttpPut("{playerId}/{sessionId}")]
        public IActionResult UpdateHandicapForPlayer(int playerId, int sessionId, [FromBody] TeamRoster tr)
        {
            _repository.UpdateHandicapForPlayer(playerId, sessionId, tr.Handicap);

            return new JsonResult("Success");
        }
    }
}