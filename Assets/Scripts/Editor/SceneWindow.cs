#if UNITY_EDITOR

using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Threading;

public class SceneWindow : EditorWindow {

    [MenuItem("Scene Creation/Open Scene Creation")]
    public static void CreateNewScene()
    {
        SceneWindow window = (SceneWindow)EditorWindow.GetWindow(typeof(SceneWindow), true, "Create Scene");
        window.minSize = new Vector2(500f, 180f);
        window.maxSize = new Vector2(500f, 180f);
    }

    public GameObject[] prefabs = new GameObject[3] { Resources.Load("Manager/_controller") as GameObject, Resources.Load("Manager/_editor") as GameObject, Resources.Load("PixelPerfectCam") as GameObject };

    Vector2 scrollPos;

    void OnGUI()
    {
        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("Create New Scene", EditorStyles.boldLabel);
        EditorGUILayout.EndHorizontal();

        //Array
        EditorGUILayout.BeginHorizontal();
        scrollPos = EditorGUILayout.BeginScrollView(scrollPos);
        ScriptableObject target = this;
        SerializedObject so = new SerializedObject(target);
        SerializedProperty goProperty = so.FindProperty("prefabs");

        EditorGUILayout.PropertyField(goProperty, true);
        so.ApplyModifiedProperties();
        EditorGUILayout.EndScrollView();
        EditorGUILayout.EndHorizontal();

        if (GUILayout.Button("Create Scene"))
        {
            EditorApplication.NewScene();
            DestroyImmediate(Camera.main.gameObject);
            foreach(GameObject i in prefabs)
            {
                if (i != null)
                    PrefabUtility.InstantiatePrefab(i);
            }
            Thread.Sleep(200);
            EditorApplication.SaveScene();
        }
    }
}

#endif
