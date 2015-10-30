using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;

public class SceneItem : Editor {

    [MenuItem("Open Scenes/Test Scenes/TestScene_01_Movement")]
	public static void OpenTestScene_01()
    {
        OpenScene("Test Scenes/TestScene_01_Movement");
    }

    [MenuItem("Open Scenes/Test Scenes/TestScene_02_Platforms")]
    public static void OpenTestScene_02()
    {
        OpenScene("Test Scenes/TestScene_02_Platforms");
    }

    [MenuItem("Open Scenes/Test Scenes/TestScene_03_Selection")]
    public static void OpenTestScene_03()
    {
        OpenScene("Test Scenes/TestScene_03_Selection");
    }

    [MenuItem("Open Scenes/Test Scenes/TestScene_04_AfterSelection")]
    public static void OpenTestScene_04()
    {
        OpenScene("Test Scenes/TestScene_04_AfterSelection");
    }

    [MenuItem("Open Scenes/Main Menu")]
    public static void MainMenu()
    {
        OpenScene("MainMenu");
    }

    static void OpenScene(string name)
    {
        if(EditorApplication.SaveCurrentSceneIfUserWantsTo())
        {
            EditorApplication.OpenScene("Assets/Scenes/" + name + ".unity");
        }
    }

}
