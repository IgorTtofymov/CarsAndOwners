using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CarsAndOwners.Models;
using CarsAndOwners.Models.Repository;
using Ninject;
using Ninject.Modules;

namespace CarsAndOwners.Util
{
    public class NinjectRegister : NinjectModule
    {
        public override void Load()
        {
            //Bind<IRepository<Car, Owner>>().To<CarSQLRepository>();
            Bind<IRepository<Owner, Car>>().To<OwnerSQLRepositopy>();
        }
    }
}