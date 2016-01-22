using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

public class MainUIManager : MonoBehaviour {

    private static MainUIManager instance;

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
    public static MainUIManager Instance { get { return instance; } }

	// Use this for initialization
	void Start () {
        cg_main.interactable = false;
        cg_options.interactable = false;
    }
	
	// Update is called once per frame
	void Update () {

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
