using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class CharacterPicture
{
    private string tag;

    private string axis;
    private int index;
    private int pictureNumber;
    private bool isLocked = false;

    private Sprite currImage;
    private Sprite chosenImage;
    private Image imageHolder;

    public bool pressed;
    public bool pressedTwice;
    private int count;

    public CharacterPicture(string axis, int index, int pictureNumber/*Sprite currImage, Image imageHolder, int index*/)
    {
        this.axis = axis;
        this.index = index;
        this.pictureNumber = pictureNumber;
        /*this.currImage = currImage;
        this.imageHolder = imageHolder;
        this.index = index;

        imageHolder.sprite = currImage;*/
    }

    /// <summary>
    /// Change the current image and set the image holder to the current image
    /// </summary>
    public void ChangePicture(Sprite imageToChange)
    {
        currImage = imageToChange;
        imageHolder.overrideSprite = currImage;

        switch (count)
        {
            case 0: tag = "Earth"; break;
            case 1: tag = "Jupiter"; break;
            case 2: tag = "Saturn"; break;
            case 3: tag = "Sun"; break;
        }
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
        tag = null;
    }

    public string Axis { get { return axis; } }
    public int Index { get { return index; } }
    public int PictureNumber
    {
        get { return pictureNumber; }
        set { pictureNumber = value; }
    }
    public bool IsLocked
    {
        get { return isLocked; }
        set { isLocked = value; }
    }

    public string Tag { get { return tag; } }
    public Sprite GetCurrImage { get { return currImage; } }
    public Sprite GetChosenImage { get { return chosenImage; } }
    public Image GetImageHolder { get { return imageHolder; } }
    public int GetCount { get { return count; } set { value = count; } }
}
