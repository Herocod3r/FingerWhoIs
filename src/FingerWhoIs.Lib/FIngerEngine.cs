using System;
using FingerWhoIs.Lib.Native;

namespace FingerWhoIs.Lib
{
    public class FingerEngine
    {
        static FingerEngine()
        {
            Loader.Init();
        }

        public string NativeLibVersion => NativeApi.GetVersion();

        public Span<byte> ConvertToAnsi(byte[] isoFmd)
        {
            var nativeCall = NativeApi.ConvertFMD(FmdFormat.Iso, isoFmd, FmdFormat.Ansi);
            Console.WriteLine($"The call to the native library for fmd convert returned {nativeCall.Code}");
            return nativeCall.Value;
        }

        public bool ValidateFMD(ReadOnlySpan<byte> fmd)
        {
            var nativeCall = NativeApi.ValidateFMD(fmd,FmdFormat.Iso);
            return nativeCall.Value.view_cnt > 0;
        }
    }
}