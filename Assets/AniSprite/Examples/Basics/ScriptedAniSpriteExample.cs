using UnityEngine;
using System.Collections;

public class ScriptedAniSpriteExample : MonoBehaviour {

	public string aniToStart = "Walk";
	public string aniToPlay = "Hit";
	public KeyCode actKey = KeyCode.Space;

	AniSprite ani;
	string oldAni;

	public void Start()
	{
		ani = GetComponent<AniSprite>();
		ani.Play(aniToStart);
	}

	public void Update()
	{
		if(Input.GetKeyDown(actKey))
		{
			StartCoroutine(OnComplete());
		}
	}

	public IEnumerator OnComplete()
	{
		oldAni = ani.currentAniName;
		ani.Play(aniToPlay);
		yield return new WaitForSeconds(0.5f);
		ani.Play(oldAni);
	}
}
