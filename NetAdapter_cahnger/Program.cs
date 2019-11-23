using System;
using System.Management;
using System.IO;
using System.Windows.Forms;

namespace NetAdapter_cahnger
{
    static class Program
    {
        [STAThread]
        static void Main()
        {
            try
            {
                string[] netAdapters = File.ReadAllLines("netAdapters.txt");
                string message = "";
                try
                {
                    message = File.ReadAllText("message.txt");
                }
                catch { }
                NetAdatersStateChange(netAdapters);
                try
                {
                    message = File.ReadAllText("message.txt");
                    MessageBox.Show(message);
                }
                catch { }

            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString(), "[error]");
            }

        }

        static void NetAdatersStateChange(params string[] list)
        {
            SelectQuery wmiQuery = new SelectQuery("SELECT * FROM Win32_NetworkAdapter WHERE NetConnectionId != NULL");
            ManagementObjectSearcher searchProcedure = new ManagementObjectSearcher(wmiQuery);
            foreach (ManagementObject item in searchProcedure.Get())
            {
                //if (((string)item["NetConnectionId"]) == "name") //"Local Network Connection")
                if (Array.IndexOf(list, (string)item["NetConnectionId"])!=-1)
                {
                    if (Convert.ToBoolean(item["NetEnabled"]))
                    {
                        item.InvokeMethod("Disable", null);
                    }
                    else
                    {
                        item.InvokeMethod("Enable", null);
                    }
                }
            }
        }

    }

}
