using Microsoft.AspNetCore.Mvc;
using Mippa.Models;
using MIPPA.ViewModels;
using System.Collections.Generic;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace Mippa.Controllers.Api
{
    [Route("api/[controller]")]
    public class LoginController : Controller
    {
        public IRepository _repository { get; set; }

        public LoginController(IRepository repo)
        {
            _repository = repo;
        }

        /// <summary>
        /// Creates a new Player
        /// </summary>
        /// <param name="player"></param>
        /// <returns></returns>
        [HttpPost]
        public LoginViewModel Login([FromBody] LoginViewModel loginViewModel)
        {
            loginViewModel = _repository.GetLoginViewModel(loginViewModel);

            return loginViewModel;
        }

        /// <summary>
        /// Update Player properties
        /// </summary>
        /// <param name="playerId"></param>
        /// <param name="player"></param>
        /// <returns></returns>
        [HttpPut("{playerId}")]
        public IActionResult Update(int playerId, [FromBody] Player player)
        {
            if (player == null || player.PlayerId != playerId)
            {
                return BadRequest();
            }

            var playerToUpdate = _repository.GetPlayer(playerId);

            if (playerToUpdate == null)
            {
                return NotFound();
            }

            _repository.UpdatePlayer(player);
            return new NoContentResult();
        }

        /// <summary>
        /// Delete a Player from the database
        /// </summary>
        /// <param name="playerId"></param>
        [HttpDelete("{playerId}")]
        public void Delete(int playerId)
        {
            _repository.RemovePlayer(playerId);
        }
    }
}
