using System;
using System.Runtime.InteropServices;
using System.Threading;

class Program
{
    private static bool isScrolling = false;
    private static CancellationTokenSource cts;

    [DllImport("user32.dll")]
    private static extern short GetAsyncKeyState(int vKey);

    [DllImport("user32.dll")]
    private static extern void mouse_event(int dwFlags, int dx, int dy, int dwData, int dwExtraInfo);

    private const int MOUSEEVENTF_WHEEL = 0x0800;
    private const int F6_KEY_CODE = 0x75;

    static void Main()
    {
        Console.WriteLine("Keybind: F6 | Mode: Toggle");

        while (true)
        {
            if (GetAsyncKeyState(F6_KEY_CODE) < 0) 
            {
                isScrolling = !isScrolling;

                if (isScrolling)
                {
                    Console.WriteLine("started.");
                    cts = new CancellationTokenSource();
                    var token = cts.Token;
                    ThreadPool.QueueUserWorkItem(_ => ScrollDownLoop(token), token);
                }
                else
                {
                    Console.WriteLine("stopped.");
                    cts.Cancel(); //
                }

                Thread.Sleep(200); 
            }

            Thread.Sleep(10);
        }
    }

    private static void ScrollDownLoop(CancellationToken token)
    {
        while (!token.IsCancellationRequested)
        {
            mouse_event(MOUSEEVENTF_WHEEL, 0, 0, -1000, 0);
        }
    }
}
