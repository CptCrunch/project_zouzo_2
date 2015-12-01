using UnityEngine;
using UnityEditor;
using UnityEditorInternal;
using System.Collections;
using System.Collections.Generic;

[CustomEditor(typeof(AniPatternLoader))]
public class AniPatternLoaderEditor : Editor {

	ReorderableList rl;
	SerializedProperty patterns;

	// Use this for initialization
	void OnEnable () {
		patterns = serializedObject.FindProperty("pattern");

		rl = new ReorderableList(serializedObject, patterns, true, true, true, true);
		RLGUI();
	}
	
	// Update is called once per frame
	public override void OnInspectorGUI () {
		serializedObject.Update();

		rl.DoLayoutList();

		serializedObject.ApplyModifiedProperties();
	}

	void  RLGUI (){	
		//header
		rl.drawHeaderCallback = (Rect rect) => {  
			EditorGUI.LabelField(rect, "Sheet Pattern");
		};
		
		rl.drawElementCallback =  
		(Rect rect, int index, bool isActive, bool isFocused) => 
		{
			EditorGUIUtility.labelWidth = 50;
			SerializedProperty p = patterns.GetArrayElementAtIndex(index);

			var toggleRect = new Rect(rect.x, rect.y, 65, EditorGUIUtility.singleLineHeight);
			var firstFieldRect = new Rect(rect.x + 70, rect.y, 100, EditorGUIUtility.singleLineHeight);
			var secFieldRect =new Rect(rect.x + 180, rect.y, 50, EditorGUIUtility.singleLineHeight);

			EditorGUI.PropertyField(toggleRect, p.FindPropertyRelative("ranged"),
			                        new GUIContent("Ranged", "Toggle for setting a range"));

			EditorGUI.PropertyField(firstFieldRect, p.FindPropertyRelative("startSprite"), new GUIContent("Sprite"));

			if(p.FindPropertyRelative("ranged").boolValue)
			{
				EditorGUI.PropertyField(secFieldRect, p.FindPropertyRelative("endSprite"), GUIContent.none);
			}

			if(p.FindPropertyRelative("startSprite").intValue < 0)
				p.FindPropertyRelative("startSprite").intValue = 0;

			if(p.FindPropertyRelative("endSprite").intValue < 0)
				p.FindPropertyRelative("endSprite").intValue = 0;
		};

	}
}
