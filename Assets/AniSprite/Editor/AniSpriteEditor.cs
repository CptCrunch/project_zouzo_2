using UnityEditorInternal;
using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;

[CustomEditor(typeof(AniSprite))]
public class AniSpriteEditor : Editor
{	
	#region properties
	ReorderableList spriteList;
	ReorderableList saveList;
	List<ReorderableList> groupLists = new List<ReorderableList>();

	//public AniSprite sprite;
	public AniSprite script;
	SerializedProperty sprites;
	SerializedProperty saves;
	SerializedProperty FPS;
	SerializedProperty step;
	SerializedProperty animateOnStart;
	SerializedProperty loop;
	SerializedProperty playSplice;
	SerializedProperty startFrame;
	SerializedProperty currentFrame;	
	SerializedProperty spliceStartFrame;
	SerializedProperty spliceEndFrame;

	private float prevFPS;

	public Texture texture;
	SpriteRenderer sp_Renderer;
	
	Rect BOX;
	Texture2D hlText;
	float scale = 1f;
	Rect scaledTexture = new Rect();
		
	Vector2 scroller;

	static AniSpritePopup popupWin;
	bool saveInGroup = false;
	string groupName = "";
	ReorderableList rlToRemove = null;
	//SP* AniGroup groupToRemove = null;
	AniGroup groupToRemove = null;
	
	public bool spriteSelected = false;
	public int spriteSelectedIndex = 0;
	#endregion


	#region Callbacks

	void  OnEnable (){	
		//serialized props
		sprites = serializedObject.FindProperty("sprites");
		saves  = serializedObject.FindProperty("saves");
		FPS = serializedObject.FindProperty("_FPS");
		step = serializedObject.FindProperty("step");
		animateOnStart = serializedObject.FindProperty("animatedOnStart");
		loop = serializedObject.FindProperty("loop");
		playSplice = serializedObject.FindProperty("playSplice");
		startFrame = serializedObject.FindProperty("startFrame");
		currentFrame = serializedObject.FindProperty("currentFrame");
		spliceStartFrame = serializedObject.FindProperty("spliceStartFrame");
		spliceEndFrame = serializedObject.FindProperty("spliceEndFrame");

		hlText = new Texture2D(1, 1);
		hlText.SetPixel(1, 1, Color.white);
		hlText.hideFlags = HideFlags.HideAndDontSave;
		
		script = target as AniSprite;
		sp_Renderer = script.GetComponent<SpriteRenderer>() as SpriteRenderer;
		if(sp_Renderer && sp_Renderer.sprite)
			texture = sp_Renderer.sprite.texture;

		spriteList = new ReorderableList(serializedObject, sprites, true, true, true, true);
		SheetGUI();
		saveList = new ReorderableList(serializedObject, saves, true, true, false, true);
		SavesGUI(saveList, serializedObject);

		foreach(AniGroup ag in script.groups)
		{
			ReorderableList rl = new ReorderableList(ag.members, typeof(AniSave), true, true, false, true);
			SavesGUI(rl, ag);
			groupLists.Add(rl);
		}

		AniSpritePopup.editor = this;
		if(AniSpritePopup.open)
		{
			popupWin = EditorWindow.GetWindow<AniSpritePopup>();
			popupWin.SetTexture();
		}

		prevFPS = script.FPS;
	}
	
	void  OnDisable (){
		DestroyImmediate(hlText);
	}
	
	bool  showSheetOptions = false;
	bool  showAnimationSaves = true;
	
