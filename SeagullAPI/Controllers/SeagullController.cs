using System;
using System.Collections.Generic;
using System.Web.Http;
using Newtonsoft.Json;
using SeagullAPI.Models;
using System.IO;
using System.Net.Http;
using System.Reflection;
using System.Configuration;

namespace SeagullAPI.Controllers
{
    public class PhotosController : ApiController
    {
        string _photoRepoPath = Path.Combine(ConfigurationManager.AppSettings["PhotoRepoPath"]);

        [HttpGet]
        [Route("api/photos/{category}")]
        public IHttpActionResult Get(string category)
        {

            if (category.Equals("exterior", StringComparison.InvariantCultureIgnoreCase) || category.Equals("interior", StringComparison.InvariantCultureIgnoreCase))
            {
                var models = new PhotoRepository(Path.Combine(_photoRepoPath, category)).GetPhotos();
                return Ok(new { results = models });
            }
            else
            {
                return NotFound();
            }

        }


    }

    

    public class PhotoRepository
    {
        string _path;
        public PhotoRepository(string path)
        {
            _path = path;
        }
        public List<PhotoModel> GetPhotos()
        {
            String[] files = Directory.GetFiles(_path);
            List<PhotoModel> models = new List<PhotoModel>();

            foreach (string photos in files)
            {
                Byte[] bytes = File.ReadAllBytes(photos);
                String file = Convert.ToBase64String(bytes);
                PhotoModel model = new PhotoModel();
                model.source = file;
                models.Add(model);
            }

            return models;
        }
    }
}
