using Microsoft.AspNetCore.Mvc;
using RedisExchangeAPI.Web.Services;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RedisExchangeAPI.Web.Controllers
{
    public class StringTypesController : Controller
    {
        private readonly RedisService _redisService;

        private readonly IDatabase db;
        public StringTypesController(RedisService redisService)
        {
            _redisService = redisService;
            db= _redisService.GetDb(1);
        }
        public IActionResult Index()
        {
           
            db.StringSet("name", "Esra Sancak");
            db.StringSet("ziyaretçi", 100);

            return View();
        }

        public IActionResult Show()
        {

            // var value = db.StringGet("name");
            // var value = db.StringGetRange("name",0,3);


            var value = db.StringLength("name");
            db.StringIncrement("ziyaretçi", 0);
            var count=  db.StringDecrementAsync("ziyaretçi", 1).Result; // await'i kullanmak istemiyorsak. ve bir data dönüyorsa
            db.StringDecrementAsync("ziyaterçi", 10).Wait(); //await 'i kullanmak istemiyorsak.
          
                ViewBag.value = value.ToString();
                ViewBag.count = count.ToString();
            
            return View();
        }
    }
}
