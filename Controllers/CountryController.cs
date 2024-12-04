using Lab3_CSharp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Lab3_CSharp;

public class CountryController : Controller
{
    public IActionResult Index(List<string>? errors)
    {
        errors ??= new List<string>();
        List<Country> countryList;

        using (ApplicationContext db = new ApplicationContext())
        {
            countryList = db.Countries.ToList();
        }

        ViewBag.countryList = countryList;
        ViewBag.errors = errors;
        
        return View();
    }

    public IActionResult Create(Country country)
    {
        List<string> errors = new List<string>();
        using (ApplicationContext db = new ApplicationContext())
        {
            var names = db.Countries.Select(c => c.CountryName);

            if (country.IsValid() && Country.IsCountryNameUnique(names, country.CountryName))
            {
                db.Countries.Add(country);
                db.SaveChanges();
            }
            else
            {
                if (!country.IsValid())
                {
                    errors.Add("Данные о стране не валидны");
                }

                if (!Country.IsCountryNameUnique(names, country.CountryName))
                {
                    errors.Add("Страна с таким названием уже существует");
                }
            }
        }
        
        return RedirectToAction("Index", routeValues: new{errors = errors});
    }
    
    public IActionResult GetCountryAbout(int id, List<string>? errors)
    {
        errors ??= new List<string>();
        Country? country;

        using (ApplicationContext db = new ApplicationContext())
        {
            country = db.Countries.FirstOrDefault(c => c.Id == id);
        }
        
        ViewBag.errors = errors;
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

        return RedirectToAction("Index");
    }

    public IActionResult Update(Country country)
    {
        List<string> _errors = new List<string>();
        using (ApplicationContext db = new ApplicationContext())
        {
            Country countryInDb = db.Countries.First(c => c.Id == country.Id);
            List<string> countryNames = db.Countries.Select(c => c.CountryName).ToList();
            
            
            if (country.IsValid() && Country.IsCountryNameUnique(countryNames, country.CountryName))
            {
                db.Countries.Remove(countryInDb);
                db.Countries.Add(country);
                db.SaveChanges();
            }
            else
            {
                if (!country.IsValid())
                {
                    _errors.Add("Данные о стране не валидны");
                }
                if (!Country.IsCountryNameUnique(countryNames, country.CountryName) && 
                    countryInDb.IsNameChanged(country.CountryName))
                {
                    _errors.Add("Страна с таким названием уже существует");
                }
                else if(!countryInDb.IsNameChanged(country.CountryName))
                {
                    _errors.Add("Страна должна иметь новое название");
                }
            }
            return RedirectToAction("GetCountryAbout", routeValues: new
            {
                id = countryInDb.Id, 
                errors = _errors
            });
        }
    }
}