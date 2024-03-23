using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Management;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace performance_monitoring
{
    class Ram
    {
        public static float GetTotalPhysicalMemory()
        {
            float totalMemory = 0;

            try
            {

                var memoryList = GetHardwareInfo("Win32_PhysicalMemory", "Capacity");

                foreach (string memorySize in memoryList)
                {
                    // Переводим значение из байтов в гигабайты и суммируем
                    totalMemory += Convert.ToSingle(memorySize) / (1024 * 1024);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }

            return totalMemory;
        }
        private static List<string> GetHardwareInfo(string WIN32_Class, string ClassItemField)
        {
            List<string> result = new List<string>();

            ManagementObjectSearcher searcher = new ManagementObjectSearcher("SELECT * FROM " + WIN32_Class);

            try
            {
                foreach (ManagementObject obj in searcher.Get())
                {
                    result.Add(obj[ClassItemField].ToString().Trim());
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return result;
        }

    }
}
