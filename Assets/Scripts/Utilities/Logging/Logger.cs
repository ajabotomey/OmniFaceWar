using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using log4net;

public static class Logger
{
    private static readonly ILog Log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

    public static void Debug(object message)
    {
        Log.Debug(message);
    }

    public static void Info(object message)
    {
        Log.Info(message);
    }

    public static void Warn(object message)
    {
        Log.Warn(message);
    }

    public static void Error(object message)
    {
        Log.Error(message);
    }

    public static void Fatal(object message)
    {
        Log.Fatal(message);
    }
}
