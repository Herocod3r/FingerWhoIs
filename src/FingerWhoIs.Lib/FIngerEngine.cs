using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using FingerWhoIs.Lib.Native;
using static FingerWhoIs.Lib.Const;

namespace FingerWhoIs.Lib
{
    public class FingerEngine
    {
        static FingerEngine()
        {
            Loader.Init();
        }
        public string NativeLibVersion => NativeApi.GetVersion();

        public Span<byte> ConvertFmdFormat(ReadOnlySpan<byte> fmd,FmdFormat sourceFormat, FmdFormat destFormat)
        {
            if(!ValidateFMD(fmd,sourceFormat)) throw new ArgumentException("This fmd is invalid");
            var nativeCall = NativeApi.ConvertFMD(sourceFormat, fmd, destFormat);
            if (nativeCall.Code != 0)
            {
                var message = nativeCall.Code switch
                {
                    InvalidParameter => "Invalid Parameter",
                    Failure => "Unable to Process this fmd",
                    _ => "Engine Error"
                };
                throw new FingerMatchException(nativeCall.Code,message);
            }
            return nativeCall.Value;
        }

        public bool ValidateFMD(ReadOnlySpan<byte> fmd, FmdFormat format = FmdFormat.Iso)
        {
            if (fmd.IsEmpty) return false;
            var nativeCall = NativeApi.ValidateFMD(fmd,format);
            return nativeCall.Value.view_cnt > 0;
        }


        public int CompareTwoFmds(ReadOnlySpan<byte> fmd1, ReadOnlySpan<byte> fmd2,FmdFormat fmdFormat1 = FmdFormat.Iso, FmdFormat fmdFormat2 = FmdFormat.Iso)
        {
            if(!ValidateFMD(fmd1,fmdFormat1) || !ValidateFMD(fmd2,fmdFormat2)) throw new ArgumentException("Invalid fmds");

            var nativeCall = NativeApi.CompareTwoFmd(fmd1, fmdFormat1, fmd2, fmdFormat2);
            if (nativeCall.Code != 0)
            {
                var message = nativeCall.Code switch
                {
                    InvalidParameter => "Invalid Parameter",
                    Failure => "Unable to Compare Fmds",
                    _ => "Engine Error"
                };
                throw new FingerMatchException(nativeCall.Code,message);
            }

            return nativeCall.Value;
        }

        public List<int> IdentifyFmd(ReadOnlySpan<byte> fmd1,ReadOnlySpan<byte[]> fmds,uint maxToReturn = 10,uint threshold = 21474, FmdFormat format = FmdFormat.Iso)
        {
            var fmdsArray = fmds.ToArray();
            for (int i = 0; i < fmdsArray.Length; i++)
            {
                if (fmdsArray[i] is null)
                {
                    fmdsArray[i] = new byte[0];
                }
            }
            var nativeCall = NativeApi.Indentify(fmd1, format, ref fmdsArray, threshold, maxToReturn);
            if (nativeCall.Code != 0)
            {
                var message = nativeCall.Code switch
                {
                    InvalidParameter => "Invalid Parameter",
                    Failure => "Unable to Compare Fmds",
                    _ => "Engine Error"
                };
                throw new FingerMatchException(nativeCall.Code,message);
            }

            if (nativeCall.Value.Count < 1) return new List<int>();

            return nativeCall.Value;
        }
    }
}