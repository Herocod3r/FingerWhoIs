using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Linq;

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
    internal struct dpfj_candidate
    {
        public uint size;
        public uint fmd_idx;
        public uint view_idx;
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

        [DllImport("dpfj",EntryPoint="dpfj_compare")]
        private static extern unsafe int dpfj_compare(int fmd1_type,byte* fmd1, uint fmd1_size,uint fmd1_view_idx,int fmd2_type,byte* fmd2,uint fmd2_size,uint fmd2_view_idx,ref int score);

        [DllImport("dpfj",EntryPoint="dpfj_identify")]
        private static extern unsafe int dpfj_identify(
            int fmd1_type, byte* fmd1, uint fmd1_size, uint fmd1_view_idx, int fmds_type, uint fmds_cnt, [MarshalAs(UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof(JaggedArrayMarshaler))] [In] byte[][] fmds,
            int* fmds_size, uint threshold_score,
            ref uint candidate_cnt, dpfj_candidate* candidates
        );
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

        internal static unsafe NativeCall<int> CompareTwoFmd(ReadOnlySpan<byte> fmd1, FmdFormat fmd1Format,ReadOnlySpan<byte> fmd2, FmdFormat fmd2Format)
        {
            int score = 0;
            int call = 0;
            fixed(byte* fmd1ptr = fmd1)
            fixed(byte* fmd2ptr = fmd2)
            {
                call = dpfj_compare((int) fmd1Format, fmd1ptr, (uint) fmd1.Length, 0, (int) fmd2Format, fmd2ptr,
                    (uint) fmd2.Length, 0, ref score);
            }

            return new NativeCall<int>{Code = call,Value = score};
        }


        internal static unsafe NativeCall<List<int>> Indentify(ReadOnlySpan<byte> fmd,FmdFormat fmdFormat,ref byte[][] fmds, uint threshold,uint maxCandidateReturend)
        {
            Span<dpfj_candidate> candidates = stackalloc dpfj_candidate[(int)maxCandidateReturend];
            Span<int> fmdSizes = fmds.Length <= 128 ? stackalloc int[fmds.Length] : new int[fmds.Length];
            for (int i = 0; i < fmds.Length; i++)
            {
                fmdSizes[i] = fmds[i].Length;
            }

            int call = 0;
            
            fixed(byte* fmd1Ptr = fmd)
            fixed(int* fmdSizesptr = fmdSizes)
            fixed(dpfj_candidate* candidatesPtr = candidates)
            {
                call = dpfj_identify((int) fmdFormat, fmd1Ptr, (uint)fmd.Length, 0, (int) fmdFormat, (uint)fmds.Length, fmds,
                    fmdSizesptr, threshold, ref maxCandidateReturend, candidatesPtr);
            }
            
            return new NativeCall<List<int>>{Code = call,Value = candidates.Slice(0,(int)maxCandidateReturend).ToArray()?.Select(x=>(int)x.fmd_idx).ToList()};
        } 
        
    }

}
