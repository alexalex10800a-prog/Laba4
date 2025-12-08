using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL2.Interfaces;
using DAL2.Repositories;
using Ninject.Modules;

namespace BLL2
{
    public class ServiceModule : NinjectModule
    {
        private string connectionString;
        public ServiceModule (string connection)
        {
            connectionString = connection;
        }
        public override void Load()
        {
            Bind<IDbRepos>().To<DbReposSQL>().InSingletonScope().WithConstructorArgument(connectionString);
        }
    }
}
