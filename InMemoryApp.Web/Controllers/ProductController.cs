using InMemoryApp.Web.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InMemoryApp.Web.Controllers
{
    public class ProductController : Controller
    {

        // cache kullancaksak, değer olarak geçmemiz gerekiyor

        private IMemoryCache _memoryCache;
        public ProductController(IMemoryCache memoryCache)
        {
            _memoryCache = memoryCache;
        }
        public IActionResult Index()
        {
            // memory'de zaman metodu var mı yok mu? kontrol edip / daha sonra set edelim.

            #region 1. yol
            //if (String.IsNullOrEmpty(_memoryCache.Get<string>("zaman")))
            //{

            //}
            #endregion

            #region  2.yol
           // alabilirse hem geriye true dönecek hem de value'u zamanCache'e atıyacak
            if (!_memoryCache.TryGetValue<string>("zaman", out string zamanCache))
            {
                // ömür belirtmek istediğimiz yer.
                //MemoryCacheEntryOptions options = new MemoryCacheEntryOptions(); // bu sınıfı kullanarak yapıyoruz.
                //options.AbsoluteExpiration = DateTime.Now.AddSeconds(10);



                //_memoryCache.Set<string>("zaman", DateTime.Now.ToString(), options); // value olarak string tutmam gerek. Bu durumda tip güvenli hale gelmiş oldu.
            }

            #endregion

            // ömür belirtmek istediğimiz yer.
            MemoryCacheEntryOptions options = new MemoryCacheEntryOptions(); // bu sınıfı kullanarak yapıyoruz.
            options.AbsoluteExpiration = DateTime.Now.AddSeconds(20);

           // options.SlidingExpiration = TimeSpan.FromSeconds(10);
            //options.Priority = CacheItemPriority.High;

            //PostEvictionDelegate => delege böyle bir metodu 4 tane parametre alan işaret ediyor.
            options.RegisterPostEvictionCallback((key,value,reason,state)=> {

                _memoryCache.Set("callback", $"{key}->{value}=> sebep :{reason}");
            });
            _memoryCache.Set<string>("zaman", DateTime.Now.ToString(), options);

            Product product = new Product { Id = 1, Name = "kalem", Price = 200 };

            _memoryCache.Set<Product>("product:1", product);
            _memoryCache.Set<double>("money", 100.99);

            return View();
        }

        public IActionResult Show()
        {
            // ilgli key'i nasıl silebiliriz ?

            #region Remove metodu
            _memoryCache.Remove("zaman");
            #endregion

            #region GetOrCreate metodu
            //_memoryCache.GetOrCreate<string>("zaman", entry =>
            //{
            //    return DateTime.Now.ToString();

            //});  // nerede kullanacaksa almaya çalışır. eğer yoksa memory'de oluşturup, ilgili datayı döner.
            #endregion

            _memoryCache.TryGetValue<string>("zaman", out string zamanCache);
            _memoryCache.TryGetValue("callback", out string callBack);

            ViewBag.zaman = zamanCache;
            ViewBag.callback = callBack;

            ViewBag.product = _memoryCache.Get<Product>("product:1");
            ViewBag.money =    _memoryCache.Get<double>("money");
            //ViewBag.zaman1 = _memoryCache.Get<string>("zaman");
            return View();
        }
    }
}
