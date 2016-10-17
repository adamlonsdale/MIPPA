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
    [Route("api/Admin")]
    public class AdminController : Controller
    {
        IRepository _repository;

        public AdminController(IRepository repository)
        {
            _repository = repository;
        }

        [HttpGet("{sessionId}")]
        public IEnumerable<TeamRoster> GetAllFromSession(int sessionId)
        {
            return _repository.GetAllPlayersFromSession(sessionId);
        }
    }
}