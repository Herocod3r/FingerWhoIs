using System;
using System.Reflection;
using NativeLibraryManager;

namespace FingerWhoIs.Lib
{
    internal static class Loader
    {
        private static bool isLoaded;
        internal static void Init()
        {
            if(isLoaded) return;
            var accessor = new ResourceAccessor(Assembly.GetExecutingAssembly());
            var libManager = new LibraryManager(
                Assembly.GetExecutingAssembly(),
                new LibraryItem(Platform.Windows, Bitness.x64,
                    new LibraryFile("dpfj.dll", accessor.Binary("dpfj.dll"))),
                new LibraryItem(Platform.Linux, Bitness.x64,
                    new LibraryFile("libdpfj.so", accessor.Binary("libdpfj.so"))));
            
            libManager.LoadNativeLibrary();
            isLoaded = true;
        }
    }
}