	public override void  OnInspectorGUI (){
		serializedObject.Update();

		EditorGUILayout.BeginVertical();
		if(texture && popupWin == null)
		{
			GUILayout.Space(25);

			//scale of tetxure
			EditorGUILayout.BeginHorizontal();
			scale = EditorGUILayout.Slider(scale, 0.1f, 10f);
			GUILayout.Label("Scale");
			if(GUILayout.Button("[+]"))
			{
				popupWin = AniSpritePopup.Init(this);
			}
			EditorGUILayout.EndHorizontal();

			//Sprite Texture
			scroller = GUILayout.BeginScrollView(scroller, GUIStyle.none, GUILayout.MinHeight(150));
			{
				scaledTexture.width = texture.width * scale; scaledTexture.height = texture.height * scale;

				GUI.DrawTexture(scaledTexture, texture, ScaleMode.ScaleToFit);
				GUILayout.Box(GUIContent.none, "Label", GUILayout.Width(scaledTexture.width), GUILayout.Height(scaledTexture.height));
				BOX = scaledTexture;
				DrawLines();
			}
			GUILayout.EndScrollView();

		}
		else if(popupWin != null)
		{
			SetColor(Color.green, false);
			EditorGUILayout.HelpBox("Sprite Viewer Open", MessageType.Info);
			UndoColor();
		}
		else
			EditorGUILayout.HelpBox("No sprite texture attached to the gameobject's SpriteRenderer", MessageType.Warning);

		//options for animation!!
		AnimationGUI();		

		showSheetOptions = EditorGUILayout.Foldout(showSheetOptions, "Sheet");
		if(showSheetOptions)
		{
			if(script.sprites.Count > 0 && script.sprites[0] != null)
				GUI.enabled = true;
			else
				GUI.enabled = false;
			LoadSprites();
			GUI.enabled = true;
			spriteList.DoLayoutList();
		}
		else
			spriteSelected = false;
			
		showAnimationSaves = EditorGUILayout.Foldout(showAnimationSaves, "Saves");
		if(showAnimationSaves)
		{
			saveList.DoLayoutList();	

			EditorGUI.BeginChangeCheck();

			for(int i = 0; i < groupLists.Count; i++)
			{
				if(groupLists[i] != null)
					groupLists[i].DoLayoutList();
			}

			if(EditorGUI.EndChangeCheck())
			{
				Undo.RecordObject(target, "Undo AniSprite Changes");
				EditorUtility.SetDirty(target);
			}
		}


		//remove selected group
		if(groupToRemove != null)
		{
			script.groups.Remove(groupToRemove);
			groupLists.Remove(rlToRemove);
			groupToRemove = null;
			rlToRemove = null;
		}

		EditorGUILayout.EndVertical();

		serializedObject.ApplyModifiedProperties();
	}
	#endregion

	#region GUI
	void LoadSprites()
	{
		if(GUILayout.Button("Load All Sprites"))
		{
			Object[] ss = AssetDatabase.LoadAllAssetsAtPath(AssetDatabase.GetAssetPath(script.sprites[0]));// as Sprite[];
			List<Object> objects = new List<Object>(ss);
			objects.RemoveAt(0);

			sprites.ClearArray();
			sprites.arraySize = objects.Count;

			for(int i = 0; i < objects.Count; i++)
			{
				sprites.GetArrayElementAtIndex(i).objectReferenceValue = objects[i] as Sprite;
			}
			Repaint();
		}
	}

	//list for sprites
	void  SheetGUI (){	
		//header
		spriteList.drawHeaderCallback = (Rect rect) => {  
			EditorGUI.LabelField(rect, "Frames");
		};

		spriteList.onSelectCallback = (ReorderableList list) => {
			spriteSelected = true;
			spriteSelectedIndex = list.index;
		};

		spriteList.drawElementCallback =  
		(Rect rect, int index, bool isActive, bool isFocused) => {
			EditorGUI.PropertyField(new Rect(rect.x, rect.y, rect.width, EditorGUIUtility.singleLineHeight),
			                        sprites.GetArrayElementAtIndex(index),  new GUIContent("Frame "+index));
		};

		spriteList.onRemoveCallback = (ReorderableList rl) => {
			Undo.RecordObject(script, "Remove Sprite Frame");

			ReorderableList.defaultBehaviours.DoRemoveButton(rl);
			ReorderableList.defaultBehaviours.DoRemoveButton(rl);			
		};
	}
	
