/// <summary>
/// AniPatternLoader - Henry Nitz 03/17/15
/// 
/// This is an add on for AniSprite. This addon allows for 
/// loading of sprites in the correct order. This is needed
/// when switching spritesheets and the animation frames are the same.
/// 
/// Pattern needs to be set in the editor. Then a spritesheet can be loaded
/// either by supplying a path to one in the resources folder or 
/// setting up a public property on a monobehaviour that has a AniSpriteHolder
/// and passing through the holder.  
/// 
/// NOTE: AniSpriteHolder has a special propertydrawer. This class can be used
/// for other things as well since the sprites are public. Though check that
/// they have been loaded first. 
/// </summary>
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class AniSpriteHolder
{
	//for checking that sprites have been loaded
	//and used by property drawer to change GUI.
	public bool loaded = false;

	[SerializeField]
	private Sprite baseSprite; //used by property draweronly.
	public Sprite[] sprites; //list of loaded sprites. 
}

[RequireComponent(typeof(AniSprite))]
[AddComponentMenu("AniSprite/AniPatternLoader")]
public class AniPatternLoader : MonoBehaviour {

	[System.Serializable]
	public class Pattern
	{
		public bool ranged = false; //if false only uses startSprite
		public int startSprite;
		public int endSprite;
	}

	public List<Pattern> pattern = new List<Pattern>();

	/// <summary>
	/// Loads up sprites from path in Resources folder.
	/// </summary>
	/// <param name="path">Path.</param>
	public void LoadUp(string path)
	{
		Sprite[] newSprites = Resources.LoadAll<Sprite>(path);
		AssignSheet(newSprites);
	}

	/// <summary>
	/// Loads up sprites from set up AniSpriteHolder
	/// </summary>
	/// <param name="sprites">Sprites.</param>
	public void LoadUp(AniSpriteHolder sprites)
	{
		if(sprites.loaded)
			AssignSheet(sprites.sprites);
		else
		{
			Debug.LogError("A AniSpriteHolder has not been loaded with sprites");
		}
	}

	void AssignSheet(Sprite[] sprites)
	{
		AniSprite ani = GetComponent<AniSprite>();
		ani.sprites.Clear();
		foreach(Pattern p in pattern)
		{
			if(p.ranged)
			{
				for(int i = p.startSprite; i <= p.endSprite; i++)
				{
					ani.sprites.Add(sprites[i]);
				}
			}
			else
			{
				ani.sprites.Add(sprites[p.startSprite]);
			}
		}
	}
}
