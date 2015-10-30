using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using KeepAliveSystem.ObjectModel;

namespace KeepAliveSystem
{
    public static class BootStrap
    {
        public static void StartChecker(string i_CheckerName, IAppChecker i_AppChecker)
        {
            Console.WriteLine("[Checker] : " + i_CheckerName + " Started.");
            new Thread(() =>
            {
                i_AppChecker.Start();
            }).Start();

        }
    }
}
