using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CarsAndOwners.Models
{
    public enum TypeOfCar
    {
        Truck,
        Car
    }
    public class Car
    {
        public int Id { get; set; }
        public string Made { get; set; }
        public string  Model { get; set; }
        [Display(Name ="Car type")]
        public TypeOfCar TypeOfCar { get; set; }
        [Display(Name ="Was made in (year)")]
        [Range(1950,2018)]
        public int YearOfMade { get; set; }
        public int Price { get; set; }
        
        public virtual ICollection<Owner> Owners { get; set; }

        public Car()
        {
            Owners = new List<Owner>();
        }
    }
}