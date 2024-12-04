using System.Collections;
using Microsoft.IdentityModel.Tokens;

namespace Lab3_CSharp.Models
{
    public class Philosopher
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public required string Surname {  get; set; }
        public required DateOnly Birth_date { get; set; }
        public DateOnly? Die_date { get; set; } = null;
        public bool IsDie { get; set; }
        public List<View> Views { get; set; } = new List<View>();
        public List<Work> Works { get; set; } = new List<Work>();
        public Country CountryLiving { get; set; }
        
        public bool IsValid()
        {
            bool isValid = true;
            
            if (Name != null)
            {
                Name = Name.Trim();
            }
            isValid = isValid && Name != null && Name != "";
            
            if (Surname != null)
            {
                Surname = Surname.Trim();
            }
            isValid = isValid && Surname != null && Surname != "";

            if (IsDie)
            {
                isValid = isValid && Die_date != null && Birth_date < Die_date;
            }
            else
            {
                isValid = isValid && Birth_date != null;
            }

            return isValid;
        }

        public bool IsSurnameNameChanged(string name, string surname)
        {
            return Name + Surname != name + surname;
        }
    }

    public class View
    {
        public int Id { get; set; }
        public required string ViewName { get; set; }
        public List<Philosopher> Philosophers { get; set; } = new List<Philosopher>();
        
        public bool IsViewNameChanged(string viewName)
        {
            return viewName != ViewName;
        }
        
        public bool IsValid()
        {
            if (ViewName != null)
            {
                ViewName = ViewName.Trim();
            }

            return ViewName != null && ViewName != "";
        }
        public static bool IsViewNameUnique(IEnumerable<string> allViewNames, string viewName)
        {
            return !allViewNames.Contains(viewName);
        }
    }

    public class Work
    {
        public int Id { get; set; }
        public required string WorkName { get; set; }
        public required Philosopher Author { get; set; }
        
        public bool IsValid()
        {
            if (WorkName != null)
            {
                WorkName = WorkName.Trim();
            }

            return WorkName != null && WorkName != "";
        }
        
        public bool IsWorkNameChanged(string workName)
        {
            return workName != WorkName;
        }
        
        public static bool IsWorkNameUnique(IEnumerable<string> allWorkNames, string WorkName)
        {
            return !allWorkNames.Contains(WorkName);
        }
    }

    public class Country
    {
        public int Id { get; set; }
        public required string CountryName { get; set; }

        public bool IsNameChanged(string countryName)
        {
            return countryName != CountryName;
        }
        public static bool IsCountryNameUnique(IEnumerable<string> allCountryNames, string countryName)
        {
            return !allCountryNames.Contains(countryName);
        }
        public bool IsValid()
        {
            if (CountryName != null)
            {
                CountryName = CountryName.Trim();
            }

            return CountryName != null && CountryName != "";
        }
    }
}
