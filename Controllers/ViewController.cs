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
    public IActionResult Index()
    {
        List<View> viewList;

        using (ApplicationContext db = new ApplicationContext())
        {
            viewList = db.Views.ToList();
        }

        ViewBag.viewList = viewList;
        
        return View();
    }
    
    public IActionResult Create(View view)
    {
        using (ApplicationContext db = new ApplicationContext())
        {
            var viewNames = db.Views.Select(v => v.ViewName);

            if (view.IsValid() && Models.View.IsViewNameUnique(viewNames, view.ViewName))
            {
                db.Views.Add(view);
                db.SaveChanges();
            }
        }
        
        return RedirectToAction("Index");
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
    
    public IActionResult GetViewAbout(int viewId)
    {
        List<Philosopher> PhilosphersNotInView;
        View? view;

        using (ApplicationContext db = new ApplicationContext())
        {
            view = db.Views.Include(v => v.Philosophers).FirstOrDefault(v => v.Id == viewId);
            PhilosphersNotInView = db.Philosophers.Where(phil => !view.Philosophers.Contains(phil)).ToList();
        }

        ViewBag.PhilosphersNotInView = PhilosphersNotInView;

        ViewUpdateModel model = new ViewUpdateModel() { Id = view.Id, 
            ViewName = view.ViewName, Philosophers = view.Philosophers};
        
        return View(model);
    }

    public IActionResult Update(ViewUpdateModel viewData)
    {
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
            if (isMakeActions)
            {
                db.SaveChanges();

                return RedirectToAction("Index");
            }
            else
            {
                return RedirectToAction("GetViewAbout", "View", new { viewId = viewInDb.Id });
            }
        }
    }
}
