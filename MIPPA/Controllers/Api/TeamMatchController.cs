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
    [Route("api/TeamMatch")]
    public class TeamMatchController : Controller
    {
        IRepository _repository;

        public TeamMatchController(IRepository repository)
        {
            _repository = repository;
        }
    }
}