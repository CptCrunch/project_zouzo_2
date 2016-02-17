using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class UIManager : MonoBehaviour {

    private static UIManager instance;

    public Vector2[] uiPositions = new Vector2[4];
    public GameObject uiPrefab;

    private Image[] characterHolder = new Image[4];
    public Sprite[] playerIcons = new Sprite[4];
    public Sprite[] playerDeathIcons = new Sprite[4];

    void Awake()
    {
        if (instance == null) { instance = this; }
        else { Debug.LogError("UIManager has already been instantiated"); }
    }

    // Use this for initialization
    void Start ()
    {
        // #get all player on the stage (gamerules)
        // #create uiPrefabs for each one
        // #set player Icons

        // --- [ set player Icons ] ---
        for (int i = 0; i < characterHolder.Length; i++)
        {
            // check if objet isn't null
            if (characterHolder[i] != null) { characterHolder[i].overrideSprite = playerIcons[i]; }
        }
    }
	
	// Update is called once per frame
	void Update ()
    {
	
	}

    public UIManager Instance { get { return instance; } }
}
