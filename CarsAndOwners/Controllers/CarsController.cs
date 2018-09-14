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
    public class CarsController : Controller
    {
        private IRepository<Car, Owner> db;

        public CarsController()
        {
            IKernel kernel = new StandardKernel();
            kernel.Bind<IRepository<Car, Owner>>().To<CarSQLRepository>();
            db = kernel.Get<IRepository<Car,Owner>>();
        }

        // GET: Cars
        public ActionResult Index()
        {
            return View(db.GetInstances());
        }

        // GET: Cars/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Car car = db.GetInstance(id);
            IEnumerable<Owner> owners = db.GetConnectedInstances().ToList();
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
            ViewBag.Owners = db.GetConnectedInstances().ToList();
            return View();
        }

        // POST: Cars/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Made,Model,TypeOfCar,YearOfMade,Price")] Car car, int[] selectedOwners)
        {
            if (ModelState.IsValid)
            {
                if (selectedOwners != null)
                {
                    foreach (var owner in db.GetConnectedInstances().Where(own => selectedOwners.Contains(own.Id)))
                    {
                        car.Owners.Add(owner);
                    }
                }
                db.Create(car);
                db.Save();
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
            Car car = db.GetInstance(id);
            if (car == null)
            {
                return HttpNotFound();
            }
            ViewBag.Owners = db.GetConnectedInstances().ToList();
            return View(car);
        }

        // POST: Cars/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Made,Model,TypeOfCar,YearOfMade,Price")] Car car, int[] selectedOwners)
        {
            if (ModelState.IsValid)
            {
                Car newCar = db.GetInstance(car.Id);
                newCar.Made = car.Made;
                newCar.Model = car.Model;
                newCar.Price = car.Price;
                newCar.TypeOfCar = car.TypeOfCar;
                newCar.YearOfMade = car.YearOfMade;
                newCar.Owners.Clear();
                if (selectedOwners != null)
                {
                    foreach (var owner in db.GetConnectedInstances().Where(own=>selectedOwners.Contains(own.Id)))
                    {
                        newCar.Owners.Add(owner);
                    }
                }
                db.Update(newCar);
                db.Save();
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
            Car car = db.GetInstance(id);

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
