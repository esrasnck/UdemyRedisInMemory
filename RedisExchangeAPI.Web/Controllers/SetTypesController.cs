using Microsoft.AspNetCore.Mvc;
using RedisExchangeAPI.Web.Services;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RedisExchangeAPI.Web.Controllers
{
    public class SetTypesController : Controller
    {
        private readonly RedisService _redisService;

        private readonly IDatabase db;

        private string listKey = "hashNames";
        public SetTypesController(RedisService redisService)
        {
            _redisService = redisService;
            db = _redisService.GetDb(3);
        }

        public IActionResult Index()
        {
            HashSet<string> namesList = new HashSet<string>();
            if(db.KeyExists(listKey))
            {
                db.SetMembers(listKey).ToList().ForEach(x =>
                {
                    namesList.Add(x.ToString());
                });
            }

            return View(namesList);
        }

        [HttpPost]
        public IActionResult Add(string name)
        {
           
            db.KeyExpire(listKey, DateTime.Now.AddMinutes(5)); // absolute time a denk geliyor. her istekte buna bir timeout verirsek, sliding time özelliği gelecek.

            #region
            //bunu istmezsek, if döngüsü ile halledebiliriz.
            //if (!db.KeyExists(listKey))
            //{
            //    db.KeyExpire(listKey, DateTime.Now.AddMinutes(5));
            //}
            #endregion

            db.SetAdd(listKey, name);
          


            return RedirectToAction("Index");
        }
        public async Task<IActionResult> DeleteItem(string name)
        {
           await db.SetRemoveAsync(listKey, name);
            return RedirectToAction("Index");
        }
       
    }
}
