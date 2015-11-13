using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KeepAliveSystem.Utilites
{
    public class ConsoleHelper
    {
        private static bool m_DebugMode = Boolean.Parse(ConfigurationManager.AppSettings["DebugMode"].ToLower());
        public static void WriteError(string i_Text)
        {
            writeInColor(i_Text, ConsoleColor.Red);

        }
        public static void WriteInfo(string i_Text)
        {
            if (m_DebugMode)
            {
                writeInColor(i_Text, ConsoleColor.Green);
            }
        }

        private static void writeInColor(string i_Text, ConsoleColor i_ConsoleColor)
        {
            Console.ForegroundColor = i_ConsoleColor;
            Console.WriteLine(DateTime.Now.ToString() + " : " +i_Text);
            Console.ResetColor();
        }
    }
}
