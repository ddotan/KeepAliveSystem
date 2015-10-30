using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IISChecker.Utilites;
using KeepAliveSystem.ObjectModel;
using KeepAliveSystem.Utilites;
using System.Threading;

namespace IISChecker.BusinessLogic
{

    public class IISCheckerEngine 
    {
        public List<string> m_IISSites;
        private IISUtilites m_ServiceUtilites = new IISUtilites();
        private bool m_WaitingForService = false;

        public void StartServiceIfNeeded(string i_IISSite)
        {
            eIISStatus serviceStatus = m_ServiceUtilites.GetServiceStatus(i_IISSite);
            if (serviceStatus == eIISStatus.Running)
            {
                ConsoleHelper.WriteInfo("[IIS Site] : " + i_IISSite + " [Status]: Running, [Action] : Ignoring.");
            }
            else if (serviceStatus == eIISStatus.Stopped)
            {
                ConsoleHelper.WriteError("[IIS Site] : " + i_IISSite + " [Status]: Stopped, [Action] : Starting service.");
                m_ServiceUtilites.StartService(i_IISSite);
            }
            else
            {
                ConsoleHelper.WriteError("[IIS Site] : " + i_IISSite + " [Status]: " + Enum.GetName(typeof(eIISStatus), serviceStatus) + " , [Action] : waiting 10 seconds and Starting iis site again.");
                Thread.Sleep(TimeSpan.FromSeconds(10));
                if (!m_WaitingForService)
                {
                    m_WaitingForService = true;
                    m_ServiceUtilites.StartService(i_IISSite);
                }
                else
                {
                    ConsoleHelper.WriteError("[IIS Site] : " + i_IISSite + " [Status]: " + Enum.GetName(typeof(eIISStatus), serviceStatus) + " , [Action] : iis site is still down , ignoring .");
                }
                m_WaitingForService = false;
            }
        }
        public IISCheckerEngine(List<string> i_IISSites)
        {
            m_IISSites = i_IISSites;
        }
        public void CheckList()
        {
            
                foreach (string iis in m_IISSites)
                {
                    StartServiceIfNeeded(iis);
                }
        }

    }
}
