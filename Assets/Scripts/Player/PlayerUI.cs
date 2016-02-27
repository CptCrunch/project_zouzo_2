using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PlayerUI : MonoBehaviour {

    public string playerName;
    public string playerCharacter;
    public int indexOfUserInterfaces;
    public bool additionalLife = true;

    // ui children
    private int childHealthBar = 0;
    private int childHPText = 1;
    private int childPlayerIcon = 3;
    private int childSpell1Icon = 4;
    private int childSpell2Icon = 5;
    private int childSpell3Icon = 6;
    private int childLiveContainer = 8;

    // children from Spells
    private int childeCCF = 0; // charge cooldown frame
    private int childCooldown = 1;
    private int childCooldownText = 2;
    private int childChargeCount = 3;

    // Use this for initialization
    void Start ()
    {
    }
	
	// Update is called once per frame
	void Update ()
    {
	
	}

    public void UpdateLifes()
    {
        LivingEntity playerVitals = GameManager._instance.GetStagePrefabByName(playerCharacter).GetComponent<Player>().playerVitals;
        Debug.Log("UI: " + playerVitals.Life);
        #region lifes
        // --- [ set lives ] ---
        #region additional
        if (additionalLife)
        {
            int childCount = 0;
            // --- [ get life amount ] ---
            if (playerVitals.Life > 5)
            {
                childCount = 6;
                // get liveContainer
                transform.GetChild(childLiveContainer).gameObject.transform.GetChild(5).gameObject.GetComponent<Text>().text = "+" + (playerVitals.Life - 5);
            }

            else { childCount = playerVitals.Life; }

            // --- [ set lifes visible / invisible ] ---
            for (int o = 0; o < 6; o++)
            {
                if (childCount > o) { if (!transform.GetChild(childLiveContainer).transform.GetChild(o).gameObject.active)
                    { transform.GetChild(childLiveContainer).transform.GetChild(o).gameObject.active = true; } }
                else { if (transform.GetChild(childLiveContainer).transform.GetChild(o).gameObject.active)
                    { transform.GetChild(childLiveContainer).transform.GetChild(o).gameObject.active = false; } }
            }
        }
        #endregion

        #region multiplication
        else
        {
            // --- [ get life amount ] ---
            if (playerVitals.Life > 5)
            {
                // set additional life text
                transform.GetChild(childLiveContainer).transform.GetChild(5).gameObject.GetComponent<Text>().text = "x" + (playerVitals.Life);

                // set first life visible
                if (!transform.GetChild(childLiveContainer).transform.GetChild(0).gameObject.active)
                { transform.GetChild(childLiveContainer).transform.GetChild(0).gameObject.active = true; }
                for (int o = 0; o < 5; o++)
                {
                    if (o != 2)
                    {
                        if (transform.GetChild(childLiveContainer).transform.GetChild(o).gameObject.active)
                        { transform.GetChild(childLiveContainer).transform.GetChild(o).gameObject.active = false; }
                    }
                }
            }

            else
            {
                for (int o = 0; o < 6; o++)
                {
                    if (playerVitals.Life > o) { if (!transform.GetChild(childLiveContainer).transform.GetChild(o).gameObject.active)
                        { transform.GetChild(childLiveContainer).transform.GetChild(o).gameObject.active = true; } }
                    else { if (transform.GetChild(childLiveContainer).transform.GetChild(o).gameObject.active)
                        { transform.GetChild(childLiveContainer).transform.GetChild(o).gameObject.active = false; } }
                }
            }
        }
        #endregion
        #endregion
    }
}
