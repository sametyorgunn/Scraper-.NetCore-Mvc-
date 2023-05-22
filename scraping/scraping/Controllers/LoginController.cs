using Microsoft.AspNetCore.Mvc;
using scraping.Models;

namespace scraping.Controllers
{
    public class LoginController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Index(Kullanici kul)
        {
            Context c = new Context();
            var kisi = c.kullanicis.Where(x => x.KulAdi == kul.KulAdi && x.Sifre == kul.Sifre).FirstOrDefault();
            if (kisi != null)
            {
                HttpContext.Session.SetInt32("userid", kisi.Id);
                return RedirectToAction("Index","Scraper");
            }
            else
            {
                return View();
            }
            
        }
    }
}
