namespace LLOR.Common
{
    public enum StatusCode
    {
        Pass = 0,

        Fail = 1,

        XFail = 100,

        Unsupported = 101,

        RepairError = 102,

        Timeout = 103,

        PartialTimeout = 104,

        Error = 254,
    }
}
