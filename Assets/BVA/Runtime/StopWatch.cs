using System;
using System.Diagnostics;
using System.Runtime.InteropServices;

public class QueryPerformance
{
#if UNITY_EDITOR || UNITY_STANDALONE_WIN
    [DllImport("Kernel32.dll")]
    private static extern bool QueryPerformanceCounter(out long performanceCount);

    [DllImport("Kernel32.dll")]
    private static extern bool QueryPerformanceFrequency(out long frequency);
    
#endif
    private long begintTime = 0;

    private long endTime = 0;

    private long frequency = 0;

    public long BegintTime
    {
        get { return begintTime; }
    }

    public long EndTime
    {
        get { return endTime; }
    }

    public long Frequency
    {
        get { return frequency; }
    }


    public QueryPerformance()
    {
#if UNITY_EDITOR || UNITY_STANDALONE_WIN
        QueryPerformanceFrequency(out frequency);
#endif
    }

    public void Start()
    {
#if UNITY_EDITOR || UNITY_STANDALONE_WIN
        QueryPerformanceCounter(out begintTime);
#endif
    }

    public void Stop()
    {
#if UNITY_EDITOR || UNITY_STANDALONE_WIN
        QueryPerformanceCounter(out endTime);
#endif
    }
    /// <summary>
    /// seconds
    /// </summary>
    public double TaskTime
    {
        get
        {
            if (frequency > 0)
                return (double)(endTime - begintTime) / frequency;
            else
                return 0;
        }
    }
}
public static class CpuWatch
{
    private static QueryPerformance tick;
    static CpuWatch()
    {
        tick = new QueryPerformance();
    }
    public static double Ellapsed
    {
        get
        {
            return tick.TaskTime;
        }
    }
    public static void Start()
    {
        tick.Start();
    }

    public static void Stop()
    {
        tick.Stop();
    }
    public static void Stop(string x)
    {
        Stop();
        PrintEllapsed(x);
    }
    public static void PrintEllapsed(string x)
    {
        string msg = $"{x} cost {tick.TaskTime}s";

        if (tick.TaskTime > 1.0)
        {
            UnityEngine.Debug.LogWarning(msg);
            return;
        }
        if (tick.TaskTime > 0.001)
            UnityEngine.Debug.Log(msg);
    }
}