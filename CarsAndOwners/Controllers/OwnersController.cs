using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using CarsAndOwners.Models;

namespace CarsAndOwners.Controllers
{
    public class OwnersController : Controller
    {
        private CarOwnerContext db = new CarOwnerContext();

        // GET: Owners
        public ActionResult Index()
        {
            Owner o1 = new Owner() { Name = "Harry" };
            db.Owners.Add(o1);
            db.SaveChanges();
            return View(db.Owners.ToList());
        }

        // GET: Owners/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Owner owner = db.Owners.Find(id);
            if (owner == null)
            {
                return HttpNotFound();
            }
            return View(owner);
        }

        // GET: Owners/Create
        public ActionResult Create()
        {
            var cars = db.Cars.ToList();
            ViewBag.Cars = cars;
            return View();
        }

        // POST: Owners/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Name,SecondName,YearOfBirth,yearsOfExpirience")] Owner owner, int[] selectedCars)
        {
            if (ModelState.IsValid)
            {
                if (selectedCars != null)
                {
                    foreach (var car in db.Cars.Where(car=>selectedCars.Contains(car.Id)))
                    {
                        owner.Cars.Add(car);
                    }
                }
                db.Owners.Add(owner);
                db.SaveChanges();
                return RedirectToAction("Index", "Owners");
            }

            return View(owner);
        }

        // GET: Owners/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Owner owner = db.Owners.Find(id);
            if (owner == null)
            {
                return HttpNotFound();
            }
            var cars = db.Cars.ToList();
            ViewBag.Cars = cars;
            return View(owner);
        }

        // POST: Owners/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name,SecondName,YearOfBirth,yearsOfExpirience")] Owner owner, int[] selectedCars)
        {
            if (ModelState.IsValid)
            {
                Owner newOwner = db.Owners.Find(owner.Id);
                newOwner.Name = owner.Name;
                newOwner.SecondName = owner.SecondName;
                newOwner.YearOfBirth = owner.YearOfBirth;
                newOwner.yearsOfExpirience = owner.yearsOfExpirience;
                newOwner.Cars.Clear();
                if (selectedCars != null)
                {
                    foreach (var car in db.Cars.Where(car=>selectedCars.Contains(car.Id)))
                    {
                        newOwner.Cars.Add(car);
                    }
                }
                db.Entry(newOwner).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(owner);
        }

        // GET: Owners/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Owner owner = db.Owners.Find(id);
            if (owner == null)
            {
                return HttpNotFound();
            }
            return View(owner);
        }

        // POST: Owners/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Owner owner = db.Owners.Find(id);
            foreach (var car in db.Cars.Include(c=>c.Owners))
            {
                if(car.Owners.Contains(owner))
                car.Owners.Remove(owner);
            }
            db.SaveChanges();
            db.Owners.Remove(owner);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
