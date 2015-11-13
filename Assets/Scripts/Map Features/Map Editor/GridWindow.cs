using System.Collections;
using UnityEditor;
using UnityEngine;

public class GridWindow : EditorWindow
{
    Grid grid;

    public void init()
    {
        grid = (Grid)FindObjectOfType(typeof(Grid));
    }

    void OnGUI()
    {
        grid.width = createSlider("Width", grid.width);
        grid.height = createSlider("Height", grid.height);
        EditorGUILayout.BeginVertical();
        EditorGUILayout.LabelField("Color");
        grid.color = EditorGUILayout.ColorField(grid.color, GUILayout.Width(200));
        EditorGUILayout.EndVertical();
    }

    private float createSlider(string labelName, float sliderPosition)
    {
        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("Grid " + labelName);
        sliderPosition = EditorGUILayout.Slider(sliderPosition, 1f, 100f, null);
        EditorGUILayout.EndHorizontal();

        return sliderPosition;
    }
}