	void  AnimationGUI ()
	{
		EditorGUILayout.BeginVertical("Box");
		
		EditorGUILayout.BeginHorizontal();
		{
			if(script.animatedOnStart)
				SetColor(Color.green, true);
			else
				SetColor(Color.red, true);
				
			if(GUILayout.Button("Animate On Start", GUILayout.Height(EditorGUIUtility.singleLineHeight)))
				animateOnStart.boolValue = !animateOnStart.boolValue;
			UndoColor();
			
			GUILayout.Space(15);
			EditorGUIUtility.labelWidth = 50;

			{
				EditorGUILayout.PropertyField(FPS);
				if(prevFPS != FPS.intValue)
				{
					if(FPS.intValue < 0)
						FPS.intValue = 0;

					if(Application.isPlaying)
					{
						script.ResetAnimationSequence();
					}
				}
				prevFPS = FPS.intValue;
			}
		}
		EditorGUILayout.EndHorizontal();
		
		//space 
		GUILayout.Space(15);
		
		EditorGUILayout.BeginHorizontal();
		{
			//play direction box
			SetColor(Color.clear, true);
			EditorGUILayout.BeginVertical("Box");
			UndoColor();
			EditorGUILayout.LabelField("Play Direction", GUILayout.Height(EditorGUIUtility.singleLineHeight));
			
			//arrow buttons
			EditorGUILayout.BeginHorizontal();
			{
				if(step.intValue < 0)
					SetColor(Color.yellow, false);
				if(GUILayout.Button("<--", GUILayout.Height(EditorGUIUtility.singleLineHeight)))
					step.intValue = -1;
					
				UndoColor();
				if(step.intValue > 0)
					SetColor(Color.yellow, false);
				if(GUILayout.Button("-->", GUILayout.Height(EditorGUIUtility.singleLineHeight)))
					step.intValue = 1;
				UndoColor();
			}
			EditorGUILayout.EndHorizontal();

			SetColor(Color.cyan, false);
			EditorGUILayout.Space();
			EditorGUILayout.PropertyField(loop);
			UndoColor();
			EditorGUILayout.Space();

			EditorGUILayout.EndVertical();
			
			///space
			GUILayout.Space(15);
			
			//starting frame
			SetColor(Color.clear, true);
			EditorGUILayout.BeginVertical("Box");
			UndoColor();
			{
				SetColor(Color.blue, true);
				EditorGUILayout.LabelField("Start Frame");
				EditorGUILayout.PropertyField(startFrame, new GUIContent("F"));
				startFrame.intValue = Mathf.Clamp(startFrame.intValue, 0, script.sprites.Count - 1);
				UndoColor();

				SetColor(Color.red, true);
				EditorGUILayout.LabelField("Splice Start / End");
				EditorGUILayout.BeginHorizontal();
				{
					EditorGUILayout.PropertyField(spliceStartFrame, new GUIContent("S"));
					spliceStartFrame.intValue = Mathf.Clamp(spliceStartFrame.intValue, 0, script.sprites.Count - 1);

					EditorGUILayout.PropertyField(spliceEndFrame, new GUIContent("E"));
					spliceEndFrame.intValue = Mathf.Clamp(spliceEndFrame.intValue, 0, script.sprites.Count - 1);
				}
				EditorGUILayout.EndHorizontal();
				UndoColor();
			}
			EditorGUILayout.EndVertical();
		}
		EditorGUILayout.EndHorizontal();
			
		if(GUILayout.Button("Save Current Animation"))
		{
			//are we saving in a group and the name is not default?
			if(saveInGroup && groupName != "")
			{
				//check for group
				AniGroup ag = script.groups.Find(x => x.name == groupName);
				//add new save
				SaveCurrentLayoutInGroup(groupName);

				//if group didn't exist before we will add a new reordable list
				if(ag == null)
				{
					groupLists.Clear();
					foreach(AniGroup ags in script.groups)
					{
						ReorderableList rl = new ReorderableList(ags.members, typeof(AniSave), true, true, false, true);
						SavesGUI(rl, ags);
						groupLists.Add(rl);
					}
				}
			}
			else
			{
				SaveCurrentLayout();
			}

			Repaint();
		}

		EditorGUILayout.BeginHorizontal();
		{
			saveInGroup = EditorGUILayout.Toggle(saveInGroup, GUILayout.Width(25));
			float OLW = EditorGUIUtility.labelWidth;
			EditorGUIUtility.labelWidth = 100f;
			GUI.enabled = saveInGroup;
			groupName = EditorGUILayout.TextField("Group Name", groupName);
			GUI.enabled = true;
			EditorGUIUtility.labelWidth = OLW;
		}
		EditorGUILayout.EndHorizontal();
		
		EditorGUILayout.EndVertical();
	}

