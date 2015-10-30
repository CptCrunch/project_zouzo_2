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

    //Player Limitation and Text
    public Text[] playerText = new Text[4];
    public bool limitPlayerAmmount;
    [Range(1,4)]
    public int playerLimit;
    private int playerCount;
    #endregion

    void Awake()
    {
        if (_instance == null) { _instance = this; }
    }

    void Update()
    {
        Joystick_1();
        Joystick_2();
        Joystick_3();
        Joystick_4();
        Keyboard();

        if (Input.GetKeyDown(KeyCode.Joystick1Button7) || Input.GetKeyDown(KeyCode.Joystick2Button7) || Input.GetKeyDown(KeyCode.Joystick3Button7) || Input.GetKeyDown(KeyCode.Joystick4Button7) || Input.GetKeyDown(KeyCode.A))
        {
            if (p1 || p2 || p3 || p4 || kb)
            {
                Application.LoadLevel(levelToLoad);
            }
        }

        for(int i = 0; i < controller.Count; i++)
        {
            playerText[i].color = Color.green;
            playerText[i].text = controller[i] + " Connected";
        }

        Gamerules._instance.playerAmmount = controller.Count;
    }

    #region Input
    //Input Joystick 1
    void Joystick_1()
    {
        if (Input.GetKeyDown(KeyCode.Joystick1Button0))
        {
            if (!p1)
            {
                if (!limitPlayerAmmount)
                {
                    controller.Add("P1");
                    p1 = true;
                }
                else
                {
                    if (playerCount < playerLimit)
                    {
                        controller.Add("P1");
                        playerCount++;
                        p1 = true;
                    }
                }
            }
        }

        if (Input.GetKeyDown(KeyCode.Joystick1Button1))
        {
            if (p1)
            {
                if (limitPlayerAmmount)
                {
                    playerCount--;
                    ResetText(controller.IndexOf("P1"));
                    controller.Remove("P1");

                    p1 = false;
                }
                else
                {
                    ResetText(controller.IndexOf("P1"));
                    controller.Remove("P1");
                   
                    p1 = false;
                }
            }
        }
    }

    //Input Joystick 2
    void Joystick_2()
    {
        if (Input.GetKeyDown(KeyCode.Joystick2Button0))
        {
            if (!p2)
            {
                if (!limitPlayerAmmount)
                {
                    controller.Add("P2");
                    p2 = true;
                }
                else
                {
                    if (playerCount <= playerLimit)
                    {
                        controller.Add("P2");
                        playerCount++;
                        p2 = true;
                    }
                }
            }
        }

        if (Input.GetKeyDown(KeyCode.Joystick2Button1))
        {
            if (p2)
            {
                if (limitPlayerAmmount)
                {
                    playerCount--;
                    ResetText(controller.IndexOf("P2"));
                    controller.Remove("P2");
                   
                    p2 = false;
                }
                else
                {
                    ResetText(controller.IndexOf("P2"));
                    controller.Remove("P2");

                    p2 = false;
                }
            }
        }
    }

    //Input Joystick 3
    void Joystick_3()
    {
        if (Input.GetKeyDown(KeyCode.Joystick3Button0))
        {
            if (!p3)
            {
                if (!limitPlayerAmmount)
                {
                    controller.Add("P3");
                    p3 = true;
                }
                else
                {
                    if (playerCount < playerLimit)
                    {
                        controller.Add("P3");
                        playerCount++;
                        p3 = true;
                    }
                }
            }
        }

        if (Input.GetKeyDown(KeyCode.Joystick3Button1))
        {
            if (p3)
            {
                if (limitPlayerAmmount)
                {
                    playerCount--;
                    ResetText(controller.IndexOf("P3"));
                    controller.Remove("P3");

                    p3 = false;
                }
                else
                {
                    ResetText(controller.IndexOf("P3"));
                    controller.Remove("P3");

                    p3 = false;
                }
            }
        }
    }

    //Input Joystick 4
    void Joystick_4()
    {
        if (Input.GetKeyDown(KeyCode.Joystick4Button0))
        {
            if (!p4)
            {
                if (!limitPlayerAmmount)
                {
                    controller.Add("P4");
                    p4 = true;
                }
                else
                {
                    if (playerCount < playerLimit)
                    {
                        controller.Add("P4");
                        playerCount++;
                        p4 = true;
                    }
                }
            }
        }

        if (Input.GetKeyDown(KeyCode.Joystick1Button1))
        {
            if (p4)
            {
                if (limitPlayerAmmount)
                {
                    playerCount--;
                    ResetText(controller.IndexOf("P4"));
                    controller.Remove("P4");

                    p1 = false;
                }
                else
                {
                    ResetText(controller.IndexOf("P4"));
                    controller.Remove("P4");

                    p1 = false;
                }
            }
        }
    }

    //Input Keyboard
    void Keyboard()
    {
        if (Input.GetKeyDown(KeyCode.LeftControl))
        {
            if (!kb)
            {
                if (!limitPlayerAmmount)
                {
                    controller.Add("KB");
                    kb = true;
                }
                else
                {
                    if (playerCount < playerLimit)
                    {
                        controller.Add("KB");
                        playerCount++;
                        kb = true;
                    }
                }
            }
        }

        if (Input.GetKeyDown(KeyCode.LeftAlt))
        {
            if (kb)
            {
                if (limitPlayerAmmount)
                {
                    playerCount--;
                    ResetText(controller.IndexOf("KB"));
                    controller.Remove("KB");
    
                    kb = false;
                }
                else
                {
                    ResetText(controller.IndexOf("KB"));
                    controller.Remove("KB");

                    kb = false;
                }
            }
        }
    }
    #endregion

    void ResetText(int index)
    {
        Debug.Log(index);
        playerText[index].color = Color.black;
        playerText[index].text = "Player " + (index + 1);
    }
}
