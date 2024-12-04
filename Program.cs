using Lab3_CSharp.Models;
using Microsoft.EntityFrameworkCore;

using (ApplicationContext db = new ApplicationContext())
{
    try
    {
        db.Database.SqlQuery<int?>(@$"select * from public.Views limit 1").SingleOrDefault();
    }
    catch (Exception e)
    {
        db.Database.Migrate();
    }
}

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();

var app = builder.Build();

app.MapControllerRoute(name: "default", pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();


