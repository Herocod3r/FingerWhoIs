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
    }
}