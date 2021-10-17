using Microsoft.AspNetCore.Mvc;
using RedisExchangeAPI.Web.Services;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RedisExchangeAPI.Web.Controllers
{
    public class SortedSetTypesController : Controller
    {
        private readonly RedisService _redisService;

        private readonly IDatabase db;

        private string listKey = "sortedSetNames";
        public SortedSetTypesController(RedisService redisService)
        {
            _redisService = redisService;
            db = _redisService.GetDb(4);
        }
        public IActionResult Index()
        {
            HashSet<string> list = new HashSet<string>();

            if (db.KeyExists(listKey))
            {
               // sortedSetScan => redisteki sıralamaya göre getirir.
               db.SortedSetScan(listKey).ToList().ForEach(x =>
               {
                   list.Add(x.ToString());
               });


                #region Score göre sıralamak için
                // setScan'de score değeri gelirken, Aşağıdaki metoda gelmiyor :(

                //db.SortedSetRangeByRank(listKey, order:Order.Descending).ToList().ForEach(x=>
                //{
                //    // küçükten büyüğe doğru sıralamak için mesela(score  göre)
                //    list.Add(x.ToString());
                //});
                #endregion
            }

            return View(list);
        }

        [HttpPost]
        public IActionResult Add(string name, int score)
        {
            db.SortedSetAdd(listKey, name, score);

            db.KeyExpire(listKey, DateTime.Now.AddDays(1));

            return RedirectToAction("Index");

        }
   

    }
}
