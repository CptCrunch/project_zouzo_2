/// <summary>
/// AniSprite - Henry Nitz 03/17/15
/// 
/// This is the main animator script of AniSprite. Referances should
/// be made to this component to control animation during runtime. 
/// </summary>

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

//Class to save animation clips. 
[System.Serializable]
public class AniSave 
{
	public string name = "newAni";
	public int start =0;
	public int end = 0;
	public int step =0;
	public int startFrame = 0;
	public bool loops = true;

	public AniSave(){}

	public AniSave (string name, int start,  int end, int step, int startFrame, bool loops){
		this.name = name;
		this.start = start;
		this.end = end;
		this.step = step;
		this.startFrame = startFrame;
		this.loops = loops;
	}
}

[System.Serializable]	
public class AniGroup
{
	public string name;
	public List<AniSave> members= new List<AniSave>();

	public AniGroup(){}
	public AniGroup(string name) { this.name = name;}
}

[RequireComponent(typeof(SpriteRenderer))]
[AddComponentMenu("AniSprite/AniSprite")]
public class AniSprite : MonoBehaviour
{	
	#region VARS
	//animation
	public List<Sprite> sprites = new List<Sprite>();
	public bool  animatedOnStart = true;

	[SerializeField]
	private int _FPS = 12;
	public int FPS 
	{
		set
		{
			_FPS = value;
			if(_FPS < 0)
				_FPS = 0;


			//was reseting the animation each time but this of course caused the animation to freeze if
			//you tried to set the FPS each frame. A future attenmpt at changing frame rates might be
			//calc an average of the prev and new FPS and check if the new FPS has been played before reseting again.
			//(or count the time instead of using wait for seconds).

			//if(Application.isPlaying)
			//{
			//	ResetAnimationSequence ();
			//}
		}

		get{ return _FPS;}
	}

	public int step = 1; //direction, -1 for reverse
	public int startFrame = 0;
	public int currentFrame = 0;	
	public bool paused = false;
	public bool loop = true;
	
	public bool  playSplice = false;
	public int spliceStartFrame = 0;
	public int spliceEndFrame = 0;
	public delegate void EventAni();
	public event EventAni onComplete;

	public string currentAniName = ""; //should make a readonly property field or function. 

	public List<AniSave> saves = new List<AniSave>();
	public List<AniGroup> groups = new List<AniGroup>();

	private SpriteRenderer sp_Renderer;	
	private int endingBound;
	private int startingBound;

	//property gets the stitcher if there is not one referenced. 
	private AniStitch _stitcher;
	public AniStitch stitcher
	{
		get 
		{
			if(_stitcher == null)
				_stitcher = GetComponent<AniStitch>();
			
			return _stitcher;
		}
		
		set
		{
			_stitcher = value;
		}
	}

	//property gets the patternloader if there is not one referenced. 
	private AniPatternLoader _patternLoader;
	public AniPatternLoader patternLoader
	{
		get 
		{
			if(_patternLoader == null)
				_patternLoader = GetComponent<AniPatternLoader>();
			
			return _patternLoader;
		}
		
		set
		{
			_patternLoader = value;
		}
	}

	#endregion

	#region ANIMATION
	public void  Awake (){
		sp_Renderer = GetComponent<SpriteRenderer>() as SpriteRenderer;
	}
	
	public void  Start (){
		currentFrame = startFrame;
		if(spliceStartFrame > 0 || spliceEndFrame > 0)
			playSplice = true;

		if(sprites.Count == 0)
		{
		   Debug.LogError(string.Format("AniSprite: GameObject {0} doesn't have any frames to play an animation", gameObject.name),
		               gameObject);
			return;
		}

		Set();
		
		if(animatedOnStart)
			Play();
	}

	//sets the renderer to sprite, current frame to start frame and calculates bounding frames for animation.
	private void  Set (){
		if(playSplice) { startingBound = spliceStartFrame; endingBound = spliceEndFrame; }
		else { startingBound = 0; endingBound = sprites.Count - 1; }
		
		if(currentFrame > endingBound)
			currentFrame = startingBound;
		else if(currentFrame < startingBound)
			currentFrame = endingBound;
			
		//sprite rendere here
		sp_Renderer.sprite = sprites[currentFrame];
	}

