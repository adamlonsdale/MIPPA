using Microsoft.AspNetCore.Mvc;
using Mippa.Models;
using System.Collections.Generic;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace Mippa.Controllers.Api
{
    [Route("api/[controller]")]
    public class VenueController : Controller
    {
        // TODO: What the hell goes here?
        public IRepository _repository { get; set; }

        public VenueController(IRepository repo)
        {
            _repository = repo;
        }

        /// <summary>
        /// Returns a list of all Venues in league
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IEnumerable<Venue> GetAll()
        {
            return _repository.GetAllVenues();
        }

        /// <summary>
        /// Obtain a specific Venue by VenueId
        /// </summary>
        /// <param name="venueId"></param>
        /// <returns></returns>
        [HttpGet("{venueId}", Name = "GetVenue")]
        public IActionResult GetById(int venueId)
        {
            var venue = _repository.GetVenue(venueId);

            if (venue == null)
            {
                return NotFound();
            }
            return new ObjectResult(venue);
        }

        /// <summary>
        /// Creates a new Venue
        /// </summary>
        /// <param name="venue"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult Create([FromBody] Venue venue)
        {
            if (venue == null)
            {
                return BadRequest();
            }
            _repository.AddVenue(venue);
            return CreatedAtRoute("GetVenue", new { venueId = venue.VenueId }, venue);
        }

        /// <summary>
        /// Delete a Venue from the database
        /// </summary>
        /// <param name="venueId"></param>
        [HttpDelete("{venueId}")]
        public void Delete(int venueId)
        {
            _repository.RemoveVenue(venueId);
        }
    }
}
