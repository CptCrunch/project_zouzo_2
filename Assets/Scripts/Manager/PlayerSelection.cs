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

    #region UI
    // TODO Make a UI-Manager
    public GameObject ReadyScreen = null;
    #endregion

    #endregion

    void Start()
    {
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
        // connect player
        if (Input.GetKeyDown(KeyCode.C)) { ConnectPlayer("KB"); }
        if (Input.GetKeyDown(KeyCode.Joystick1Button7)) { ConnectPlayer("P1"); }
        if (Input.GetKeyDown(KeyCode.Joystick2Button7)) { ConnectPlayer("P2"); }
        if (Input.GetKeyDown(KeyCode.Joystick3Button7)) { ConnectPlayer("P3"); }
        if (Input.GetKeyDown(KeyCode.Joystick4Button7)) { ConnectPlayer("P4"); }

        // disconnect player
        if (Input.GetKeyDown(KeyCode.X)) { DisConnectPlayer("KB"); }
        if (Input.GetKeyDown(KeyCode.Joystick1Button2)) { DisConnectPlayer("P1"); }
        if (Input.GetKeyDown(KeyCode.Joystick2Button2)) { DisConnectPlayer("P2"); }
        if (Input.GetKeyDown(KeyCode.Joystick3Button2)) { DisConnectPlayer("P3"); }
        if (Input.GetKeyDown(KeyCode.Joystick4Button2)) { DisConnectPlayer("P4"); }

        // switch char
        foreach(CharacterPicture character in characterPicture)
        {
            if (character != null)
            {
                if (!character.IsLocked)
                {
                    if (character.Axis == "KB")
                    {
                        if (Input.GetKeyDown(KeyCode.A)) { SwitchCharacter(character, "right"); }
                        if (Input.GetKeyDown(KeyCode.D)) { SwitchCharacter(character, "left"); }
                    }

                    if (character.Axis == "P1")
                    {
                        if (Input.GetKeyDown(KeyCode.Joystick1Button5)) { SwitchCharacter(character, "right"); }
                        if (Input.GetKeyDown(KeyCode.Joystick1Button4)) { SwitchCharacter(character, "left"); }
                    }

                    if (character.Axis == "P2")
                    {
                        if (Input.GetKeyDown(KeyCode.Joystick2Button5)) { SwitchCharacter(character, "right"); }
                        if (Input.GetKeyDown(KeyCode.Joystick2Button4)) { SwitchCharacter(character, "left"); }
                    }

                    if (character.Axis == "P3")
                    {
                        if (Input.GetKeyDown(KeyCode.Joystick3Button5)) { SwitchCharacter(character, "right"); }
                        if (Input.GetKeyDown(KeyCode.Joystick3Button4)) { SwitchCharacter(character, "left"); }
                    }

                    if (character.Axis == "P4")
                    {
                        if (Input.GetKeyDown(KeyCode.Joystick4Button5)) { SwitchCharacter(character, "right"); }
                        if (Input.GetKeyDown(KeyCode.Joystick4Button4)) { SwitchCharacter(character, "left"); }
                    }
                }
            }
        }

        // log in char
        foreach (CharacterPicture character in characterPicture)
        {
            if (character != null)
            {
                if (!character.IsLocked)
                {
                    if (character.Axis == "KB")
                    {
                        if (Input.GetKeyDown(KeyCode.V)) { LockCharacter(character); }
                    }

                    if (character.Axis == "P1")
                    {
                        if (Input.GetKeyDown(KeyCode.Joystick1Button0)) { LockCharacter(character); }
                    }

                    if (character.Axis == "P2")
                    {
                        if (Input.GetKeyDown(KeyCode.Joystick2Button0)) { LockCharacter(character); }
                    }

                    if (character.Axis == "P3")
                    {
                        if (Input.GetKeyDown(KeyCode.Joystick3Button0)) { LockCharacter(character); }
                    }

                    if (character.Axis == "P4")
                    {
                        if (Input.GetKeyDown(KeyCode.Joystick4Button0)) { LockCharacter(character); }
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
            for (int i = 0; i < characterPicture.Length; i++)
            {
                if (characterPicture[i] == null)
                {
                    characterPicture[i] = new CharacterPicture(axis, i, 0);
                    characterHolder[i].overrideSprite = scrollSplasharts[i];
                    break;
                }
            }
        }
    }

    private void DisConnectPlayer(string axis)
    {
        for(int i = 0; i < characterPicture.Length; i++)
        {
            if(characterPicture[i] != null)
            {
                if(characterPicture[i].Axis == axis)
                {
                    characterHolder[characterPicture[i].Index].overrideSprite = notConnectedImg[characterPicture[i].Index];
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
            scrollSplasharts[character.PictureNumber] = characterSplashartBlocked[character.PictureNumber];
            characterHolder[character.Index].overrideSprite = characterSplashartLocked[character.PictureNumber];
            character.IsLocked = true;

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
}