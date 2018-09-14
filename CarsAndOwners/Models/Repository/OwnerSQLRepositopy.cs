using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;

namespace CarsAndOwners.Models.Repository
{
    public class OwnerSQLRepositopy : IRepository<Owner, Car>
    {
        private CarOwnerContext db;
        public OwnerSQLRepositopy()
        {
            db = new CarOwnerContext();
        }
        public void Create(Owner item)
        {
            db.Owners.Add(item);
        }

        public void Delete(int id)
        {
            Owner owner = db.Owners.Find(id);
            foreach (var car in db.Cars.Include(c => c.Owners))
            {
                if (car.Owners.Contains(owner))
                    car.Owners.Remove(owner);
            }
            db.Owners.Remove(owner);
        }

        private bool disposed = false;

        public virtual void Dispose(bool disposing)
        {
            if (!disposed)
            {
                if (disposing)
                {
                    db.Dispose();
                }
            }
            disposed = true;
        }
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public IEnumerable<Car> GetConnectedInstances()
        {
            return db.Cars;
        }

        public Owner GetInstance(int? id)
        {
            return db.Owners.Find(id);
        }

        public IEnumerable<Owner> GetInstances()
        {
            return db.Owners;
        }

        public void Save()
        {
            db.SaveChanges();
        }

        public void Update(Owner item)
        {
            db.Entry(item).State = EntityState.Modified;
        }
    }
}