	//serlized saves
	void  SavesGUI (ReorderableList list, SerializedObject group){		
		//header
		list.drawHeaderCallback = (Rect rect) => {  
			EditorGUI.LabelField(rect, "AniSaves");
		};
		
		//element layout
		list.drawElementCallback =  
		(Rect rect, int index, bool isActive, bool isFocused) => {
			SerializedProperty aniSave = saves.GetArrayElementAtIndex(index);
			
			EditorGUIUtility.labelWidth = 25;
			SetColor(Color.cyan, false);
			
			EditorGUI.PropertyField(new Rect(rect.x, rect.y, 35, EditorGUIUtility.singleLineHeight), 
			                        aniSave.FindPropertyRelative("loops"),
			                        new GUIContent("Ani", "Toggle for looping"));
			UndoColor();
			
			//SP* saves[index].name = EditorGUI.TextField(new Rect(rect.x+45, rect.y, rect.width - 105, EditorGUIUtility.singleLineHeight), saves[index].name);
			EditorGUI.PropertyField(new Rect(rect.x+45, rect.y, rect.width - 105, EditorGUIUtility.singleLineHeight),
			                        aniSave.FindPropertyRelative("name"), GUIContent.none);
			
			SetColor(Color.green, true);
			if(GUI.Button(new Rect(rect.x + rect.width - 60, rect.y, 60, EditorGUIUtility.singleLineHeight), "Load"))
			{
				LoadAniSave(aniSave);

				if(EditorApplication.isPlaying)
					script.Play();

				Repaint();
			}
			UndoColor();
		};
	}

	//list only saves -- not seralized
	void  SavesGUI (ReorderableList list, AniGroup group){
		//header
		list.drawHeaderCallback = (Rect rect) => {  
			EditorGUI.LabelField(rect, group.name);
			if(GUI.Button(new Rect(rect.width - 75, rect.y, 75, rect.height), "Remove"))
			{
				groupToRemove = group;
				rlToRemove = list;
			}
		};
		
		//element layout
		list.drawElementCallback =  
		(Rect rect, int index, bool isActive, bool isFocused) => {
			AniSave aniSave = group.members[index];

			EditorGUIUtility.labelWidth = 25;
			SetColor(Color.cyan, false);
			
			aniSave.loops =	EditorGUI.Toggle(new Rect(rect.x, rect.y, 35, EditorGUIUtility.singleLineHeight), 
			                                 new GUIContent("Ani", "Toggle for looping"),
			                                 aniSave.loops);
			UndoColor();
			
			aniSave.name = EditorGUI.TextField(new Rect(rect.x+45, rect.y, rect.width - 105, EditorGUIUtility.singleLineHeight),
			                                   GUIContent.none, aniSave.name);
			
			SetColor(Color.green, true);
			if(GUI.Button(new Rect(rect.x + rect.width - 60, rect.y, 60, EditorGUIUtility.singleLineHeight), "Load"))
			{

				Undo.RecordObject(target, "Load AniSave");
				script.LoadSave(group.name, aniSave.name);

				if(EditorApplication.isPlaying)
					script.Play();

				Repaint();
			}
			UndoColor();
	
		};

		list.onRemoveCallback = (ReorderableList rl) => {
			Undo.RecordObject(script, "Remove AniSave");
			ReorderableList.defaultBehaviours.DoRemoveButton(rl);
		};
	}

