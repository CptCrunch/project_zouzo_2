/// <summary>
/// AniStitch - Henry Nitz 03/17/15
/// 
/// This is an add on component for the AniSprite animator.
/// This uses the saves from AniSprite to link together animations.
/// Gets a referance to AniSprite through the var ANISPRITE and can
/// can control animations through this component. 
/// 
/// There is only one public function which Play for starting
/// a animation sequence. 
/// </summary>
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class AniCompundSlice
{
	public string name;
	public string aniGroup;
	public string aniName;
	public int times;
	public int framerate;
	public bool inGroup = false;
}

[System.Serializable]
public class AniCompound
{
	public string name;
	public List<AniCompundSlice> slices = new List<AniCompundSlice>();
	public bool loop = false;
	public int times = 1;
}

[RequireComponent(typeof(AniSprite))]
[AddComponentMenu("AniSprite/AniStitch")]
public class AniStitch : MonoBehaviour {

	//full list of sets of linked animations
	public List<AniCompound> patchworks = new List<AniCompound>();

	private AniCompound activeStitch;

	public delegate void EventAni();
	public event EventAni onComplete;

	bool sliceFinished = false;

	//property gets the anisprite if there is not one referenced. 
	private AniSprite _aniSprite;
	public AniSprite aniSprite
	{
		get{
			if(_aniSprite == null)
				_aniSprite = GetComponent<AniSprite>();

			return _aniSprite;
		}

		set{ _aniSprite = value; }
	}

	public void Play(string name)
	{
		activeStitch = patchworks.Find( x => x.name== name);
		SetGoupedValues(activeStitch);
		aniSprite.onComplete += NextSlice;

		StopCoroutine("Run");
		StartCoroutine("Run");
	}

	void SetGoupedValues(AniCompound ac)
	{
		foreach(AniCompundSlice slice in ac.slices)
		{
			if(slice.aniGroup != "")
				slice.inGroup = true;
		}
	}

	void NextSlice()
	{
		sliceFinished = true;
	}

	IEnumerator Run()
	{
		bool looping = true;
		int stitchTicks = 0;
		while(looping || stitchTicks < activeStitch.times)
		{
			looping = activeStitch.loop;
			foreach(AniCompundSlice slice in activeStitch.slices)
			{
				int sliceTicks = 0;

				while(sliceTicks < slice.times)
				{
					if(slice.inGroup)
						aniSprite.Play(slice.aniGroup, slice.aniName);
					else
						aniSprite.Play(slice.aniName);

					aniSprite.FPS = slice.framerate;
					aniSprite.loop = false;

					while(sliceFinished == false)
					{
						yield return new WaitForEndOfFrame();
					}

					sliceFinished = false;

					sliceTicks++;
				}
			}
			if(!activeStitch.loop)
				stitchTicks++;
		}

		if(onComplete != null)
			onComplete();

		aniSprite.Stop();
	}
}













