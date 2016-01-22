using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

public class MainUIManager : MonoBehaviour {

    private static MainUIManager instance;

    public GameObject[] btnList_mainMenu = new GameObject[3];
    public int mainMenu_currentBtn;

    private int[] joystickY = new int[4];

    public Animator preStartMenu;
    public Animator mainMenu;
    public Animator optionsMenu;
    public CanvasGroup cg_preMain;
    public CanvasGroup cg_main;
    public CanvasGroup cg_options;

    public GameObject btn_options;
    public GameObject btn_fight;
    public GameObject btn_quit;

    [Header("Debug")]
    public DebugValues Tags;

    // set singleton variable to this instance
    void Awake() { instance = this; }
        
    // instance getter
    public static MainUIManager Instance { get { return instance; } }

	// Use this for initialization
	void Start () {
        Tags.ResetValues();
        CustomDebug.Active = Tags.Active;
        CustomDebug.EnableTag("Main", Tags.Main);
        CustomDebug.EnableTag("Player", Tags.Player);
        CustomDebug.EnableTag("Damage", Tags.Damage);
        CustomDebug.EnableTag("Spells", Tags.Spells);
        CustomDebug.EnableTag("UI", Tags.UI);
        CustomDebug.EnableTag("Testing", Tags.Testing);

        mainMenu_currentBtn = 0;

        cg_main.interactable = false;
        cg_options.interactable = false;
    }
	
	// Update is called once per frame
	void Update () {

        for (int i = 1; i <= joystickY.Length; i++)
        {
            if (Input.GetAxis("P" + i + "_Vertical") > 0.3f && joystickY[i - 1] == 0) { joystickY[i - 1] = 1; }
            if (Input.GetAxis("P" + i + "_Vertical") < -0.3f && joystickY[i - 1] == 0) { joystickY[i - 1] = -1; }
            if (Input.GetAxis("P" + i + "_Vertical") <= 0.3f && Input.GetAxis("P" + i + "_Vertical") >= -0.3f && joystickY[i - 1] != 0) { joystickY[i - 1] = 0; }
        }

        if (cg_main.interactable == true)
        {
            if (Input.GetKeyDown(KeyCode.DownArrow) || joystickY[0] == -1 || joystickY[1] == -1 || joystickY[2] == -1 || joystickY[3] == -1)
            {
                mainMenu_currentBtn++;
                if (mainMenu_currentBtn > btnList_mainMenu.Length - 1) { mainMenu_currentBtn = 0; }
                Debug.Log(mainMenu_currentBtn);
            }

            if (Input.GetKeyDown(KeyCode.UpArrow) || joystickY[0] == 1 || joystickY[1] == 1 || joystickY[2] == 1 || joystickY[3] == 1)
            {
                mainMenu_currentBtn--;
                if (mainMenu_currentBtn < 0) { mainMenu_currentBtn = btnList_mainMenu.Length - 1; }
                Debug.Log(mainMenu_currentBtn);
            }

        }

        if (preStartMenu.GetBool("preMain_active") == true)
        {
            if(Input.anyKey)
            {
                // deactivate premenu
                StartCoroutine(preMainAnimControl(false, 1));
                CustomDebug.Log("preMain_deactivate", "UI");

                // activate mainmenu
                StartCoroutine(mainAnimControl(true, 2));
                CustomDebug.Log(mainMenu.GetBool("main_active"), "UI");

                // enable buttons in mainmenu
                cg_main.interactable = true;
            }
        } 
        
        if (mainMenu.GetBool("main_active") == true)
        {
            if (mainMenu_currentBtn == 0)
            {
                if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.Joystick1Button0) || Input.GetKeyDown(KeyCode.Joystick2Button0) || Input.GetKeyDown(KeyCode.Joystick3Button0) || Input.GetKeyDown(KeyCode.Joystick4Button0))
                {
                    CustomDebug.Log("fight pressed", "UI");
                }
            }
            else if (mainMenu_currentBtn == 1)
            {
                if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.Joystick1Button0) || Input.GetKeyDown(KeyCode.Joystick2Button0) || Input.GetKeyDown(KeyCode.Joystick3Button0) || Input.GetKeyDown(KeyCode.Joystick4Button0))
                {
                    CustomDebug.Log("options pressed", "UI");
                }

            }
            else if (mainMenu_currentBtn == 2)
            {
                if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.Joystick1Button0) || Input.GetKeyDown(KeyCode.Joystick2Button0) || Input.GetKeyDown(KeyCode.Joystick3Button0) || Input.GetKeyDown(KeyCode.Joystick4Button0))
                {
                    CustomDebug.Log("quit pressed", "UI");
                }

            }

        }

        if (joystickY[0] == -1) { joystickY[0] = -2; }
        if (joystickY[1] == -1) { joystickY[1] = -2; }
        if (joystickY[2] == -1) { joystickY[2] = -2; }
        if (joystickY[3] == -1) { joystickY[3] = -2; }
    }

    // optionsButton controller
    public void pressOptionBtn()
    {
        // deactivate mainMenu
        StartCoroutine(mainAnimControl(false, 0));
        cg_main.interactable = false;

        CustomDebug.Log(mainMenu.GetBool("main_active"), "UI");

        CustomDebug.Log("options pressed", "UI");
        // activate optionsMenu
        StartCoroutine(optionsAnimControl(true, 2));
        cg_options.interactable = true;
    }

    // fightButton controller
    public void pressFightBtn()
    {
        // deactivate mainMenu
        StartCoroutine(mainAnimControl(false, 0));
        cg_main.interactable = false;

        CustomDebug.Log(mainMenu.GetBool("main_active"), "UI");

        CustomDebug.Log("fight pressed", "UI");
        // activate optionsMenu
        //StartCoroutine(optionsAnimControl(true, 2));
        //cg_options.interactable = true;
    }

    // optionsButton controller
    public void pressQuitBtn()
    {
        // deactivate mainMenu
        StartCoroutine(mainAnimControl(false, 0));
        cg_main.interactable = false;

        CustomDebug.Log(mainMenu.GetBool("main_active"), "UI");

        CustomDebug.Log("quit pressed", "UI");
        //Application.Quit();
    }

    // preMenu controller
    IEnumerator preMainAnimControl(bool active, float waitTime)
    {
        if (active == false)
        {
            preStartMenu.SetBool("preMain_active", false);
            
        }
        else if (active)
        {
            yield return new WaitForSeconds(waitTime);
            preStartMenu.SetBool("preMain_active", true);
        }

        yield return null;
    }

    // mainMenu controller
    IEnumerator mainAnimControl(bool active, float waitTime)
    {
        if (active == false)
        {
            mainMenu .SetBool("main_active", false);
        }
        else if (active)
        {
            yield return new WaitForSeconds(waitTime);
            mainMenu.SetBool("main_active", true);
        }

        yield return null;
    }

    // optionsMenu controller
    IEnumerator optionsAnimControl(bool active, float waitTime)
    {
        if (active == false)
        {
            optionsMenu.SetBool("options_active", false);
        }
        else if (active)
        {
            yield return new WaitForSeconds(waitTime);
            optionsMenu.SetBool("options_active", true);
        }

        yield return null;
    }

    
}
