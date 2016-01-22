using UnityEngine;
using System.Collections;

public class SceneUIManager : MonoBehaviour {

    [Header("Debug")]
    public DebugValues Tags;

    void Start()
    {
        Tags.ResetValues();
        CustomDebug.Active = Tags.Active;
        CustomDebug.EnableTag("Main", Tags.Main);
        CustomDebug.EnableTag("Player", Tags.Player);
        CustomDebug.EnableTag("Damage", Tags.Damage);
        CustomDebug.EnableTag("Spells", Tags.Spells);
        CustomDebug.EnableTag("UI", Tags.UI);
        CustomDebug.EnableTag("Testing", Tags.Testing);
    }

    // Update is called once per frame
    void Update () {
	
	}
}
