using System;
using System.Runtime.CompilerServices;
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
    internal struct dpfj_fmd_record_params
    {
        public uint record_length;
        public uint cbeff_id;
        public uint capture_equipment_comp;
        public uint capture_equipment_id;
        public uint width;
        public uint height;
        public uint resolution;
        public uint view_cnt;
    }
    

    [StructLayout(LayoutKind.Sequential)]
    internal struct dpfj_version
    {
        public uint size;
        public dpfj_ver_info lib_ver;
        public dpfj_ver_info api_ver;
    }

    internal class NativeCall<TValue>
    {
        public int Code { get; set; }
        public TValue Value { get; set; }
    }
    
    
    internal class NativeApi
    {
        [DllImport("dpfj",EntryPoint="dpfj_version")]
        private static extern int dpfj_version(ref dpfj_version version);

        [DllImport("dpfj",EntryPoint="dpfj_fmd_convert")]
        private static extern unsafe int dpfj_fmd_convert(int fmd1_type,byte* fmd1,uint fmd1_size,int fmd2_type,byte* fmd2,ref uint fmd2_size);
        
        [DllImport("dpfj",EntryPoint="dpfj_get_fmd_record_params")]
        private static extern unsafe void dpfj_get_fmd_record_params(int fmd_type,byte* fmd,ref dpfj_fmd_record_params @params);

        //private static extern unsafe void dpfj_compare(int fmd1_type,byte* fmd1, uint fmd1_size,uint fmd1_view_idx,);
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

        internal static unsafe NativeCall<byte[]> ConvertFMD(FmdFormat sourceFormat,ReadOnlySpan<byte> sourceFmd,FmdFormat destFormat)
        {
            Span<byte> rspBytes = new byte[1024];
            uint size = (uint)rspBytes.Length;
            var nativeCall = new NativeCall<byte[]>();
            fixed (byte* bytePr = rspBytes)
            fixed (byte* inputPr = sourceFmd)
            {
                
                var cal = dpfj_fmd_convert((int) sourceFormat,inputPr, (uint)sourceFmd.Length, (int) destFormat, bytePr, ref size);
                nativeCall.Code = cal;
                if (cal != 0) return nativeCall;
            }
            
            nativeCall.Value = rspBytes.Slice(0, (int)size).ToArray();
           return nativeCall;
        }

        internal static unsafe NativeCall<dpfj_fmd_record_params> ValidateFMD(ReadOnlySpan<byte> fmd,FmdFormat format)
        {
            var r = new dpfj_fmd_record_params();
            
            fixed(byte* inputPr = fmd)
            {
                dpfj_get_fmd_record_params((int) format, inputPr, ref r);
            }
            
            Console.WriteLine($"Call returned a view count of {r.view_cnt}");
            return new NativeCall<dpfj_fmd_record_params>{ Value = r};
        }

        /*internal static NativeCall<float> CompareTwoFmd()
        {
            
        }
        */

        
        /*private static extern int dpfj_create_fmd_from_raw(
            ref byte[] imageData,uint imageSize, uint imageWidth,uint imageHeight,uint imageDpi,int finger_pos,int cbeff_id,
            )*/
        
        

    }

    public enum FmdFormat
    {
        Ansi = 0x001B0001,Iso = 0x01010001
    }
}
