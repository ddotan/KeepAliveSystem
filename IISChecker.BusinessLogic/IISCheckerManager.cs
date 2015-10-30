using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using KeepAliveSystem.ObjectModel;

namespace IISChecker.BusinessLogic
{
    public class IISCheckerManager : IAppChecker
    {
        private List<string> m_IISList;
        private IISCheckerEngine m_Checker;
        private int m_WaitBetweenChecksInterval;
        public IISCheckerManager()
        {
            m_IISList = getProcess();
            foreach (string str in m_IISList)
            {
                Console.WriteLine("Service checker listening to IIS named : " + str);
            }
            m_Checker = new IISCheckerEngine(m_IISList);
            m_WaitBetweenChecksInterval = int.Parse(ConfigurationManager.AppSettings["WaitBetweenChecksInterval"]);
        }
        public void Start()
        {
            if (m_IISList.Count == 1 && string.IsNullOrEmpty(m_IISList[0]) )
            {
                Console.WriteLine("[IIS] appconfig list is empty, shutting down");
                return;
            }
            while (true)
            {
                m_Checker.CheckList();
                Thread.Sleep(5 * 1000);
            }
        }
        public List<string> getProcess()
        {
            List<string> process;
            process = ConfigurationManager.AppSettings["IISSitesNames"].Split(',').ToList<string>();
            return process;
        }
    }
}
