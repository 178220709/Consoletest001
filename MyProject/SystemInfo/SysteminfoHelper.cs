using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Management;
using System.Runtime.InteropServices;


namespace MyProject.SystemInfo
{
        //定义CPU的信息结构 
        [StructLayout(LayoutKind.Sequential)]
        public struct CPU_INFO
        {
            public uint dwOemId;
            public uint dwPageSize;
            public uint lpMinimumApplicationAddress;
            public uint lpMaximumApplicationAddress;
            public uint dwActiveProcessorMask;
            public uint dwNumberOfProcessors;
            public uint dwProcessorType;
            public uint dwAllocationGranularity;
            public uint dwProcessorLevel;
            public uint dwProcessorRevision;
        }



    public class SysteminfoHelper
    {
        public static string GetCpuID()
        {
            try
            {
                ManagementClass mc = new ManagementClass("Win32_Processor");
                ManagementObjectCollection moc = mc.GetInstances();
                return moc.Cast<ManagementObject>()
                    .Select(a => a.Properties["ProcessorId"].Value.ToString()).First();
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
          
        }  
        
        public static string GetMemorySize()
        {
            try
            {
                string st = "";
                ManagementClass mc = new ManagementClass("Win32_ComputerSystem");
                ManagementObjectCollection moc = mc.GetInstances();
                foreach (ManagementObject mo in moc)
                {
                    st = mo["TotalPhysicalMemory"].ToString();
                }
                return (Int64.Parse(st) / (1024 * 1024)).ToString();
                
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }


        [DllImport("kernel32 ")]
        private static extern void GetSystemInfo(ref   CPU_INFO cpuinfo);

        public static string GetSystemInfo()
        {
            //调用GetSystemInfo函数获取CPU的相关信息 
            CPU_INFO CpuInfo;
            CpuInfo = new CPU_INFO();
            GetSystemInfo(ref   CpuInfo);
            StringBuilder sb = new StringBuilder();
            sb.Append("本计算机中有 " + CpuInfo.dwNumberOfProcessors.ToString() + "个CPU ");
            sb.Append(System.Environment.NewLine);
            sb.Append("CPU的类型为 " + CpuInfo.dwProcessorType.ToString());
            sb.Append(System.Environment.NewLine);
            sb.Append("CPU等级为 " + CpuInfo.dwProcessorLevel.ToString());
            sb.Append(System.Environment.NewLine);
            sb.Append("CPU的OEM ID为 " + CpuInfo.dwOemId.ToString());
            sb.Append(System.Environment.NewLine);
            sb.Append("CPU中的页面大小为 " + CpuInfo.dwPageSize.ToString());
            sb.Append(System.Environment.NewLine);

            return sb.ToString();
        }


        public static string GetProcessInfo()
        {  try
            {
                string s = "";
                System.Int32 processid;
                Process process;
                //Retrieve the additional information about a specific process
                processid = int.Parse(GetCpuID());
                process = System.Diagnostics.Process.GetProcessById(processid);
                s = s + "该进程的总体优先级类别:" + Convert.ToString(process.PriorityClass) + " \r\n";
                s = s + "由该进程打开的句柄数:" + process.HandleCount + "\r\n";
                s = s + "该进程的主窗口标题:" + process.MainWindowTitle + "\r\n";
                s = s + " 该进程允许的最小工作集大小:" + process.MinWorkingSet.ToString() + " \r\n";
                s = s + "该进程允许的最大工作集大小:" + process.MaxWorkingSet.ToString() + " \r\n";
                s = s + "该进程的分页内存大小:" + process.PagedMemorySize + "\r\n";
                s = s + "该进程的峰值分页内存大小:" + process.PeakPagedMemorySize + "\r\n";
                return s; 
                
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }
    }

}
