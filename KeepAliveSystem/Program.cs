using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IISChecker.BusinessLogic;
using KeepAliveSystem.ObjectModel;
using ServiceChecker.BusinessLogic;

namespace KeepAliveSystem
{
    public class Program
    {
        static void Main(string[] args)
        {
            BootStrap.StartChecker("Services Checker", new ServiceCheckerManager());
            BootStrap.StartChecker("IIS sites Checker", new IISCheckerManager());

        }
    }
}
