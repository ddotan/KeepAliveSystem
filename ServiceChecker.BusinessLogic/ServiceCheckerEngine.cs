using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KeepAliveSystem.Utilites;
using ServiceChecker.Utilites;
using System.Threading;

namespace ServiceChecker.BusinessLogic
{
    public class ServiceCheckerEngine
    {
        public List<string> m_ServiceList;
        private bool m_WaitingForService = false;
        private ServiceUtilites m_ServiceUtilites = new ServiceUtilites();
        public ServiceCheckerEngine(List<string> i_ServiceList)
        {
            m_ServiceList = i_ServiceList;
        }
        public void CheckList()
        {
            foreach (string service in m_ServiceList)
            {
                try
                {
                    StartServiceIfNeeded(service);
                }
                catch (Exception ex)
                {
                }
            }
        }
        public void StartServiceIfNeeded(string i_ServiceName)
        {
            eServiceStatus serviceStatus = m_ServiceUtilites.GetServiceStatus(i_ServiceName);
            if (serviceStatus == eServiceStatus.Running)
            {
                ConsoleHelper.WriteInfo("[Service] : " + i_ServiceName + " [Status]: Running, [Action] : Ignoring.");
            }
            else if (serviceStatus == eServiceStatus.Stopped)
            {
                ConsoleHelper.WriteError("[Service] : " + i_ServiceName + " [Status]: Stopped, [Action] : Starting service.");
                m_ServiceUtilites.StartService(i_ServiceName);
            }
            else
            {
                ConsoleHelper.WriteError("[Service] : " + i_ServiceName + " [Status]: " + Enum.GetName(typeof(eServiceStatus), serviceStatus) + " , [Action] : waiting 10 seconds and Starting service again.");
                Thread.Sleep(TimeSpan.FromSeconds(10));
                if (!m_WaitingForService)
                {
                    m_WaitingForService = true;
                    m_ServiceUtilites.StartService(i_ServiceName);
                }
                else
                {
                    ConsoleHelper.WriteError("[Service] : " + i_ServiceName + " [Status]: " + Enum.GetName(typeof(eServiceStatus), serviceStatus) + " , [Action] : service is still down , ignoring .");
                }
                m_WaitingForService = false;
            }
        }
    }
}
