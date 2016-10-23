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
    [Route("api/Statistics")]
    public class StatisticsController : Controller
    {
        private IRepository _repository;

        public StatisticsController(IRepository repository)
        {
            _repository = repository;
        }

        [HttpGet("{sessionId}/{format}")]
        public StatisticsViewModel GetStatistics(int sessionId, Format format)
        {
            return _repository.GetStatisticsForSession(sessionId, format);
        }

        [HttpGet("{sessionId}", Name = "GetSession")]
        public Session GetById(int sessionId)
        {
            var session = _repository.GetSession(sessionId);

            if (session == null)
            {
                return null;
            }
            return session;
        }
    }
}