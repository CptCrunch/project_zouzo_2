using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class UIManager : MonoBehaviour {

    private static UIManager instance;

    public GameObject[] userInterfaces = new GameObject[4];
    public Canvas canvas;

    private Image[] characterHolder = new Image[4];

    // -- [ FPS Counter ] --
    public Text fpsCount;
    int frameCount = 0;
    float dt = 0.0f;
    float fps = 0.0f;
    float updateRate = 4.0f;
    //----------------------

    // -- [ Time Display ] --
    public Text timer;
    //-----------------------

    void Awake()
    {
        if (instance == null) { instance = this; }
        else { Debug.LogWarning("UIManager has already been instantiated"); Destroy(gameObject); }
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
                userInterfaces[i].active = true;
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
        // -- [ FPS Counter ] --
        if(Input.GetKeyDown(KeyCode.F)) {
            if (GameManager._instance.showFPS)
                GameManager._instance.showFPS = false;
            else
                GameManager._instance.showFPS = true;
        }

        if (GameManager._instance.showFPS) {
            fpsCount.enabled = true;

            frameCount++;
            dt += Time.deltaTime;
            if (dt > 1.0f / updateRate) {
                fps = frameCount / dt;
                frameCount = 0;
                dt -= 1.0f / updateRate;
            }

            fpsCount.text = Mathf.RoundToInt(fps).ToString();
        } else {
            fpsCount.enabled = false;
        }
        //----------------------
    }

    // -- [ Timer ] --
    public void SetTimer(float t) {
        if(timer != null)
            timer.text = t.ToString("f0");
    }

    public static UIManager Instance { get { return instance; } }
}
