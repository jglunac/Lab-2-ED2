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
    [Route("api/movie/populate")]
    public class Populate : Controller
    {

        [HttpPost]
        public async Task<HttpStatusCode> InPopulate([FromForm] IFormFile file)
        {
            List<Movie> result = new List<Movie>();
            using var Memory = new MemoryStream();
            if (file == null)
            {
                return HttpStatusCode.InternalServerError;
            }
            await file.CopyToAsync(Memory);

            try
            {
                string contenido = Encoding.ASCII.GetString(Memory.ToArray());
                result = DeserializeMovie(contenido);
                foreach (var movie in result)
                {
                    string titulo = "NA";
                    string release = "NA";
                    if (movie.Title != null)
                    {
                        titulo = movie.Title;
                    }
                    if (movie.ReleaseDate != null)
                    {
                    release = movie.ReleaseDate;
                    }
                release = release.Substring(release.Length - 4, 4);
                movie.Key = titulo + "-" + release;
                Data.tree.Insert(movie);
                }
                return HttpStatusCode.OK;
            }
            catch (Exception)
            {
                return HttpStatusCode.InternalServerError;
            }
}

        [HttpDelete]
        [Route("{id}")]
        public HttpStatusCode Delete(string id)
        {
            try
            {
                if (Data.tree.Delete(id) == true)
                {
                    return HttpStatusCode.OK;
                }
                else
                {
                    return HttpStatusCode.NotFound;
                }
            }
            catch (Exception)
            {

                return HttpStatusCode.OK;
            }
            
        }


        public static List<Movie> DeserializeMovie(string content)
        {
            return JsonSerializer.Deserialize<List<Movie>>(content, new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            });
        }
    }
}
