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

    public CharacterPicture[] characterPicture = new CharacterPicture[4];
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
        if (Input.GetKeyDown(KeyCode.Joystick1Button0) && characterPicture[0].pressedTwice != true)
        {
            if (characterPicture[0].pressed)
            {
                characterPicture[0].pressed = false;
                characterPicture[0].pressedTwice = true;
                characterPicture[0].ChoseSprite();
            }
            else
            {
                characterPicture[0].ChangePicture(characterSplashart[0]);
                characterPicture[0].pressed = true;
            }
        }

        //Deactivate/Disconnect Keyboard
        if (Input.GetKeyDown(KeyCode.Joystick1Button1))
        {
            characterPicture[0].ResetPicture(characterSplashartBW[0]);
        }

        //Cycle thru the splasharts
        if (Input.GetKeyDown(KeyCode.Joystick1Button6) && characterPicture[0].pressed)
        {
            characterPicture[0].CountUpDown(-1);
            characterPicture[0].ChangePicture(characterSplashart[characterPicture[0].GetCount]);
        }

        if (Input.GetKeyDown(KeyCode.Joystick1Button7) && characterPicture[0].pressed)
        {
            characterPicture[0].CountUpDown(1);
            characterPicture[0].ChangePicture(characterSplashart[characterPicture[0].GetCount]);
        }
    }
}


public class CharacterPicture
{
    private Sprite currImage;
    private Sprite chosenImage;
    private Image imageHolder;
    private int index;

    public bool pressed;
    public bool pressedTwice;
    private int count;

    public CharacterPicture(Sprite currImage, Image imageHolder, int index)
    {
        this.currImage = currImage;
        this.imageHolder = imageHolder;
        this.index = index;

        imageHolder.sprite = currImage;
    }

    /// <summary>
    /// Change the current image and set the image holder to the current image
    /// </summary>
    public void ChangePicture(Sprite imageToChange)
    {
        currImage = imageToChange;
        imageHolder.overrideSprite = currImage;
    }

    /// <summary>
    /// Count up or down, based on the parameter
    /// </summary>
    /// <param name="i">If the Parameter is 1 it goes up, if its -1 it goes down</param>
    public void CountUpDown(int i)
    {
        if (i == -1) count--;
        if (i == 1) count++;
        if (count > 3) count = 0;
        if (count < 0) count = 3;
    }

    /// <summary>
    /// Finalize the image, cant be changed afterwards (ingame)
    /// </summary>
    public void ChoseSprite()
    {
        chosenImage = currImage;
        imageHolder.overrideSprite = chosenImage;
    }

    /// <summary>
    /// Reset the whole image with all it variables
    /// </summary>
    /// <param name="standartImage">Sprite where it resets to</param>
    public void ResetPicture(Sprite standartImage)
    {
        currImage = standartImage;
        imageHolder.overrideSprite = currImage;
        pressed = false;
        pressedTwice = false;
        count = 0;
    }

    public Sprite GetCurrImage { get { return currImage; } }
    public Image GetImageHolder { get { return imageHolder; } }
    public int GetCount { get { return count; } set { value = count; } }
}
