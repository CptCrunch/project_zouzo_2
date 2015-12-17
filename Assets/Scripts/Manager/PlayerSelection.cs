using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class PlayerSelection : MonoBehaviour {

    #region Variables
    public static PlayerSelection _instance;

    public List<string> controller = new List<string>();
    private bool p1, p2, p3, p4, kb;
    public int levelToLoad;

    // Player Limitation and Text/
    public CharacterSplashArts splashart;

    public bool limitPlayerAmmount;
    [Range(1,4)]
    public int playerLimit;
    private int playerCount;

    private int splashCounter_pic1, splashCounter_pic2, splashCounter_pic3, splashCounter_pic4, splashCounter_pic5;
    private bool sp_p1, sp_p2, sp_p3, sp_p4, sp_kb;
    public string[] chosenPics = new string[4];
    #endregion

    void Awake()
    {
        if (_instance == null) { _instance = this; }
    }

    void Start()
    {
        splashCounter_pic1 = splashart.playerSplashart.Length;
        splashCounter_pic2 = splashart.playerSplashart.Length;
        splashCounter_pic3 = splashart.playerSplashart.Length;
        splashCounter_pic4 = splashart.playerSplashart.Length;
        splashCounter_pic5 = splashart.playerSplashart.Length;

        //for (int i = 0; i < splashart.splashartHolder.Length; i++)
        //{
        //    splashart.splashartHolder[i].sprite = splashart.playerSplashartGrayscale[i];
        //}
    }

    void Update()
    {
        Joystick_1();
        Joystick_2();
        Joystick_3();
        Joystick_4();
        Keyboard();

        // load level on confirm
        if (Input.GetKeyDown(KeyCode.Joystick1Button7) || Input.GetKeyDown(KeyCode.Joystick2Button7) || Input.GetKeyDown(KeyCode.Joystick3Button7) || Input.GetKeyDown(KeyCode.Joystick4Button7) || Input.GetKeyDown(KeyCode.L))
        {
            if (sp_p1 || sp_p2 || sp_p3 || sp_p4 || sp_kb)
            {
                Gamerules._instance.connectedControllers = controller;
                Gamerules._instance.chosenCharacters = chosenPics;
                Application.LoadLevel(levelToLoad);
            }
        }

        // visualize connected controlsers
        for (int i = 0; i < controller.Count; i++) {
            splashart.playerText[i].color = Color.green;
            splashart.playerText[i].text = controller[i] + " Connected";
        }

        Gamerules._instance.playerAmmount = controller.Count;
    }

    #region Input
    // Input Joystick 1
    void Joystick_1() {

        // connect joystic 1
        if (Input.GetKeyDown(KeyCode.Joystick1Button7) && !p1) {
            if (!limitPlayerAmmount) {
                controller.Add("P1");
                p1 = true;
            }

            else {
                if (playerCount < playerLimit) {
                    controller.Add("P1");
                    playerCount++;
                    p1 = true;
                }
            }
        }

        // disconnect joysic 1
        if (Input.GetKeyDown(KeyCode.Joystick1Button1) && p1) {
            if (limitPlayerAmmount) {
                playerCount--;
                ResetText(controller.IndexOf("P1"));
                controller.Remove("P1");
                
                p1 = false;
            }
            
            else {
                ResetText(controller.IndexOf("P1"));
                controller.Remove("P1");
                
                p1 = false;
            }
        }

        int counter = 0;
        foreach (string i in controller)
        {
            if (i == "P1" && p1)
            {
                if (Input.GetKeyDown(KeyCode.Joystick1Button4) && !sp_p1)
                {

                    splashCounter_pic1--;
                    if (splashCounter_pic1 == 0) { splashCounter_pic1 = splashart.playerSplashart.Length; }

                    SplashArtChange(counter, splashCounter_pic1);
                }

                if (Input.GetKeyDown(KeyCode.Joystick1Button5) && !sp_p1)
                {
                    splashCounter_pic1++;
                    if (splashCounter_pic1 >= splashart.playerSplashart.Length + 1) { splashCounter_pic1 = 1; }

                    SplashArtChange(counter, splashCounter_pic1);
                }

                if (Input.GetKeyDown(KeyCode.Joystick1Button0))
                {
                    ChooseSplashArt(counter, splashCounter_pic1);
                    sp_p1 = true;
                }
            }
            counter++;
        }

        if (!p1)
        {
            splashart.splashartHolder[counter].overrideSprite = null;
        }

        if(Input.GetKeyDown(KeyCode.Joystick1Button1) && p1)
        {
            sp_p1 = true;
        }
    }

    //Input Joystick 2
    void Joystick_2() {
        
        // connect joysick 2
        if (Input.GetKeyDown(KeyCode.Joystick2Button0) && !p2) {
            if (!limitPlayerAmmount) {
                controller.Add("P2");
                p2 = true;
            }
            
            else{
                if (playerCount <= playerLimit) {
                    controller.Add("P2");
                    playerCount++;
                    p2 = true;
                }
            }
        }

        // disconnect joysick 2
        if (Input.GetKeyDown(KeyCode.Joystick2Button1) && p2) {
            if (limitPlayerAmmount) {
                playerCount--;
                ResetText(controller.IndexOf("P2"));
                controller.Remove("P2");
                
                p2 = false;
            }
            
            else {
                ResetText(controller.IndexOf("P2"));
                controller.Remove("P2");

                p2 = false;
            }
        }

        int counter = 0;
        foreach (string i in controller)
        {
            if (i == "P2" && p2)
            {
                if (Input.GetKeyDown(KeyCode.Joystick2Button4))
                {

                    splashCounter_pic2--;
                    if (splashCounter_pic2 == 0) { splashCounter_pic2 = splashart.playerSplashart.Length; }

                    SplashArtChange(counter, splashCounter_pic1);
                }

                if (Input.GetKeyDown(KeyCode.Joystick2Button5))
                {
                    splashCounter_pic2++;
                    if (splashCounter_pic2 >= splashart.playerSplashart.Length + 1) { splashCounter_pic2 = 1; }

                    SplashArtChange(counter, splashCounter_pic2);
                }
            }
            counter++;
        }

        if (!p2)
        {
            splashart.splashartHolder[counter].overrideSprite = null;
        }
    }

    //Input Joystick 3
    void Joystick_3() {
        
        // connect joysick 3
        if (Input.GetKeyDown(KeyCode.Joystick3Button0) && !p3) {
            if (!limitPlayerAmmount) {
                controller.Add("P3");
                p3 = true;
            }
            
            else {
                if (playerCount < playerLimit) {
                    controller.Add("P3");
                    playerCount++;
                    p3 = true;
                }
            }
        }

        // disconnect joysick 3
        if (Input.GetKeyDown(KeyCode.Joystick3Button1) && p3) {
            if (limitPlayerAmmount) {
                playerCount--;
                ResetText(controller.IndexOf("P3"));
                controller.Remove("P3");

                p3 = false;
            }

            else {
                ResetText(controller.IndexOf("P3"));
                controller.Remove("P3");

                p3 = false;
            }
        }

        int counter = 0;
        foreach (string i in controller)
        {
            if (i == "P3" && p3)
            {
                if (Input.GetKeyDown(KeyCode.Joystick3Button4))
                {

                    splashCounter_pic3--;
                    if (splashCounter_pic3 == 0) { splashCounter_pic3 = splashart.playerSplashart.Length; }

                    SplashArtChange(counter, splashCounter_pic3);
                }

                if (Input.GetKeyDown(KeyCode.Joystick3Button5))
                {
                    splashCounter_pic3++;
                    if (splashCounter_pic3 >= splashart.playerSplashart.Length + 1) { splashCounter_pic3 = 1; }

                    SplashArtChange(counter, splashCounter_pic3);
                }
            }
            counter++;
        }

        if (!p3)
        {
            splashart.splashartHolder[counter].overrideSprite = null;
        }
    }

    //Input Joystick 4
    void Joystick_4() {

        // connect joysick 4
        if (Input.GetKeyDown(KeyCode.Joystick4Button0) && !p4) {
            if (!limitPlayerAmmount) {
                controller.Add("P4");
                p4 = true;
            }
            
            else {
                if (playerCount < playerLimit) {
                    controller.Add("P4");
                    playerCount++;
                    p4 = true;
                }
            }
        }

        // disconnect joysick 4
        if (Input.GetKeyDown(KeyCode.Joystick1Button1) && p4) {
            if (limitPlayerAmmount) {
                playerCount--;
                ResetText(controller.IndexOf("P4"));
                controller.Remove("P4");

                p1 = false;
            }
            
            else {
                ResetText(controller.IndexOf("P4"));
                controller.Remove("P4");

                p1 = false;
            }
        }

        int counter = 0;
        foreach (string i in controller)
        {
            if (i == "P4" && p4)
            {
                if (Input.GetKeyDown(KeyCode.Joystick4Button4))
                {

                    splashCounter_pic4--;
                    if (splashCounter_pic4 == 0) { splashCounter_pic4 = splashart.playerSplashart.Length; }

                    SplashArtChange(counter, splashCounter_pic4);
                }

                if (Input.GetKeyDown(KeyCode.Joystick4Button5))
                {
                    splashCounter_pic4++;
                    if (splashCounter_pic4 >= splashart.playerSplashart.Length + 1) { splashCounter_pic4 = 1; }

                    SplashArtChange(counter, splashCounter_pic4);
                }
            }
            counter++;
        }

        if (!p4)
        {
            splashart.splashartHolder[counter].overrideSprite = null;
        }
    }

    //Input Keyboard
    void Keyboard() {
        // connect keyboard
        if (Input.GetKeyDown(KeyCode.LeftControl) && !kb) {
            if (!limitPlayerAmmount) {
                controller.Add("KB");
                kb = true;
            }
            
            else {
                if (playerCount < playerLimit) {
                    controller.Add("KB");
                    playerCount++;
                    kb = true;
                }
            }
        }

        // disconnect keyboard
        if (Input.GetKeyDown(KeyCode.LeftAlt) && kb) {
            if (limitPlayerAmmount) {
                playerCount--;
                ResetText(controller.IndexOf("KB"));
                controller.Remove("KB");
                
                kb = false;
                sp_kb = false;
            }
            
            else {
                ResetText(controller.IndexOf("KB"));
                controller.Remove("KB");
                
                kb = false;
                sp_kb = false;
            }
        }

        int counter = 0;
        foreach (string i in controller)
        {
            if (i == "KB" && kb)
            {
                if (Input.GetKeyDown(KeyCode.A) && !sp_kb)
                {

                    splashCounter_pic5--;
                    if (splashCounter_pic5 == 0) { splashCounter_pic5 = splashart.playerSplashart.Length; }

                    SplashArtChange(counter, splashCounter_pic5);
                }

                if (Input.GetKeyDown(KeyCode.D) && !sp_kb)
                {
                    splashCounter_pic5++;
                    if (splashCounter_pic5 >= splashart.playerSplashart.Length + 1) { splashCounter_pic5 = 1; }

                    SplashArtChange(counter, splashCounter_pic5);
                }

                if (Input.GetKeyDown(KeyCode.Space))
                {
                    ChooseSplashArt(counter, splashCounter_pic5);
                    sp_kb = true;
                }
            }
            counter++;
        }

        if (!kb)
        {
            splashart.splashartHolder[counter].overrideSprite = null;
        }
    }
    #endregion

    void ResetText(int index) {
        Debug.Log(index);
        splashart.playerText[index].color = Color.black;
        splashart.playerText[index].text = "Player " + (index + 1);
    }

    private void SplashArtChange(int index, int splashCounterChange)
    {
        switch (splashCounterChange)
        {
            case 1:
                splashart.splashartHolder[index].overrideSprite = splashart.playerSplashart[splashCounterChange - 1];
                break;

            case 2:
                splashart.splashartHolder[index].overrideSprite = splashart.playerSplashart[splashCounterChange - 1];
                break;

            case 3:
                splashart.splashartHolder[index].overrideSprite = splashart.playerSplashart[splashCounterChange - 1];
                break;

            case 4:
                splashart.splashartHolder[index].overrideSprite = splashart.playerSplashart[splashCounterChange - 1];
                break;
        }
    }
    private void ChooseSplashArt(int index, int splashCounter)
    {
        switch (splashCounter)
        {
            case 1:
                chosenPics[index] = "Earth";
                break;

            case 2:
                chosenPics[index] = "Saturn";
                break;

            case 3:
                chosenPics[index] = "Jupiter";
                break;

            case 4:
                chosenPics[index] = "Sun";
                break;
        }
    }
}

[System.Serializable]
public class CharacterSplashArts
{
    public Text[] playerText = new Text[4];
    public Sprite[] playerSplashart = new Sprite[4];
    public Sprite[] playerSplashartGrayscale = new Sprite[4];
    public Image[] splashartHolder = new Image[4];
}