	#endregion

	#region Helper Functions
	void  DrawLines (){			
		if(script.sprites.Count > 0)
		{
			for(int i= 0; i < script.sprites.Count; i++)
			{		
				if(script.sprites[i])
					DrawBorder(script.sprites[i].rect);
			}
			
			//splice highlight
			if(script.spliceStartFrame > 0 || script.spliceEndFrame > 0)
			{
				for(int i = script.spliceStartFrame; i <= script.spliceEndFrame; i++)
				{
					HighlightFrame(script.sprites[i].rect, Color.red);
				}
			}

			if(spriteSelected && script.sprites.Count > spriteSelectedIndex
			   && script.sprites[spriteSelectedIndex] != null)
			{
				HighlightFrame(script.sprites[spriteSelectedIndex].rect, Color.yellow);
			}

			if(Application.isPlaying)
			{
				HighlightFrame(script.sprites[script.currentFrame].rect, Color.blue);
			}
			else if (Application.isEditor)
			{
				//start frame highlight
				if(script.sprites.Count > script.startFrame && script.sprites[script.startFrame] != null)
					HighlightFrame(script.sprites[script.startFrame].rect, Color.blue);
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
			color = (color / 3) + (prevColor / 2);
			
		GUI.backgroundColor = color;
	}
	
	void  UndoColor (){
		GUI.backgroundColor = prevColor;
	}
	#endregion

	#region AniSprite Loaders
	public void  SaveCurrentLayout ()
	{	

		if(startFrame.intValue < spliceStartFrame.intValue || startFrame.intValue > spliceEndFrame.intValue)
			startFrame.intValue = spliceStartFrame.intValue;
		Undo.RecordObject(script, "Creat AniSave");
		AniSave newSave = new AniSave("newAni", spliceStartFrame.intValue, spliceEndFrame.intValue, step.intValue, startFrame.intValue, loop.boolValue);
		script.saves.Add(newSave);
	}
	
	public void  SaveCurrentLayoutInGroup (string groupName)
	{	
		//make save
		if(startFrame.intValue < spliceStartFrame.intValue || startFrame.intValue > spliceEndFrame.intValue)
			startFrame.intValue = spliceStartFrame.intValue;
		
		AniSave newSave = new AniSave("newAni", spliceStartFrame.intValue, spliceEndFrame.intValue, step.intValue, startFrame.intValue, loop.boolValue);

		//check for group
		AniGroup group = script.groups.Find(x => x.name == groupName);
		if(group != null) 
		{
			Undo.RecordObject(script, "Create AniGroup Save");
			group.members.Add(newSave);
		}
		else //if no group make one and add it
		{
			Undo.RecordObject(script, "Create AniGroupSave");
			AniGroup ag = new AniGroup();
			ag.name = groupName;
			ag.members.Add(newSave);
			script.groups.Add(ag);
		}
	}
	
	private void LoadAniSave(SerializedProperty save)
	{
		if(save != null)
		{
			playSplice.boolValue = true;
			
			startFrame.intValue = save.FindPropertyRelative("startFrame").intValue;
			currentFrame.intValue = startFrame.intValue;
			spliceStartFrame.intValue = save.FindPropertyRelative("start").intValue;
			spliceEndFrame.intValue = save.FindPropertyRelative("end").intValue;
			step.intValue = save.FindPropertyRelative("step").intValue;
			loop.boolValue = save.FindPropertyRelative("loops").boolValue;
		}
	}
	
	public void  LoadSave ( int index  ){
		SerializedProperty saveToLoad;
		saveToLoad = saves.GetArrayElementAtIndex(index);
		Undo.RecordObject(target, "Load AniSave Values");
		LoadAniSave(saveToLoad);		
	}
	
	//load from grouped save
	public void  LoadSave (SerializedProperty aniSave){
		Undo.RecordObject(target, "Load AniSave Values");
		LoadAniSave(aniSave);		
	}
	#endregion
}
