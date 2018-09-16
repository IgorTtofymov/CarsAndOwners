using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using CarsAndOwners.Models;
using CarsAndOwners.Models.Repository;
using Ninject;

namespace CarsAndOwners.Controllers
{
    public class OwnersController : Controller
    {
        private IRepository<Owner, Car> db;
        public OwnersController(IRepository<Owner, Car> repo)
        {
            db = repo;
        }

        // GET: Owners
        public ActionResult Index()
        {
            return View(db.GetInstances());
        }

        // GET: Owners/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Owner owner = db.GetInstance(id);
            if (owner == null)
            {
                return HttpNotFound();
            }
            return View(owner);
        }

        // GET: Owners/Create
        public ActionResult Create()
        {
            var cars = db.GetConnectedInstances().ToList();
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
                    foreach (var car in db.GetConnectedInstances().Where(car=>selectedCars.Contains(car.Id)))
                    {
                        owner.Cars.Add(car);
                    }
                }
                db.Create(owner);
                db.Save();
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
            Owner owner = db.GetInstance(id);
            if (owner == null)
            {
                return HttpNotFound();
            }
            var cars = db.GetConnectedInstances().ToList();
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
                Owner newOwner = db.GetInstance(owner.Id);
                newOwner.Name = owner.Name;
                newOwner.SecondName = owner.SecondName;
                newOwner.YearOfBirth = owner.YearOfBirth;
                newOwner.yearsOfExpirience = owner.yearsOfExpirience;
                newOwner.Cars.Clear();
                if (selectedCars != null)
                {
                    foreach (var car in db.GetConnectedInstances().Where(car=>selectedCars.Contains(car.Id)))
                    {
                        newOwner.Cars.Add(car);
                    }
                }
                db.Update(newOwner);
                db.Save();
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
            Owner owner = db.GetInstance(id);
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
            db.Delete(id);
            db.Save();
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
