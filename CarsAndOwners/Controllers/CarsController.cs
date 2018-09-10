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
    public class CarsController : Controller
    {
        private CarOwnerContext db = new CarOwnerContext();

        // GET: Cars
        public ActionResult Index()
        {
            return View(db.Cars.ToList());
        }

        // GET: Cars/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Car car = db.Cars.Find(id);
            IEnumerable<Owner> owners = db.Owners.ToList();
            ViewBag.Owners = owners;
            if (car == null)
            {
                return HttpNotFound();
            }
            return View(car);
        }

        // GET: Cars/Create
        public ActionResult Create()
        {
            ViewBag.Owners = db.Owners.ToList();
            return View();
        }

        // POST: Cars/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Made,Model,TyoeOfCar,YearOfMade,Price")] Car car, int[] selectedOwners)
        {
            if (ModelState.IsValid)
            {
                if (selectedOwners != null)
                {
                    foreach (var owner in db.Owners.Where(own => selectedOwners.Contains(own.Id)))
                    {
                        car.Owners.Add(owner);
                    }
                }
                db.Cars.Add(car);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(car);
        }

        // GET: Cars/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Car car = db.Cars.Find(id);
            if (car == null)
            {
                return HttpNotFound();
            }
            ViewBag.Owners = db.Owners.ToList();
            return View(car);
        }

        // POST: Cars/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Made,Model,TyoeOfCar,YearOfMade,Price")] Car car, int[] selectedOwners)
        {
            if (ModelState.IsValid)
            {
                Car newCar = db.Cars.Find(car.Id);
                newCar.Made = car.Made;
                newCar.Model = car.Model;
                newCar.Price = car.Price;
                newCar.TyoeOfCar = car.TyoeOfCar;
                newCar.YearOfMade = car.YearOfMade;
                newCar.Owners.Clear();
                if (selectedOwners != null)
                {
                    foreach (var owner in db.Owners.Where(own=>selectedOwners.Contains(own.Id)))
                    {
                        newCar.Owners.Add(owner);
                    }
                }
                db.Entry(newCar).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(car);
        }

        // GET: Cars/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Car car = db.Cars.Find(id);

            if (car == null)
            {
                return HttpNotFound();
            }
            return View(car);
        }

        // POST: Cars/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Car car = db.Cars.Find(id);
            foreach (Owner owner in db.Owners.Include(o=>o.Cars))
            {
                if(owner.Cars.Contains(car))
                owner.Cars.Remove(car);
            }
            db.Cars.Remove(car);
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
