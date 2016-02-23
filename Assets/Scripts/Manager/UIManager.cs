using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class UIManager : MonoBehaviour {

    private static UIManager instance;

    public GameObject[] userInterfaces = new GameObject[4];
    public Canvas canvas;

    private Image[] characterHolder = new Image[4];

    void Awake()
    {
        if (instance == null) { instance = this; }
        else { Debug.LogError("UIManager has already been instantiated"); Destroy(gameObject); }
    }

    void Start ()
    {
        // --- [ create UI for each player ] ---
        // get all player on the stage (gamerules)
        GameObject[] playerOnStage = GameManager._instance.PlayerOnStage;
        for (int i = 0; i < playerOnStage.Length; i++)
        {
            // check if entry is filled
            if (playerOnStage[i] != null)
            {
                // enable UI
                playerOnStage[i].active = true;
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
	}

    public UIManager Instance { get { return instance; } }
}
