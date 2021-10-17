using Microsoft.AspNetCore.Mvc;
using RedisExchangeAPI.Web.Services;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RedisExchangeAPI.Web.Controllers
{
    public class HashTypesController : BaseController
    {
        public string hashKey { get; set; } = "dictionary";
        public HashTypesController(RedisService redisService) : base(redisService)
        {
        }

        public IActionResult Index()
        {
            Dictionary<string, string> list = new Dictionary<string, string>();
            if (db.KeyExists(hashKey))
            {
                 // db.HashGet(hashKey, "pen"); // tek datayı da bura üzerinden alabiliriz gibi.

                db.HashGetAll(hashKey).ToList().ForEach(x =>
                {
                    list.Add(x.Name, x.Value);
                });
            }
            return View(list);
        }

        [HttpPost]
        public IActionResult Add(string name, string val)
        {
            db.HashSet(hashKey,name,val);
            return RedirectToAction("Index");

        }

        public IActionResult DeleteItem(string name)
        {
            db.HashDelete(hashKey, name);
            return RedirectToAction("Index");
        }
    }
}
