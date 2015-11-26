using UnityEngine;
using UnityEditor;
using System.Collections;

public class AniSpritePopup : EditorWindow {

	public static AniSpriteEditor editor;
	public static bool open = false;

	Texture texture;
	
	Rect BOX;
	Texture2D hlText;
	float scale = 1f;
	Rect scaledTexture = new Rect();
	
	Vector2 scroller;
	
	public static AniSpritePopup Init(AniSpriteEditor editor)
	{
		AniSpritePopup.editor = editor;
		AniSpritePopup window = (AniSpritePopup)EditorWindow.GetWindow(typeof(AniSpritePopup), false, "AS Viewer");
		//window.position = new Rect(Screen.width/2,Screen.height/2, 250, 250);
		//window.ShowPopup();
		return window ;
	}

	void  OnEnable (){	
		open = true;

		if(hlText == null)
		{
			hlText = new Texture2D(1, 1);
			hlText.SetPixel(1, 1, Color.white);
			hlText.hideFlags = HideFlags.HideAndDontSave;
		}

		if(editor)
			texture = editor.texture;
	}

	public void SetTexture() { texture = editor.texture; Repaint(); }

	void OnDisable()
	{
		open= false;
		DestroyImmediate(hlText);
	}

	void Update()
	{
		Repaint();
	}

	void  OnGUI (){
		if(texture && editor)
		{			
			//scale of tetxure
			scale = EditorGUILayout.Slider(scale, 0.1f, 10f);
			
			//Sprite Texture
			scroller = GUILayout.BeginScrollView(scroller);
			{
				scaledTexture.width = texture.width * scale; scaledTexture.height = texture.height * scale;
				
				GUI.DrawTexture(scaledTexture, texture, ScaleMode.ScaleToFit);
				SetColor(Color.clear, false);
				GUILayout.Box(GUIContent.none, GUILayout.Width(scaledTexture.width), GUILayout.Height(scaledTexture.height));
				UndoColor();
				BOX = scaledTexture;
				DrawLines();
			}
			GUILayout.EndScrollView();	
			
		}
		else
		{
			EditorGUILayout.HelpBox("No GameObject with AniSprite is selected" +
				" or there is not Sprite attached to the SpriteRenderer.", MessageType.Warning);
		}
	}
	
	void  DrawLines (){			
		if(editor.script.sprites.Count > 0)
		{
			for(int i= 0; i < editor.script.sprites.Count; i++)
			{		
				if(editor.script.sprites[i] != null)
					DrawBorder(editor.script.sprites[i].rect);
			}
			
			//splice highlight
			if(editor.script.spliceStartFrame > 0 || editor.script.spliceEndFrame > 0)
			{
				for(int i = editor.script.spliceStartFrame; i <= editor.script.spliceEndFrame; i++)
				{
					HighlightFrame(editor.script.sprites[i].rect, Color.red);
				}
			}
			
			if(editor.spriteSelected)
			{
				HighlightFrame(editor.script.sprites[editor.spriteSelectedIndex].rect, Color.yellow);
			}
			
			if(Application.isPlaying)
			{
				HighlightFrame(editor.script.sprites[editor.script.currentFrame].rect, Color.blue);
			}
			else if (Application.isEditor)
			{
				//start frame highlight
				if(editor.script.sprites[editor.script.startFrame] != null)
					HighlightFrame(editor.script.sprites[editor.script.startFrame].rect, Color.blue);
			}
		}
	}
	
	Vector3[] polyLine = new Vector3[5];
	void  DrawBorder ( Rect rect  ){
		rect.position *= scale;
		rect.size *= scale;
		
		rect.y = scaledTexture.height - rect.y - rect.height;
		polyLine[0] = RP(rect.x, rect.y);
		polyLine[1] = RP(rect.x + rect.width, rect.y);
		polyLine[2] = RP(rect.x + rect.width, rect.y + rect.height);
		polyLine[3] = RP(rect.x, rect.y + rect.height);
		polyLine[4] = RP(rect.x, rect.y);
		Handles.DrawPolyLine(polyLine);
	}
	
	void  HighlightFrame ( Rect rect ,   Color color  ){
		rect.position *= scale;
		rect.size *= scale;
		rect.y = scaledTexture.height - rect.y - rect.height;
		
		Color prevColor = GUI.color;
		GUI.color = color * 0.75f;
		GUI.DrawTexture(rect, hlText);
		GUI.color = prevColor;
	}
	
	//relative position for drawing lines in the box. 
	Vector2  RP ( float x ,   float y  ){
		return new Vector2(BOX.position.x + x, BOX.position.y + y);
	}
	
	Color prevColor;	
	void  SetColor ( Color color ,   bool mix  ){
		prevColor = GUI.backgroundColor;
		if(mix)
			color = color / 2 + prevColor / 2;
		
		GUI.backgroundColor = color;
	}
	
	void  UndoColor (){
		GUI.backgroundColor = prevColor;
	}

}
