using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerSelection : MonoBehaviour {

    public List<string> controller = new List<string>();
    private bool p1, p2, p3, p4, kb;
    public int levelToLoad;

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Joystick1Button0))
        {
            if (!p1) { controller.Add("P1"); p1 = true; }
        }

        if(Input.GetKeyDown(KeyCode.Joystick2Button0))
        {
            if (!p2) { controller.Add("P2"); p2 = true; }
        }

        if(Input.GetKeyDown(KeyCode.Joystick3Button0))
        {
            if (!p3) { controller.Add("P3"); p3 = true; }
        }

        if(Input.GetKeyDown(KeyCode.Joystick4Button0))
        {
            if (!p4) { controller.Add("P4"); p4 = true; }
        }

        if (Input.GetKeyDown(KeyCode.LeftControl)) 
        {
            if (!kb) { controller.Add("KB"); kb = true; }
        }

        if (Input.GetKeyDown(KeyCode.Joystick1Button7) || Input.GetKeyDown(KeyCode.Joystick2Button7) || Input.GetKeyDown(KeyCode.Joystick3Button7) || Input.GetKeyDown(KeyCode.Joystick4Button7) || Input.GetKeyDown(KeyCode.A))
        {
            if (p1 || p2 || p3 || p4 || kb)
            {
                Application.LoadLevel(levelToLoad);
            }
        }

        Gamerules._instance.playerAmmount = controller.Count;
    }
}
