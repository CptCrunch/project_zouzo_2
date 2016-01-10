using UnityEngine;
using UnityEngine.Internal;
using System.Collections;
using System.Collections.Generic;

namespace CustomDebuger
{
    public sealed class CustomDebuger
    {
        private static bool active;

        #region Extras
        public static bool Active { get { return active; } set { active = value; } }
        public static void LogArray(object[] array)
        {
            if (active)
            {
                string output = "Type: " + array.GetType() + "\n" + "Length: " + array.Length + "\n\n Values:\n";
                int count = 0;

                foreach (object item in array)
                {
                    output += "Location: " + count + " -> " + item.ToString() + "\n";
                    count++;
                }

                Debug.Log(output);
            }
        }
        public static void LogArrayList(ArrayList arrayList)
        {
            if (active)
            {
                string output = "Type: " + arrayList.GetType() + "\n" + "Count: " + arrayList.Count + "\n\n Values:\n";
                int count = 0;

                foreach (object item in arrayList)
                {
                    output += "Location: " + count + " -> " + item.ToString() + "\n";
                    count++;
                }

                Debug.Log(output);
            }
        }
        #endregion

        #region Log, Error, Warning
        public static void Log(object message)
        {
            if (active)
                Debug.Log(message.ToString());
        }
        public static void Log(object message, Object context)
        {
            if (active)
                Debug.Log(message.ToString(), context);
        }
        public static void LogError(object message)
        {
            if (active)
                Debug.LogError(message.ToString());
        }
        public static void LogError(object message, Object context)
        {
            if (active)
                Debug.LogError(message.ToString(), context);
        }
        public static void LogWarning(object message)
        {
            if (active)
                Debug.LogWarning(message.ToString());
        }
        public static void LogWarning(object message, Object context)
        {
            if (active)
                Debug.LogWarning(message.ToString(), context);
        }
        #endregion

        #region Formating
        public static void LogFormat(string format, params object[] args)
        {
            if (active)
                Debug.LogFormat(format, args);
        }
        public static void LogFormat(Object context, string format, params object[] args)
        {
            if (active)
                Debug.LogFormat(context, format, args);
        }
        public static void LogErrorFormat(string format, params object[] args)
        {
            if (active)
                Debug.LogErrorFormat(format, args);
        }
        public static void LogErrorFormat(Object context, string format, params object[] args)
        {
            if (active)
                Debug.LogErrorFormat(context, format, args);
        }
        public static void LogWarningFormat(string format, params object[] args)
        {
            if (active)
                Debug.LogWarningFormat(format, args);
        }
        public static void LogWarningFormat(Object context, string format, params object[] args)
        {
            if (active)
                Debug.LogWarningFormat(context, format, args);
        }
        #endregion

        #region Draw Line
        public static void DrawLine(Vector3 start, Vector3 end)
        {
            if (active)
                Debug.DrawLine(start, end);
        }
        [ExcludeFromDocs]
        public static void DrawLine(Vector3 start, Vector3 end, Color color)
        {
            if (active)
                Debug.DrawLine(start, end, color);
        }
        [ExcludeFromDocs]
        public static void DrawLine(Vector3 start, Vector3 end, Color color, float duration)
        {
            if (active)
                Debug.DrawLine(start, end, color, duration);
        }
        public static void DrawLine(Vector3 start, Vector3 end, [DefaultValue("Color.white")] Color color, [DefaultValue("0.0f")] float duration, [DefaultValue("true")] bool depthTest)
        {
            if (active)
                Debug.DrawLine(start, end, color, duration, depthTest);
        }
        #endregion
    }
}
