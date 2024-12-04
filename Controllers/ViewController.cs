using Lab3_CSharp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Lab3_CSharp;

public class ViewController : Controller
{
    public class ViewUpdateModel
    {
        public int Id { get; set; }
        public string ViewName { get; set; }
        public List<Philosopher> Philosophers { get; set; }
        public int addPhilosopherId { get; set; } = -1;
        public int removePhilosopherId { get; set; } = -1;

        public bool IsNeedAddPhilosopher()
        {
            return addPhilosopherId != -1;
        }
        
        public bool IsNeedRemovePhilosopher()
        {
            return removePhilosopherId != -1;
        }
    }
    
    // GET
    public IActionResult Index(List<string>? errors)
    {
        List<View> viewList;
        errors ??= new List<string>();
        using (ApplicationContext db = new ApplicationContext())
        {
            viewList = db.Views.ToList();
        }

        ViewBag.viewList = viewList;
        ViewBag.errors = errors;
        
        return View();
    }
    
    public IActionResult Create(View view)
    {
        List<string> _errors = new List<string>();
        using (ApplicationContext db = new ApplicationContext())
        {
            var viewNames = db.Views.Select(v => v.ViewName);

            if (view.IsValid() && Models.View.IsViewNameUnique(viewNames, view.ViewName))
            {
                db.Views.Add(view);
                db.SaveChanges();
            }
            else
            {
                if (!view.IsValid())
                {
                    _errors.Add("Данные о философском течении не валидны");
                }

                if (!Models.View.IsViewNameUnique(viewNames, view.ViewName))
                {
                    _errors.Add("Философское течение с таким названием уже существует");
                }
            }
        }
        
        return RedirectToAction("Index", routeValues: new
        {
            errors = _errors
        });
    }
    
    public IActionResult Delete(int viewId)
    {
        using (ApplicationContext db = new ApplicationContext())
        {
            View? viewDelete = db.Views.FirstOrDefault(v => v.Id == viewId);
        
            if (viewDelete != null)
            {
                db.Views.Remove(viewDelete);
                db.SaveChanges();
            }
        }

        return RedirectToAction("Index");
    }
    
    public IActionResult GetViewAbout(int viewId, List<string>? errors)
    {
        List<Philosopher> PhilosphersNotInView;
        View? view;
        errors ??= new List<string>();

        using (ApplicationContext db = new ApplicationContext())
        {
            view = db.Views.Include(v => v.Philosophers).FirstOrDefault(v => v.Id == viewId);
            PhilosphersNotInView = db.Philosophers.Where(phil => !view.Philosophers.Contains(phil)).ToList();
        }

        ViewBag.PhilosphersNotInView = PhilosphersNotInView;

        ViewUpdateModel model = new ViewUpdateModel{ 
            Id = view.Id, 
            ViewName = view.ViewName, Philosophers = view.Philosophers
        };
        
        ViewBag.errors = errors;
        return View(model);
    }

    public IActionResult Update(ViewUpdateModel viewData)
    {
        List<string> _errors = new List<string>();
        View view = new View {Id = viewData.Id, ViewName = viewData.ViewName};
        using (ApplicationContext db = new ApplicationContext())
        {
            View viewInDb = db.Views.Include(v => v.Philosophers)
                .First(v => v.Id == view.Id);
            List<string> viewNames = db.Views.Select(v => v.ViewName).ToList();
            bool pred1 = !viewInDb.Philosophers.Exists(p => p.Id == viewData.addPhilosopherId);
            bool pred2 = viewInDb.Philosophers.Exists(p => p.Id == viewData.removePhilosopherId);
            bool isMakeActions = false;
            
            if(view.IsValid() && Models.View.IsViewNameUnique(viewNames, view.ViewName))
            {
                viewInDb.ViewName = view.ViewName;
                isMakeActions = true;
            }
            else
            {
                if (!view.IsValid())
                {
                    _errors.Add("Данные о философском течении не валидны");
                }
                if (!Models.View.IsViewNameUnique(viewNames, view.ViewName) && 
                    viewInDb.IsViewNameChanged(view.ViewName))
                {
                    _errors.Add("Философское течение с таким названием уже существует");
                }
                else if(!viewInDb.IsViewNameChanged(view.ViewName))
                {
                    _errors.Add("Философское течение должно иметь новое название");
                }    
            }
            
            if (pred1 && viewData.IsNeedAddPhilosopher())
            {
                viewInDb.Philosophers.Add(
                    db.Philosophers.FirstOrDefault(p => p.Id == viewData.addPhilosopherId)
                    );
                isMakeActions = true;
            }
            if (pred2 && viewData.IsNeedRemovePhilosopher())
            {
                viewInDb.Philosophers.Remove(
                    db.Philosophers.FirstOrDefault(p => p.Id == viewData.removePhilosopherId)
                    );
                isMakeActions = true;
            }
            if (isMakeActions && _errors.Count == 0)
            {
                db.SaveChanges();

                return RedirectToAction("GetViewAbout");
            }
            else
            {
                return RedirectToAction("GetViewAbout", new
                {
                    viewId = viewInDb.Id,
                    errors = _errors
                });
            }
        }
    }
}
