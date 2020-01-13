using System;
using FingerWhoIs.Lib;
using Xunit;

namespace FingerWhoIs.Test
{
    public class FingerEngineTests
    {
        [Theory]
        [InlineData("aGVsbG8=")]
        [InlineData("")]
        [InlineData(null)]
        public void Test_ValidateFmd_Invalid(string print)
        {
            var fmd = !string.IsNullOrEmpty(print) ? Convert.FromBase64String(print) : null;
            var engine = new FingerEngine();

            var result = engine.ValidateFMD(fmd);
            Assert.False(result);

        }


        [Theory]
        [InlineData("Rk1SACAyMAAAAADYAAABAAFoAMgAyAEAAABcH4CdAD6/AIB5AGXIAICYAG1HAICuAG3OAIDNAHLUAEBoAHTDAEC4AJFOAECBAJZNAECqAJnbAIA4AKjJAIDiAMBtAIDCAMTtAIBWAM7eAIBsANlbAIAUAO3TAECOAQRqAIDIAQXyAEBhAQnoAICKARbrAEDoAST4AECxASZuAEClATJuAIA4ATTeAIAZATndAIA9AUVZAICCAU1rAEBaAVLqAEB9AVTxAECkAVR9AICYAVbzAICYAVpwAAAA")]
        public void Test_ValidateFmd_Valid(string print)
        {
            var fmd = !string.IsNullOrEmpty(print) ? Convert.FromBase64String(print) : null;
            var engine = new FingerEngine();

            var result = engine.ValidateFMD(fmd);
            Assert.True(result);
        }



        [Fact]
        public void Test_CompareFmds()
        {
            var fmd = Convert.FromBase64String("Rk1SACAyMAAAAADYAAABAAFoAMgAyAEAAABcH4CdAD6/AIB5AGXIAICYAG1HAICuAG3OAIDNAHLUAEBoAHTDAEC4AJFOAECBAJZNAECqAJnbAIA4AKjJAIDiAMBtAIDCAMTtAIBWAM7eAIBsANlbAIAUAO3TAECOAQRqAIDIAQXyAEBhAQnoAICKARbrAEDoAST4AECxASZuAEClATJuAIA4ATTeAIAZATndAIA9AUVZAICCAU1rAEBaAVLqAEB9AVTxAECkAVR9AICYAVbzAICYAVpwAAAA");
            var engine = new FingerEngine();
            var result = engine.CompareTwoFmds(fmd,fmd);

            Assert.True(result <= 21474);
        }
        
        [Fact]
        public void Test_IdentifySingleFmd()
        {
            var fmd = Convert.FromBase64String("Rk1SACAyMAAAAADYAAABAAFoAMgAyAEAAABcH4CdAD6/AIB5AGXIAICYAG1HAICuAG3OAIDNAHLUAEBoAHTDAEC4AJFOAECBAJZNAECqAJnbAIA4AKjJAIDiAMBtAIDCAMTtAIBWAM7eAIBsANlbAIAUAO3TAECOAQRqAIDIAQXyAEBhAQnoAICKARbrAEDoAST4AECxASZuAEClATJuAIA4ATTeAIAZATndAIA9AUVZAICCAU1rAEBaAVLqAEB9AVTxAECkAVR9AICYAVbzAICYAVpwAAAA");
            var engine = new FingerEngine();
            var result = engine.IdentifyFmd(fmd,new [] {fmd,null});

            Assert.NotNull(result);
            Assert.NotEmpty(result);
        }
    }
}
