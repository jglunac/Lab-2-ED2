using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using API.Models;
using System.Text.Json;
using Microsoft.AspNetCore.Http;
using System.IO;
using System.Text;
using DataStructures;
using Microsoft.AspNetCore.Hosting;

namespace API.Controllers
{
    [ApiController]
    [Route("api")]
    public class MovieController : ControllerBase
    {

        private readonly ILogger<MovieController> _logger;
        private IWebHostEnvironment _env;
        public MovieController(ILogger<MovieController> logger)
        {
            _logger = logger;
        }

        [HttpGet("{InOrder}")]

        public IActionResult InOrder()
        {
            List<Movie> recorrido = new List<Movie>();
            Data.tree.InOrder(recorrido);
            JsonSerializer.Serialize(recorrido);
            return Ok(recorrido);
        }

        [HttpGet("{PreOrder}")]

        public IActionResult PreOrder()
        {
            List<Movie> recorrido = new List<Movie>();
            Data.tree.PreOrder(recorrido);
            JsonSerializer.Serialize(recorrido);
            return Ok(recorrido);
        }

        [HttpGet("{PostOrder}")]

        public IActionResult PostOrder()
        {
            List<Movie> recorrido = new List<Movie>();
            Data.tree.PostOrder(recorrido);
            JsonSerializer.Serialize(recorrido);
            return Ok(recorrido);
        }

        [HttpGet]

        public string SetOrder([FromForm] IFormFile file)
        {
            if (file == null)
            {
                return "Por favor asegúrese que envió un archivo, la key debe ser: file";
            }
            var Memory = new MemoryStream();
            var delegado = new DiskBTree<Movie>.ToTObj(ConvertToMovie);
            file.CopyToAsync(Memory);
            string contenido = Encoding.ASCII.GetString(Memory.ToArray());
            string path = _env.WebRootPath;
            try
            {
                Order result = JsonSerializer.Deserialize<Order>(contenido);
                if (result.order < 2)
                {
                    Data.tree = new DiskBTree<Movie>(136, 3, path, delegado);
                    return "Grado del árbol inválido, se utilizará grado 3. Arbol generado correctamente";
                }
                else
                {
                    Data.tree = new DiskBTree<Movie>(136, result.order, path, delegado);
                    return "Grado " + result.order + " aceptado. Arbol generado";
                }
            }
            catch (Exception)
            {
                return "Fallo crítico al generar el árbol";

            }
        }

        static Movie ConvertToMovie(string line)
        {
            Movie mov = new Movie();
            line = line.Replace("Ꮄ", "");
            string[] item = line.Split("¬");
            mov.title = item[0];
            mov.director = item[1];
            mov.imdbRating = double.Parse(item[2]);
            mov.releaseDate = item[3];
            mov.genre = item[4];
            mov.rottenTomatoesRating = int.Parse(item[5]);
            return mov;
        }
    }



}
}
