using System;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class PlayerSelection : MonoBehaviour
{
    #region Variables
    public static PlayerSelection _instance;
    [Tooltip("Level Index to load after everyone pressed twice")]
    public int levelToLoad;

    [Tooltip("Limit the player ammount")]
    [Range(1, 4)]
    public int playerLimit;
    public int playerCount;

    #region Player Splashart
    public Sprite[] characterSplashart = new Sprite[4];
    public Sprite[] characterSplashartBW = new Sprite[4];
    public Image[] characterHolder = new Image[4];
    private Sprite[] characterSplashartBackup = new Sprite[4];

    private string[] controller = new string[4];
    private CharacterPicture[] characterPicture = new CharacterPicture[4];
    private Dictionary<string, bool> chosenPics = new Dictionary<string, bool>();
    private Dictionary<string, string> controllerToPlayer = new Dictionary<string, string>();
    #endregion

    #region UI
    // TODO Make a UI-Manager
    public GameObject ReadyScreen = null;
    #endregion

    #endregion

    void Start()
    {
        characterSplashart.CopyTo(characterSplashartBackup, 0);

        //Set All Dictionary Entries 
        chosenPics.Add("Earth", false); chosenPics.Add("Jupiter", false); chosenPics.Add("Saturn", false); chosenPics.Add("Sun", false);
        controllerToPlayer.Add("Earth", ""); controllerToPlayer.Add("Jupiter", ""); controllerToPlayer.Add("Saturn", ""); controllerToPlayer.Add("Sun", "");

        //Set All Splasharts to the default sprite
        for (int i = 0; i < 4; i++)
        {
            characterPicture[i] = new CharacterPicture(characterSplashartBW[i], characterHolder[i], i);
        }
    }

    void FixedUpdate()
    {
        Keyboard();
        Gamepad_1();
        Gamepad_2();
        Gamepad_3();

        //If all player have pressed twice 
        if(playerCount == playerLimit)
        {
            //Set the Ready Screen Active
            ReadyScreen.SetActive(true);

            //Start or Backspace is pressed go to next scene and copy dictionary to gamerules
            if(Input.GetKeyDown(KeyCode.Backspace) || Input.GetKeyDown(KeyCode.Joystick1Button7) || Input.GetKeyDown(KeyCode.Joystick2Button7) || Input.GetKeyDown(KeyCode.Joystick3Button7))
            {
                try { Gamerules._instance.chosenPics = chosenPics; } catch(Exception) { print("Failed to copy chosenPics Dictionary"); }
                try { Gamerules._instance.controllerToPlayer = controllerToPlayer; } catch (Exception) { print("Failed to copy controllerToPlayer Dictionary"); }
                Application.LoadLevel(levelToLoad);
            }
        } else { ReadyScreen.SetActive(false); }
    }

    void Keyboard()
    {
        //Activate Keyboard
        if (Input.GetKeyDown(KeyCode.C) && characterPicture[0].pressedTwice != true && playerCount < playerLimit)
        {
            if (characterPicture[0].pressed && SearchForKeyInDictionary(characterPicture[0].Tag) != true)
            {
                characterPicture[0].pressed = false;
                characterPicture[0].pressedTwice = true;
                characterPicture[0].ChoseSprite();

                SetAllPlayerRelatedVariables(characterPicture[0].Tag, "KB", true, true);

                //Player Count/Limit
                playerCount++;
            } 
            else
            {
                characterPicture[0].ChangePicture(characterSplashart[0]);
                characterPicture[0].pressed = true;

                controller[0] = "KB";
            }
        }

        //Deactivate/Disconnect Keyboard
        if(Input.GetKeyDown(KeyCode.X) && playerCount > 0)
        {
            SetAllPlayerRelatedVariables(characterPicture[0].Tag, "KB", false, false);

            characterPicture[0].ResetPicture(characterSplashartBW[0]);

            controller[0] = null;

            //Player Count/Limit
            playerCount--;
        }

        //Cycle thru the splasharts
        if(Input.GetKeyDown(KeyCode.A) && characterPicture[0].pressed)
        {
            characterPicture[0].CountUpDown(-1);
            characterPicture[0].ChangePicture(characterSplashart[characterPicture[0].GetCount]);
        }

        if (Input.GetKeyDown(KeyCode.D) && characterPicture[0].pressed)
        {
            characterPicture[0].CountUpDown(1);
            characterPicture[0].ChangePicture(characterSplashart[characterPicture[0].GetCount]);
        }
    }

    void Gamepad_1()
    {
        //Activate Gamepad_1
        if (Input.GetKeyDown(KeyCode.Joystick1Button0) && characterPicture[1].pressedTwice != true && playerCount < playerLimit)
        {
            if (characterPicture[1].pressed && SearchForKeyInDictionary(characterPicture[1].Tag) != true)
            {
                characterPicture[1].pressed = false;
                characterPicture[1].pressedTwice = true;
                characterPicture[1].ChoseSprite();

                SetAllPlayerRelatedVariables(characterPicture[1].Tag, "P1", true, true);

                //Player Count/Limit
                playerCount++;
            }
            else
            {
                characterPicture[1].ChangePicture(characterSplashart[1]);
                characterPicture[1].pressed = true;

                controller[1] = "P1";
            }
        }

        //Deactivate/Disconnect Gamepad_1
        if (Input.GetKeyDown(KeyCode.Joystick1Button1) && playerCount > 0)
        {
            SetAllPlayerRelatedVariables(characterPicture[1].Tag, "P1", false, false);

            characterPicture[1].ResetPicture(characterSplashartBW[1]);

            controller[1] = null;

            //Player Count/Limit
            playerCount--;
        }

        //Cycle thru the splasharts
        if (Input.GetKeyDown(KeyCode.Joystick1Button4) && characterPicture[1].pressed)
        {
            characterPicture[1].CountUpDown(-1);
            characterPicture[1].ChangePicture(characterSplashart[characterPicture[1].GetCount]);
        }

        if (Input.GetKeyDown(KeyCode.Joystick1Button5) && characterPicture[1].pressed)
        {
            characterPicture[1].CountUpDown(1);
            characterPicture[1].ChangePicture(characterSplashart[characterPicture[1].GetCount]);
        }
    }

    void Gamepad_2()
    {
        //Activate Gamepad_1
        if (Input.GetKeyDown(KeyCode.Joystick2Button0) && characterPicture[2].pressedTwice != true && playerCount < playerLimit)
        {
            if (characterPicture[2].pressed && SearchForKeyInDictionary(characterPicture[2].Tag) != true)
            {
                characterPicture[2].pressed = false;
                characterPicture[2].pressedTwice = true;
                characterPicture[2].ChoseSprite();

                SetAllPlayerRelatedVariables(characterPicture[2].Tag, "P2", true, true);

                //Player Count/Limit
                playerCount++;
            }
            else
            {
                characterPicture[2].ChangePicture(characterSplashart[2]);
                characterPicture[2].pressed = true;

                controller[2] = "P2";
            }
        }

        //Deactivate/Disconnect Gamepad_1
        if (Input.GetKeyDown(KeyCode.Joystick2Button1) && playerCount > 0)
        {
            SetAllPlayerRelatedVariables(characterPicture[2].Tag, "P2", false, false);

            characterPicture[2].ResetPicture(characterSplashartBW[2]);

            controller[2] = null;

            //Player Count/Limit
            playerCount--;
        }

        //Cycle thru the splasharts
        if (Input.GetKeyDown(KeyCode.Joystick2Button4) && characterPicture[2].pressed)
        {
            characterPicture[2].CountUpDown(-1);
            characterPicture[2].ChangePicture(characterSplashart[characterPicture[2].GetCount]);
        }

        if (Input.GetKeyDown(KeyCode.Joystick2Button5) && characterPicture[2].pressed)
        {
            characterPicture[2].CountUpDown(1);
            characterPicture[2].ChangePicture(characterSplashart[characterPicture[2].GetCount]);
        }
    }

    void Gamepad_3()
    {
        //Activate Gamepad_1
        if (Input.GetKeyDown(KeyCode.Joystick3Button0) && characterPicture[3].pressedTwice != true && playerCount < playerLimit)
        {
            if (characterPicture[3].pressed && SearchForKeyInDictionary(characterPicture[3].Tag) != true)
            {
                characterPicture[3].pressed = false;
                characterPicture[3].pressedTwice = true;
                characterPicture[3].ChoseSprite();

                SetAllPlayerRelatedVariables(characterPicture[3].Tag, "P3", true, true);

                //Player Count/Limit
                playerCount++;
            }
            else
            {
                characterPicture[3].ChangePicture(characterSplashart[3]);
                characterPicture[3].pressed = true;

                controller[3] = "P3";
            }
        }

        //Deactivate/Disconnect Gamepad_1
        if (Input.GetKeyDown(KeyCode.Joystick3Button1) && playerCount > 0)
        {
            SetAllPlayerRelatedVariables(characterPicture[3].Tag, "P3", false, false);

            characterPicture[3].ResetPicture(characterSplashartBW[3]);

            controller[3] = null;

            //Player Count/Limit
            playerCount--;
        }

        //Cycle thru the splasharts
        if (Input.GetKeyDown(KeyCode.Joystick3Button4) && characterPicture[3].pressed)
        {
            characterPicture[3].CountUpDown(-1);
            characterPicture[3].ChangePicture(characterSplashart[characterPicture[3].GetCount]);
        }

        if (Input.GetKeyDown(KeyCode.Joystick3Button5) && characterPicture[3].pressed)
        {
            characterPicture[3].CountUpDown(1);
            characterPicture[3].ChangePicture(characterSplashart[characterPicture[3].GetCount]);
        }
    }

    /// <summary>
    /// Set the dictionaries based on the given tag and controller and change the splashart to grayscale
    /// </summary>
    /// <param name="tag">Earth, Jupiter, Saturn or Sun</param>
    /// <param name="controller">KB, P1, P2 or P3</param>
    /// <param name="value">Set the chosenPic Dicitonary entry to the given parameter</param>
    /// <param name="bw">Turn it into grayscale or not</param>
    private void SetAllPlayerRelatedVariables(string tag, string controller, bool value, bool bw)
    {
        switch (tag)
        {
            case "Earth":
                chosenPics["Earth"] = value;
                controllerToPlayer["Earth"] = controller;
                if (bw) { characterSplashart[0] = characterSplashartBW[0]; } else { characterSplashart[0] = characterSplashartBackup[0]; }
                break;
            case "Jupiter":
                chosenPics["Jupiter"] = value;
                controllerToPlayer["Jupiter"] = controller;
                if (bw) { characterSplashart[1] = characterSplashartBW[1]; } else { characterSplashart[1] = characterSplashartBackup[1]; }
                break;
            case "Saturn":
                chosenPics["Saturn"] = value;
                controllerToPlayer["Saturn"] = controller;
                if (bw) { characterSplashart[2] = characterSplashartBW[2]; } else { characterSplashart[2] = characterSplashartBackup[2]; }
                break;
            case "Sun":
                chosenPics["Sun"] = value;
                controllerToPlayer["Sun"] = controller;
                if (bw) { characterSplashart[3] = characterSplashartBW[3]; } else { characterSplashart[3] = characterSplashartBackup[3]; }
                break;
        }
    }

    /// <summary>
    /// Search for a key in chosenPics
    /// </summary>
    /// <param name="search">The key to search for</param>
    /// <returns>Return true if given parameter is found in the dictionary</returns>
    private bool SearchForKeyInDictionary(string search)
    {
        foreach (var item in chosenPics)
        {
            if (item.Key == search)
            {
                if (item.Value == true)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        return false;
    }
}


