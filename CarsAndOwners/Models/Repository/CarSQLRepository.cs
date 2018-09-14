using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
namespace CarsAndOwners.Models.Repository
{
    public class CarSQLRepository : IRepository<Car, Owner>
    {
        private CarOwnerContext db;

        public CarSQLRepository()
        {
            this.db = new CarOwnerContext();
        }

        public void Create(Car item)
        {
            db.Cars.Add(item);
        }

        public void Delete(int id)
        {
            Car car = db.Cars.Find(id);
            if (car != null)
            {
                foreach (Owner owner in db.Owners.Include(o => o.Cars))
                {
                    if (owner.Cars.Contains(car))
                        owner.Cars.Remove(car);
                }
                db.Cars.Remove(car); ;
            }
        }

        private bool disposed = false;

        public IEnumerable<Owner> GetConnectedInstances()
        { return db.Owners; }
        public virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    db.Dispose();
                }
            }
            this.disposed = true;
        }
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public Car GetInstance(int? id)
        {
            return db.Cars.Find(id);
        }

        public IEnumerable<Car> GetInstances()
        {
            return db.Cars;
        }

        public void Save()
        {
            db.SaveChanges() ;
        }

        public void Update(Car item)
        {
            db.Entry(item).State = EntityState.Modified;
        }
    }
}