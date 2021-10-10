using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace IDistributedCacheRedisApp.Web.Controllers
{
    public class ProductsController : Controller
    {
        private IDistributedCache _distrubutedCache;
        public ProductsController(IDistributedCache distributedCache)
        {
            _distrubutedCache = distributedCache;
            
        }

        public IActionResult Index()
        {
            DistributedCacheEntryOptions cacheOptions = new DistributedCacheEntryOptions();
            cacheOptions.AbsoluteExpiration = DateTime.Now.AddMinutes(1);
            _distrubutedCache.SetString("isim", "Kenan", cacheOptions);

            return View();
        }

        public IActionResult Show()
        {
            string name = _distrubutedCache.GetString("isim");
            ViewBag.name = name;
            return View();
        }

        public IActionResult Remove()
        {
            _distrubutedCache.Remove("isim");
            return View();
        }

        // Asekron metotlar için
        public async Task<IActionResult> Index2()
        {
            DistributedCacheEntryOptions cacheOptions = new DistributedCacheEntryOptions();
            cacheOptions.AbsoluteExpiration = DateTime.Now.AddMinutes(2);
            _distrubutedCache.SetString("isim", "Kenan", cacheOptions);
            await _distrubutedCache.SetStringAsync("soyisim", "Dönmez", cacheOptions);
            return View();
        }

        public async Task<IActionResult> Show2()
        {
            string name2 = await _distrubutedCache.GetStringAsync("soyisim");
            ViewBag.name2 = name2;
            return View();
        }

        public async Task<IActionResult> Remove2()
        {
           await _distrubutedCache.RemoveAsync("soyisim");
            return View();
        }

        public async Task<IActionResult> ImageCache()
        {
            string path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Images/download.jpg");

            byte[] imageByte = System.IO.File.ReadAllBytes(path);

            await _distrubutedCache.SetAsync("resim", imageByte);

            return View();
        }

        public async Task<IActionResult> ImageUrl()
        {
            byte[] resimByte = await _distrubutedCache.GetAsync("resim");
            
            return File(resimByte,"image/jpg");
        }
    }
}
