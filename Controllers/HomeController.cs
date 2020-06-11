using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using CRUDelicious.Models;
using Microsoft.AspNetCore.Http;

namespace CRUDelicious.Controllers
{
    public class HomeController : Controller
    {
        private CrudContext dbContext;
        public HomeController(CrudContext context)
        {
            dbContext = context;
        }

        [Route("")]
        [HttpGet]
        public IActionResult Index()
        {
            // Change this line back to query CreatedAt once you've deleted all the old models that didn't have DateTime.Now;
            // For now, OrderByDescending based on DishId;
            Dishes[] allDishes = dbContext.Dishes.OrderByDescending(d => d.CreatedAt).ToArray();
            return View("Index", allDishes);
        }

        [Route("New")]
        [HttpGet]
        public IActionResult New()
        {
            return View("New");
        }

        [Route("AddDish")]
        [HttpPost]
        public IActionResult AddDish(Dishes newDish)
        {
            if(ModelState.IsValid)
            {
                dbContext.Add(newDish);
                dbContext.SaveChanges();
                return RedirectToAction("Index");
            }
            else
            {
                return View("New");
            }
        }

        [Route("Display/{DishId}")]
        [HttpGet]
        public IActionResult Display(int DishId)
        {
            Dishes thisDish = dbContext.Dishes
                .SingleOrDefault(dish => dish.DishId == DishId);
            return View("Display", thisDish);
        }

        [Route("Delete/{DishId}")]
        [HttpGet]
        public IActionResult Delete(int DishId)
        {
            Dishes thisDish = dbContext.Dishes
                .SingleOrDefault(dish => dish.DishId == DishId);
                dbContext.Dishes.Remove(thisDish);
                dbContext.SaveChanges();
            return RedirectToAction("Index");
        }

        [Route("EditDish/{DishId}")]
        [HttpGet]

        public IActionResult EditDish(int DishId)
        {
            Dishes ToEdit = dbContext.Dishes
                .FirstOrDefault(dish => dish.DishId == DishId);
            return View("EditDish", ToEdit);
        }

        [Route("UpdateDish/{DishId}")]
        [HttpPost]

        public IActionResult UpdateDish(int DishId, Dishes UpDish)
        {
            if(ModelState.IsValid)
            {
                UpDish.DishId = DishId;
                dbContext.Update(UpDish);
            
                dbContext.Entry(UpDish).Property("CreatedAt").IsModified = false;
                dbContext.SaveChanges();
                return RedirectToAction("Index");
            }
            else
            {
                return EditDish(DishId);
            }
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
