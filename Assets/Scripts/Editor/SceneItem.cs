﻿using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;

public class SceneItem : Editor {

    //TestScenes

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
    public static void OpenTestScene_03_1()
    {
        OpenScene("Test Scenes/TestScene_03_Selection");
    }

    [MenuItem("Open Scenes/Test Scenes/TestScene_03_AfterSelection")]
    public static void OpenTestScene_03_2()
    {
        OpenScene("Test Scenes/TestScene_03_AfterSelection");
    }

    [MenuItem("Open Scenes/Test Scenes/TestScene_04_Animations")]
    public static void OpenTestScene_04()
    {
        OpenScene("Test Scenes/TestScene_04_Animations");
    }

    [MenuItem("Open Scenes/Test Scenes/TestScene_05_JumpPad")]
    public static void OpenTestScene_05()
    {
        OpenScene("Test Scenes/TestScene_05_JumpPad");
    }

    [MenuItem("Open Scenes/Test Scenes/TestScene_06_Editor")]
    public static void OpenTestScene_06()
    {
        OpenScene("Test Scenes/TestScene_06_Editor");
    }

    [MenuItem("Open Scenes/Test Scenes/TestScene_07_Abilities")]
    public static void OpenTestScene_07()
    {
        OpenScene("Test Scenes/TestScene_07_Abilities");
    }

    [MenuItem("Open Scenes/Test Scenes/TestScene_08_AnimationAndGraphics")]
    public static void OpenTestScene_08()
    {
        OpenScene("Test Scenes/TestScene_08_AnimationAndGraphics");
    }

    //levelTestScenes

    [MenuItem("Open Scenes/Level Test Scenes/LevelTestScene_01")]
    public static void OpenLevelTestScene_01()
    {
        OpenScene("Test Scenes/LevelTestScene_01");
    }

    [MenuItem("Open Scenes/Level Test Scenes/LevelTestScene_02")]
    public static void OpenLevelTestScene_02()
    {
        OpenScene("Test Scenes/LevelTestScene_02");
    }

    //Prototyp

    [MenuItem("Open Scenes/Prototyp/Prototyp_Selection")]
    public static void Open_Prototyp_Selection()
    {
        OpenScene("Prototype_Selection");
    }

    [MenuItem("Open Scenes/Prototyp/Prototyp_Game")]
    public static void Open_Prototyp_Game()
    {
        OpenScene("Prototype_Game");
    }

    //Final

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
