using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.IO;
using Microsoft.VisualBasic;
using System.Net;
using Microsoft.AspNetCore.Http;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text;
using DataStructures;
using Microsoft.AspNetCore.Hosting;
using API.Models;

namespace API.Controllers
{
    [ApiController]
    [Route("api/movie/[controller]")]
    public class Populate : Controller
    {

        [HttpPost]
        public async Task<string> InPopulate([FromForm] IFormFile file)
        {
            List<Movie> result = new List<Movie>();
            using var Memory = new MemoryStream();
            if (file == null)
            {
                return "Por favor asegúrese que envió un archivo, la key debe ser: file";
            }
            await file.CopyToAsync(Memory);

            try
            {
                string contenido = Encoding.ASCII.GetString(Memory.ToArray());
                result = JsonSerializer.Deserialize<List<Movie>>(contenido);
                foreach (var movie in result)
                {
                    Data.tree.Insert(movie);
                }
                return "Ok";
            }
            catch (Exception)
            {
                return "InternalServerError";
            }
        }

        [HttpDelete("{id}")]

        public string Delete([FromForm] string id)
        {
            try
            {
                return Data.tree.Delete(id);
            }
            catch (Exception)
            {
                return "InternalServerError";
            }
        }
    }
}
