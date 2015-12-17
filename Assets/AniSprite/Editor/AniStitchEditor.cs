using UnityEngine;
using UnityEditor;
using UnityEditorInternal;
using System.Collections;
using System.Collections.Generic;

[CustomEditor(typeof(AniStitch))]
public class AniStitchEditor : Editor {

	AniStitch stitcher;

	List<ReorderableList> groupLists = new List<ReorderableList>();
	ReorderableList rlToRemove = null;
	AniCompound groupToRemove = null;

	Vector2 scroller;
	string newPatchName = "";

	bool[] open;

	void OnEnable()
	{
		stitcher = target as AniStitch;

		foreach(AniCompound ac in stitcher.patchworks)
		{
			ReorderableList rl = new ReorderableList(ac.slices, typeof(AniCompundSlice), true, true, true, true);
			rl.elementHeight = EditorGUIUtility.singleLineHeight * 2;
			SavesGUI(rl, ac);
			groupLists.Add(rl);
		}

		open = new bool[stitcher.patchworks.Count];
		open [0] = true;
	}

	public override void OnInspectorGUI()
	{
		EditorGUI.BeginChangeCheck();

		EditorGUILayout.BeginVertical();
		{
			newPatchName = EditorGUILayout.TextField("New Stitch Name", newPatchName);
			if(GUILayout.Button("Create Stitch"))
			{
				//check for group
				AniCompound ac = stitcher.patchworks.Find(x => x.name == newPatchName);

				//if group didn't exist before we will add a new reordable list
				if(ac == null)
				{
					AniCompound anicomp = new AniCompound();
					anicomp.name = newPatchName;
					stitcher.patchworks.Add(anicomp);

					groupLists.Clear();
					foreach(AniCompound acs in stitcher.patchworks)
					{
						ReorderableList rl = new ReorderableList(acs.slices, typeof(AniCompundSlice), true, true, true, true);
						SavesGUI(rl, acs);
						groupLists.Add(rl);
					}
				}

				
				open = new bool[groupLists.Count];
				open[groupLists.Count - 1] = true;
			}

			for(int i = 0; i < groupLists.Count; i++)
			{
				EditorGUILayout.BeginHorizontal();
				{
					open[i] = EditorGUILayout.Foldout(open[i], stitcher.patchworks[i].name);
				}
				EditorGUILayout.EndHorizontal();

				if(open[i])
				{
					groupLists[i].DoLayoutList();
				}
				EditorGUILayout.Space();
			}
				
		}
		EditorGUILayout.EndVertical();

		//remove selected group
		if(groupToRemove != null)
		{
			stitcher.patchworks.Remove(groupToRemove);
			groupLists.Remove(rlToRemove);
			groupToRemove = null;
			rlToRemove = null;

			open = new bool[groupLists.Count];
			open[0] = true;
		}

		if(EditorGUI.EndChangeCheck())
		{
			Undo.RecordObject(target, "Undo AniStitch Changes");
			EditorUtility.SetDirty(target);
		}
	}

	void  SavesGUI (ReorderableList list, AniCompound ac){
		//header
		list.drawHeaderCallback = (Rect rect) => {  
			EditorGUI.LabelField(rect, "Name:");
			ac.name = EditorGUI.TextField(new Rect(rect.x + 50, rect.y, 100, rect.height), ac.name);

			SetColor(Color.cyan, false);
			EditorGUIUtility.labelWidth = 10;
			ac.loop = EditorGUI.Toggle(new Rect(rect.width - 155, rect.y, 45, rect.height),
			                           new GUIContent("L", "Should this stitch loop?"),
			                           ac.loop);
			UndoColor();

			EditorGUIUtility.labelWidth = 25;
			ac.times= EditorGUI.IntField(new Rect(rect.width - 125, rect.y, 45, rect.height),
			                             new GUIContent("X's", "Times to repeat stitch, this is overrided if looping is on."),
			                             ac.times);
			if(ac.times < 0) ac.times = 0;

			if(GUI.Button(new Rect(rect.width - 75, rect.y, 75, rect.height), "Remove"))
			{
				Undo.RecordObject(stitcher, "Undo Remove Stitch");
				groupToRemove = ac;
				rlToRemove = list;
			}
		};
		
		//element layout
		list.drawElementCallback =  
		(Rect rect, int index, bool isActive, bool isFocused) => {
			EditorGUIUtility.labelWidth = 40;
			float SLH = EditorGUIUtility.singleLineHeight;
			list.elementHeight = SLH * 2;

			//line one
			float total = 0;
			float spacing = 0;
			float length = 35;
			EditorGUI.LabelField(new Rect(rect.x, rect.y, length, SLH),
			                     "Slice");

			Color prevColor = GUI.contentColor;
			GUI.contentColor = Color.grey / 2 + GUI.contentColor / 2;
			spacing = 5;
			total += spacing + length;
			length = 55;
			EditorGUI.LabelField(new Rect(rect.x + total, rect.y, length, SLH),
			                     "Group");

			spacing = 5;
			total += spacing + length;
			length = 55;
			EditorGUI.LabelField(new Rect(rect.x + total, rect.y, length, SLH),
			                                           "Name");
			spacing = 5;
			total += spacing + length;
			length = 25;
			EditorGUI.LabelField(new Rect(rect.x + total, rect.y, length, SLH),
			                                        "X's");
			spacing = 5;
			total += spacing + length;
			length = 25;
			EditorGUI.LabelField(new Rect(rect.x + total, rect.y, length, SLH),
			                                              "FPS");
			GUI.contentColor = prevColor;

			//line two
			total = 0;
			spacing = 0;
			length = 35;
			//space
			spacing = 5;
			total += spacing + length;
			length = 55;
			ac.slices[index].aniGroup = EditorGUI.TextField(new Rect(rect.x + total, rect.y + SLH, length, SLH),
			                                            ac.slices[index].aniGroup);
			spacing = 5;
			total += spacing + length;
			length = 55;
			ac.slices[index].aniName = EditorGUI.TextField(new Rect(rect.x + total, rect.y + SLH, length, SLH),
			                                           ac.slices[index].aniName);
			spacing = 5;
			total += spacing + length;
			length = 25;
			ac.slices[index].times = EditorGUI.IntField(new Rect(rect.x + total, rect.y + SLH, length, SLH),
			                                        ac.slices[index].times);
			if(ac.slices[index].times < 0) ac.slices[index].times = 0;
			spacing = 5;
			total += spacing + length;
			length = 25;
			ac.slices[index].framerate = EditorGUI.IntField(new Rect(rect.x + total, rect.y + SLH, length, SLH),
			                                              ac.slices[index].framerate);
			if(ac.slices[index].framerate < 0) ac.slices[index].framerate = 0;
		};

		list.onAddCallback = (ReorderableList rl) =>
		{
			ReorderableList.defaultBehaviours.DoAddButton(rl);
			this.Repaint();
		};
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
