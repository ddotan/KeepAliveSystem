using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using KeepAliveSystem.Utilites;

namespace ServiceChecker.Utilites
{
    public enum eServiceStatus
    {
        Running, Stopped, Paused, Stopping, Starting, UnKnown
    }
    public class ServiceUtilites
    {
        private static int m_ServiceStartTimeInteval;
        public eServiceStatus GetServiceStatus(string i_ServiceName)
        {
            ServiceController sc = new ServiceController(i_ServiceName);
            eServiceStatus serviceStatus;
            switch (sc.Status)
            {
                case ServiceControllerStatus.Running:
                    serviceStatus = eServiceStatus.Running;
                    break;
                case ServiceControllerStatus.Stopped:
                    serviceStatus = eServiceStatus.Stopped;
                    break;

                case ServiceControllerStatus.Paused:
                    serviceStatus = eServiceStatus.Paused;
                    break;

                case ServiceControllerStatus.StopPending:
                    serviceStatus = eServiceStatus.Stopping;
                    break;

                case ServiceControllerStatus.StartPending:
                    serviceStatus = eServiceStatus.Starting;
                    break;

                default:
                    serviceStatus = eServiceStatus.UnKnown;
                    break;

            }
            return serviceStatus;
        }
        public ServiceUtilites()
        {
            m_ServiceStartTimeInteval = int.Parse(ConfigurationManager.AppSettings["WaitIntervalForServiceToStart"]);
        }
        public void StartService(string i_ServiceName)
        {
            try
            {
                ServiceController service = new ServiceController(i_ServiceName);
                TimeSpan timeout = TimeSpan.FromSeconds(m_ServiceStartTimeInteval);
                service.Start();
                service.WaitForStatus(ServiceControllerStatus.Running, timeout);
                ConsoleHelper.WriteInfo("[Service] : " + i_ServiceName + " [Status] : Service is up !");

            }
            catch (Exception ex)
            {
                ConsoleHelper.WriteError("[Service] : " + i_ServiceName + " [Error] : couldnt Start Service. [Exception] : " + ex.Message);

            }
        }
    }
}
