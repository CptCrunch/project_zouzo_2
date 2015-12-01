using UnityEngine;
using System.Collections;

public class SwapableAni : MonoBehaviour {

	public string stitch;
	public string[] paths;
	public AniSpriteHolder[] holder;
	int index = 0;

	AniStitch stitcher;

	// Use this for initialization
	void Start () {
		stitcher = GetComponent<AniStitch>();
		stitcher.Play(stitch);
	}
	
	// Update is called once per frame
	void Update () {
		if(Input.anyKeyDown)
		{
			if(index == 0) index = 1; else index = 0;

			GetComponent<AniPatternLoader>().LoadUp(holder[index]);
		}
	}
}
