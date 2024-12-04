using Lab3_CSharp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Lab3_CSharp;

public class WorkController : Controller
{
    public class CreateWorkModel
    {
        public string WorkName { get; set; }
        public int AuthorId { get; set; }
    }

    public class GetWorkAbout_UpdateWorkModel
    {
        public int Id { get; set; }
        public string WorkName { get; set; }
        public int newAuthorId  { get; set; } = -1;
        public Philosopher Author { get; set; } = null;
    }
    
    // GET
    public IActionResult Index(List<string>? errors)
    {
        List<Work> workList;
        errors ??= new List<string>();
        List<Philosopher> philosophersList;

        using (ApplicationContext db = new ApplicationContext())
        {
            workList = db.Works.ToList();
            philosophersList = db.Philosophers.ToList();
        }
        
        ViewBag.workList = workList;
        ViewBag.freePhilosophers = philosophersList;
        ViewBag.errors = errors;
        
        return View();
    }

    public IActionResult Create(CreateWorkModel inputModel)
    {
        Work work = new() { WorkName = inputModel.WorkName, Author = null};
        List<string> _errors = new List<string>();
        
        using (ApplicationContext db = new ApplicationContext())
        {
            List<string> workNames = db.Works.Select(w => w.WorkName).ToList();
            Philosopher? phil = db.Philosophers.FirstOrDefault(p => p.Id == inputModel.AuthorId);

            if (work.IsValid() && Work.IsWorkNameUnique(workNames, inputModel.WorkName) && phil != null)
            {
                work.Author = phil;
                db.Works.Add(work);
                db.SaveChanges();
            }
            else
            {
                if (!work.IsValid())
                {
                    _errors.Add("Данные о философском труде не валидны");
                }

                if (!Models.Work.IsWorkNameUnique(workNames, work.WorkName))
                {
                    _errors.Add("Философский труд с таким названием уже существует");
                }
            }
        }

        return RedirectToAction("Index", routeValues: new
        {
            errors = _errors
        });
    }
    
    public IActionResult Delete(int workId)
    {
        using (ApplicationContext db = new ApplicationContext())
        {
            Work? workDelete = db.Works.FirstOrDefault(w => w.Id == workId);
        
            if (workDelete != null)
            {
                db.Works.Remove(workDelete);
                db.SaveChanges();
            }
        }

        return RedirectToAction("Index");
    }

    public IActionResult GetWorkAbout(int workId, List<string>? errors)
    {
        List<Philosopher> PhilosphersIsNotAuthor;
        Work? work;
        errors ??= new List<string>();

        using (ApplicationContext db = new ApplicationContext())
        {
            work = db.Works.Include(v => v.Author).FirstOrDefault(w => w.Id == workId);
            PhilosphersIsNotAuthor = db.Philosophers.Where(phil => work.Author.Id != phil.Id).ToList();
        }

        ViewBag.PhilosphersIsNotAuthor = PhilosphersIsNotAuthor;
        ViewBag.errors = errors;
        GetWorkAbout_UpdateWorkModel model = new() { Id = work.Id, 
            WorkName = work.WorkName, Author = work.Author};
        
        return View(model);
    }

    public IActionResult Update(GetWorkAbout_UpdateWorkModel model)
    {
        List<string> _errors = new List<string>();
        Work work = new() { WorkName = model.WorkName, Author = null};
        using (ApplicationContext db = new ApplicationContext())
        {
            Work workInDb = db.Works.Include(w => w.Author)
                .FirstOrDefault(w => w.Id == model.Id)!;
            List<string> workNames = db.Works.Select(w => w.WorkName).ToList();
            Philosopher? author = db.Philosophers.FirstOrDefault(p => p.Id == model.newAuthorId);
            bool isMakeActions = false;

            if (work.IsValid() && Work.IsWorkNameUnique(workNames, work.WorkName))
            {
                workInDb.WorkName = work.WorkName;
                isMakeActions = true;
            }
            else
            {
                if (!work.IsValid())
                {
                    _errors.Add("Данные о философском труде не валидны");
                }
                if (!Models.Work.IsWorkNameUnique(workNames, work.WorkName) && 
                    workInDb.IsWorkNameChanged(work.WorkName))
                {
                    _errors.Add("Философский труд с таким названием уже существует");
                }
                else if(!workInDb.IsWorkNameChanged(work.WorkName))
                {
                    _errors.Add("Философский труд должнен иметь новое название");
                }    
            }

            if (author != null)
            {
                workInDb.Author = author;
                isMakeActions = true;
            }

            if (isMakeActions && _errors.Count == 0)
            {
                db.SaveChanges();
            }
            
            return RedirectToAction("GetWorkAbout", routeValues: new
            {
                workId = workInDb.Id,
                errors = _errors
            });
        }
    }
}