namespace LLOR.Repair.Diagnostics
{
    using System;
    
    public static class Logger
    {
        public static bool DetailedLogging;

        public static void Log(string message)
        {
            if (DetailedLogging)
                Console.WriteLine(message);
        }
    }
}
