using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using API.Models;
using System.Text.Json;

namespace API.Controllers
{
    [ApiController]
    [Route("api")]
    public class MovieController : ControllerBase
    {

        private readonly ILogger<MovieController> _logger;

        public MovieController(ILogger<MovieController> logger)
        {
            _logger = logger;
        }

        [HttpGet("{InOrder}")]

        public IActionResult InOrder()
        {
            List<Movie> recorrido = new List<Movie>();
            //Data.tree.InOrder(recorrido);
            JsonSerializer.Serialize(recorrido);
            return Ok(recorrido);
        }

        [HttpGet("{PreOrder}")]

        public IActionResult PreOrder()
        {
            List<Movie> recorrido = new List<Movie>();
            //Data.tree.PreOrder(recorrido);
            JsonSerializer.Serialize(recorrido);
            return Ok(recorrido);
        }

        [HttpGet("{PostOrder}")]

        public IActionResult PostOrder()
        {
            List<Movie> recorrido = new List<Movie>();
            //Data.tree.PostOrder(recorrido);
            JsonSerializer.Serialize(recorrido);
            return Ok(recorrido);
        }

        [HttpGet]

        public string SetOrder()
        {
            return "Espero el orden";
        }

        [HttpDelete]

        public string Delete()
        {

        }
      
    }
}
