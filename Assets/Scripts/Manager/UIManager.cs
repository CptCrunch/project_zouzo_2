using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

public class UIManager : MonoBehaviour {

    private static UIManager instance;

    public GameObject[] btnList_mainMenu = new GameObject[3];
    private int currentBtn_menu = 0;

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


    // set singleton variable to this instance
    void Awake() { instance = this; }

    // instance getter
    public static UIManager Instance { get { return instance; } }

    

	// Use this for initialization
	void Start () {
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
                currentBtn_menu++;
                if (currentBtn_menu > btnList_mainMenu.Length - 1) { currentBtn_menu = 0; }
                Debug.Log(currentBtn_menu);
            }

            if (Input.GetKeyDown(KeyCode.UpArrow) || joystickY[0] == 1 || joystickY[1] == 1 || joystickY[2] == 1 || joystickY[3] == 1)
            {
                currentBtn_menu--;
                if (currentBtn_menu < 0) { currentBtn_menu = btnList_mainMenu.Length - 1; }
                Debug.Log(currentBtn_menu);
            }
        }
      
        if (preStartMenu.GetBool("preMain_active") == true)
        {
            if(Input.anyKey)
            {
                // deactivate premenu
                StartCoroutine(preMainAnimControl(false, 1));
                /*Debug.Log("preMain_deactivate");*/

                // activate mainmenu
                StartCoroutine(mainAnimControl(true, 2));
                Debug.Log(mainMenu.GetBool("main_active"));

                // enable buttons in mainmenu
                cg_main.interactable = true;
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

        Debug.Log(mainMenu.GetBool("main_active"));

        // deactivate optionsMenu
        StartCoroutine(optionsAnimControl(true, 2));
        cg_options.interactable = true;
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
