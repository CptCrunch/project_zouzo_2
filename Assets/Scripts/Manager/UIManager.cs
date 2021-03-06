﻿using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class UIManager : MonoBehaviour {

    private static UIManager instance;

    public GameObject[] userInterfaces = new GameObject[4];

    // ui variables
    // ui children
    private int childHealthBar = 0;
    private int childHPText = 1;
    private int childPlayerIcon = 3;
    private int childSpell1Icon = 4;
    private int childSpell2Icon = 5;
    private int childSpell3Icon = 6;
    private int childLiveContainer = 8;

    // children from Spells
    private int childeCCF = 0; // charge cooldown frame
    private int childCooldown = 1;
    private int childCooldownText = 2;
    private int childChargeCount = 3;

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

                // get PlayerUI script
                PlayerUI playerUIScript = userInterfaces[i].GetComponent<PlayerUI>();

                // transfer index of the ui and the name and character of the player, to the PlayerUI script
                playerUIScript.indexOfUserInterfaces = i;
                playerUIScript.playerName = playerOnStage[i].GetComponent<Player>().playerVitals.Name;
                playerUIScript.playerCharacter = playerOnStage[i].GetComponent<Player>().playerVitals.Character;
            }

            else
            {
                // disable UI
                userInterfaces[i].active = false;
            }
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

                // --- [ set lifes ] ---
                UpdateLifes(playerVitals.Character);

                // --- [ set healthbar ] ---
                // check if objet is visible and set its size
                if (userInterfaces[i].active) { userInterfaces[i].transform.GetChild(childHealthBar).gameObject.GetComponent<Scrollbar>().size = playerVitals.CurrHealth / playerVitals.MaxHealth; }

                // set hp text
                userInterfaces[i].transform.GetChild(childHPText).gameObject.GetComponent<Text>().text = playerVitals.CurrHealth + " / " + playerVitals.MaxHealth;

                // --- [ set death icon ] ---
                if (playerVitals.CurrHealth <= 0) { if (userInterfaces[i].transform.GetChild(childPlayerIcon).gameObject.GetComponent<Image>() != GameManager._instance.GetPlayerDeathIconByName(playerVitals.Character)) { userInterfaces[i].transform.GetChild(childPlayerIcon).gameObject.GetComponent<Image>().overrideSprite = GameManager._instance.GetPlayerDeathIconByName(playerVitals.Character); } }

                // -- [ set standard icon ] ---
                else { if (userInterfaces[i].transform.GetChild(childPlayerIcon).gameObject.GetComponent<Image>() != GameManager._instance.GetPlayerStandardIconByName(playerVitals.Character)) { userInterfaces[i].transform.GetChild(childPlayerIcon).gameObject.GetComponent<Image>().overrideSprite = GameManager._instance.GetPlayerStandardIconByName(playerVitals.Character); } }

                // --- [ set spell icons ] ---
                Image spellOneIcon = userInterfaces[i].transform.GetChild(childSpell1Icon).gameObject.GetComponent<Image>();
                Image spellTwoIcon = userInterfaces[i].transform.GetChild(childSpell2Icon).gameObject.GetComponent<Image>();
                Image spellThreeIcon = userInterfaces[i].transform.GetChild(childSpell3Icon).gameObject.GetComponent<Image>();

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
                            // set icon visible
                            spellIcons[o].gameObject.active = true;
                            // set correcht icon
                            spellIcons[o].sprite = ability.Icons[ability.SpellCount];
                        }

                        // --- [ cooldown text ] ---
                        if (ability.CurrCooldown > 0)
                        {
                            // set cooldown text to visible, if it isn't yet
                            if (!spellIcons[o].transform.GetChild(childCooldownText).gameObject.active) { spellIcons[o].transform.GetChild(childCooldownText).gameObject.active = true; }
                            // set cooldown
                            spellIcons[o].transform.GetChild(childCooldownText).gameObject.GetComponent<Text>().text = ability.CurrCooldown.ToString();
                        }

                        else { if (spellIcons[o].transform.GetChild(childCooldownText).gameObject.active) { spellIcons[o].transform.GetChild(childCooldownText).gameObject.active = false; } }

                        // --- [ charge counter ] ---
                        if (ability.ID == 5)
                        {
                            // transfer leo spell
                            Leo leo = (Leo)ability;
                            // check if charge counter is visible, if not set it visible
                            if (!spellIcons[o].transform.GetChild(childChargeCount).gameObject.active) { spellIcons[o].transform.GetChild(childChargeCount).gameObject.active = true; }
                            // set charge counter
                            spellIcons[o].transform.GetChild(childChargeCount).gameObject.GetComponent<Text>().text = leo.CurrCharge.ToString();

                            // set frame visible if it isn't
                            if (!spellIcons[o].transform.GetChild(childeCCF).gameObject.active) { spellIcons[o].transform.GetChild(childeCCF).gameObject.active = true; }
                            // set fill amount
                            spellIcons[o].transform.GetChild(childeCCF).gameObject.GetComponent<Image>().fillAmount = leo.CurrChargeCooldown / leo.MaxChargeCooldown;
                        }

                        else
                        {
                            // set charge frame invisible, if it isn't yet 
                            if (spellIcons[o].transform.GetChild(childeCCF).gameObject.active) { spellIcons[o].transform.GetChild(childeCCF).gameObject.active = false; }
                            // set charge counter invisible, if it isn't yet 
                            if (spellIcons[o].transform.GetChild(childChargeCount).gameObject.active) { spellIcons[o].transform.GetChild(childChargeCount).gameObject.active = false; }
                        }

                        // set the cooldown image sizte
                        spellIcons[o].transform.GetChild(childCooldown).gameObject.GetComponent<Scrollbar>().size = ability.CurrCooldown / ability.MaxCooldown;// set the cooldown image sizte
                        spellIcons[o].transform.GetChild(childCooldown).gameObject.GetComponent<Scrollbar>().size = ability.CurrCooldown / ability.MaxCooldown;
                    }

                    // --- [ set icon invislible ] ---
                    else
                    {
                        // check if the icon is already invislible
                        if (spellIcons[o].gameObject.active)
                        {
                            // set icon invisible
                            spellIcons[o].gameObject.active = false;
                            // remove the icons sprite
                            spellIcons[o].sprite = null;
                        }
                    }
                }
            }
        }

        /*/ --- [ FPS Counter ] ---
        // toggle pfs
        if(Input.GetKeyDown(KeyCode.F)) { GameManager._instance.showFPS != GameManager._instance.showFPS; }

        // --- [ show pfs ] ---
        if (GameManager._instance.showFPS)
        {
            // set pfs visible
            fpsCount.enabled = true;

            // add trameCount
            frameCount++;

            // save Time.deltaTime
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

    public void UpdateLifes(string _character)
    {
        if (GetUserInterfaceFromPlayerCharacter(_character) != null)
        {
            GetUserInterfaceFromPlayerCharacter(_character).GetComponent<PlayerUI>().UpdateLifes();
        }
    }

    private GameObject GetUserInterfaceFromPlayerCharacter(string _character)
    {
        foreach (GameObject ui in userInterfaces)
        {
            string playerCharacter = ui.GetComponent<PlayerUI>().playerCharacter;
            // check if ui is players ui
            if (playerCharacter == _character) { return ui; }
        }
        return null;
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
