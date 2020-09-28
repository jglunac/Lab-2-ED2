﻿using System;
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
using System.Net;

namespace API.Controllers
{
    [ApiController]
    [Route("api/movie")]
    public class MovieController : ControllerBase
    {
        private IWebHostEnvironment _env;
        public MovieController(IWebHostEnvironment env)
        {
            _env = env;
        }

        
        [HttpGet]
        [Route("{travel}")]

        public IActionResult InOrder(string travel)
        {
            List<Movie> recorrido = new List<Movie>();
            if (travel == "InOrder")
            {
                Data.tree.InOrder(recorrido);
            }
            else if (travel == "PreOrder")
            {
                Data.tree.PreOrder(recorrido);
            }
            else if (travel == "PostOrder")
            {
                Data.tree.PostOrder(recorrido);
            }
            foreach (var item in recorrido)
            {
                if (item.Title == "nulltitle")
                {
                    item.Title = null;
                }
                if (item.Director == "nulldirector")
                {
                    item.Director = null;
                }
                if (item.ReleaseDate == "nulldate")
                {
                   item.ReleaseDate = null;
                }
                if (item.Genre == "nullgenre")
                {
                    item.Genre = null;
                }
            }
            JsonSerializer.Serialize(recorrido);
            return Ok(recorrido);
        }
        [HttpDelete]
        public HttpStatusCode Delete()
        {
            try
            {
                if (Data.tree != null)
                {
                    Data.tree.DeleteTree();
                    return HttpStatusCode.OK;
                }
                else
                {
                    return HttpStatusCode.InternalServerError;
                }
                
            }
            catch (Exception)
            {
                return HttpStatusCode.InternalServerError;
            }
        }
        [HttpPost]

        public HttpStatusCode SetOrder([FromForm] IFormFile file)
        {
            if (file == null)
            {
                return HttpStatusCode.BadRequest;
            }
            var Memory = new MemoryStream();
            var delegado = new DiskBTree<Movie>.ToTObj(ConvertToMovie);
            file.CopyToAsync(Memory);
            string contenido = Encoding.ASCII.GetString(Memory.ToArray());
            string path = _env.ContentRootPath;
            try
            {
                Order result = JsonSerializer.Deserialize<Order>(contenido);
                if (result.order < 3)
                {
                    Data.tree = new DiskBTree<Movie>(137, 3, path, delegado);
                    return HttpStatusCode.Created;
                }
                else
                {
                    Data.tree = new DiskBTree<Movie>(137, result.order, path, delegado);
                    return HttpStatusCode.Created;
                }
            }
            catch (Exception)
            {
                return HttpStatusCode.InternalServerError;

            }
        }

        static Movie ConvertToMovie(string line)
        {
            Movie mov = new Movie();
            mov.ToTObj(line);
            return mov;
        }
    }



}

