using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BLL2;
using BLL2.Interface;
using BLL2.Services;
using Ninject.Modules;
namespace Lab5MVC_asp.net.Util
{
    public class NinjectRegistrations : NinjectModule
    {
        public override void Load()
        {
            Bind<IDbCrud>().To<DBDataOperations>();
            Bind<IReportService>().To<ReportServiceForLab4>();
        }
    }
}
