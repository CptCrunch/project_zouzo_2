using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class UIManager : MonoBehaviour {

    private static UIManager instance;

    public GameObject[] userInterfaces = new GameObject[4];

    // --- [ FPS Counter ] ---
    public Text fpsCount;
    int frameCount = 0;
    float dt = 0.0f;
    float fps = 0.0f;
    float updateRate = 4.0f;

    // --- [ Time Display ] ---
    public Text timer;

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

        // --- [ set player Icons ] ---
        for (int i = 0; i < userInterfaces.Length; i++)
        {
            // check if objet is visible
            if (userInterfaces[i].active) { userInterfaces[i].transform.GetChild(2).gameObject.GetComponent<Image>().overrideSprite = GameManager._instance.GetPlayerStandardIconByName(playerOnStage[i].GetComponent<Player>().playerVitals.Character); }
        }
    }
	
	// Update is called once per frame
	void Update ()
    {
        // get all player on the stage (gamerules)
        GameObject[] playerOnStage = GameManager._instance.PlayerOnStage;

        // --- [ set ui for each player ] ---
        for (int i = 0; i < userInterfaces.Length; i++)
        {
            if (playerOnStage[i] != null)
            {
                // get player script
                PlayerAbilities playerAbilitiesScript = playerOnStage[i].GetComponent<PlayerAbilities>();

                // get player vitals
                LivingEntity playerVitals = playerOnStage[i].GetComponent<Player>().playerVitals;

                // --- [ set healthbar ] ---
                // check if objet is visible
                if (userInterfaces[i].active) { userInterfaces[i].transform.GetChild(0).gameObject.GetComponent<Scrollbar>().size = playerVitals.CurrHealth / playerVitals.MaxHealth; }

                // --- [ set death icon ] ---
                if (playerVitals.CurrHealth <= 0) { if (userInterfaces[i].transform.GetChild(2).gameObject.GetComponent<Image>() != GameManager._instance.GetPlayerDeathIconByName(playerVitals.Character)) { userInterfaces[i].transform.GetChild(2).gameObject.GetComponent<Image>().overrideSprite = GameManager._instance.GetPlayerDeathIconByName(playerVitals.Character); } }

                // -- [ set standard icon ] ---
                else { if (userInterfaces[i].transform.GetChild(2).gameObject.GetComponent<Image>() != GameManager._instance.GetPlayerStandardIconByName(playerVitals.Character)) { userInterfaces[i].transform.GetChild(2).gameObject.GetComponent<Image>().overrideSprite = GameManager._instance.GetPlayerStandardIconByName(playerVitals.Character); } }

                // --- [ set spell icons ] ---
                Image spellOneIcon = userInterfaces[i].transform.GetChild(3).gameObject.GetComponent<Image>();
                Image spellTwoIcon = userInterfaces[i].transform.GetChild(4).gameObject.GetComponent<Image>();
                Image spellThreeIcon = userInterfaces[i].transform.GetChild(5).gameObject.GetComponent<Image>();

                Image[] spellIcons = new Image[3] { spellOneIcon, spellTwoIcon, spellThreeIcon };

                for (int o = 0; o < spellIcons.Length; o++)
                {
                    Attacks ability = playerAbilitiesScript.abilityArray[o + 1];

                    // check  if the player has a ability in slot 1
                    if (ability != null)
                    {
                        // check if icon has correct sprite
                        if (spellIcons[o].sprite != ability.Icons[ability.SpellCount])
                        {
                            spellIcons[o].gameObject.active = true;
                            spellIcons[o].sprite = ability.Icons[ability.SpellCount];
                        }
                    }

                    // set icon invislible if the player has no ability in slot 1
                    else
                    {
                        if (spellIcons[o].gameObject.active)
                        {
                            spellIcons[o].gameObject.active = false;
                            spellIcons[o].sprite = null;
                        }
                    }

                    if (ability.CurrCooldown > 0)
                    {
                        spellIcons[o].transform.GetChild(0).gameObject.active = true;
                        spellIcons[o].transform.GetChild(0).gameObject.GetComponent<Text>().text = ability.CurrCooldown.ToString();
                    }

                    else
                    {
                        if (spellIcons[o].transform.GetChild(0).gameObject.active) { spellIcons[o].transform.GetChild(0).gameObject.active = false; }
                    }

                    if (ability.ID == 5)
                    {
                        Leo leo = (Leo)ability;
                        if (!spellIcons[o].transform.GetChild(1).gameObject.active) { spellIcons[o].transform.GetChild(1).gameObject.active = true; }
                        spellIcons[o].transform.GetChild(1).gameObject.GetComponent<Text>().text = leo.CurrCharge.ToString();
                    }

                    else
                    {
                        if (spellIcons[o].transform.GetChild(1).gameObject.active) { spellIcons[o].transform.GetChild(1).gameObject.active = false; }
                    }
                }
            }
        }

        /*/ -- [ FPS Counter ] --
        if(Input.GetKeyDown(KeyCode.F))
        {
            if (GameManager._instance.showFPS)
            {
                GameManager._instance.showFPS = false;
            }

            else
            {
                GameManager._instance.showFPS = true;
            }
        }

        if (GameManager._instance.showFPS)
        {
            fpsCount.enabled = true;

            frameCount++;
            dt += Time.deltaTime;

            if (dt > 1.0f / updateRate)
            {
                fps = frameCount / dt;
                frameCount = 0;
                dt -= 1.0f / updateRate;
            }

            fpsCount.text = Mathf.RoundToInt(fps).ToString();
        }

        else
        {
            fpsCount.enabled = false;
        }*/
    }

    // --- [ Timer ] ---
    public void SetTimer(float t)
    {
        if (timer != null)
        {
            timer.text = t.ToString("f0");
        }
    }
    
    public static UIManager Instance { get { return instance; } }
}
