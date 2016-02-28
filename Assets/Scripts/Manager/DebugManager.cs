using UnityEngine;
using UnityEngine.Internal;
using UnityEngineInternal;
using System;
using System.Collections;

public static class DebugManager {

    private static bool enabled;

    public static bool Enabled { get { return enabled; } set { enabled = value; } }

    public static void Log(object content)
    {
        if(enabled)
        {
            Debug.Log(content);
        }
    }
    public static void LogError(object content)
    {
        if (enabled)
        {
            Debug.LogError(content);
        }
    }
    public static void LogWarning(object content)
    {
        if (enabled)
        {
            Debug.LogWarning(content);
        }
    }

    public static void LogErrorFormat(UnityEngine.Object context, string format, params object[] args)
    {
        if (enabled)
        {
            Debug.LogErrorFormat(context, format, args);
        }
    }
    public static void LogWarningFormat(UnityEngine.Object context, string format, params object[] args)
    {
        if (enabled)
        {
            Debug.LogWarningFormat(context, format, args);
        }
    }
    public static void LogFormat(UnityEngine.Object context, string format, params object[] args)
    {
        if (enabled)
        {
            Debug.LogFormat(context, format, args);
        }
    }

    public static void LogException(Exception content)
    {
        if (enabled)
        {
            Debug.LogException(content);
        }
    }
    public static void DrawLine(Vector3 start, Vector3 end)
    {
        if (enabled)
        {
            Debug.DrawLine(start, end);
        }
    }
    public static void DrawLine(Vector3 start, Vector3 end, Color color)
    {
        if (enabled)
        {
            Debug.DrawLine(start, end, color);
        }
    }
    public static void DrawLine(Vector3 start, Vector3 end, Color color, float duration)
    {
        if (enabled)
        {
            Debug.DrawLine(start, end, color, duration);
        }
    }
    public static void DrawLine(Vector3 start, Vector3 end, [DefaultValue("Color.white")] Color color, [DefaultValue("0.0f")] float duration, [DefaultValue("true")] bool depthTest)
    {
        if (enabled)
        {
            Debug.DrawLine(start, end, color, duration, depthTest);
        }
    }
}
