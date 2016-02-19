using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class UIManager : MonoBehaviour {

    private static UIManager instance;

    public Vector2[] uiPositions = new Vector2[4];
    public GameObject uiPrefab;
    public GameObject[] UIs = new GameObject[4];

    private Image[] characterHolder = new Image[4];

    void Awake()
    {
        if (instance == null) { instance = this; }
        else { Debug.LogError("UIManager has already been instantiated"); }
    }

    void Start ()
    {
        // --- [ create UI for each player ] ---
        // get all player on the stage (gamerules)
        GameObject[] playerOnStage = Gamerules._instance.PlayerOnStage;
        for (int i = 0; i < playerOnStage.Length; i++)
        {
            // check if entry is filled
            if (playerOnStage[i] == null)
            {
                // create ui
                GameObject newUI = Instantiate(uiPrefab, uiPositions[i], new Quaternion(0, 0, 0, 0)) as GameObject;

                // set ui on canvas
                newUI.transform.parent = gameObject.transform;

                // set ui possition
                newUI.transform.position = uiPositions[i];

                // add ui to UIs
                Util.IncludeGameObject(UIs, newUI);
            }
        }
        // #set player Icons

        /*/ --- [ set player Icons ] ---
        for (int i = 0; i < characterHolder.Length; i++)
        {
            // check if objet isn't null
            if (characterHolder[i] != null) { characterHolder[i].overrideSprite = playerIcons[i]; }
        }*/
    }
	
	// Update is called once per frame
	void Update ()
    {
        Debug.Log(UIs[0].transform.position);
	}

    public UIManager Instance { get { return instance; } }
}
