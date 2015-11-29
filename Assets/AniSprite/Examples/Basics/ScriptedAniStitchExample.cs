using UnityEngine;
using System.Collections;

public class ScriptedAniStitchExample : MonoBehaviour {

	public string stitchName;

	// Use this for initialization
	void Start () {
		GetComponent<AniStitch>().Play(stitchName);
	}
}
