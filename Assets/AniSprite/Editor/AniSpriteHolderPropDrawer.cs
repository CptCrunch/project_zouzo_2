using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;

[CustomPropertyDrawer(typeof(AniSpriteHolder))]
public class AniSpriteHolderPropDrawer : PropertyDrawer
{
	public override void OnGUI (Rect position, SerializedProperty property , GUIContent label) {
		// Using BeginProperty / EndProperty on the parent property means that
		// prefab override logic works on the entire property.
		EditorGUI.BeginProperty (position, label, property);

		var fieldRect = new Rect (position.x, position.y, position.width - 65, position.height);
		var buttonRect = new Rect (position.x + position.width - 45, position.y, 45, position.height);
		SerializedProperty sprites = property.FindPropertyRelative("sprites");
		SerializedProperty baseSprite = property.FindPropertyRelative("baseSprite");

		EditorGUI.indentLevel = 1;

		if(property.FindPropertyRelative("loaded").boolValue == false)
		{
			EditorGUI.PropertyField(fieldRect, baseSprite, GUIContent.none);
			
			if(GUI.Button(buttonRect, "Load"))
			{
				Object[] newSprites = LoadSprites(baseSprite.objectReferenceValue);
				
				sprites.ClearArray();
				sprites.arraySize = newSprites.Length;
				
				for(int i = 0; i < newSprites.Length; i++)
				{
					sprites.GetArrayElementAtIndex(i).objectReferenceValue = newSprites[i];
				}
				
				property.FindPropertyRelative("loaded").boolValue = true;
			}
		}
		else
		{
			Object mainAsset = AssetDatabase.LoadMainAssetAtPath(
				AssetDatabase.GetAssetPath(sprites.GetArrayElementAtIndex(0).objectReferenceValue));
			EditorGUI.ObjectField(fieldRect, mainAsset, typeof(Texture2D), false);
			
			if(GUI.Button(buttonRect, "Clear"))
			{
				sprites.ClearArray();
				
				property.FindPropertyRelative("loaded").boolValue = false;
			}
		}		
		
		EditorGUI.EndProperty ();
	}
	
	Object[] LoadSprites(Object sprite)
	{
		Object[] ss = AssetDatabase.LoadAllAssetsAtPath(AssetDatabase.GetAssetPath(sprite));
		List<Object> objects = new List<Object>(ss);
		objects.RemoveAt(0);
		return objects.ToArray();
	}
}
