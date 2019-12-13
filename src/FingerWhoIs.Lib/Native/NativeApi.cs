using System;
using System.Runtime.InteropServices;

namespace FingerWhoIs.Lib.Native
{
    [StructLayout(LayoutKind.Sequential)]
    internal struct dpfj_ver_info
    {
        public int major;
        public int minor;
        public int maintenance;
    }

    [StructLayout(LayoutKind.Sequential)]
    internal struct dpfj_version
    {
        public uint size;
        public dpfj_ver_info lib_ver;
        public dpfj_ver_info api_ver;
    }
    
    internal class NativeApi
    {
        [DllImport("dpfj",EntryPoint="dpfj_version")]
        private static extern int dpfj_version(ref dpfj_version version);
        internal static string GetVersion()
        {
            dpfj_version version = new dpfj_version();
            version.size = 100;
            var res = dpfj_version(ref version);
            if(res == 0) return $"{version.lib_ver.major}.{version.lib_ver.minor}.{version.lib_ver.maintenance}";
            else
            {
                Console.WriteLine("Version not found");
            }

            return default;

        }

    }
}