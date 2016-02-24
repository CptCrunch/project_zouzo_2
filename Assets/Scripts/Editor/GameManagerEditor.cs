using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;

[CustomEditor(typeof(GameManager))]
public class GameManagerEditor : Editor {

    bool showStages, showPlayer, showGameOptions, showDebug = false;
    GameManager myTarget;
    
    public override void OnInspectorGUI() {
        serializedObject.Update();
        myTarget = (GameManager)target;

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
            myTarget.playerAmmount = Util.CreateIntSlider("Player Ammount","Maximum ammount of Player", myTarget.playerAmmount, 1, 4);
            //Damage Modifier Slider
            myTarget.damageModifier = Util.CreateFloatSlider("Damage Modifier", "In Percent (100% = normal Damage)", 100.0f, 0, 200);
            //Life Limit
            myTarget.lifeLimit = EditorGUILayout.IntField(new GUIContent("Life Limit", "Defines the maximal lifes for every player"), myTarget.lifeLimit);
            //Life lose per Death
            myTarget.lifeLosePerDeath = EditorGUILayout.IntField(new GUIContent("Life lose per Death", "Defines how many lifes are subtracted per death"), myTarget.lifeLosePerDeath);
            //Time Limit 
            myTarget.timeLimit = EditorGUILayout.FloatField(new GUIContent("Time Limit", "Time in Minutes"), myTarget.timeLimit);
            //Player Spawn Tag
            myTarget.spawnTag = EditorGUILayout.TextField(new GUIContent("Spawnpoint Tag", "Spawnpoints must have this given tag"), myTarget.spawnTag);

            //Register Players on stage
            if(GUILayout.Button("Register Player on Stage"))
            {
                GameObject[] stagePlayers = GameObject.FindGameObjectsWithTag("Player");

                for(int i = 0; i < stagePlayers.Length; i++) {
                    myTarget.charPics[i] = new CharacterPicture("Testing", i, i + 1);
                    Debug.Log("<color=green>Player "+ stagePlayers[i].name +" registered</color>");
                }

                myTarget.playerOnStage = stagePlayers;

            }

            EditorGUILayout.EndVertical();
        }
        //----------------------------------------------------------------------------

        EditorGUILayout.Space();

        //Player Variables------------------------------------------------------------
        showPlayer = EditorGUILayout.Foldout(showPlayer, "Player Options");
        if (showPlayer) {
            EditorGUILayout.BeginVertical();
            //Player Max Health
            myTarget.playerMaxHealth = EditorGUILayout.IntField("Player Max Health", myTarget.playerMaxHealth);
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
            myTarget.Tags.Active = Util.CreateBoolCheck("Active", "", myTarget.Tags.Active);
            EditorGUILayout.Space();

            EditorGUILayout.BeginHorizontal();
            myTarget.Tags.Main = Util.CreateBoolCheck("Main", "", myTarget.Tags.Main);
            myTarget.Tags.Testing = Util.CreateBoolCheck("Testing", "", myTarget.Tags.Testing);
            EditorGUILayout.EndHorizontal();
            EditorGUILayout.BeginHorizontal();
            myTarget.Tags.Player = Util.CreateBoolCheck("Player", "", myTarget.Tags.Player);
            myTarget.Tags.Damage = Util.CreateBoolCheck("Damage", "", myTarget.Tags.Damage);
            EditorGUILayout.EndHorizontal();
            EditorGUILayout.BeginHorizontal();
            myTarget.Tags.Spells = Util.CreateBoolCheck("Spells", "", myTarget.Tags.Spells);
            myTarget.Tags.Cooldown = Util.CreateBoolCheck("Cooldown", "", myTarget.Tags.Cooldown);
            EditorGUILayout.EndHorizontal();
            EditorGUILayout.BeginHorizontal();
            myTarget.Tags.UI = Util.CreateBoolCheck("UI", "", myTarget.Tags.UI);
            myTarget.Tags.Animation = Util.CreateBoolCheck("Animation", "", myTarget.Tags.Animation);
            EditorGUILayout.EndHorizontal();
            EditorGUILayout.BeginHorizontal();
            myTarget.Tags.Condition = Util.CreateBoolCheck("Condition", "", myTarget.Tags.Condition);
            myTarget.Tags.Controles = Util.CreateBoolCheck("Controles", "", myTarget.Tags.Controles);
            EditorGUILayout.EndHorizontal();
            EditorGUILayout.BeginHorizontal();
            myTarget.Tags.MapFeature = Util.CreateBoolCheck("MapFeature", "", myTarget.Tags.MapFeature);
            myTarget.Tags.Time = Util.CreateBoolCheck("Time", "", myTarget.Tags.Time);
            EditorGUILayout.EndVertical();

            if (GUILayout.Button("Show FPS")) {
                if (myTarget.showFPS)
                    myTarget.showFPS = false;
                else
                    myTarget.showFPS = true;
            }
            EditorGUILayout.HelpBox("Press F to enable the FPS-Counter in game!", MessageType.Info);
        }
        
    }

    

    private static string[] ReadNames() {
        List<string> temp = new List<string>();
        foreach (UnityEditor.EditorBuildSettingsScene S in UnityEditor.EditorBuildSettings.scenes) {
            if (S.enabled) {
                string name = S.path.Substring(S.path.LastIndexOf('/') + 1);
                name = name.Substring(0, name.Length - 6);
                if (name.Contains("Stage"))
                {
                    temp.Add(name);
                }
            }
        }
        return temp.ToArray();
    }

}
