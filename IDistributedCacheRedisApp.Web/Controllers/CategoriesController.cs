using IDistributedCacheRedisApp.Web.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IDistributedCacheRedisApp.Web.Controllers
{
    public class CategoriesController : Controller
    {
        private IDistributedCache _distrubutedCache;
        public CategoriesController(IDistributedCache distributedCache)
        {
            _distrubutedCache = distributedCache;
        }

        public async Task<IActionResult> Index()
        {

            DistributedCacheEntryOptions cacheEntryOptions = new DistributedCacheEntryOptions();
            cacheEntryOptions.AbsoluteExpiration = DateTime.Now.AddMinutes(2);

            Category category = new Category { Id = 2, Name = "Bilgisayar2", Price = 100 };

            string jsonCategory = JsonConvert.SerializeObject(category);
             await _distrubutedCache.SetStringAsync("category:2",jsonCategory, cacheEntryOptions);

            return View();
        }

        public IActionResult Show()
        {
            string jsonCategory = _distrubutedCache.GetString("category:2");
            Category category = JsonConvert.DeserializeObject<Category>(jsonCategory);
            ViewBag.Category = category;
            return View();
        }

        // binary'e serilaze etmek için
        public IActionResult Index2()
        {
            DistributedCacheEntryOptions cacheEntryOptions = new DistributedCacheEntryOptions();
            cacheEntryOptions.AbsoluteExpiration = DateTime.Now.AddMinutes(2);

            Category category = new Category { Id = 2, Name = "Bilgisayar3", Price = 100 };

            string jsonCategory = JsonConvert.SerializeObject(category);
            Byte[] byteCategory = Encoding.UTF8.GetBytes(jsonCategory);

            _distrubutedCache.Set("category:2", byteCategory);


            return View();
        }

        public async Task<IActionResult> Show2()
        {
            Byte[] byteCategory = await _distrubutedCache.GetAsync("category:2");

            string jsonCategory = Encoding.UTF8.GetString(byteCategory);

            Category category = JsonConvert.DeserializeObject<Category>(jsonCategory);
            ViewBag.Category2 = category;

            return View();
        }
    }
}
