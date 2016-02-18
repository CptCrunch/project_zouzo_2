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
    public string levelToLoad;

    private int playerCount = 0;
    private int playerLockedCount = 0;

    #region Player Splashart
    public Sprite[] notConnectedImg = new Sprite[4];
    public Sprite[] characterSplashart = new Sprite[4];
    public Sprite[] characterSplashartLocked = new Sprite[4];
    public Sprite[] characterSplashartBlocked = new Sprite[4];

    public Image[] characterHolder = new Image[4];

    private Sprite[] scrollSplasharts = new Sprite[2];
    private string[] controller = new string[2];
    private CharacterPicture[] characterPicture = new CharacterPicture[2];
    #endregion

    private int[] gamepadSwep = new int[4];

    #region UI
    public GameObject ReadyScreen = null;
    #endregion

    #endregion

    void Start()
    {
        // set notConnected images
        for (int i = 0; i < characterHolder.Length; i++) { characterHolder[i].overrideSprite = notConnectedImg[i]; }

        // set scrollSplasharts images
        for (int i = 0; i < scrollSplasharts.Length; i++) { scrollSplasharts[i] = characterSplashart[i]; }

        // disable ReadyScreen
        ReadyScreen.active = false;
    }

    void Update()
    {
        // --- [ set gamepadswep ] ---
        for (int i = 1; i <= gamepadSwep.Length; i++)
        {
            if (Input.GetAxis("P" + i + "_Horizontal") > 0.3f && gamepadSwep[i - 1] == 0) { gamepadSwep[i - 1] = 1; }
            if (Input.GetAxis("P" + i + "_Horizontal") < -0.3f && gamepadSwep[i - 1] == 0) { gamepadSwep[i - 1] = -1; }
            if (Input.GetAxis("P" + i + "_Horizontal") <= 0.3f && Input.GetAxis("P" + i + "_Horizontal") >= -0.3f && gamepadSwep[i - 1] != 0) { gamepadSwep[i - 1] = 0; }
        }

        // --- [ go to next scene ] ---
        // check if scene is ready
        if (ReadyScreen.active)
        {
            // check if any player whants to start the game
            if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.Joystick1Button0) || Input.GetKeyDown(KeyCode.Joystick2Button0) || Input.GetKeyDown(KeyCode.Joystick3Button0) || Input.GetKeyDown(KeyCode.Joystick4Button0))
            {
                // save picture data
                Gamerules._instance.charPics = characterPicture;

                bool isStage = false;
                // check if levelToLoad is a registered stage
                foreach (string stage in Gamerules._instance.registerdStages) { if (stage == levelToLoad) { isStage = true; break; } }
                
                // load level if it is registered
                if (isStage) { Application.LoadLevel(levelToLoad); }
                // print an Error that stage isn't registered
                else { Debug.LogError("The stage to load isn't registered, check the Gamerules script"); }
                
            }
        }

        // --- [ lock/delock in char ] ---
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

        // --- [ connect player ] ---
        if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Return)) { ConnectPlayer("KB"); }
        if (Input.GetKeyDown(KeyCode.Joystick1Button7)) { ConnectPlayer("P1"); }
        if (Input.GetKeyDown(KeyCode.Joystick2Button7)) { ConnectPlayer("P2"); }
        if (Input.GetKeyDown(KeyCode.Joystick3Button7)) { ConnectPlayer("P3"); }
        if (Input.GetKeyDown(KeyCode.Joystick4Button7)) { ConnectPlayer("P4"); }

        // --- [ disconnect player ] ---
        if (Input.GetKeyDown(KeyCode.Escape)) { DisConnectPlayer("KB"); }
        if (Input.GetKeyDown(KeyCode.Joystick1Button6)) { DisConnectPlayer("P1"); }
        if (Input.GetKeyDown(KeyCode.Joystick2Button6)) { DisConnectPlayer("P2"); }
        if (Input.GetKeyDown(KeyCode.Joystick3Button6)) { DisConnectPlayer("P3"); }
        if (Input.GetKeyDown(KeyCode.Joystick4Button6)) { DisConnectPlayer("P4"); }

        // --- [ switch char ] ---
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

        // --- [ enable / disable ready screen ] ---
        // check if all player are locked and if minimum of players is reached
        if (playerCount == playerLockedCount && playerCount >= 2)
        {
            // ceck if ready screen isn't visible, if not set it vislible
            if (!ReadyScreen.active) { ReadyScreen.active = true; }
        }

        else
        {
            // check if ready screen is invisible, if so set it invislible
            if (ReadyScreen.active) { ReadyScreen.active = false; }
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
                    characterPicture[i] = new CharacterPicture(axis, i, i);
                    // add one to player count
                    playerCount++;
                    // update Character
                    characterPicture[i].UpdateCharacter();
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
                    // set disconnected picture
                    characterHolder[characterPicture[i].Index].overrideSprite = notConnectedImg[characterPicture[i].Index];
                    // delite palyer
                    characterPicture[i] = null;
                    // sub one from player count
                    playerCount--;
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

        character.UpdateCharacter();
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
            // replace the players splashart to the locked one
            characterHolder[character.Index].overrideSprite = characterSplashartLocked[character.PictureNumber];
            // set the isLocked variable of the instance to true
            character.IsLocked = true;
            // update character
            character.UpdateCharacter();
            // add one to player locked count
            playerLockedCount++;

            // --- [ reload all splasharts from all not locked in palyers ] ---
            // get all pictures of the characterPictures
            foreach (CharacterPicture picture in characterPicture)
            {
                // check if picture isn't null
                if (picture != null)
                {
                    // check if picture isn't locked
                    if (!picture.IsLocked)
                    {
                        // reload picture
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
        // sub one from player locked count
        playerLockedCount--;

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