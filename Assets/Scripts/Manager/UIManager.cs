using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

public class UIManager : MonoBehaviour {

    private static UIManager instance;

    public Animator preStartMenu;
    public Animator mainMenu;
    public CanvasGroup cg_preMain;
    public CanvasGroup cg_main;


    // set singleton variable to this instance
    void Awake() { instance = this; }

    // instance getter
    public static UIManager Instance { get { return instance; } }

    

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {

        if (preStartMenu.GetBool("preMain_active") == true)
        {
            if(Input.anyKey)
            {
                StartCoroutine(preMainAnimControl(false, 1));
                Debug.Log("preMain_deactivate");
            }
        }
        else if (preStartMenu.GetBool("preMain_active") == false)
        {
            StartCoroutine(mainAnimControl(true, 2));
            cg_main.interactable = true;

        }
        
   }

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

    IEnumerator mainAnimControl(bool active, float waitTime)
    {
        if (active == false)
        {
            mainMenu.SetBool("main_active", false);
        }
        else if (active)
        {
            yield return new WaitForSeconds(waitTime);
            mainMenu.SetBool("main_active", true);
        }

        yield return null;
    }
}
