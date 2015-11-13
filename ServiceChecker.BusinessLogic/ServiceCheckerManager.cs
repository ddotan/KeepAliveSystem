using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using KeepAliveSystem.ObjectModel;
namespace ServiceChecker.BusinessLogic
{
    public class ServiceCheckerManager : IAppChecker
    {
        private List<string> m_ServiceList;
        private ServiceCheckerEngine m_Checker;
        private int m_WaitBetweenCheckSeconds;
        public ServiceCheckerManager()
        {
            m_ServiceList = getProcess();
            foreach (string str in m_ServiceList)
            {
                Console.WriteLine("Service checker listening to service named : " + str);
            }
            m_Checker = new ServiceCheckerEngine(m_ServiceList);
            m_WaitBetweenCheckSeconds = int.Parse(ConfigurationManager.AppSettings["WaitBetweenCheckSeconds"]);
        }
        public void Start()
        {
            if (m_ServiceList.Count == 1 && string.IsNullOrEmpty(m_ServiceList[0]))
            {
                Console.WriteLine("[Services] List is empty, shutting down");
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
            List<string> process = null;
            process = ConfigurationManager.AppSettings["ServicesNames"].Split(',').ToList<string>();
            return process;
        }
    }

}
