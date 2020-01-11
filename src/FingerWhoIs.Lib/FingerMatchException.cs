using System;

namespace FingerWhoIs.Lib
{
    public class FingerMatchException : Exception
    {
        public int Code { get; }

        public FingerMatchException(int code)
        {
            Code = code;
        }

        public FingerMatchException(int code, string message) : base(message)
        {
            Code = code;
        }
    }
}