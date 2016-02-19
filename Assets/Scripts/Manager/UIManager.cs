using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class UIManager : MonoBehaviour {

    private static UIManager instance;

    public Vector2[] uiPositions = new Vector2[4];
    public GameObject uiPrefab;
    public GameObject[] userInterfaces = new GameObject[4];
    private Image uiPanel;

    private Image[] characterHolder = new Image[4];

    void Awake()
    {
        if (instance == null) { instance = this; }
        else { Debug.LogError("UIManager has already been instantiated"); }

        uiPanel = transform.FindChild("UIPanel").GetComponent<Image>();
    }

    void Start ()
    {
        // --- [ create UI for each player ] ---
        // get all player on the stage (gamerules)
        GameObject[] playerOnStage = GameManager._instance.PlayerOnStage;
        for (int i = 0; i < playerOnStage.Length; i++)
        {
            // check if entry is filled
            if (playerOnStage[i] == null)
            {
                // create ui
                GameObject newUI = Instantiate(uiPrefab, uiPositions[i], new Quaternion(0, 0, 0, 0)) as GameObject;

                // set ui on canvas
                newUI.transform.parent = uiPanel.transform;
               
                // set ui possition
                newUI.transform.position = new Vector2(-uiPanel.rectTransform.rect.xMin, -uiPanel.rectTransform.rect.yMin + uiPanel.rectTransform.rect.height / 2);

                // add ui to UIs
                Util.IncludeGameObject(userInterfaces, newUI);
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
