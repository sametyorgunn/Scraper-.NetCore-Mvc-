using HtmlAgilityPack;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.VisualBasic.Syntax;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using scraping.Models;

namespace scraping.Controllers
{
    public class ScraperController : Controller
    {
       
        public IActionResult Index(string url,string Titleway,string fiyatway,string exceladi,string resimway)
        {
            try
            {
                var userid = HttpContext.Session.GetInt32("userid");
                if (userid != 0 || userid != null)
                {
                    if (string.IsNullOrEmpty(Titleway) || string.IsNullOrEmpty(fiyatway))
                    {
                        return View();
                    }
                    Context c = new Context();
                    var html = @$"{url}";
                    HtmlWeb web = new HtmlWeb();
                    var htmlDoc = web.Load(html);

                    HtmlNodeCollection title = htmlDoc.DocumentNode.SelectNodes($"{Titleway}");
                    HtmlNodeCollection fiyat = htmlDoc.DocumentNode.SelectNodes($"{fiyatway}");
                    HtmlNodeCollection resim = htmlDoc.DocumentNode.SelectNodes($"{resimway}");

                    if (resim != null)
                    {
                        List<Urun> urun = new List<Urun>();
                        for (int i = 0; i < title.Count; i++)
                        {
                            HtmlAttribute srcAttribute = resim[i].Attributes["src"];

                            Urun uruns = new Urun
                            {
                                Fiyat = fiyat[i].InnerText,
                                Title = title[i].InnerText,
                                Img = srcAttribute.Value.ToString()
                            };
                            urun.Add(uruns);
                        }
                        var urunler = urun.ToList();
                        ExceleAktar(urun, exceladi);
                        return View(urunler);
                    }
                    else
                    {
                        List<Urun> urun = new List<Urun>();
                        for (int i = 0; i < title.Count; i++)
                        {

                            Urun uruns = new Urun
                            {
                                Fiyat = fiyat[i].InnerText,
                                Title = title[i].InnerText
                            };
                            urun.Add(uruns);
                        }
                        var urunler = urun.ToList();
                        ExceleAktar(urun, exceladi);
                        return View(urunler);
                    }
                   
                }
                else
                {
                    return RedirectToAction("Index", "Login");
                }
            }
            catch(Exception ex)
            {
                return RedirectToAction("Index", "Scraper");
            }
           
        }

        public IActionResult ExceleAktar(List<Urun>urun,string exceladi)
        {
            try
            {
                var userid = HttpContext.Session.GetInt32("userid");
                if (userid != 0 || userid != null)
                {
                    if (exceladi != null && urun != null)
                    {
                        Context c = new Context();
                        Excel ex = new Excel();
                        ex.Adi = exceladi;
                        c.excels.Add(ex);
                        c.SaveChanges();

                        var uruns = urun.ToList();
                        using FileStream fileStream = new FileStream($"wwwroot/Excels/{exceladi}.xlsx", FileMode.OpenOrCreate);
                        using IWorkbook workbook = new XSSFWorkbook();
                        ISheet sheet = workbook.CreateSheet("Sheet1");

                        IRow row1 = sheet.CreateRow(0);
                        row1.CreateCell(0).SetCellValue("Başlık");
                        row1.CreateCell(1).SetCellValue("Fiyat");
                        row1.CreateCell(2).SetCellValue("Resim");

                        int satir = 1;
                        foreach (var i in uruns)
                        {
                            IRow row2 = sheet.CreateRow(satir++);
                            row2.CreateCell(0).SetCellValue(i.Title);
                            row2.CreateCell(1).SetCellValue(i.Fiyat);
                            row2.CreateCell(2).SetCellValue(i.Img);
                        }
                        workbook.Write(fileStream, false);
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        return RedirectToAction("Index","Scraper");
                    }
                }
                else
                {
                    return RedirectToAction("Index", "Login");
                }
            }
            catch(Exception ex)
            {
                return RedirectToAction("Index", "Login");
            }
           

        }

        public IActionResult Exceller()
        {
            try
            {
                var userid = HttpContext.Session.GetInt32("userid");
                if (userid != 0 || userid != null)
                {
                    Context c = new Context();
                    var excel = c.excels.ToList();
                    return View(excel);
                }
                else
                {
                    return RedirectToAction("Index", "Login");
                }
            }
            catch(Exception ex)
            {
                return RedirectToAction("Index", "Login");
            }
           
        }
    }
}
