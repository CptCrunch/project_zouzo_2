using System;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class PlayerSelection : MonoBehaviour
{
    #region Variables
    public static PlayerSelection _instance;

    public string[] controller = new string[4];
    public int levelToLoad;

    public bool limitPlayerAmmount;
    [Range(1, 4)]
    public int playerLimit;
    private int playerCount;

    public Sprite[] characterSplashart = new Sprite[4];
    public Sprite[] characterSplashartBW = new Sprite[4];
    public Image[] characterHolder = new Image[4];

    private CharacterPicture[] characterPicture = new CharacterPicture[4];

    // TODO public -> private
    public Sprite[] chosenPics = new Sprite[4];
    #endregion

    void Start()
    {
        for (int i = 0; i < 4; i++)
        {
            characterPicture[i] = new CharacterPicture(characterSplashartBW[i], characterHolder[i], i);
        }

    }

    void Update()
    {
        Keyboard();
        Gamepad_1();
    }

    void Keyboard()
    {
        //Activate Keyboard
        if (Input.GetKeyDown(KeyCode.C) && characterPicture[0].pressedTwice != true)
        {
            if (characterPicture[0].pressed)
            {
                characterPicture[0].pressed = false;
                characterPicture[0].pressedTwice = true;
                characterPicture[0].ChoseSprite();
                chosenPics[0] = characterPicture[0].GetChosenImage;
            }
            else
            {
                characterPicture[0].ChangePicture(characterSplashart[0]);
                characterPicture[0].pressed = true;

                controller[0] = "KB";
            }
        }

        //Deactivate/Disconnect Keyboard
        if(Input.GetKeyDown(KeyCode.X))
        {
            characterPicture[0].ResetPicture(characterSplashartBW[0]);

            controller[0] = null;
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
        //Activate Keyboard
        if (Input.GetKeyDown(KeyCode.Joystick1Button0) && characterPicture[1].pressedTwice != true)
        {
            if (characterPicture[1].pressed)
            {
                characterPicture[1].pressed = false;
                characterPicture[1].pressedTwice = true;
                characterPicture[1].ChoseSprite();
            }
            else
            {
                characterPicture[1].ChangePicture(characterSplashart[1]);
                characterPicture[1].pressed = true;

                controller[1] = "P1";
            }
        }

        //Deactivate/Disconnect Keyboard
        if (Input.GetKeyDown(KeyCode.Joystick1Button1))
        {
            characterPicture[1].ResetPicture(characterSplashartBW[1]);

            controller[1] = null;
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

}


