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

    private int playerCount;

    #region Player Splashart
    public Sprite[] notConnectedImg = new Sprite[2];
    public Sprite[] characterSplashart = new Sprite[2];
    public Sprite[] characterSplashartLocked = new Sprite[2];
    public Sprite[] characterSplashartBlocked = new Sprite[2];

    private Sprite[] scrollSplasharts = new Sprite[2];

    public Image[] characterHolder = new Image[2];

    private string[] controller = new string[4];
    private CharacterPicture[] characterPicture = new CharacterPicture[4];
    #endregion

    private int[] gamepadSwep = new int[4];

    #region UI
    // TODO Make a UI-Manager
    public GameObject ReadyScreen = null;
    #endregion

    #endregion

    void Start()
    {
        //CustomDebuger.Log(Test.Instance.TestInt);

        // set notConnected images
        for (int i = 0; i < characterHolder.Length; i++)
        {
            characterHolder[i].overrideSprite = notConnectedImg[i];
        }

        // set scrollSplasharts images
        for (int i = 0; i < scrollSplasharts.Length; i++)
        {
            scrollSplasharts[i] = characterSplashart[i];
        }
    }

    void Update()
    {
        for (int i = 1; i <= gamepadSwep.Length; i++)
        {
            if (Input.GetAxis("P" + i + "_Horizontal") > 0.3f && gamepadSwep[i - 1] == 0) { gamepadSwep[i - 1] = 1; }
            if (Input.GetAxis("P" + i + "_Horizontal") < -0.3f && gamepadSwep[i - 1] == 0) { gamepadSwep[i - 1] = -1; }
            if (Input.GetAxis("P" + i + "_Horizontal") <= 0.3f && Input.GetAxis("P" + i + "_Horizontal") >= -0.3f && gamepadSwep[i - 1] != 0) { gamepadSwep[i - 1] = 0; }
        }

        // lock/delock in char
        foreach (CharacterPicture character in characterPicture)
        {
            // check if entry isn't empty
            if (character != null)
            {
                // check if char is already locked
                if (character.IsLocked)
                {
                    // lock char
                    if (character.Axis == "KB" && Input.GetKeyDown(KeyCode.Backspace)) { DelockCharacter(character); }
                    if (character.Axis == "P1" && Input.GetKeyDown(KeyCode.Joystick1Button1)) { DelockCharacter(character); }
                    if (character.Axis == "P2" && Input.GetKeyDown(KeyCode.Joystick2Button1)) { DelockCharacter(character); }
                    if (character.Axis == "P3" && Input.GetKeyDown(KeyCode.Joystick3Button1)) { DelockCharacter(character); }
                    if (character.Axis == "P4" && Input.GetKeyDown(KeyCode.Joystick4Button1)) { DelockCharacter(character); }
                }

                else
                {
                    // delock char
                    if (character.Axis == "KB" && Input.GetKeyDown(KeyCode.Return)) { LockCharacter(character); }
                    if (character.Axis == "P1" && Input.GetKeyDown(KeyCode.Joystick1Button0)) { LockCharacter(character); }
                    if (character.Axis == "P2" && Input.GetKeyDown(KeyCode.Joystick2Button0)) { LockCharacter(character); }
                    if (character.Axis == "P3" && Input.GetKeyDown(KeyCode.Joystick3Button0)) { LockCharacter(character); }
                    if (character.Axis == "P4" && Input.GetKeyDown(KeyCode.Joystick4Button0)) { LockCharacter(character); }
                }

            }
        }

        // connect player
        if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Return)) { ConnectPlayer("KB"); }
        if (Input.GetKeyDown(KeyCode.Joystick1Button7)) { ConnectPlayer("P1"); }
        if (Input.GetKeyDown(KeyCode.Joystick2Button7)) { ConnectPlayer("P2"); }
        if (Input.GetKeyDown(KeyCode.Joystick3Button7)) { ConnectPlayer("P3"); }
        if (Input.GetKeyDown(KeyCode.Joystick4Button7)) { ConnectPlayer("P4"); }

        // disconnect player
        if (Input.GetKeyDown(KeyCode.Escape)) { DisConnectPlayer("KB"); }
        if (Input.GetKeyDown(KeyCode.Joystick1Button6)) { DisConnectPlayer("P1"); }
        if (Input.GetKeyDown(KeyCode.Joystick2Button6)) { DisConnectPlayer("P2"); }
        if (Input.GetKeyDown(KeyCode.Joystick3Button6)) { DisConnectPlayer("P3"); }
        if (Input.GetKeyDown(KeyCode.Joystick4Button6)) { DisConnectPlayer("P4"); }

        // switch char
        foreach(CharacterPicture character in characterPicture)
        {
            // check if entry isn't empty
            if (character != null)
            {
                // check if char is already locked
                if (!character.IsLocked)
                {
                    if (character.Axis == "KB")
                    {
                        if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.RightArrow)) { SwitchCharacter(character, "right"); }
                        if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.LeftArrow)) { SwitchCharacter(character, "left"); }
                    }

                    if (character.Axis == "P1")
                    {
                        if (Input.GetKeyDown(KeyCode.Joystick1Button5) || gamepadSwep[0] == 1) { SwitchCharacter(character, "right"); gamepadSwep[0] = 2; }
                        if (Input.GetKeyDown(KeyCode.Joystick1Button4) || gamepadSwep[0] == -1) { SwitchCharacter(character, "left"); gamepadSwep[0] = -2; }
                    }

                    if (character.Axis == "P2")
                    {
                        if (Input.GetKeyDown(KeyCode.Joystick2Button5) || gamepadSwep[1] == 1) { SwitchCharacter(character, "right"); gamepadSwep[1] = 2; }
                        if (Input.GetKeyDown(KeyCode.Joystick2Button4) || gamepadSwep[1] == -1) { SwitchCharacter(character, "left"); gamepadSwep[1] = -2; }
                    }

                    if (character.Axis == "P3")
                    {
                        if (Input.GetKeyDown(KeyCode.Joystick3Button5) || gamepadSwep[2] == 1) { SwitchCharacter(character, "right"); gamepadSwep[2] = 2; }
                        if (Input.GetKeyDown(KeyCode.Joystick3Button4) || gamepadSwep[2] == -1) { SwitchCharacter(character, "left"); gamepadSwep[2] = -2; }
                    }

                    if (character.Axis == "P4")
                    {
                        if (Input.GetKeyDown(KeyCode.Joystick4Button5) || gamepadSwep[3] == 1) { SwitchCharacter(character, "right"); gamepadSwep[3] = 2; }
                        if (Input.GetKeyDown(KeyCode.Joystick4Button4) || gamepadSwep[3] == -1) { SwitchCharacter(character, "left"); gamepadSwep[3] = -2; }
                    }
                }
            }
        }
    }

    private void ConnectPlayer(string axis)
    {
        // find out if player is already connected
        bool connected = false;
        foreach (CharacterPicture entry in characterPicture)
        {
            if (entry != null)
            {
                if (entry.Axis == axis)
                {
                    connected = true;
                }
            }
        }

        // continue if player isn't connected yet
        if (!connected)
        {
            // get free entry
            for (int i = 0; i < characterPicture.Length; i++)
            {
                if (characterPicture[i] == null)
                {
                    // create new player
                    characterPicture[i] = new CharacterPicture(axis, i, 0);
                    // show chars
                    characterHolder[i].overrideSprite = scrollSplasharts[i];
                    break;
                }
            }
        }
    }

    private void DisConnectPlayer(string axis)
    {
        // get all palyer
        for(int i = 0; i < characterPicture.Length; i++)
        {
            // check if entry is empty
            if(characterPicture[i] != null)
            {
                // get player with right axis
                if(characterPicture[i].Axis == axis)
                {
                    // delock char
                    if (characterPicture[i].IsLocked) { DelockCharacter(characterPicture[i]); }
                    // set disconnected pcture
                    characterHolder[characterPicture[i].Index].overrideSprite = notConnectedImg[characterPicture[i].Index];
                    // delite palyer
                    characterPicture[i] = null;
                }
            }
        }
    }

    private void SwitchCharacter(CharacterPicture character, string direction)
    {
        if(direction == "right")
        {
            character.PictureNumber++;
            if (character.PictureNumber >= scrollSplasharts.Length) { character.PictureNumber = 0; }
            characterHolder[character.Index].overrideSprite = scrollSplasharts[character.PictureNumber];
        }

        if (direction == "left")
        {
            character.PictureNumber--;
            if (character.PictureNumber < 0) { character.PictureNumber = scrollSplasharts.Length - 1; }
            characterHolder[character.Index].overrideSprite = scrollSplasharts[character.PictureNumber];
        }
    }

    private void LockCharacter(CharacterPicture character)
    {
        // check if the character is already locked in 
        bool isCharLocked = false;
        foreach (CharacterPicture picture in characterPicture)
        {
            if (picture != null)
            {
                if (picture.PictureNumber == character.PictureNumber && picture.IsLocked)
                {
                    isCharLocked = true;
                }
            }
        }

        if (!isCharLocked)
        {
            // replace the splash arts to the blocked one in scrollSplasharts
            scrollSplasharts[character.PictureNumber] = characterSplashartBlocked[character.PictureNumber];
            // replace the players splashart to the locked in one
            characterHolder[character.Index].overrideSprite = characterSplashartLocked[character.PictureNumber];
            // set the isLocked variable of the instance to true
            character.IsLocked = true;

            // reload all splasharts from all not locked in palyers
            foreach (CharacterPicture picture in characterPicture)
            {
                if (picture != null)
                {
                    if (!picture.IsLocked)
                    {
                        characterHolder[picture.Index].overrideSprite = scrollSplasharts[picture.PictureNumber];
                    }
                }
            }
        }
    }

    private void DelockCharacter(CharacterPicture character)
    {
        // replace the splash arts to the not blocked one in scrollSplasharts
        scrollSplasharts[character.PictureNumber] = characterSplashart[character.PictureNumber];
        // set the isLocked variable of the instance to false
        character.IsLocked = false;

        // reload all splasharts from all not locked in palyers
        foreach (CharacterPicture picture in characterPicture)
        {
            if (picture != null)
            {
                if (!picture.IsLocked)
                {
                    characterHolder[picture.Index].overrideSprite = scrollSplasharts[picture.PictureNumber];
                }
            }
        }
    }
}