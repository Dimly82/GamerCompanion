using System.Diagnostics;
using System.Runtime.InteropServices;

namespace GamerCompanion.Helpers;

public static class ActiveWindowHelper {
    [DllImport("user32.dll")]
    private static extern IntPtr GetForegroundWindow();

    [DllImport("user32.dll")]
    private static extern uint GetWindowThreadProcessId(IntPtr hWnd, out uint processId);

    public static string GetActiveProcessName() {
        IntPtr hwnd = GetForegroundWindow();
        if (hwnd == IntPtr.Zero)
            return null;

        GetWindowThreadProcessId(hwnd, out uint pid);
        try {
            Process p = Process.GetProcessById((int)pid);
            return p.ProcessName;
        } catch {
            return null;
        }
    }

    public static string GetActiveWindowTitle() {
        IntPtr hwnd = GetForegroundWindow();
        if (hwnd == IntPtr.Zero)
            return null;

        var buffer = new System.Text.StringBuilder(256);
        _ = GetWindowText(hwnd, buffer, buffer.Capacity);
        return buffer.ToString();
    }

    [DllImport("user32.dll")]
    private static extern int GetWindowText(IntPtr hWnd, System.Text.StringBuilder lpString, int nMaxCount);
}

