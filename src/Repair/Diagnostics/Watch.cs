namespace LLOR.Repair.Diagnostics
{
    using System;
    using System.Diagnostics;

    public class Watch : IDisposable
    {
        private Stopwatch watch;

        private Measure measure;

        public Watch(Measure measure)
        {
            this.measure = measure;

            watch = new Stopwatch();
            watch.Start();
        }

        public void Dispose()
        {
            Logger.Log($"Watch;{measure};{watch.ElapsedMilliseconds}");
            watch.Stop();
        }
    }
}
