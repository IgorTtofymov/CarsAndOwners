using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CarsAndOwners.Models
{
    public class Owner
    {
        public int Id { get; set; }
        public string Name { get; set; }
        [Display(Name="Second name")]
        public string SecondName { get; set; }
        [Display(Name="Born in (year)")]
        public int YearOfBirth { get; set; }
        [Display(Name="Driving for (years)")]
        public int yearsOfExpirience { get; set; }

        public virtual ICollection<Car> Cars { get; set; }

        public Owner()
        {
            Cars = new List<Car>();
        }
    }
}