	IEnumerator  Animate (){
		bool run = true;
		while(run)
		{
			//wait for fps if it is above zero, else check back each frame. 
			if(!paused && _FPS > 0)
			{
				yield return new WaitForSeconds(1f / _FPS);
				currentFrame += step;
				Set();					
			}
			else
				yield return new WaitForEndOfFrame();

			//logic for desiding to run callback.
			if(!loop && !paused && onComplete != null)
			{
				//only do callback if the frame is the last frame.
				if(step > 0 && currentFrame == spliceEndFrame
				   || step < 0 && currentFrame == spliceStartFrame)
				{
					//need to wait one more frame
					if(_FPS > 0)
						yield return new WaitForSeconds(1f / _FPS);

					//stop animation loop
					run = false;
					onComplete();					
					break;
				}				
			}
		}

	}
	#endregion

	#region CONTROLLERS
	//sets frame and stops all animation
	public void  SetFrame ( int frame  ){
		currentFrame = frame;
		startingBound = 0;
		endingBound = sprites.Count - 1;
		Set();
		StopCoroutine("Animate");
	}

	public void  SetFrame (string aniName){
		LoadSave(aniName);
		Set();
		StopCoroutine("Animate");
	}

	public void  SetFrame (string aniGroup, string aniName){
		LoadSave(aniGroup, aniName);
		Set();
		StopCoroutine("Animate");
	}

	//move by one frame
	public void  StepForward (){
		currentFrame++;
		Set();
	}

	public void  StepBackwards (){
		currentFrame--;
		Set();
	}

	//starts an animation from the current frame.
	public void  Play (){
		ResetAnimationSequence ();
	}

	//stops animation and starts with new.
	public void  Play ( int startFrame ){
		currentFrame = startFrame;
		Set();
		ResetAnimationSequence ();
	}
	
	public void  Play ( string aniName  ){
		LoadSave(aniName);
		Set();
		ResetAnimationSequence ();
	}

	public void  Play ( string aniGroup, string aniName  ){
		LoadSave(aniGroup, aniName);
		Set();
		ResetAnimationSequence ();
	}

	//plays a stitch through the ANISTITCH component.
	public void PlayStitch(string stitchName)
	{
		stitcher.Play(stitchName);
	}

	//change playback direction
	public void  PlayForward (){
		step = 1;
		Play();
	}

	public void  PlayBackward (){
		step = -1;
		Play();
	}

	public void  Stop (){
		StopCoroutine("Animate");
	}

	//shortcut for stoping and satrting the animation coroutine
	public void ResetAnimationSequence()
	{
		StopCoroutine("Animate");
		StartCoroutine("Animate");
	}

	#endregion

	#region LOADERS
	private void LoadAniSave(AniSave save)
	{
		if(save != null)
		{
			playSplice = true;
			
			startFrame = save.startFrame;
			currentFrame = startFrame;
			spliceStartFrame = save.start;
			spliceEndFrame = save.end;
			step = save.step;
			loop = save.loops;

			currentAniName = save.name;
		}
	}
	
	public void  LoadSave ( string name  ){

		AniSave saveToLoad = saves.Find(x => { return x.name.ToLower() == name.ToLower();});

		LoadAniSave(saveToLoad);		
	}
	
	public void  LoadSave ( string groupName, string aniName  ){
		AniGroup groupToLoad = null;
		AniSave saveToLoad = null;

		//search for group
		for(int i= 0; i < groups.Count; i++)
		{
			if(groups[i].name == groupName)
			{
				groupToLoad = groups[i];
				break;
			}
		}

		//if group is found
		if(groupToLoad != null)
		{
			//search for save
			saveToLoad = groupToLoad.members.Find(x => {return x.name.ToLower() == aniName.ToLower();});
		}

		LoadAniSave(saveToLoad);		
	}
	#endregion
}