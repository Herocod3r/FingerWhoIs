using System;
using System.Diagnostics;
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
            var nativeCall = NativeApi.ValidateFMD(fmd,format);
            return nativeCall.Value.view_cnt > 0;
        }
    }
}