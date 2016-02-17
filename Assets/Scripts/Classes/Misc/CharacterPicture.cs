using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class CharacterPicture
{
    private string name;
    private string character;
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

    public CharacterPicture(string axis, int index, int pictureNumber)
    {
        this.axis = axis;
        this.index = index;
        this.pictureNumber = pictureNumber;
        name = IndexToStanddartName(index);
    }

    #region Getter & Setter
    public string Axis { get { return axis; } }
    public string Name { get { return name; } set { name = value; } }
    public int Index { get { return index; } }
    public bool IsLocked { get { return isLocked; } set { isLocked = value; } }
    public string Character { get { return character; } set { character = value; } }
    public int PictureNumber { get { return pictureNumber; } set { pictureNumber = value; } }
    #endregion

    public string PictureNumToCharacter(int _pictureNum)
    {
        switch (_pictureNum)
        {
            case 0: return "Earth";
            case 1: return "Sun";
            case 2: return "Saturn";
            case 3: return "Jupitar";
        }

        Debug.LogError("Wrong PictureNumTocharacter input");
        return null;
    }

    public int CharacterToPictureNum(string _character)
    {
        switch (_character)
        {
            case "Earth": return 0;
            case "Sun": return 1;
            case "Saturn": return 2;
            case "Jupitar": return 3;
        }

        Debug.LogError("Wrong characterToPictureNum input");
        return 0;
    }

    public void UpdateCharacter() { character = PictureNumToCharacter(pictureNumber); }

    public string IndexToStanddartName(int _index)
    {
        switch (_index)
        {
            case 0: return "Player1";
            case 1: return "Player2";
            case 2: return "Player3";
            case 3: return "Player4";
        }

        Debug.LogError("Wrong PictureNumTocharacter input");
        return null;
    }
}
