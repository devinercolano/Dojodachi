using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using Newtonsoft.Json;

namespace dojodachi.Controllers{

    public static class SessionExtensions {
        public static void SetObjectAsJson(this ISession session, string key, object value) {
            session.SetString(key, JsonConvert.SerializeObject(value));
        }
        
        public static T GetObjectFromJson<T>(this ISession session, string key) {
            string value = session.GetString(key);
            return value == null ? default(T) : JsonConvert.DeserializeObject<T>(value);
        }
    }

    public class gameController : Controller {
        string action = "";
        string printString = "";

        [HttpGet]
        [Route("")]
        public IActionResult Index() {
            if(HttpContext.Session.GetObjectFromJson<Pet>("CurrentPet") == null) {
                Pet spike = new Pet();
                HttpContext.Session.SetObjectAsJson("CurrentPet", spike);
                ViewBag.fullness = spike.fullness;
                ViewBag.happiness = spike.happiness;
                ViewBag.meals = spike.meals;
                ViewBag.energy = spike.energy;
            }

            else if (HttpContext.Session.GetObjectFromJson<Pet>("CurrentPet").fullness >= 100 && HttpContext.Session.GetObjectFromJson<Pet>("CurrentPet").happiness >= 100 && HttpContext.Session.GetObjectFromJson<Pet>("CurrentPet").energy >= 100) {
                return RedirectToAction("Win");
            }

            // If fullness or happiness ever drop to 0, you lose, and a restart button should be displayed.
            else if (HttpContext.Session.GetObjectFromJson<Pet>("CurrentPet").fullness == 0 || HttpContext.Session.GetObjectFromJson<Pet>("CurrentPet").happiness == 0)
            {
                return RedirectToAction("Lose");
            }

            else
            {
                ViewBag.fullness = HttpContext.Session.GetObjectFromJson<Pet>("CurrentPet").fullness;
                ViewBag.happiness = HttpContext.Session.GetObjectFromJson<Pet>("CurrentPet").happiness;
                ViewBag.meals = HttpContext.Session.GetObjectFromJson<Pet>("CurrentPet").meals;
                ViewBag.energy = HttpContext.Session.GetObjectFromJson<Pet>("CurrentPet").energy;
                if(TempData["printString"] != null){
                    ViewBag.printString = TempData["printString"];
                }
            }
            return View("dojodachi");
        }

        [HttpPost]
        [Route("feed")]
        public IActionResult Feed(){
            action = "fed";
            Pet spike = HttpContext.Session.GetObjectFromJson<Pet>("CurrentPet");
            Random rand = new Random();
                if(spike.meals != 0){
                    Random howMuch = new Random();
                    int fullnessFactor = howMuch.Next(5, 11);
                    spike.fullness += fullnessFactor;
                    TempData["printString"] = "You " + action + " your Dojodachi! fullness +" + fullnessFactor;
                    int petFullness = spike.fullness;
                    spike.meals = spike.meals -1;
            }
            HttpContext.Session.SetObjectAsJson("CurrentPet", spike);
            return RedirectToAction("Index");
        }

        [HttpPost]
        [Route("play")]
        public IActionResult Play(){
            action = "played with";
            Pet spike = HttpContext.Session.GetObjectFromJson<Pet>("CurrentPet");
            Random rand = new Random();
                Random howMuch = new Random();
                int happinesFactor = howMuch.Next(5,11);
                spike.happiness += happinesFactor;
                spike.energy = spike.energy - 5;
                TempData["printString"] = "You " + action + " your Dojodachi! happines +" + happinesFactor + " Energy -5";

            HttpContext.Session.SetObjectAsJson("CurrentPet", spike);
            return RedirectToAction("Index");
        }

        [HttpPost]
        [Route("work")]
        public IActionResult Work(){
            action = "worked with";
            Pet spike = HttpContext.Session.GetObjectFromJson<Pet>("CurrentPet");
            Random rand = new Random();
            spike.energy = spike.energy - 5;
            int mealsFactor = rand.Next(1, 4);
            spike.meals += mealsFactor;
            TempData["printString"] = "You " + action + " your Dojodachi! meals +" + mealsFactor + " Energy -5";

            HttpContext.Session.SetObjectAsJson("CurrentPet", spike);
            return RedirectToAction("Index");
        }

        [HttpPost]
        [Route("sleep")]
        public IActionResult Sleep(){
            Pet spike = HttpContext.Session.GetObjectFromJson<Pet>("CurrentPet");
            spike.energy = spike.energy + 15;
            spike.fullness = spike.fullness - 5;
            spike.happiness = spike.happiness - 5;
            TempData["printString"] = "Your Dojodachi slept! Energy +15, fulllness -5, happiness -5 ";
            HttpContext.Session.SetObjectAsJson("CurrentPet", spike);
            return RedirectToAction("Index");
        }

        [HttpGet]
        [Route("win")]
        public IActionResult win(){
            return View("win");
        }
        [HttpGet]
        [Route("lose")]
        public IActionResult Lose()
        {
            return View("lose");
        }
        
        [HttpPost]
        [Route("restart")]
        public IActionResult Restart()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Index");
        }
    }
}