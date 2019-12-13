using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FingerWhoIs.Lib;

namespace FingerWhoIs.App
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Console.WriteLine("App to test Finger Print Identification Library.");
            var engine = new FingerEngine();
            Console.WriteLine($"The Current Native Version Is ==> {engine.NativeLibVersion}");
            Console.ReadLine();
        }
        
    }
}