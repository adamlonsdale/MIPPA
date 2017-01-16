using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Mippa.Models;

namespace Mippa.Controllers.Api
{
    [Produces("application/json")]
    [Route("api/Backdoor")]
    public class BackdoorController : Controller
    {
        private IRepository _repository;

        public BackdoorController(IRepository repository)
        {
            _repository = repository;
        }

        [HttpGet]
        public void FinalizeAllScorecards()
        {
            _repository.FinalizeAllScorecards();
        }

        [HttpGet("{sessionId}")]
        public void SetSessionInactive(int sessionId)
        {
            _repository.SetSessionInactive(sessionId);
        }
    }
}