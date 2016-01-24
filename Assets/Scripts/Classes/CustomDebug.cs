using UnityEngine;
using UnityEngine.Internal;
using System.Collections;
using System.Linq;
using System.Collections.Generic;

namespace UnityEngine
{
    public sealed class CustomDebug
    {
        private static bool active;

        #region Extras
        public static bool Active { get { return active; } set { active = value; } }
        public static void LogArray(object[] array)
        {
            if (active)
            {
                string output = "Type: " + array.GetType() + "\n Length: " + array.Length + "\n\n Values:\n";
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
                string output = "Type: " + arrayList.GetType() + "\n" + "Count: " + arrayList.Count + "\n\n" + "Values: " + "\n";
                int count = 0;

                foreach (object item in arrayList)
                {
                    output += "Location: " + count + " -> " + item.ToString() + "\n";
                    count++;
                }

                Debug.Log(output);
            }
        }
        public static void LogList(object list)
        {
            if (active)
            {
                var result = ((IEnumerable)list).Cast<object>().ToList();

                string output = "Type: " + result.GetType() + "\n" + "Count: " + result.Count + "\n\n" + "Values: " + "\n";
                int count = 0;

                foreach (object item in result)
                {
                    output += "Location: " + count + " -> " + item.ToString() + "\n";
                    count++;
                }

                Debug.Log(output);
            }
        }
        public static void LogDictionary(object dictionary)
        {
            if (active)
            {
                if (typeof(IDictionary).IsAssignableFrom(dictionary.GetType()))
                {
                    IDictionary idict = (IDictionary)dictionary;
                    Dictionary<string, string> newDict = new Dictionary<string, string>();

                    foreach (object key in idict.Keys)
                    {
                        newDict.Add(key.ToString(), idict[key].ToString());
                    }

                    int count = 0;
                    string output = "Type: " + newDict.GetType() + "\n" + "Count: " + newDict.Count + "\n\n" + "Values: " + "\n";

                    foreach (KeyValuePair<string, string> entry in newDict)
                    {
                        output += "Location: " + count + " -> " + "Key: " + entry.Key + ", Value: " + entry.Value + "\n";
                        count++;
                    }

                    Debug.Log(output);
                } 
            }
        }
        public static void LogGameObject(GameObject gameObject, bool advanced)
        {
            if (active && gameObject != null)
            {
                int count = 0;
                string output = "Type: " + gameObject.GetType() + "\n" + "Tag: " + gameObject.tag + "\n\n";
                output += "Transform: \t" + "<color=brown>x:</color>" + gameObject.transform.position.x + " <color=green>y:</color> " + gameObject.transform.position.y + " <color=teal>z:</color> " + gameObject.transform.position.z + "\n";
                output += "Rotation: \t\t" + "<color=brown>x:</color>" + gameObject.transform.rotation.x + " <color=green>y:</color> " + gameObject.transform.rotation.y + " <color=teal>z:</color> " + gameObject.transform.rotation.z + "\n";
                output += "Scale: \t\t" + "<color=brown>x:</color>" + gameObject.transform.localScale.x + " <color=green>y:</color> " + gameObject.transform.localScale.y + " <color=teal>z:</color> " + gameObject.transform.localScale.z + "\n\n";

                if(advanced)
                {
                    output += "Components: \n";

                    foreach(Component comp in gameObject.GetComponents(typeof(Component)))
                    {
                        output += count + " -> " + comp.GetType() + "\t \n";
                        count++;
                    }
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
