using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
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

            var result = Convert.FromBase64String("Rk1SACAyMAAAAADYAAABAAFoAMgAyAEAAABcH4CdAD6/AIB5AGXIAICYAG1HAICuAG3OAIDNAHLUAEBoAHTDAEC4AJFOAECBAJZNAECqAJnbAIA4AKjJAIDiAMBtAIDCAMTtAIBWAM7eAIBsANlbAIAUAO3TAECOAQRqAIDIAQXyAEBhAQnoAICKARbrAEDoAST4AECxASZuAEClATJuAIA4ATTeAIAZATndAIA9AUVZAICCAU1rAEBaAVLqAEB9AVTxAECkAVR9AICYAVbzAICYAVpwAAAA");
            var result1 = Convert.FromBase64String("aGVsbG8=");
            //var call = engine.ConvertToAnsi(result1);
            //Console.WriteLine($"The Call to convert returned a byte array of length {call.Length}");
            Console.WriteLine($"Is Fmd Valid {engine.ValidateFMD(result1)}");
            Console.WriteLine($"The Current Native Version Is ==> {engine.NativeLibVersion}");
            Console.ReadLine();
        }
        
    }
}