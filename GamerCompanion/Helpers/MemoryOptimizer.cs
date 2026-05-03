using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Text;

namespace GamerCompanion.Helpers {
    public static class MemoryOptimizer {
        [DllImport("psapi.dll")]
        private static extern bool EmptyWorkingSet(IntPtr hProcess);

        public static void Optimize() {
            GC.Collect();
            GC.WaitForPendingFinalizers();
            GC.Collect();

            foreach (var process in Process.GetProcesses()) {
                try {
                    EmptyWorkingSet(process.Handle);
                } catch { }
            }
        }
    }
}
