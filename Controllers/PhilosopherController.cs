using Lab3_CSharp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Lab3_CSharp;

public class PhilosopherController : Controller
{
    public class CreatePhilosopherModel
    {
        public required string Name { get; set; }
        public required string Surname {  get; set; }
        public required DateOnly Birth_date { get; set; }
        public DateOnly? Die_date { get; set; } = null;
        public bool IsDie { get; set; }
        public int CountryLivingId { get; set; } = -1;
    }

    public class GetPhilosopherAbout_UpdatePhilosopherModel
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public required string Surname {  get; set; }
        public required DateOnly Birth_date { get; set; }
        public DateOnly? Die_date { get; set; } = null;
        public bool IsDie { get; set; }
        public List<View> PhilosopherViewList { get; set; } = null;
        public List<string> PhilosopherWorkNameList { get; set; } = null;
        public Country CountryLiving = null;
        
        public int CountryLivingId { get; set; } = -1;
        
        public int addViewId { get; set; } = -1;
        public int removeViewId { get; set; } = -1;
        
        public bool IsNeedAddView()
        {
            return addViewId != -1;
        }
        public bool IsNeedRemoveView()
        {
            return removeViewId != -1;
        }
        public bool IsNeedChangeCountry()
        {
            return CountryLivingId != -1;
        }
    }
    
    // GET
    public IActionResult Index()
    {
        List<Philosopher> philosopherList;
        List<Country> countryList;

        using (ApplicationContext db = new ApplicationContext())
        {
            philosopherList = db.Philosophers.ToList();
            countryList = db.Countries.ToList();
        }

        ViewBag.philosopherList = philosopherList;
        ViewBag.countryList = countryList;
        
        return View();
    }
    
    public IActionResult Create(CreatePhilosopherModel inputModel)
    {
        Philosopher philosopher = new() { Name = inputModel.Name, Surname = inputModel.Surname, 
            Birth_date = inputModel.Birth_date, Die_date = inputModel.Die_date, IsDie = inputModel.IsDie};

        using (ApplicationContext db = new ApplicationContext())
        {
            Country? country = db.Countries.FirstOrDefault(c => c.Id == inputModel.CountryLivingId);

            if (philosopher.IsValid() && country != null)
            {
                philosopher.CountryLiving = country;
                db.Philosophers.Add(philosopher);
                db.SaveChanges();
            }
        }

        return RedirectToAction("Index");
    }
    
    public IActionResult Delete(int philosopherId)
    {
        using (ApplicationContext db = new ApplicationContext())
        {
            Philosopher? philosopherDelete = db.Philosophers.FirstOrDefault(p => p.Id == philosopherId);
        
            if (philosopherDelete != null)
            {
                db.Philosophers.Remove(philosopherDelete);
                db.SaveChanges();
            }
        }

        return RedirectToAction("Index");
    }

    public IActionResult GetPhilosopherAbout(int philosopherId)
    {
        List<Country>? countriesNotInPhilosopher = null;
        Philosopher? philosopher = null;
        List<View>? viewsNotInPhilosopher = null; 
        
        using (ApplicationContext db = new ApplicationContext())
        {
            philosopher = db.Philosophers.Include(p => p.CountryLiving)
                .Include(p => p.Views)
                .Include(p => p.Works)
                .FirstOrDefault(p => p.Id == philosopherId);
            
            
            if (philosopher != null)
            {
                viewsNotInPhilosopher = db.Views.Where(v => !philosopher.Views.Contains(v)).ToList();
                countriesNotInPhilosopher = db.Countries.Where(c => c.Id != philosopher.CountryLiving.Id)
                    .ToList();
            }
            
        }

        IActionResult result = RedirectToAction("Index");

        if (philosopher != null)
        {
            GetPhilosopherAbout_UpdatePhilosopherModel model = new()
            {
                Id = philosopher.Id,
                Name = philosopher.Name,
                Surname = philosopher.Surname,
                Birth_date = philosopher.Birth_date,
                Die_date = philosopher.Die_date,
                IsDie = philosopher.IsDie,
                PhilosopherViewList = philosopher.Views.ToList(),
                PhilosopherWorkNameList = philosopher.Works.Select(v => v.WorkName).ToList(),
                CountryLiving = philosopher.CountryLiving
            };

            ViewBag.countriesNotInPhilosopher = countriesNotInPhilosopher;
            ViewBag.viewsNotInPhilosopher = viewsNotInPhilosopher;
            
            result = View(model);
        }
        
        return result;
    }

    public IActionResult Update(GetPhilosopherAbout_UpdatePhilosopherModel model)
    {
        IActionResult result = RedirectToAction("GetPhilosopherAbout", routeValues: new{philosopherId = model.Id});

        using (ApplicationContext db = new ApplicationContext())
        {
            Philosopher? phil = db.Philosophers.Include(p => p.CountryLiving)
                .Include(p => p.Views).FirstOrDefault(p => p.Id == model.Id);

            if (phil != null)
            {
                phil.Name = model.Name;
                phil.Surname = model.Surname;
                phil.Birth_date = model.Birth_date;
                phil.Die_date = model.Die_date;
                phil.IsDie = model.IsDie;

                if (phil.IsValid())
                {
                    if (model.IsNeedAddView())
                    {
                        View? view = db.Views.FirstOrDefault(v => v.Id == model.addViewId);
                        if (view != null)
                        {
                            phil.Views.Add(view);
                        }
                    }
                    if (model.IsNeedRemoveView() && phil.Views.FirstOrDefault(
                            v => v.Id == model.removeViewId) != null
                        )
                    {
                        phil.Views.Remove(phil.Views.FirstOrDefault(v => v.Id == model.removeViewId)!);
                    }
                    if (model.IsNeedChangeCountry() && db.Countries.FirstOrDefault(
                            c => c.Id == model.CountryLivingId) != null
                       )
                    {
                        phil.CountryLiving = db.Countries.FirstOrDefault(c => c.Id == model.CountryLivingId)!;
                    }
                    

                    db.SaveChanges();
                    result = RedirectToAction("Index");
                }
            }
        }
        
        return result;
    }
}