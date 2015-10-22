using UnityEngine;
using UnityEditor;
using System.Collections;

public class SceneItem : Editor {

    [MenuItem("Open Scenes/Test Scenes/TestScene_Movement")]
	public static void OpenTestScene_01()
    {
        OpenScene("Test Scenes/TestScene_Movement");
    }

    [MenuItem("Open Scenes/Test Scenes/TestScene_02")]
    public static void OpenTestScene_02()
    {
        OpenScene("Test Scenes/TestScene_02");
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
