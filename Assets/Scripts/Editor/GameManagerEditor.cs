using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;

[CustomEditor(typeof(GameManager))]
public class GameManagerEditor : Editor {

    bool showStages, showPlayer, showGameOptions, showDebug = false;

    public override void OnInspectorGUI() {
        serializedObject.Update();
        GameManager myTarget = (GameManager)target;

        //Registerd Stages Array------------------------------------------------------
        showStages = EditorGUILayout.Foldout(showStages, "Stage Options");
        if (showStages) {
            EditorGUILayout.BeginVertical();
            SerializedProperty tps = serializedObject.FindProperty("registerdStages");
            EditorGUI.BeginChangeCheck();
            EditorGUILayout.PropertyField(tps, true);
            if (EditorGUI.EndChangeCheck())
                serializedObject.ApplyModifiedProperties();
            EditorGUIUtility.LookLikeControls();

            if (GUILayout.Button(new GUIContent("Register Scenes", "Registers all scenes from the build path"))) {
                myTarget.registerdStages = ReadNames();
            }
            EditorGUILayout.EndVertical();
        }
        //----------------------------------------------------------------------------

        EditorGUILayout.Space();

        //Player Variables------------------------------------------------------------
        showGameOptions = EditorGUILayout.Foldout(showGameOptions, "Game Options");
        if (showGameOptions) {
            EditorGUILayout.BeginVertical();
            //Player Ammount Slider
            myTarget.playerAmmount = CreateIntSlider("Player Ammount","Maximum ammount of Player", myTarget.playerAmmount, 1, 4);
            //Damage Modifier Slider
            myTarget.damageModifier = CreateFloatSlider("Damage Modifier", "In Percent (100% = normal Damage)", myTarget.damageModifier, 0, 200);
            //Life Limit
            myTarget.lifeLimit = EditorGUILayout.IntField(new GUIContent("Life Limit", "Defines the maximal lifes for every player"), myTarget.lifeLimit);
            //Time Limit
            myTarget.timeLimit = EditorGUILayout.FloatField(new GUIContent("Time Limit", "Time in Minutes"), myTarget.timeLimit);
            //Player Spawn Tag
            myTarget.spawnTag = EditorGUILayout.TextField(new GUIContent("Spawnpoint Tag", "Spawnpoints must have this given tag"), myTarget.spawnTag);
            EditorGUILayout.EndVertical();
        }
        //----------------------------------------------------------------------------

        EditorGUILayout.Space();

        //Player Variables------------------------------------------------------------
        showPlayer = EditorGUILayout.Foldout(showPlayer, "Player Options");
        if (showPlayer) {
            EditorGUILayout.BeginVertical();
            //Player Max Health
            myTarget.playerMaxHealth = EditorGUILayout.FloatField("Player Max Health", myTarget.playerMaxHealth);
            //Ability Limit
            myTarget.abilityLimit = EditorGUILayout.IntField(new GUIContent("Ability Limit", "Defines the maximal use of an ability"), myTarget.abilityLimit);
            //Time Death/Spawn
            myTarget.timeDeathSpawn = EditorGUILayout.FloatField("Time Death/Spawn", myTarget.timeDeathSpawn);

            //Player Info Array
            EditorGUILayout.BeginVertical();
            SerializedProperty tps = serializedObject.FindProperty("playerInfo");
            EditorGUI.BeginChangeCheck();
            EditorGUILayout.PropertyField(tps, true);
            if (EditorGUI.EndChangeCheck())
                serializedObject.ApplyModifiedProperties();
            EditorGUIUtility.LookLikeControls();

            EditorGUILayout.EndVertical();
        }
        //----------------------------------------------------------------------------

        EditorGUILayout.Space();

        //Debug------------------------------------------------------------------------
        showDebug = EditorGUILayout.Foldout(showDebug, "Debug Options");
        if (showDebug) {
            EditorGUILayout.BeginVertical();
            myTarget.Tags.Active = CreateBoolCheck("Active", "", myTarget.Tags.Active);
            EditorGUILayout.Space();

            EditorGUILayout.BeginHorizontal();
            myTarget.Tags.Main = CreateBoolCheck("Main", "", myTarget.Tags.Main);
            myTarget.Tags.Testing = CreateBoolCheck("Testing", "", myTarget.Tags.Testing);
            EditorGUILayout.EndHorizontal();
            EditorGUILayout.BeginHorizontal();
            myTarget.Tags.Player = CreateBoolCheck("Player", "", myTarget.Tags.Player);
            myTarget.Tags.Damage = CreateBoolCheck("Damage", "", myTarget.Tags.Damage);
            EditorGUILayout.EndHorizontal();
            EditorGUILayout.BeginHorizontal();
            myTarget.Tags.Spells = CreateBoolCheck("Spells", "", myTarget.Tags.Spells);
            myTarget.Tags.Cooldown = CreateBoolCheck("Cooldown", "", myTarget.Tags.Cooldown);
            EditorGUILayout.EndHorizontal();
            EditorGUILayout.BeginHorizontal();
            myTarget.Tags.UI = CreateBoolCheck("UI", "", myTarget.Tags.UI);
            myTarget.Tags.Animation = CreateBoolCheck("Animation", "", myTarget.Tags.Animation);
            EditorGUILayout.EndHorizontal();
            EditorGUILayout.BeginHorizontal();
            myTarget.Tags.Condition = CreateBoolCheck("Condition", "", myTarget.Tags.Condition);
            myTarget.Tags.Controles = CreateBoolCheck("Controles", "", myTarget.Tags.Controles);
            EditorGUILayout.EndHorizontal();
            EditorGUILayout.BeginHorizontal();
            myTarget.Tags.MapFeature = CreateBoolCheck("MapFeature", "", myTarget.Tags.MapFeature);
            EditorGUILayout.EndVertical();
        }
    }

    private bool CreateBoolCheck(string labelName, string tooltip, bool value) {
        GUILayout.BeginHorizontal();
        GUILayout.Label(new GUIContent(labelName, tooltip), GUILayout.MaxWidth(64));
        value = EditorGUILayout.Toggle(value, GUILayout.MaxWidth(32));
        GUILayout.EndHorizontal();

        return value;
    }

    private float CreateFloatSlider(string labelName, string tooltip, float sliderPosition, float leftValue, float rightValue) {
        GUILayout.BeginHorizontal();
        GUILayout.Label(new GUIContent(labelName, tooltip));
        sliderPosition = EditorGUILayout.Slider(sliderPosition, leftValue, rightValue, null);
        GUILayout.EndHorizontal();

        return sliderPosition;
    }

    private int CreateIntSlider(string labelName, string tooltip, int sliderPosition, int leftValue, int rightValue) {
        GUILayout.BeginHorizontal();
        GUILayout.Label(new GUIContent(labelName, tooltip));
        sliderPosition = EditorGUILayout.IntSlider(sliderPosition, leftValue, rightValue, null);
        GUILayout.EndHorizontal();

        return sliderPosition;
    }

    private static string[] ReadNames() {
        List<string> temp = new List<string>();
        foreach (UnityEditor.EditorBuildSettingsScene S in UnityEditor.EditorBuildSettings.scenes) {
            if (S.enabled) {
                string name = S.path.Substring(S.path.LastIndexOf('/') + 1);
                name = name.Substring(0, name.Length - 6);
                temp.Add(name);
            }
        }
        return temp.ToArray();
    }
}
