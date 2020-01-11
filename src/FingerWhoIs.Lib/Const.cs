namespace FingerWhoIs.Lib
{
    internal class Const
    {
        private const int _DP_FACILITY = 0x05BA;

        private const int Error = _DP_FACILITY << 16;
        
        const int Success = 0;

        public const int Failure = 0x0b | Error;

        public const int NoData = 0x0c | Error;

        public const int InsufficientMemory = 0x0d | Error;

        public const int InvalidParameter = 0x14 | Error;

        public const int InvalidFid = 0x65 | Error;

        public const int ImageAreaSmall = 0x66 | Error;

        public const int InavlidFmd = 0xc9 | Error;

        public const int EnrollmentInProgress = 0x12d | Error;

        public const int EnrollmentNotStarted = 0x12e | Error;

        public const int EnrollmentNotReady = 0x12f | Error;

        public const int EnrollmentInvalidSet = 0x130 | Error;

    }
}