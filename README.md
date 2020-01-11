# FingerWhoIs

A crossplatform Fingerprint Identification Lib built on Digital Persona FingerJetEngine Api for .Net


### Setup
* Available on NuGet: [FingerWhoIs](https://www.nuget.org/packages/FingerWhois/) 

### Sample Apis

``` Csharp

            var engine = new FingerEngine();

            var result1 = Convert.FromBase64String("aGVsbG8=");
            var call = engine.ConvertFmdFormat(result,FmdFormat.Iso,FmdFormat.Ansi);
            Console.WriteLine($"Is Fmd Valid {engine.ValidateFMD(result1)}");
            var confScore = engine.CompareTwoFmds(result1, result1);
            var idsc = engine.IdentifyFmd(result1, new[] {result1},1);
            Console.WriteLine($"The number of items found is {idsc.Count} and result is {JsonSerializer.Serialize(idsc)}");
            Console.WriteLine($"The Current Native Version Is ==> {engine.NativeLibVersion}");
            Console.ReadLine();

```

### Current Support

 - Linux (x64)
 - Windows (x64)

 Might build out more support for macos and and x86 architecture going forward


> Included a dockerfile to test it out for linux, you can also clone and run the Console app if you have a windows box or linux
