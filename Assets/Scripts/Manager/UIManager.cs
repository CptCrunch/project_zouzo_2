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
        GameObject[] playerOnStage = Gamerules._instance.PlayerOnStage;

        // get all player on the stage (gamerules)
        for (int i = 0; i < playerOnStage.Length; i++)
        {
            // check if entry is filled
            if (playerOnStage[i] == null)
            {
                // create ui
                GameObject newUI = Instantiate(uiPrefab, uiPositions[i], new Quaternion(0, 0, 0, 0)) as GameObject;

                // set ui on canvas
                newUI.transform.parent = gameObject.transform;
            }
        }
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
