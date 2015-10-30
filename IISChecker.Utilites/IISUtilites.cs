using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using KeepAliveSystem.Utilites;
using Microsoft.Web.Administration;

namespace IISChecker.Utilites
{
    public enum eIISStatus
    {
        Running,
        Stopped,
        Stopping,
        Starting,
        Unknown
    }
    public class IISUtilites
    {

        private static int m_ServiceStartTimeInteval;
        private ServerManager m_IISServerManager = new ServerManager();
        public eIISStatus GetServiceStatus(string i_IISSite)
        {
            Site site = m_IISServerManager.Sites.FirstOrDefault(s => s.Name == i_IISSite);
            eIISStatus iisStatus;
            if (site == null)
            {
                ConsoleHelper.WriteError("Couldnt find IIS Site named : " + i_IISSite);
                iisStatus = eIISStatus.Unknown;
            }
            else
            {

                switch (site.State)
                {
                    case ObjectState.Started:
                        iisStatus = eIISStatus.Running;
                        break;
                    case ObjectState.Starting:
                        iisStatus = eIISStatus.Starting;
                        break;
                    case ObjectState.Stopped:
                        iisStatus = eIISStatus.Stopped;
                        break;
                    case ObjectState.Stopping:
                        iisStatus = eIISStatus.Stopping;
                        break;
                    case ObjectState.Unknown:
                        iisStatus = eIISStatus.Unknown;
                        break;

                    default:
                        iisStatus = eIISStatus.Unknown;
                        break;
                }
            }
            return iisStatus;
        }
    
        public IISUtilites()
        {
            m_ServiceStartTimeInteval = int.Parse(ConfigurationManager.AppSettings["WaitIntervalForServiceToStart"]);
        }
        public void StartService(string i_IISSite)
        {
            try
            {
                Site site = m_IISServerManager.Sites.FirstOrDefault(s => s.Name == i_IISSite);
                site.Start();
                Thread.Sleep(TimeSpan.FromSeconds(m_ServiceStartTimeInteval));
                if (GetServiceStatus(i_IISSite) == eIISStatus.Running)
                {
                    ConsoleHelper.WriteInfo("[IIS Site] : " + i_IISSite + " [Status] : IIS Site is up !");
                }
                else
                {
                    ConsoleHelper.WriteError("[IIS Site] : " + i_IISSite + " [Status] : IIS Site is still down !");

                }
            }
            catch (Exception ex)
            {
                ConsoleHelper.WriteError("[IIS Site] : " + i_IISSite + " [Error] : Coulnt start IIS Site. [Exception] : " + ex.Message);

            }
        }
     
    }
}