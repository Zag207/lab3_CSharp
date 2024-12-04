using Lab3_CSharp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Lab3_CSharp;

public class CountryController : Controller
{
    public IActionResult Index()
    {
        List<Country> countryList;

        using (ApplicationContext db = new ApplicationContext())
        {
            countryList = db.Countries.ToList();
        }

        ViewBag.countryList = countryList;
        
        return View();
    }

    public IActionResult Create(Country country)
    {
        using (ApplicationContext db = new ApplicationContext())
        {
            var names = db.Countries.Select(c => c.CountryName);

            if (country.IsValid(names))
            {
                db.Countries.Add(country);
                db.SaveChanges();
            }
        }
        
        return Redirect($"~/Country/Index/");
    }
    
    public IActionResult GetCountryAbout(Country country)
    {
        return View(country);
    }
    
    public IActionResult Delete(int countryId)
    {
        using (ApplicationContext db = new ApplicationContext())
        {
            Country? countryDelete = db.Countries.FirstOrDefault(c => c.Id == countryId);

            if (countryDelete != null)
            {
                db.Countries.Remove(countryDelete);
                db.SaveChanges();
            }
        }

        return Redirect($"~/Country/Index/");
    }

    public IActionResult Update(Country country)
    {
        using (ApplicationContext db = new ApplicationContext())
        {
            Country countryInDb = db.Countries.First(c => c.Id == country.Id);
            List<string>? countryNames = db.Countries.Select(c => c.CountryName).ToList();
            
            if (country.IsValid(countryNames))
            {
                db.Countries.Remove(countryInDb);
                db.Countries.Add(country);
                db.SaveChanges();

                return Redirect($"~/Country/Index");
            }
            else
            {
                return RedirectToAction("GetCountryAbout", "Country", countryInDb);
            }
        }
    }
}